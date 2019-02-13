using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using TargetAudience.Common.Models;
using TargetAudience.Functions.Utils;

namespace TargetAudience.Functions.Services
{
	public class FaceService
	{
		private static FaceClient _client;
		public static FaceClient Client
		{
			get
			{
				_client = _client ?? new FaceClient(
			new ApiKeyServiceClientCredentials(FaceApiKey),
			new System.Net.Http.DelegatingHandler[] { })
				{ Endpoint = FaceApiRegion };

				return _client;
			}
		}

		static string FaceApiKey = Environment.GetEnvironmentVariable("FaceApiKey");
		static string FaceApiRegion = Environment.GetEnvironmentVariable("FaceApiRegion");

		const string LoyalCustomerGroup = "loyal_customers";
		const string AnonymousCustomerGroup = "anonymous_customers";

		public FaceService()
		{

		}

		/// <summary>
		/// Detects the audience makeup from an image
		/// </summary>
		/// <returns>The faces.</returns>
		/// <param name="imageFileStream">Image file stream.</param>
		public async Task<List<Member>> DetectMembers(Stream imageFileStream, DateTime timestamp)
		{
			// The list of Face attributes to return.
			IList<FaceAttributeType> faceAttributes =
				new FaceAttributeType[]
				{
					FaceAttributeType.Age,
					FaceAttributeType.Emotion,
					FaceAttributeType.Gender,
					FaceAttributeType.FacialHair,
					FaceAttributeType.Glasses,
					FaceAttributeType.Makeup,
					FaceAttributeType.Smile,
				};

			try
			{
				var faces = await Client.Face.DetectWithStreamAsync(imageFileStream, true, false, faceAttributes);

				// Parse Individuals
				var individuals = new List<Member>();
				foreach (var item in faces)
				{
					var member = item.ToMember();
					member.Timestamp = timestamp;
					individuals.Add(member);
				}

				return individuals;
			}

			// Catch and display Face API errors.
			catch (APIErrorException f)
			{
				return null;
			}
			// Catch and display all other errors.
			catch (Exception e)
			{
				return null;
			}
		}
	}
}