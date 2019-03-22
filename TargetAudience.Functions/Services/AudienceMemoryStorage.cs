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
		List<Member> memoryList;


		public AudienceMemoryStorage(Dictionary<string, List<Member>> dictionary = null)
		{
			memory = dictionary ?? new Dictionary<string, List<Member>>();
			memoryList = new List<Member>();
		}

		/// <summary>
		/// Deletes storage items from storage.
		/// </summary>
		/// <returns>The async.</returns>
		/// <param name="locations">Locations.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		public Task<int> DeleteAsync(string[] locations, CancellationToken cancellationToken)
		{
			if (locations == null)
				throw new ArgumentNullException(nameof(locations));

			int count = 0;
			lock (_syncroot)
			{
				count = memoryList.RemoveAll(x => locations.Contains(x.Location));
			}

			return Task.FromResult(count);
		}

		/// <summary>
		/// Reads storage items from storage.
		/// </summary>
		/// <returns>The async.</returns>
		/// <param name="locations">Locations.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		public Task<List<Member>> ReadAsync(string[] locations, CancellationToken cancellationToken, int maxItemCount = -1)
		{
			if (locations == null)
				throw new ArgumentNullException(nameof(locations));

			var items = new List<Member>();
			if (maxItemCount == -1)
				maxItemCount = 999;

			lock (_syncroot)
			{
				items = memoryList.Where(x => locations.Contains(x.Location)).Select(x => x).Take(maxItemCount).ToList();
			}

			return Task.FromResult<List<Member>>(items);
		}

		/// <summary>
		/// Writes storage items to storage.
		/// </summary>
		/// <returns>The async.</returns>
		/// <param name="changes">Changes.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		public Task<List<Member>> WriteAsync(List<Member> changes, CancellationToken cancellationToken)
		{
			if (changes == null)
			{
				throw new ArgumentNullException(nameof(changes));
			}

			lock (_syncroot)
			{
				// TODO consider overwrite option?
				memoryList.AddRange(changes);
			}

			return Task.FromResult(changes);
		}

		// TODO Depreciate?
		public Task<List<Member>> WriteAsync(string location, List<Member> changes, CancellationToken cancellationToken)
		{
			if (string.IsNullOrEmpty(location))
				throw new ArgumentException("Location value cannot be null");

			if (changes == null)
				throw new ArgumentNullException(nameof(changes));

			lock (_syncroot)
			{
				foreach (var item in changes)
					item.Location = location;

				memoryList.AddRange(changes);
			}

			return Task.FromResult(changes);
		}

		/// <summary>
		/// Queries the user statistics over a given time period
		/// </summary>
		/// <returns>The time span.</returns>
		/// <param name="locations">Locations.</param>
		/// <param name="fromDate">From date.</param>
		/// <param name="toDate">To date.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		public Task<List<Member>> QueryTimeSpan(string[] locations, DateTime fromDate, DateTime toDate, CancellationToken cancellationToken)
		{
			if (locations == null)
				throw new ArgumentNullException(nameof(locations));

			var diff = toDate.Subtract(fromDate);
			if (diff < TimeSpan.Zero)
				throw new ArgumentException(nameof(fromDate));

			var results = new List<Member>();

			lock (_syncroot)
			{
				results = memoryList.Where(x => locations.Contains(x.Location))
					.Where(x => x.Timestamp.Ticks > fromDate.Ticks && x.Timestamp.Ticks < toDate.Ticks)
					.ToList();
			}

			return Task.FromResult<List<Member>>(results);
		}

		public Task<List<Member>> QueryTimeSpan(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken)
		{
			var results = new List<Member>();

			lock (_syncroot)
			{
				results = memoryList.Where(x => x.Timestamp.Ticks > fromDate.Ticks && x.Timestamp.Ticks < toDate.Ticks).ToList();
			}

			return Task.FromResult<List<Member>>(results);
		}

		public Task<List<LocationWindow>> QueryMemberLocations(string persistentMemberId, DateTime fromDate, DateTime toDate, TimeSpan minimumDuration, CancellationToken cancellationToken)
		{
			var results = new List<LocationWindow>();
			lock (_syncroot)
			{
				// Find member results
				var memberResults = memoryList
						.Where(x => x.PersistedFaceId == persistentMemberId)
						.Where(x => x.Timestamp.Ticks > fromDate.Ticks && x.Timestamp.Ticks < toDate.Ticks)
						.OrderBy(x => x.Timestamp)
						.ToList();

				results = memberResults.Select(x => x.Location).Distinct().Select(x => new LocationWindow() { Location = x }).ToList();

				Member prevEntry;
				Member curEntry;
				List<Member> memberLocationResults;

				// Populate each LocationWindow
				foreach (var window in results)
				{
					memberLocationResults = memberResults
						.Where(x => x.Location == window.Location)
						.ToList();

					List<Member> filteredWindowEntries = new List<Member>();
					filteredWindowEntries.Add(memberLocationResults[0]);

					if (memberLocationResults.Count() > 1)
					{
						for (int i = 1; i < memberLocationResults.Count(); i++)
						{
							// Compare member results to determine if the minimum duration has been met
							prevEntry = memberLocationResults[i - 1];
							curEntry = memberLocationResults[i];

							var span = curEntry.Timestamp.Subtract(prevEntry.Timestamp);
							if (span.Subtract(minimumDuration) > TimeSpan.Zero)
							{
								// We have a verified entry
								filteredWindowEntries.Add(curEntry);
							}
						}
					}

					window.AddMembers(filteredWindowEntries);
				}
			}

			var orderedResults = results.OrderBy(x => x.StartDate).ToList();
			return Task.FromResult<List<LocationWindow>>(orderedResults);
		}

		public Task<List<Member>> UniqueMembersTimeSpan(string[] locations, DateTime fromDate, DateTime toDate, CancellationToken cancellationToken)
		{
			if (locations == null)
				throw new ArgumentNullException(nameof(locations));

			var diff = toDate.Subtract(fromDate);
			if (diff < TimeSpan.Zero)
				throw new ArgumentException(nameof(fromDate));

			var results = new List<Member>();

			lock (_syncroot)
			{
				results = memoryList
					.Where(x => locations.Contains(x.Location))
					.Where(x => x.Timestamp.Ticks > fromDate.Ticks && x.Timestamp.Ticks < toDate.Ticks)
					.Where(x => x.PersistedFaceId != null)
					.Distinct()
					.ToList();
			}

			return Task.FromResult<List<Member>>(results);
		}
	}
}
