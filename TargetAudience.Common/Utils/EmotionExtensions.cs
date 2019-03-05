using System;
using System.Collections.Generic;
using System.Linq;
using TargetAudience.Common.Enums;
using TargetAudience.Common.Models;

namespace TargetAudience.Common.Utils
{
	public static class EmotionExtensions
	{
		public static EmotionType Parse(this EmotionValues emotion)
		{
			var emotions = new List<(EmotionType type, double value)>()
			{
				(EmotionType.Anger, emotion.Anger),
				(EmotionType.Contempt, emotion.Contempt),
				(EmotionType.Disgust, emotion.Disgust),
				(EmotionType.Fear, emotion.Fear),
				(EmotionType.Happiness, emotion.Happiness),
				(EmotionType.Neutral, emotion.Neutral),
				(EmotionType.Sadness, emotion.Sadness),
				(EmotionType.Surprise, emotion.Surprise)
			};

			return emotions.OrderByDescending(x => x.value).First().type;
		}

		public static EmotionValues AverageEmotions(this IEnumerable<EmotionValues> collection)
		{
			var averageEmotion = new EmotionValues();

			averageEmotion.Anger = collection.Average(x => x.Anger);
			averageEmotion.Contempt = collection.Average(x => x.Contempt);
			averageEmotion.Disgust = collection.Average(x => x.Disgust);
			averageEmotion.Fear = collection.Average(x => x.Fear);
			averageEmotion.Happiness = collection.Average(x => x.Happiness);
			averageEmotion.Neutral = collection.Average(x => x.Neutral);
			averageEmotion.Sadness = collection.Average(x => x.Sadness);
			averageEmotion.Surprise = collection.Average(x => x.Surprise);

			return averageEmotion;
		}

		public static EmotionType PrimaryEmotion(this IEnumerable<EmotionType> emotions)
		{
			var collection = new Dictionary<EmotionType, int>
			{
				{EmotionType.Anger, 0},
				{EmotionType.Contempt, 0},
				{EmotionType.Disgust, 0},
				{EmotionType.Fear, 0},
				{EmotionType.Happiness, 0},
				{EmotionType.Neutral, 0},
				{EmotionType.Sadness, 0},
				{EmotionType.Surprise, 0}
			};

			foreach (var item in emotions)
				collection[item] += 1;

			return collection.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
		}
	}
}
