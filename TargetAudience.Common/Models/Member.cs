using System;
using Newtonsoft.Json;
using TargetAudience.Common.Enums;

namespace TargetAudience.Common.Models
{
	public class Member
	{
		[JsonProperty("gender")]
		public GenderType Gender { get; set; }

		[JsonProperty("age")]
		public double Age { get; set; }

		[JsonProperty("faceId")]
		public string FaceId { get; set; }

		[JsonProperty("smile")]
		public double Smile { get; set; } = 0;

		[JsonProperty("facialHair")]
		public double FacialHair { get; set; } = 0;

		[JsonProperty("glasses")]
		public double Glasses { get; set; } = 0;

		[JsonProperty("makeup")]
		public double Makeup { get; set; } = 0;

		[JsonProperty("emotion")]
		public EmotionValues Emotion { get; set; }

		[JsonProperty("timestamp")]
		public DateTime Timestamp { get; set; }

		public Member(string id)
		{
			FaceId = id;
		}
	}
}
