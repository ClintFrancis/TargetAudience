using System;
using Newtonsoft.Json;

namespace TargetAudience.Common.Models
{
	public class ExternalEvent
	{
		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("tags")]
		public string Tags { get; set; }
	}
}
