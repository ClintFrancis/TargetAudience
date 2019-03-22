using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace TargetAudience.Common.Models
{
	public class LocationWindow
	{
		[JsonProperty("location")]
		public string Location { get; set; }

		[JsonProperty("startDate")]
		public DateTime StartDate { get; set; }

		[JsonProperty("endDate")]
		public DateTime EndDate { get; set; }

		[JsonProperty("members")]
		public Member[] Members { get; set; }

		public void AddMembers(IEnumerable<Member> members)
		{
			if (members.Count() < 1)
				throw new ArgumentOutOfRangeException("'members' must contain at least one entry");

			Members = members.OrderBy(x => x.Timestamp).ToArray();
			StartDate = Members.FirstOrDefault().Timestamp;
			EndDate = Members.LastOrDefault().Timestamp;
		}
	}
}
