using System;
using System.IO;
using System.Threading.Tasks;

namespace TargetAudience.Common.Interfaces
{
	public interface IFileStorageService
	{
		Task<Uri> StoreImage(byte[] image, string containerName, string fileName);
		Task<Uri> StoreImage(Stream image, string containerName, string fileName);
		Task RemoveFile(string containerName, string fileName);
		Task ResetContainer(string containerName);
	}
}
