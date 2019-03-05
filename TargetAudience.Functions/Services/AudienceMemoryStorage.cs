﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TargetAudience.Common.Interfaces;
using TargetAudience.Common.Models;

namespace TargetAudience.Functions.Services
{
	public class AudienceMemoryStorage : IAudienceStorageService
	{
		private readonly object _syncroot = new object();
		Dictionary<string, List<Member>> memory;

		public AudienceMemoryStorage(Dictionary<string, List<Member>> dictionary = null)
		{
			memory = dictionary ?? new Dictionary<string, List<Member>>();
		}

		/// <summary>
		/// Deletes storage items from storage.
		/// </summary>
		/// <returns>The async.</returns>
		/// <param name="locations">Locations.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		public Task DeleteAsync(string[] locations, CancellationToken cancellationToken)
		{
			if (locations == null)
				throw new ArgumentNullException(nameof(locations));

			lock (_syncroot)
			{
				foreach (var key in locations)
				{
					memory.Remove(key);
				}
			}

			return Task.CompletedTask;
		}

		/// <summary>
		/// Reads storage items from storage.
		/// </summary>
		/// <returns>The async.</returns>
		/// <param name="locations">Locations.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		public Task<IDictionary<string, List<Member>>> ReadAsync(string[] locations, CancellationToken cancellationToken)
		{
			if (locations == null)
				throw new ArgumentNullException(nameof(locations));

			var storeItems = new Dictionary<string, List<Member>>(locations.Length);
			lock (_syncroot)
			{
				foreach (var key in locations)
				{
					if (memory.TryGetValue(key, out var list))
					{
						if (list != null)
						{
							storeItems.Add(key, list.ToList());
						}
					}
				}
			}

			return Task.FromResult<IDictionary<string, List<Member>>>(storeItems);
		}

		/// <summary>
		/// Writes storage items to storage.
		/// </summary>
		/// <returns>The async.</returns>
		/// <param name="changes">Changes.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		public Task WriteAsync(IDictionary<string, List<Member>> changes, CancellationToken cancellationToken)
		{
			if (changes == null)
			{
				throw new ArgumentNullException(nameof(changes));
			}

			lock (_syncroot)
			{
				foreach (var change in changes)
				{
					List<Member> newState = new List<Member>(change.Value);

					if (memory.TryGetValue(change.Key, out var oldState))
					{
						newState.AddRange(oldState);
					}

					memory[change.Key] = newState;
				}
			}

			return Task.CompletedTask;
		}

		public Task WriteAsync(string location, List<Member> changes, CancellationToken cancellationToken)
		{
			if (string.IsNullOrEmpty(location))
				throw new ArgumentException("Location value cannot be null");

			if (changes == null)
				throw new ArgumentNullException(nameof(changes));

			lock (_syncroot)
			{
				List<Member> newState = new List<Member>(changes);
				if (memory.TryGetValue(location, out var oldState))
				{
					newState.AddRange(oldState);
				}

				memory[location] = newState;
			}

			return Task.CompletedTask;
		}

		/// <summary>
		/// Queries the user statistics over a given time period
		/// </summary>
		/// <returns>The time span.</returns>
		/// <param name="locations">Locations.</param>
		/// <param name="fromDate">From date.</param>
		/// <param name="toDate">To date.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		public Task<IDictionary<string, List<Member>>> QueryTimeSpan(string[] locations, DateTime fromDate, DateTime toDate, CancellationToken cancellationToken)
		{
			if (locations == null)
				throw new ArgumentNullException(nameof(locations));

			var diff = toDate.Subtract(fromDate);
			if (diff < TimeSpan.Zero)
				throw new ArgumentException(nameof(fromDate));

			var results = new Dictionary<string, List<Member>>(locations.Length);

			lock (_syncroot)
			{
				foreach (var key in locations)
				{
					if (memory.TryGetValue(key, out var list))
					{
						if (list != null)
						{
							results[key] = list
							.Where(x => x.Timestamp.Ticks > fromDate.Ticks && x.Timestamp.Ticks < toDate.Ticks)
							.ToList();
						}
					}
				}
			}

			return Task.FromResult<IDictionary<string, List<Member>>>(results);
		}

		public Task<IDictionary<string, List<Member>>> QueryTimeSpan(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken)
		{
			var keys = memory.Keys.ToArray();
			return QueryTimeSpan(keys, fromDate, toDate, cancellationToken);
		}

		public Task<List<LocationWindow>> QueryMemberLocations(string persistentMemberId, DateTime fromDate, DateTime toDate, TimeSpan minimumDuration, CancellationToken cancellationToken)
		{
			var results = new List<LocationWindow>();
			lock (_syncroot)
			{
				// Get all member results
				foreach (var location in memory.Keys)
				{
					var locationEntries = memory[location];

					var locationResults = locationEntries
						.Where(x => x.PersistedFaceId == persistentMemberId)
						.Where(x => x.Timestamp.Ticks > fromDate.Ticks && x.Timestamp.Ticks < toDate.Ticks)
						.OrderBy(x => x.Timestamp)
						.ToList();

					if (locationResults.Count > 1)
					{
						Member prevEntry;
						Member curEntry;
						List<Member> windowEntries = new List<Member>();
						windowEntries.Add(locationResults[0]);

						for (int i = 1; i < locationResults.Count; i++)
						{
							prevEntry = locationResults[i - 1];
							curEntry = locationResults[i];

							var span = curEntry.Timestamp.Subtract(prevEntry.Timestamp);
							if (span.Subtract(minimumDuration) > TimeSpan.Zero)
							{
								// We have an entry
								windowEntries.Add(curEntry);
							}

							// Create a new LocationWindow
							else
							{
								results.Add(LocationWindow.Create(location, windowEntries));

								// Reset the window entries
								windowEntries = new List<Member>();
								windowEntries.Add(curEntry);
							}
						}
					}

					else
					{
						results.Add(LocationWindow.Create(location, locationResults));
					}
				}
			}

			var orderedResults = results.OrderBy(x => x.StartDate).ToList();
			return Task.FromResult<List<LocationWindow>>(orderedResults);
		}

		public Task<IDictionary<string, List<Member>>> UniqueMembersTimeSpan(string[] locations, DateTime fromDate, DateTime toDate, CancellationToken cancellationToken)
		{
			if (locations == null)
				throw new ArgumentNullException(nameof(locations));

			var diff = toDate.Subtract(fromDate);
			if (diff < TimeSpan.Zero)
				throw new ArgumentException(nameof(fromDate));

			var results = new Dictionary<string, List<Member>>(locations.Length);

			lock (_syncroot)
			{
				foreach (var key in locations)
				{
					if (memory.TryGetValue(key, out var list))
					{
						if (list != null)
						{
							results[key] = list
							.Where(x => x.Timestamp.Ticks > fromDate.Ticks && x.Timestamp.Ticks < toDate.Ticks)
							.Where(x => x.PersistedFaceId != null) // todo filter results so that we dont have duplicates?
							.ToList();
						}
					}
				}
			}

			return Task.FromResult<IDictionary<string, List<Member>>>(results);
		}
	}
}