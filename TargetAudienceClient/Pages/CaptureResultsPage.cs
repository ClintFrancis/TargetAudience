using System;
using TargetAudienceClient.Converters;
using TargetAudienceClient.Forms.Models;
using TargetAudienceClient.Views;
using Xamarin.Forms;

namespace TargetAudienceClient.Pages
{
	public class CaptureResultsPage : ContentPage, IStatefulContent
	{
		CaptureResultsViewModel captureResultsModel;

		public CaptureResultsPage()
		{
			BindingContext = captureResultsModel = new CaptureResultsViewModel(Navigation);
			Title = "Results";

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

			var averageAge = new EntryCell { Label = "Average Age", Placeholder = "", IsEnabled = false };
			averageAge.SetBinding(EntryCell.TextProperty, new Binding("AverageAge", BindingMode.Default, stringFormat: "{0:F1}"));

			// Genders
			var genderSection = new TableSection("Genders");

			// Male Audience
			var maleAudience = captureResultsModel.MaleAudience;
			if (maleAudience != null)
			{
				var malesCell = new LinkViewCell("Male Audience");
				malesCell.Tapped += async (sender, e) =>
					await Navigation.PushAsync(new MemberGroupDetailsPage(captureResultsModel.MaleAudience));

				genderSection.Add(malesCell);
			}

			// Female Audience
			var femaleAudience = captureResultsModel.FemaleAudience;
			if (femaleAudience != null)
			{
				var femalesCell = new LinkViewCell("Female Audience");
				femalesCell.Tapped += async (sender, e) =>
					await Navigation.PushAsync(new MemberGroupDetailsPage(captureResultsModel.FemaleAudience));

				genderSection.Add(femalesCell);
			}

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
						averageAge
					},
					genderSection
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
