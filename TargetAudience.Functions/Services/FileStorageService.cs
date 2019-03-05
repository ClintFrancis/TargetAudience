using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Threading.Tasks;
using TargetAudience.Common.Interfaces;

namespace TargetAudience.Functions.Services
{
	public class FileStorageService : IFileStorageService
	{
		private static FileStorageService _instance;
		public static FileStorageService Instance
		{
			get
			{
				_instance = _instance ?? new FileStorageService();
				return _instance;
			}
		}

		string storageConnection = Environment.GetEnvironmentVariable("FileStorageConnectionString");
		CloudStorageAccount storageAccount;
		CloudBlobClient blobClient;

		private FileStorageService()
		{
			storageAccount = CloudStorageAccount.Parse(storageConnection);
			blobClient = storageAccount.CreateCloudBlobClient();
		}

		public async Task<Uri> StoreImage(byte[] image, string containerName, string fileName)
		{
			var blockBlob = GetBlockBlob(containerName, fileName);
			await blockBlob.UploadFromByteArrayAsync(image, 0, image.Length);

			return blockBlob.Uri;
		}

		public async Task<Uri> StoreImage(Stream image, string containerName, string fileName)
		{
			var blockBlob = GetBlockBlob(containerName, fileName);
			await blockBlob.UploadFromStreamAsync(image, image.Length);

			return blockBlob.Uri;
		}

		CloudBlockBlob GetBlockBlob(string containerName, string fileName)
		{
			CloudBlobContainer container = blobClient.GetContainerReference(containerName);
			return container.GetBlockBlobReference(fileName);
		}

		public async Task RemoveFile(string containerName, string fileName)
		{
			CloudBlobContainer container = blobClient.GetContainerReference(containerName);

			CloudBlob cloudBlob = container.GetBlobReference(fileName);
			await cloudBlob.DeleteIfExistsAsync();
		}

		public async Task ResetContainer(string containerName)
		{
			BlobContinuationToken blobContinuationToken = null;
			CloudBlobContainer container = blobClient.GetContainerReference(containerName);

			var contents = await container.ListBlobsSegmentedAsync(blobContinuationToken);
			foreach (var blob in contents.Results)
			{
				CloudBlob cloudBlob = container.GetBlobReference(blob.Uri.ToString());
				await cloudBlob.DeleteIfExistsAsync();
			}
		}
	}
}
