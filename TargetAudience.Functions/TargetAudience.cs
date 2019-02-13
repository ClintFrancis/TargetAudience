
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TargetAudience.Common.Interfaces;
using TargetAudience.Common.Models;
using TargetAudience.Functions.Models;
using TargetAudience.Functions.Services;
using TargetAudience.Functions.Utils;

namespace TargetAudience.Functions
{
	public static class TargetedAds
	{
		static IStorageService storage = new MemoryStorage();
		static FaceService faceService = new FaceService();
		const string DefaultLocationId = "default";

		[FunctionName("IdentifyAudience")]
		public static async Task<IActionResult> IdentifyAudience([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req, ILogger log)
		{
			string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
			var data = JsonConvert.DeserializeObject<AudienceRequest>(requestBody);

			try
			{
				// Decode data from Base64
				byte[] imageBytes = Convert.FromBase64String(data.Image);

				// Process the image
				using (Stream stream = new MemoryStream(imageBytes))
				{
					var members = await faceService.DetectMembers(stream, DateTime.Now);

					if (members.Count > 0)
					{
						// Storage the results
						var token = new System.Threading.CancellationToken();
						var location = !string.IsNullOrEmpty(data.Location) ? data.Location : DefaultLocationId;
						await storage.WriteAsync(location, members, token);

						var audience = MemberUtils.CreateAudience(members);

						return new OkObjectResult(audience);
					}
				}
			}
			catch (Exception ex)
			{
				return new BadRequestObjectResult("An error occured: " + ex.Message);
			}

			return new BadRequestObjectResult("Unable to detect faces.");
		}

		[FunctionName("AudienceHistory")]
		public static async Task<IActionResult> AudienceHistory([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req, ILogger log)
		{
			string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
			var data = JsonConvert.DeserializeObject<AudienceHistoryRequest>(requestBody);

			try
			{
				var currentTime = DateTime.Now;
				var pastTime = currentTime.Subtract(TimeSpan.FromMinutes(data.Minutes));
				var token = new System.Threading.CancellationToken();
				var locations = (data.Locations?.Length > 0) ? data.Locations : new string[] { DefaultLocationId };

				var collection = await storage.QueryTimeSpan(locations, pastTime, currentTime, token);

				var allMembers = new List<Member>();
				foreach (var item in collection.Values)
					allMembers.AddRange(item);

				var response = MemberUtils.CreateAudience(allMembers);

				return new OkObjectResult(response);
			}
			catch (Exception ex)
			{
				return new BadRequestObjectResult("An error occured: " + ex.Message);
			}
		}
	}
}
