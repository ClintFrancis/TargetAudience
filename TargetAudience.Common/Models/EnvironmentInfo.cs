using System;
using Newtonsoft.Json;

namespace TargetAudience.Common.Models
{
	public class EnvironmentInfo
	{
		[JsonProperty("time")]
		public string Time { get; set; }

		[JsonProperty("weather")]
		public string Weather { get; set; }

		[JsonProperty("location")]
		public string Location { get; set; }

		[JsonProperty("storeLocation")]
		public string StoreLocation { get; set; }

		[JsonProperty("externalEvents")]
		public ExternalEvent[] ExternalEvents { get; set; }
	}
}
