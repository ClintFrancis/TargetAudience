using System;
using Xamarin.Forms;

namespace TargetAudienceClient.Views
{
	[Xamarin.Forms.ContentProperty("View")]
	public class LinkViewCell : ViewCell
	{
		public LinkViewCell(string label)
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

			var layout = new StackLayout
			{
				Orientation = StackOrientation.Horizontal,
				Padding = padding,
				Children =
				{
					new Label(){Text = label}

				}
			};
			this.View = layout;
		}
	}
}
