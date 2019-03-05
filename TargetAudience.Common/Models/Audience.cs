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

		[JsonProperty("males")]
		public MemberGroup Males { get; set; }

		[JsonProperty("females")]
		public MemberGroup Females { get; set; }

		public List<Member> GetAllIndividuals()
		{
			var allIndividuals = new List<Member>();
			if (Males != null) allIndividuals.AddRange(Males.Individuals);
			if (Females != null) allIndividuals.AddRange(Females.Individuals);
			return allIndividuals;
		}
	}
}
