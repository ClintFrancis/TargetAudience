using System;
using TargetAudience.Common.Models;

namespace TargetAudienceClient.Models
{
	public class AudienceData
	{
		public string Location { get; set; }
		public byte[] ImageData { get; set; }
		public Audience Audience { get; set; }

		public AudienceData()
		{
		}
	}
}
