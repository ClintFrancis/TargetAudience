using System;
using Newtonsoft.Json;
using TargetAudience.Common.Enums;

namespace TargetAudience.Common.Models
{
	public class EmotionValues
	{
		[JsonProperty("anger")]
		public double Anger { get; set; }

		[JsonProperty("contempt")]
		public double Contempt { get; set; }

		[JsonProperty("disgust")]
		public double Disgust { get; set; }

		[JsonProperty("fear")]
		public double Fear { get; set; }

		[JsonProperty("happiness")]
		public double Happiness { get; set; }

		[JsonProperty("neutral")]
		public double Neutral { get; set; }

		[JsonProperty("sadness")]
		public double Sadness { get; set; }

		[JsonProperty("surprise")]
		public double Surprise { get; set; }

		/// <summary>
		/// Initializes a new instance of the EmotionData class.
		/// </summary>
		public EmotionValues() { }

		/// <summary>
		/// Initializes a new instance of the EmotionData class.
		/// </summary>
		public EmotionValues(double anger = 0.0, double contempt = 0.0, double disgust = 0.0, double fear = 0.0, double happiness = 0.0, double neutral = 0.0, double sadness = 0.0, double surprise = 0.0)
		{
			Anger = anger;
			Contempt = contempt;
			Disgust = disgust;
			Fear = fear;
			Happiness = happiness;
			Neutral = neutral;
			Sadness = sadness;
			Surprise = surprise;
		}
	}
}