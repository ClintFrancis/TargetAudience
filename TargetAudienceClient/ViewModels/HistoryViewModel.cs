using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microcharts;
using SkiaSharp;
using TargetAudienceClient.Constants;
using Xamarin.Forms;

namespace TargetAudienceClient.ViewModels
{
	public class HistoryViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		INavigation navigation;

		// TODO
		// Location multiselect
		// Audience Gender
		// Age Range
		// Date Range
		// EMotions
		// unique individuals
		// Facial hair (males)
		// Makeup (females)
		// Glasses


		public Chart GenderChart { get; private set; }

		public Chart AgeChart { get; private set; }


		public HistoryViewModel(INavigation navigation)
		{
			this.navigation = navigation;

			GenderChart = new PieChart()
			{
				HoleRadius = .7f,
				LabelTextSize = 40,
				Margin = 60
			};
			GenderChart.Entries = new ChartEntry[]
				{
					new ChartEntry(.7f)
					{
						 Label = "Male",
						 ValueLabel = "7",
						 Color = CustomColors.DarkBlue
					},
					new ChartEntry(.3f)
					{
						 Label = "Female",
						 ValueLabel = "3",
						 Color = CustomColors.LightBlue
					},
				};

			AgeChart = new LineChart()
			{
				BackgroundColor = SKColor.Empty,
				LineAreaAlpha = 0x0,
				PointSize = 20,
				LineMode = LineMode.Straight,
				Entries = new ChartEntry[]
				{
					new ChartEntry(212)
					{
						 Label = "UWP",
						 ValueLabel = "212",
						 Color = CustomColors.DarkBlue
					},
					new ChartEntry(248)
					{
						 Label = "Android",
						 ValueLabel = "248",
						 Color = CustomColors.DarkBlue
						 },
					 new ChartEntry(128)
					 {
						 Label = "iOS",
						 ValueLabel = "128",
						 Color = CustomColors.DarkBlue
					 },
					 new ChartEntry(500)
					 {
					 Color = CustomColors.DarkBlue
					 }}
			};
		}

		Chart CreateChart()
		{
			var genderChart = new LineChart()
			{
				LabelTextSize = 24,
				BackgroundColor = SKColor.Empty,
				LineMode = LineMode.Straight,
				PointMode = PointMode.Circle,
				PointSize = 20,
				LineAreaAlpha = 0x0
			};
			return genderChart;
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
