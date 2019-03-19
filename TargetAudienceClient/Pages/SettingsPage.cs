using System;
using TargetAudienceClient.ViewModels;
using Xamarin.Forms;

namespace TargetAudienceClient.Pages
{
	public class SettingsPage : ContentPage
	{
		public SettingsPage()
		{
			BindingContext = new SettingsViewModel();

			var cameraSwitch = new SwitchCell { Text = "Rear Camera" };
			cameraSwitch.SetBinding(SwitchCell.OnProperty, new Binding("DefaultCameraRear"));

			var cameraLocation = new EntryCell { Label = "Camera Location", Keyboard = Keyboard.Default };
			cameraLocation.SetBinding(EntryCell.TextProperty, new Binding("CameraLocation", BindingMode.TwoWay));

			var timerEntry = new EntryCell { Label = "Camera Refresh", Keyboard = Keyboard.Numeric };
			timerEntry.SetBinding(EntryCell.TextProperty, new Binding("TimerInterval", BindingMode.TwoWay));

			var historyEntry = new EntryCell { Label = "History Refresh", Keyboard = Keyboard.Numeric };
			timerEntry.SetBinding(EntryCell.TextProperty, new Binding("HistoryInterval", BindingMode.TwoWay));

			//var clearDataLabel = new TextCell { Text = "Clear All Data" };
			//clearDataLabel.Tapped += async (s, e) =>
			//{
			//	var result = await DisplayAlert("Delete All Data", "Are you sure you wish to clear all stored data online?", "YES", "Cancel");
			//	if (result)
			//	{
			//		var response = await (BindingContext as SettingsModel).ResetData();
			//		DisplayAlert(response, "", "Ok");
			//	}
			//};

			Title = "Settings";
			Content = new TableView
			{
				Root = new TableRoot{
					new TableSection("Camera") {
						cameraLocation,
						cameraSwitch
					},

					new TableSection("Monitor Interval"){
						timerEntry
					},

					new TableSection("History Interval"){
						timerEntry
					},

					//new TableSection("Data"){
					//	clearDataLabel
					//},
				}
			};
		}
	}
}


