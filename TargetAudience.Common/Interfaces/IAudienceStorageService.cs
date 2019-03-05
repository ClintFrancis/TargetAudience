using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TargetAudience.Common.Models;

namespace TargetAudience.Common.Interfaces
{
	public interface IAudienceStorageService
	{
		Task DeleteAsync(string[] locations, CancellationToken cancellationToken);
		Task<IDictionary<string, List<Member>>> ReadAsync(string[] locations, CancellationToken cancellationToken);
		Task WriteAsync(IDictionary<string, List<Member>> changes, CancellationToken cancellationToken);
		Task WriteAsync(string location, List<Member> changes, CancellationToken cancellationToken);
		Task<IDictionary<string, List<Member>>> QueryTimeSpan(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken);
		Task<IDictionary<string, List<Member>>> QueryTimeSpan(string[] locations, DateTime fromDate, DateTime toDate, CancellationToken cancellationToken);
		Task<List<LocationWindow>> QueryMemberLocations(string persistentMemberId, DateTime fromDate, DateTime toDate, TimeSpan minimumDuration, CancellationToken cancellationToken);
	}
}
