using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Microcharts;
using Microcharts.Forms;
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
