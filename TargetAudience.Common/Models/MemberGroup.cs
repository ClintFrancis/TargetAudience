using Newtonsoft.Json;
using TargetAudience.Common.Enums;

namespace TargetAudience.Common.Models
{
	public class MemberGroup
	{
		[JsonProperty("averageAge")]
		public double AverageAge { get; set; } = 0;

		[JsonProperty("averageSmile")]
		public double AverageSmile { get; set; } = 0;

		[JsonProperty("averageFacialHair")]
		public double AverageFacialHair { get; set; } = 0;

		[JsonProperty("averageGlasses")]
		public double AverageGlasses { get; set; } = 0;

		[JsonProperty("averageMakeup")]
		public double AverageMakeup { get; set; } = 0;

		[JsonProperty("averageEmotion")]
		public string AverageEmotion { get; set; } = "";

		[JsonProperty("individuals")]
		public Member[] Individuals { get; set; }

		[JsonProperty("gender")]
		public GenderType Gender { get; set; }

		[JsonProperty("total")]
		public int Total
		{
			get
			{
				return (Individuals != null) ? Individuals.Length : 0;
			}
		}

		public MemberGroup(GenderType gender)
		{
			Gender = gender;
		}
	}
}
