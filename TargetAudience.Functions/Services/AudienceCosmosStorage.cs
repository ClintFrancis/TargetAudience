using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using TargetAudience.Common.Interfaces;
using TargetAudience.Common.Models;

namespace TargetAudience.Functions.Services
{
	public class AudienceCosmosStorage : IAudienceStorageService
	{
		internal DocumentClient Client { get; private set; }
		DocumentCollection audienceCollection;
		bool initialized;
		string databaseName;
		string collectionName;

		public AudienceCosmosStorage()
		{

		}

		public async Task CheckInitialized()
		{
			if (initialized)
				return;

			var endpoint = Environment.GetEnvironmentVariable("DocumentEndpointUri");
			var key = Environment.GetEnvironmentVariable("DocumentAuthKey");
			databaseName = Environment.GetEnvironmentVariable("DocumentDatabaseName");
			collectionName = Environment.GetEnvironmentVariable("DocumentCollectionName");

			if (String.IsNullOrEmpty(endpoint))
			{
				throw new MissingFieldException("A DocumentEndpointUri string has not been defined in the system environment variables. " +
					"Add an environment variable named 'DocumentEndpointUri' with your " +
					"connection string as a value.");
			}
			if (String.IsNullOrEmpty(key))
			{
				throw new MissingFieldException(
					"A DocumentAuthKey string has not been defined in the system environment variables. " +
					"Add an environment variable named 'DocumentAuthKey' with your" +
					"Auth Key string as a value.");
			}
			if (String.IsNullOrEmpty(databaseName))
			{
				throw new MissingFieldException(
					"A DatabaseId string has not been defined in the system environment variables. " +
					"Add an environment variable named 'DatabaseId' with your" +
					"Database Id string as a value.");
			}

			if (String.IsNullOrEmpty(collectionName))
			{
				throw new MissingFieldException(
					"A CollectionId string has not been defined in the system environment variables. " +
					"Add an environment variable named 'CollectionId' with your" +
					"Collection Id string as a value.");
			}

			Client = new DocumentClient(new Uri(endpoint), key);

			// Create the Database if required
			await Client.CreateDatabaseIfNotExistsAsync(new Database { Id = databaseName });

			// Get a reference to the Document Collection
			var response = await Client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(databaseName), new DocumentCollection { Id = collectionName });
			audienceCollection = response.Resource;

			initialized = true;
		}

