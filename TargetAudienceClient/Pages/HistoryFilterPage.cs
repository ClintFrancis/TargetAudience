using System;
using System.Threading.Tasks;
using TargetAudienceClient.ViewModels;
using TargetAudienceClient.Views;
using Xamarin.Forms;

namespace TargetAudienceClient.Pages
{
	public class HistoryFilterPage : ContentPage
	{
		public HistoryFilterPage()
		{
			BindingContext = new HistoryFilterViewModel();

			var maxDate = DateTime.Now;

			var rangeLabel = new TextCell() { IsEnabled = false };
			rangeLabel.SetBinding(TextCell.TextProperty, new Binding("TotalTimeSpan"));

			var startDatePicker = new DatePicker() { TextColor = Color.DimGray };
			startDatePicker.MaximumDate = maxDate;
			startDatePicker.SetBinding(DatePicker.DateProperty, new Binding("StartDate", BindingMode.TwoWay));

			var startTimePicker = new TimePicker() { TextColor = Color.DimGray };
			startTimePicker.SetBinding(TimePicker.TimeProperty, new Binding("StartTime", BindingMode.TwoWay));

			var endDatePicker = new DatePicker() { TextColor = Color.DimGray };
			endDatePicker.MaximumDate = maxDate;
			endDatePicker.SetBinding(DatePicker.DateProperty, new Binding("EndDate", BindingMode.TwoWay));

			var endTimePicker = new TimePicker() { TextColor = Color.DimGray };
			endTimePicker.SetBinding(TimePicker.TimeProperty, new Binding("EndTime", BindingMode.TwoWay));

			var uniqueMembersSwitch = new SwitchCell() { Text = "Unique Members Only" };
			uniqueMembersSwitch.SetBinding(SwitchCell.OnProperty, new Binding("UniqueMembersOnly", BindingMode.TwoWay));

			var allLocationsSwitch = new SwitchCell() { Text = "All Locations" };
			allLocationsSwitch.SetBinding(SwitchCell.OnProperty, new Binding("UseAllLocations", BindingMode.TwoWay));

			Content = new TableView
			{
				HasUnevenRows = true,
				Root = new TableRoot {
					new TableSection("Range")
					{
					rangeLabel
					},
					new TableSection("Filter")
					{
						new LabelledCustomViewCell("Start Date:", startDatePicker),
						new LabelledCustomViewCell("Start Time:", startTimePicker),
						new LabelledCustomViewCell("End Date:", endDatePicker),
						new LabelledCustomViewCell("End Time:", endTimePicker),
						uniqueMembersSwitch,
						allLocationsSwitch
					}
				},
				Intent = TableIntent.Settings
			};
		}
	}
}

