using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using TargetAudience.Common.Models;
using TargetAudience.Functions.Utils;

namespace TargetAudience.Functions.Services
{
	public static partial class FaceService
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

		const string AnonymousCustomerGroup = "anonymous_customers";
		const string ImageContainerName = "captures";

		// The list of Face attributes to return.
		static IList<FaceAttributeType> DefaultFaceAttributeTypes =
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

		/// <summary>
		/// Detects the audience makeup from an image
		/// </summary>
		/// <returns>The faces.</returns>
		/// <param name="imageStream">Image file stream.</param>
		public static async Task<List<Member>> DetectMembers(Stream imageStream, DateTime timestamp)
		{
			try
			{
				var faces = await Client.Face.DetectWithStreamAsync(imageStream, true, false, DefaultFaceAttributeTypes);

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

		/// <summary>
		/// Detects the audience makeup from an image
		/// </summary>
		/// <returns>The faces.</returns>
		/// <param name="imageFileStream">Image file stream.</param>
		public static async Task<List<Member>> CaptureMembers(Stream imageFileStream, DateTime timestamp)
		{
			#region TEMP
			await ResetFaceLists();
			#endregion

			try
			{
				string fileName = Guid.NewGuid().ToString() + ".png";
				var url = await FileStorageService.Instance.StoreImage(imageFileStream, ImageContainerName, fileName);

				var faces = await Client.Face.DetectWithUrlAsync(url.ToString(), true, false, DefaultFaceAttributeTypes);

				// Parse Individuals
				var individuals = new List<Member>();
				foreach (var item in faces)
				{
					var member = item.ToMember();
					member.Timestamp = timestamp;

					var similarFace = await FindSimilarPersistedFaceAsync(url.ToString(), (Guid)item.FaceId, item);

					if (similarFace != null && similarFace.Confidence > .5)
					{
						member.FaceId = null;
						member.PersistedFaceId = similarFace.PersistedFaceId.ToString();
					}

					individuals.Add(member);
				}

				// Delete the image from storage once done.
				await FileStorageService.Instance.RemoveFile(ImageContainerName, fileName);

				return individuals;
			}

			// Catch and display Face API errors.
			catch (APIErrorException f)
			{
				throw new Exception(f.ToString());
			}
			// Catch and display all other errors.
			catch (Exception e)
			{
				throw new Exception(e.ToString());
			}
		}

	}
}