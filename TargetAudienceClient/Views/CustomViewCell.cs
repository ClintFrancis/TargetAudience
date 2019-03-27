using Xamarin.Forms;

namespace TargetAudienceClient.Views
{
	[Xamarin.Forms.ContentProperty("View")]
	public class CustomViewCell : ViewCell
	{
		public CustomViewCell(View customView)
		{
			var layout = new StackLayout
			{
				Orientation = StackOrientation.Vertical,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Children =
				{
					customView
				}
			};

			this.View = layout;
		}

		public CustomViewCell(Layout layout)
		{
			this.View = layout;
		}
	}
}
