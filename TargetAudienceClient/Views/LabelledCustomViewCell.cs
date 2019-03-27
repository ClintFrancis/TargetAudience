using System;
using TargetAudienceClient.Utils;
using Xamarin.Forms;

namespace TargetAudienceClient.Views
{
	[Xamarin.Forms.ContentProperty("View")]
	public class LabelledCustomViewCell : ViewCell
	{
		public TimePicker Picker;
		public Label DisplayLabel;

		public LabelledCustomViewCell(string label, View view)
		{
			DisplayLabel = new Label()
			{
				Text = label,
				VerticalTextAlignment = TextAlignment.Center
			};

			Picker = new TimePicker();
			Picker.TextColor = Color.DimGray;

			var layout = new StackLayout
			{
				Padding = CellPadding.Default,
				Orientation = StackOrientation.Horizontal,
				VerticalOptions = LayoutOptions.StartAndExpand,
				Children =
				{
					DisplayLabel,
					view
				}
			};

			this.View = layout;
		}
	}
}


