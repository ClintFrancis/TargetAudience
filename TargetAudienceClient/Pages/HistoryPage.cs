using System;
using TargetAudienceClient.Constants;
using TargetAudienceClient.Views;
using Xamarin.Forms;

namespace TargetAudienceClient.Pages
{
	public class HistoryPage : ContentPage, IStatefulContent
	{
		public HistoryPage()
		{
			Title = "Audience History";

			Content = new TableView
			{
				HasUnevenRows = true,
				Root = new TableRoot {
					new TableSection("Test"){
						new EntryCell { Label = "\U0001F600", Placeholder = "", IsEnabled = false },
						new EntryCell { Label = "ABV", Placeholder = "", IsEnabled = false },
						new EntryCell { Label = "Amsterdam", Placeholder = "", IsEnabled = false },
						new EntryCell { Label = "Location", Placeholder = "", IsEnabled = false },
						new LinkViewCell("test") { StyleId = DisclosureTypes.Disclosure }
					},
					//maleTableSection
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
