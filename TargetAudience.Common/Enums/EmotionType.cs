using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TargetAudience.Common.Enums
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum EmotionType
	{
		Anger,
		Contempt,
		Disgust,
		Fear,
		Happiness,
		Neutral,
		Sadness,
		Surprise
	}
}
