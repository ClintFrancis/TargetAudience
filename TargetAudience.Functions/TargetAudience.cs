
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TargetAudience.Common.Interfaces;
using TargetAudience.Common.Messages;
using TargetAudience.Common.Models;
using TargetAudience.Functions.Models;
using TargetAudience.Functions.Services;
using TargetAudience.Functions.Utils;

namespace TargetAudience.Functions
{
	public static class TargetedAds
	{
		static IAudienceStorageService storage = new AudienceCosmosStorage();
		const string DefaultLocationId = "default";

		[FunctionName("CaptureAudience")]
		public static async Task<IActionResult> CaptureAudience([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req, ILogger log)
		{
			string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
			var data = JsonConvert.DeserializeObject<AudienceRequest>(requestBody);
			var response = new AudienceResponse();

			try
			{
				// Decode data from Base64
				byte[] imageBytes = Convert.FromBase64String(data.Image);

				// Process the image
				using (Stream stream = new MemoryStream(imageBytes))
				{
					var members = await FaceService.CaptureMembers(stream, DateTime.Now);

					if (members.Count > 0)
					{
						// Storage the results
						var token = new System.Threading.CancellationToken();
						var location = !string.IsNullOrEmpty(data.Location) ? data.Location : DefaultLocationId;
						await storage.WriteAsync(location, members, token);

						response.Audience = MemberUtils.CreateAudience(members);
						response.StatusCode = (int)HttpStatusCode.OK;

						return new OkObjectResult(response);
					}
				}
			}
			catch (Exception ex)
			{
				response.Message = ex.Message;
				response.StatusCode = (int)HttpStatusCode.InternalServerError;
				return new BadRequestObjectResult(response);
			}

			response.Message = "Unable to detect faces.";
			response.StatusCode = (int)HttpStatusCode.BadRequest;
			return new BadRequestObjectResult(response);
		}

		[FunctionName("IdentifyAudience")]
		public static async Task<IActionResult> IdentifyAudience([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req, ILogger log)
		{
			string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
			var data = JsonConvert.DeserializeObject<AudienceRequest>(requestBody);
			var response = new AudienceResponse();
			try
			{
				// Decode data from Base64
				byte[] imageBytes = Convert.FromBase64String(data.Image);

				// Process the image
				using (Stream stream = new MemoryStream(imageBytes))
				{
					var members = await FaceService.DetectMembers(stream, DateTime.Now);

					if (members.Count > 0)
					{
						// Storage the results
						var token = new System.Threading.CancellationToken();
						var location = !string.IsNullOrEmpty(data.Location) ? data.Location : DefaultLocationId;
						await storage.WriteAsync(location, members, token);

						response.Audience = MemberUtils.CreateAudience(members);
						response.StatusCode = (int)HttpStatusCode.OK;

						return new OkObjectResult(response);
					}
				}
			}
			catch (Exception ex)
			{
				response.Message = ex.Message;
				response.StatusCode = (int)HttpStatusCode.InternalServerError;
				return new BadRequestObjectResult(response);
			}

			response.Message = "Unable to detect faces.";
			response.StatusCode = (int)HttpStatusCode.BadRequest;
			return new BadRequestObjectResult(response);
		}

		[FunctionName("AudienceHistory")]
		public static async Task<IActionResult> AudienceHistory([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req, ILogger log)
		{
			string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
			var data = JsonConvert.DeserializeObject<AudienceHistoryRequest>(requestBody);
			var response = new AudienceResponse();

			try
			{
				var endDate = (data.EndDate != default(DateTime)) ? data.EndDate : DateTime.Now;
				var startDate = (data.StartDate != default(DateTime)) ? data.StartDate : endDate.Subtract(TimeSpan.FromMinutes(data.Minutes));
				var token = new System.Threading.CancellationToken();
				var locations = (data.Locations?.Length > 0) ? data.Locations : new string[] { DefaultLocationId };

				List<Member> collection;
				if (data.UniqueMembers)
					collection = await storage.UniqueMembersTimeSpan(locations, startDate, endDate, token);

				else
					collection = await storage.QueryTimeSpan(locations, startDate, endDate, token);

				response.Audience = MemberUtils.CreateAudience(collection);
				response.StatusCode = (int)HttpStatusCode.OK;

				return new OkObjectResult(response);
			}
			catch (Exception ex)
			{
				response.Message = ex.Message;
				response.StatusCode = (int)HttpStatusCode.InternalServerError;
				return new BadRequestObjectResult(response);
			}
		}

		[FunctionName("SocialTrendsByLocation")]
		public static async Task<IActionResult> SocialTrendsByLocation([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req, ILogger log)
		{
			string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
			var data = JsonConvert.DeserializeObject<AudienceHistoryRequest>(requestBody);

			try
			{
				//...
				// https://www.toptal.com/api-developers/social-network-apis

				return new OkObjectResult("OK");
			}
			catch (Exception ex)
			{
				return new BadRequestObjectResult("An error occured: " + ex.Message);
			}
		}
	}
}
