﻿using System;
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
		Task<List<Member>> QueryTimeSpan(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken);
		Task<List<Member>> QueryTimeSpan(string[] locations, DateTime fromDate, DateTime toDate, CancellationToken cancellationToken);
		Task<List<LocationWindow>> QueryMemberLocations(string persistentMemberId, DateTime fromDate, DateTime toDate, TimeSpan minimumDuration, CancellationToken cancellationToken);
	}
}
