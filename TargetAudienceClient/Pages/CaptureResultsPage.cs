using System;
using TargetAudienceClient.Converters;
using TargetAudienceClient.Forms.Models;
using Xamarin.Forms;

namespace TargetAudienceClient.Pages
{
	public class CaptureResultsPage : ContentPage, IStatefulContent
	{
		CaptureResultsViewModel captureResultsModel;

		public CaptureResultsPage()
		{
			BindingContext = captureResultsModel = new CaptureResultsViewModel(Navigation);
			Title = "Capture";

			// TODO image bitmap data
			var image = new Image();
			image.SetBinding(Image.SourceProperty, new Binding("ImageData", BindingMode.Default, converter: new ByteArrayToImageSourceConverter()));
			image.Aspect = Aspect.AspectFit;
			//image.WidthRequest = 200;
			image.HeightRequest = 200;

			var location = new EntryCell { Label = "Location", Placeholder = "", IsEnabled = false };
			location.SetBinding(EntryCell.TextProperty, new Binding("Location"));

			var audienceTotal = new EntryCell { Label = "Audience Total", Placeholder = "", IsEnabled = false };
			audienceTotal.SetBinding(EntryCell.TextProperty, new Binding("AudienceTotal"));

			var averageGender = new EntryCell { Label = "Average Gender", Placeholder = "", IsEnabled = false };
			averageGender.SetBinding(EntryCell.TextProperty, new Binding("AverageGender", BindingMode.Default));

			var anverageAge = new EntryCell { Label = "Average Age", Placeholder = "", IsEnabled = false };
			anverageAge.SetBinding(EntryCell.TextProperty, new Binding("AverageAge", BindingMode.Default, stringFormat: "{0:F1}"));

			Content = new TableView
			{
				HasUnevenRows = true,
				Root = new TableRoot {
					new TableSection("Photo"){
						new ViewCell() {View = image}
					},
					new TableSection("Audience"){
						location,
						audienceTotal,
						averageGender,
						anverageAge
						//entryEmotion
					}
				},
				Intent = TableIntent.Settings
			};
		}

		public void DidAppear()
		{
			//throw new NotImplementedException();
		}

		public void DidDisappear()
		{
			//throw new NotImplementedException();
		}
	}
}
