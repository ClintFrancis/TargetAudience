using System;
using Microcharts;
using Microcharts.Forms;
using SkiaSharp;
using TargetAudienceClient.Constants;
using TargetAudienceClient.ViewModels;
using TargetAudienceClient.Views;
using Xamarin.Forms;

namespace TargetAudienceClient.Pages
{
	public class HistoryPage : ContentPage, IStatefulContent
	{
		//ChartViewCell mainChartCell;
		HistoryViewModel historyViewModel;

		public HistoryPage()
		{
			BindingContext = historyViewModel = new HistoryViewModel(Navigation);
			Title = "Audience History";

			this.ToolbarItems.Add(
				new ToolbarItem("Settings", null, () => OpenSettings()) { Icon = "folder.png" } // TODO Change icon to show settings / config
			);

			var chartView = new ChartView();
			chartView.HeightRequest = 300;
			chartView.SetBinding(ChartView.ChartProperty, new Binding("DemoChart"));

			Content = new TableView
			{

				HasUnevenRows = true,
				Root = new TableRoot {
					new TableSection("Chart Demo"){
						new CustomViewCell(chartView, 300)
						{
							IsEnabled = false
						}
					},
					new TableSection("Test"){
						new EntryCell { Label = "\U0001F600", Placeholder = "", IsEnabled = false },
						new EntryCell { Label = "ABV", Placeholder = "", IsEnabled = false },
						new EntryCell { Label = "Amsterdam", Placeholder = "", IsEnabled = false },
						new EntryCell { Label = "Location", Placeholder = "", IsEnabled = false },
						new LinkViewCell("test") { StyleId = DisclosureTypes.Disclosure }
					}
				},
				Intent = TableIntent.Settings
			};
		}

		private void OpenSettings()
		{
			throw new NotImplementedException();
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