		/// <summary>
		/// Deletes storage items from storage.
		/// </summary>
		/// <returns>The async.</returns>
		/// <param name="locations">Locations.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		public async Task<int> DeleteAsync(string[] locations, CancellationToken cancellationToken)
		{
			await CheckInitialized();

			if (locations == null)
				throw new ArgumentNullException(nameof(locations));

			var results = Client.CreateDocumentQuery<Member>(audienceCollection.SelfLink)
					.Where(x => locations.Contains(x.Location))
					.ToList();

			int deleted = 0;
			foreach (var item in results)
			{
				var result = await Client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, item.Id));
				deleted++;
			}

			return deleted;
		}

		// todo delete within timeframe DateTime fromDate, DateTime toDate, 

		/// <summary>
		/// Reads storage items from storage.
		/// </summary>
		/// <returns>The async.</returns>
		/// <param name="locations">Locations.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		public async Task<List<Member>> ReadAsync(string[] locations, CancellationToken cancellationToken, int maxItemCount = -1)
		{
			await CheckInitialized();

			if (locations == null)
				throw new ArgumentNullException(nameof(locations));

			var items = new List<Member>();
			FeedOptions queryOptions = (maxItemCount > 0) ? new FeedOptions { MaxItemCount = maxItemCount } : null;
			items = Client.CreateDocumentQuery<Member>(audienceCollection.SelfLink, queryOptions)
					.Where(x => locations.Contains(x.Location))
					.ToList();

			return items;
		}

		/// <summary>
		/// Writes storage items to storage.
		/// </summary>
		/// <returns>The async.</returns>
		/// <param name="changes">Changes.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		public async Task<List<Member>> WriteAsync(List<Member> changes, CancellationToken cancellationToken)
		{
			await CheckInitialized();

			if (changes == null)
				throw new ArgumentNullException(nameof(changes));

			var response = new List<Member>();
			foreach (var item in changes)
			{
				var result = await Client.CreateDocumentAsync(audienceCollection.SelfLink, item);
				item.Id = result.Resource.Id;
				response.Add(item);
			}

			return response;
		}

		public async Task<List<Member>> WriteAsync(string location, List<Member> changes, CancellationToken cancellationToken)
		{
			await CheckInitialized();

			if (string.IsNullOrEmpty(location))
				throw new ArgumentException("Location value cannot be null");

			if (changes == null)
				throw new ArgumentNullException(nameof(changes));

			var response = new List<Member>();
			try
			{
				foreach (var item in changes)
				{
					item.Location = location;
					var result = await Client.UpsertDocumentAsync(audienceCollection.SelfLink, item);
					item.Id = result.Resource.Id;
					response.Add(item);
				}
			}
			catch (DocumentClientException de)
			{
				Console.WriteLine(de.Message);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			return response;
		}

		/// <summary>
		/// Queries the user statistics over a given time period
		/// </summary>
		/// <returns>The time span.</returns>
		/// <param name="locations">Locations.</param>
		/// <param name="fromDate">From date.</param>
		/// <param name="toDate">To date.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		public async Task<List<Member>> QueryTimeSpan(string[] locations, DateTime fromDate, DateTime toDate, CancellationToken cancellationToken)
		{
			await CheckInitialized();
			throw new NotImplementedException();

			if (locations == null)
				throw new ArgumentNullException(nameof(locations));

			var diff = toDate.Subtract(fromDate);
			if (diff < TimeSpan.Zero)
				throw new ArgumentException(nameof(fromDate));

			var results = new List<Member>();

			//results = memoryList.Where(x => locations.Contains(x.Location))
			//.Where(x => x.Timestamp.Ticks > fromDate.Ticks && x.Timestamp.Ticks < toDate.Ticks)
			//.ToList();


			return results;
		}

		public async Task<List<Member>> QueryTimeSpan(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken)
		{
			await CheckInitialized();
			throw new NotImplementedException();

			var results = new List<Member>();

			//results = memoryList.Where(x => x.Timestamp.Ticks > fromDate.Ticks && x.Timestamp.Ticks < toDate.Ticks).ToList();

			return results;
		}

		public async Task<List<LocationWindow>> QueryMemberLocations(string persistentMemberId, DateTime fromDate, DateTime toDate, TimeSpan minimumDuration, CancellationToken cancellationToken)
		{
			await CheckInitialized();
			throw new NotImplementedException();

			var results = new List<LocationWindow>();
			/*
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
			*/

			var orderedResults = results.OrderBy(x => x.StartDate).ToList();
			return orderedResults;
		}

		public async Task<List<Member>> UniqueMembersTimeSpan(string[] locations, DateTime fromDate, DateTime toDate, CancellationToken cancellationToken)
		{
			await CheckInitialized();
			throw new NotImplementedException();

			if (locations == null)
				throw new ArgumentNullException(nameof(locations));

			var diff = toDate.Subtract(fromDate);
			if (diff < TimeSpan.Zero)
				throw new ArgumentException(nameof(fromDate));

			var results = new List<Member>();


			//results = memoryList
			//.Where(x => locations.Contains(x.Location))
			//.Where(x => x.Timestamp.Ticks > fromDate.Ticks && x.Timestamp.Ticks < toDate.Ticks)
			//.Where(x => x.PersistedFaceId != null)
			//.Distinct()
			//.ToList();

			return results;
		}
	}
}
