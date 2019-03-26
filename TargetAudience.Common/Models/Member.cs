using System;
using Newtonsoft.Json;
using TargetAudience.Common.Enums;

namespace TargetAudience.Common.Models
{
	public class Member : IEquatable<Member>
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


		public bool Equals(Member other)
		{
			////Check whether the compared object is null. 
			//if (Object.ReferenceEquals(other, null)) return false;

			////Check whether the compared object references the same data. 
			//if (Object.ReferenceEquals(this, other)) return true;

			//Check whether the PersistedFaceID properties are equal. 
			return PersistedFaceId.Equals(other.PersistedFaceId);
		}

		public override string ToString()
		{
			return JsonConvert.SerializeObject(this);
		}
	}
}
