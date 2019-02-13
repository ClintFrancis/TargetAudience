using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using TargetAudience.Common.Enums;

namespace TargetAudience.Common.Models
{
	public class Audience
	{
		[JsonProperty("total")]
		public double Total { get; set; }

		[JsonProperty("averageGender")]
		public GenderType AverageGender { get; set; }

		[JsonProperty("averageAge")]
		public double AverageAge { get; set; }

		[JsonProperty("male")]
		public MemberGroup Male { get; set; }

		[JsonProperty("female")]
		public MemberGroup Female { get; set; }

		public List<Member> GetAllIndividuals()
		{
			var allIndividuals = new List<Member>(Male.Individuals);
			allIndividuals.AddRange(Female.Individuals);
			return allIndividuals;
		}
	}
}
