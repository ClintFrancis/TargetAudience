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
		public View CustomView;

		public CustomViewCell(View customView, int height)
		{
			Thickness padding;
			switch (Device.RuntimePlatform)
			{
				case Device.iOS:
				//padding = new Thickness(20, 15);
				//break;
				case Device.Android:
				case Device.UWP:
				default:
					padding = new Thickness(0);
					break;
			}

			CustomView = customView;

			var layout = new StackLayout
			{
				Orientation = StackOrientation.Vertical,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Padding = padding,
				Children =
				{
					CustomView
				}
			};

			this.View = layout;
		}

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
		}
	}
}
