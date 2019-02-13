using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using TargetAudience.Common.Enums;
using TargetAudience.Common.Models;
using TargetAudience.Common.Utils;

namespace TargetAudience.Functions.Utils
{
	public static class MemberUtils
	{
		public static Member ToMember(this DetectedFace value)
		{
			var attribs = value.FaceAttributes;
			var member = new Member(value.FaceId.ToString())
			{
				Gender = attribs.Gender.ToGenderType(),
				Age = (double)attribs.Age,
				Smile = (double)attribs.Smile,
				FacialHair = attribs.FacialHair.Average(),
				Glasses = attribs.Glasses.Average(),
				Makeup = attribs.Makeup.Average(),
				Emotion = new EmotionValues
				(
					attribs.Emotion.Anger,
					attribs.Emotion.Contempt,
					attribs.Emotion.Disgust,
					attribs.Emotion.Fear,
					attribs.Emotion.Happiness,
					attribs.Emotion.Neutral,
					attribs.Emotion.Sadness,
					attribs.Emotion.Surprise
				)
			};

			return member;
		}

		public static Audience CreateAudience(IEnumerable<Member> members)
		{
			if (members == null)
				return null;

			var result = new Audience();

			result.Male = CreateGroup(GenderType.Male, members);
			result.Female = CreateGroup(GenderType.Female, members);
			result.Total = result.Male.Total + result.Female.Total;

			var averageAge = (result.Male.AverageAge + result.Female.AverageAge);
			if (result.Male.Total > 0 && result.Female.Total > 0)
				averageAge *= .5;

			result.AverageAge = Math.Round(averageAge);
			result.AverageGender = (result.Male.Total > result.Female.Total) ? GenderType.Male : GenderType.Female;

			return result;
		}

		public static MemberGroup CreateGroup(GenderType gender, IEnumerable<Member> members)
		{
			var groupMembers = members.Where(x => x.Gender == gender);

			var result = new MemberGroup(gender);
			if (groupMembers.Count() == 0)
				return result;

			result.Individuals = groupMembers.ToArray();

			// Parse Averages
			var emotions = groupMembers.Select(x => x.Emotion).AverageEmotions();
			result.AverageEmotion = emotions.Parse().ToString();
			result.AverageAge = Math.Round(groupMembers.Select(x => (double)x.Age).Average(), 2);
			result.AverageFacialHair = Math.Round(groupMembers.Select(x => x.FacialHair).Average(), 2);
			result.AverageGlasses = Math.Round(groupMembers.Select(x => x.Glasses).Average(), 2);
			result.AverageMakeup = Math.Round(groupMembers.Select(x => x.Makeup).Average(), 2);
			result.AverageSmile = Math.Round(groupMembers.Select(x => x.Smile).Average(), 2);

			return result;
		}
	}
}
