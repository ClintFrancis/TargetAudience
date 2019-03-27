using System;
using TargetAudienceClient.Utils;
using Xamarin.Forms;

namespace TargetAudienceClient.Views
{
	[Xamarin.Forms.ContentProperty("View")]
	public class LinkViewCell : ViewCell
	{
		public LinkViewCell(string label)
		{
			var padding = CellPadding.Default;

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
