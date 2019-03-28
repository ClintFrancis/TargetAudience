using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TargetAudience.Common.Models;

namespace TargetAudience.Common.Interfaces
{
	public interface IAudienceStorageService
	{
		Task<int> DeleteAsync(string[] locations, CancellationToken cancellationToken);
		Task<List<Member>> ReadAsync(string[] locations, CancellationToken cancellationToken, int maxItemCount = -1);
		Task<List<Member>> WriteAsync(List<Member> changes, CancellationToken cancellationToken);
		Task<List<Member>> WriteAsync(string location, List<Member> changes, CancellationToken cancellationToken);
		Task<List<Member>> QueryTimeSpan(DateTime startDate, DateTime endDate, CancellationToken cancellationToken, int maxItemCount = -1);
		Task<List<Member>> QueryTimeSpan(DateTime startDate, DateTime endDate, string[] locations, CancellationToken cancellationToken, int maxItemCount = -1);
		Task<List<LocationWindow>> QueryMemberLocations(string persistentMemberId, DateTime startDate, DateTime endDate, TimeSpan minimumDuration, CancellationToken cancellationToken);
		Task<List<Member>> UniqueMembersTimeSpan(DateTime startDate, DateTime endDate, string[] locations, CancellationToken cancellationToken, int maxItemCount = -1);
		Task<string[]> GetLocations();
	}
}
