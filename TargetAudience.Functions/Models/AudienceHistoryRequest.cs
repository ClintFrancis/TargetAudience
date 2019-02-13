using System;
using Newtonsoft.Json;
using TargetAudience.Common.Models;

namespace TargetAudience.Functions.Models
{
	public class AudienceHistoryRequest
	{
		[JsonProperty("minutes")]
		public int Minutes { get; set; }

		[JsonProperty("locations")]
		public string[] Locations { get; set; }
	}
}
