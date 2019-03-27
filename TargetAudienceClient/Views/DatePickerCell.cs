﻿using System;
using TargetAudienceClient.Utils;
using Xamarin.Forms;

namespace TargetAudienceClient.Views
{
	[Xamarin.Forms.ContentProperty("View")]
	public class DatePickerCell : ViewCell
	{
		public DatePicker Picker;
		public Label DisplayLabel;

		public DatePickerCell(string label)
		{
			DisplayLabel = new Label()
			{
				Text = label,
				VerticalTextAlignment = TextAlignment.Center
			};

			Picker = new DatePicker();
			Picker.TextColor = Color.DimGray;

			var layout = new StackLayout
			{
				Padding = CellPadding.Default,
				Orientation = StackOrientation.Horizontal,
				VerticalOptions = LayoutOptions.StartAndExpand,
				Children =
				{
					DisplayLabel,
					Picker
				}
			};

			this.View = layout;
		}
	}
}

