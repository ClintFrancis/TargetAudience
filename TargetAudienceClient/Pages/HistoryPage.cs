using System;
using System.Collections.Generic;
using Microcharts.Forms;
using TargetAudienceClient.Constants;
using TargetAudienceClient.ViewModels;
using TargetAudienceClient.Views;
using Xamarin.Forms;

namespace TargetAudienceClient.Pages
{
	public class HistoryPage : ContentPage, IStatefulContent
	{

		public HistoryPage()
		{
			BindingContext = new HistoryViewModel(Navigation);
			Title = "Audience History";

			this.ToolbarItems.Add(
				new ToolbarItem("Settings", null, () => OpenSettings()) { Icon = "folder.png" } // TODO Change icon to show settings / config
			);

			var genderChartView = new ChartView();
			genderChartView.BackgroundColor = Color.Transparent;
			genderChartView.HeightRequest = 200;
			genderChartView.SetBinding(ChartView.ChartProperty, new Binding("GenderChart"));

			var agesChartView = new ChartView();
			agesChartView.HeightRequest = 200;
			agesChartView.SetBinding(ChartView.ChartProperty, new Binding("AgeChart"));

			Content = new TableView
			{
				HasUnevenRows = true,
				Root = new TableRoot {
					// TODO move to filter screen later
					new TableSection("Filter")
					{
						new TextCell()
						{
							Text = "Summary to go here",
							IsEnabled = false
						}
					},

					new TableSection("Audience Makeup")
					{
						new CustomViewCell(genderChartView) { IsEnabled = false },
						new LinkViewCell("Male Audience") { StyleId = DisclosureTypes.Disclosure },
						new LinkViewCell("Female Audience") { StyleId = DisclosureTypes.Disclosure }
					},
					new TableSection("Ages")
					{
						new CustomViewCell(agesChartView) { IsEnabled = false }
					},
					//new TableSection("Test"){
					//	new EntryCell { Label = "\U0001F600", Placeholder = "", IsEnabled = false },
					//	new EntryCell { Label = "ABV", Placeholder = "", IsEnabled = false },
					//	new EntryCell { Label = "Amsterdam", Placeholder = "", IsEnabled = false },
					//	new EntryCell { Label = "Location", Placeholder = "", IsEnabled = false },
					//	new LinkViewCell("test") { StyleId = DisclosureTypes.Disclosure }
					//}
				},
				Intent = TableIntent.Settings
			};
		}

		private void OpenSettings()
		{
			Navigation.PushAsync(new HistoryFilterPage());
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
