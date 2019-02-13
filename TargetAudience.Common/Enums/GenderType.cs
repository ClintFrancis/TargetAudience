using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TargetAudience.Common.Enums
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum GenderType
	{
		Male,
		Female,
		Genderless
	}
}
