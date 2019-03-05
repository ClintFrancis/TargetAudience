using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace TargetAudience.Functions.Utils
{
	public static class ImageUtils
	{
		public static byte[] ConvertToByteArray(string base64Image)
		{
			// Decode data from Base64
			byte[] imageBytes = Convert.FromBase64String(base64Image);

			Image image;
			using (var ms = new MemoryStream(imageBytes))
			{
				image = Image.FromStream(ms);

				if (!ImageFormat.Png.Equals(image.RawFormat))
				{
					image.Save(ms, ImageFormat.Png);
					return ms.ToArray();
				}
				else
				{
					return ms.ToArray();
				}
			}
		}

		public static Bitmap ConvertToBitmap(Stream stream)
		{
			stream.Seek(0, SeekOrigin.Begin);
			Bitmap bm = new Bitmap(stream);
			stream.Seek(0, SeekOrigin.Begin);
			return bm;
		}

		public static Bitmap CropBitmap(Bitmap src, Rectangle cropRect)
		{
			Bitmap target = new Bitmap(cropRect.Width, cropRect.Height);

			using (Graphics g = Graphics.FromImage(target))
			{
				g.DrawImage(src, new Rectangle(0, 0, target.Width, target.Height),
								 cropRect,
								 GraphicsUnit.Pixel);
			}

			return target;
		}

		public static byte[] ToByteArray(this Bitmap image, ImageFormat format)
		{
			using (MemoryStream ms = new MemoryStream())
			{
				image.Save(ms, format);
				return ms.ToArray();
			}
		}
	}
}
