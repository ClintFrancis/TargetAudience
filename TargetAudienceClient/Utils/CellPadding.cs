using System;

using Xamarin.Forms;

namespace TargetAudienceClient.Utils
{
	public static class CellPadding
	{
		public static Thickness Default
		{
			get
			{
				Thickness padding;
				switch (Device.RuntimePlatform)
				{
					case Device.iOS:
						padding = new Thickness(20, 15);
						break;
					case Device.Android:
					case Device.UWP:
					default:
						padding = new Thickness(0);
						break;
				}

				return padding;
			}
		}
	}
}

