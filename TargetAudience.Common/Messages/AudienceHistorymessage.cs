using System;
using Newtonsoft.Json;

namespace TargetAudience.Common.Messages
{
	public class AudienceHistoryRequest
	{
		[JsonProperty("minutes")]
		public int Minutes { get; set; }

		[JsonProperty("startDate")]
		public DateTime StartDate { get; set; }

		[JsonProperty("endDate")]
		public DateTime EndDate { get; set; }

		[JsonProperty("uniqueMembersOnly")]
		public bool UniqueMembersOnly { get; set; }

		[JsonProperty("locations")]
		public string[] Locations { get; set; }
	}
}
