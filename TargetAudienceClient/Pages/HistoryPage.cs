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

			var filterSummary = new TextCell() { IsEnabled = false };
			filterSummary.SetBinding(TextCell.TextProperty, new Binding("FilterSummary"));

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
					new TableSection("Filter")
					{
						filterSummary
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
					}
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

		protected override void OnAppearing()
		{
			var viewModel = (HistoryViewModel)BindingContext;
			if (viewModel.RefreshCommand.CanExecute(null))
				viewModel.RefreshCommand.Execute(null);
		}
	}
}
