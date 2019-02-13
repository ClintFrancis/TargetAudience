using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using TargetAudience.Common.Enums;
using TargetAudience.Common.Utils;

namespace TargetAudience.Common.Models
{
	public class MemberHistory
	{
		[JsonProperty("count")]
		public int Count { get; set; }

		[JsonProperty("span")]
		public TimeSpan Span { get; set; }

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

		[JsonProperty("makeup")]
		public EmotionMarker[] Emotions { get; set; }

		public MemberHistory(List<Member> history)
		{
			if (history == null || history.Count > 2)
				throw new ArgumentException(nameof(history));

			var member = history[0];
			FaceId = member.FaceId;
			Gender = member.Gender;
			Age = member.Age;
			Smile = member.Smile;
			FacialHair = member.FacialHair;
			Glasses = member.Glasses;
			Makeup = member.Makeup;
			Count = history.Count;

			// Span
			var firstSighting = history[0].Timestamp;
			var lastSighting = history[history.Count - 1].Timestamp;
			Span = lastSighting.Subtract(firstSighting);

			// Emotions
			Emotions = history.Select(x => new EmotionMarker() { Emotion = x.Emotion.Parse(), Timestamp = x.Timestamp }).ToArray();
		}
	}

	public class EmotionMarker
	{
		[JsonProperty("emotion")]
		public EmotionType Emotion { get; set; }

		[JsonProperty("timestamp")]
		public DateTime Timestamp { get; set; }
	}
}
