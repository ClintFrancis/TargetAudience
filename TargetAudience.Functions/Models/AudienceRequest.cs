using System;
using Newtonsoft.Json;

namespace TargetAudience.Functions.Models
{
	public class AudienceRequest
	{
		[JsonProperty("location")]
		public string Location { get; set; }

		[JsonProperty("image")]
		public string Image { get; set; }
	}
}
