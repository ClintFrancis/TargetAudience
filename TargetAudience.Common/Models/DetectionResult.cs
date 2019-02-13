using System;
using Newtonsoft.Json;

namespace TargetAudience.Common.Models
{
	public class DetectionResult
	{
		[JsonProperty("environment")]
		public EnvironmentInfo Environment { get; set; }

		[JsonProperty("shoppers")]
		public Audience Shoppers { get; set; }

		[JsonProperty("media")]
		public Media Media { get; set; }
	}
}
