using System;
using Newtonsoft.Json;

namespace TargetAudience.Common.Models
{
	public class Media
	{
		[JsonProperty("mediaType")]
		public string MediaType { get; set; }

		[JsonProperty("mediaUrl")]
		public Uri MediaUrl { get; set; }
	}
}
