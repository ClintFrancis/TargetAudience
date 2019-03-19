using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microcharts;
using SkiaSharp;
using Xamarin.Forms;

namespace TargetAudienceClient.ViewModels
{
	public class HistoryViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		INavigation navigation;

		public Chart DemoChart
		{
			get; private set;
		}

		public HistoryViewModel(INavigation navigation)
		{
			this.navigation = navigation;

			var entries = new List<ChartEntry>
			{
				new ChartEntry(212)
				 {
					 Label = "UWP",
					 ValueLabel = "212",
					 Color = SKColor.Parse("#2c3e50")
				 },
				 new ChartEntry(248)
				 {
					 Label = "Android",
					 ValueLabel = "248",
					 Color = SKColor.Parse("#77d065")
				 },
				 new ChartEntry(128)
				 {
					 Label = "iOS",
					 ValueLabel = "128",
					 Color = SKColor.Parse("#b455b6")
				 },
				 new ChartEntry(500)
				 {
				 Color = SKColor.Parse("#3498db")
				 }
			};

			DemoChart = new LineChart()
			{
				Entries = entries
			};
		}

		bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
		{
			if (Object.Equals(storage, value))
				return false;

			storage = value;
			OnPropertyChanged(propertyName);
			return true;
		}

		protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
