using System;
using Newtonsoft.Json;
using TargetAudience.Common.Models;

namespace TargetAudience.Common.Messages
{
	public class AudienceRequest
	{
		[JsonProperty("location")]
		public string Location { get; set; }

		[JsonProperty("image")]
		public string Image { get; set; }
	}

	public class AudienceResponse : BaseResponse
	{
		[JsonProperty("audience")]
		public Audience Audience { get; set; }
	}
}
