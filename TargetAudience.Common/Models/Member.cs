using System;
using Newtonsoft.Json;
using TargetAudience.Common.Enums;

namespace TargetAudience.Common.Models
{
	public class Member
	{
		[JsonProperty(PropertyName = "id")]
		public string Id { get; set; }

		[JsonProperty("gender")]
		public GenderType Gender { get; set; }

		[JsonProperty("age")]
		public double Age { get; set; }

		[JsonProperty("faceId")]
		public string FaceId { get; set; }

		[JsonProperty("persistedFaceId")]
		public string PersistedFaceId { get; set; }

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

		[JsonProperty("location")]
		public string Location { get; set; }

		public override string ToString()
		{
			return JsonConvert.SerializeObject(this);
		}
	}
}
