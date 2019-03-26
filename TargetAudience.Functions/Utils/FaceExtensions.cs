using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using TargetAudience.Common.Enums;
using TargetAudience.Common.Models;

namespace TargetAudience.Functions.Utils
{
	public static class FaceExtensions
	{
		public static double Size(this FaceRectangle rect)
		{
			return rect.Width * rect.Height;
		}

		public static Rectangle ToRectangle(this FaceRectangle rect)
		{
			return new Rectangle(rect.Left, rect.Top, rect.Width, rect.Height);
		}

		public static double Average(this FacialHair facialHair)
		{
			return Math.Round((facialHair.Beard + facialHair.Moustache) / 2, 2);
		}

		public static double Average(this GlassesType? value)
		{
			return (value != null && value != GlassesType.NoGlasses) ? 1 : 0;
		}

		public static double Average(this Makeup makeup)
		{
			return (makeup.EyeMakeup || makeup.LipMakeup) ? 1 : 0;
		}

		public static GenderType ToGenderType(this Gender? gender)
		{
			if (gender == Gender.Male)
				return GenderType.Male;

			if (gender == Gender.Female)
				return GenderType.Female;

			return GenderType.None;
		}
	}
}
