using System;
using Newtonsoft.Json;

namespace TargetAudience.Common.Messages
{
	public class LocationResponse : BaseResponse
	{
		[JsonProperty("locations")]
		public string[] Locations { get; set; }
	}
}

