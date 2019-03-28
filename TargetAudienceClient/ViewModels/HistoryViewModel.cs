using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microcharts;
using SkiaSharp;
using TargetAudienceClient.Constants;
using TargetAudienceClient.Services;
using Xamarin.Forms;

namespace TargetAudienceClient.ViewModels
{
	public class HistoryViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		INavigation navigation;
		IHistoryService historyService;

		public ICommand RefreshCommand { get; private set; }

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

		string filterSummary;
		public string FilterSummary
		{
			get { return filterSummary; }
			protected set { SetProperty(ref filterSummary, value); }
		}

		public HistoryViewModel(INavigation navigation)
		{
			this.navigation = navigation;
			historyService = ServiceContainer.Resolve<IHistoryService>();
			historyService.Updated += HistoryService_Updated;

			RefreshCommand = new Command(async () => await RefreshHistoryData());

			GenderChart = new PieChart()
			{
				HoleRadius = .7f,
				LabelTextSize = 30,
				Margin = 60
			};

			AgeChart = new LineChart()
			{
				BackgroundColor = SKColor.Empty,
				LineAreaAlpha = 0x0,
				PointSize = 20,
				LineMode = LineMode.Straight
			};
		}

		private async Task RefreshHistoryData()
		{
			var format = "dd MMMM hh:mm tt";
			FilterSummary = historyService.StartDate.ToString(format) + " - " +
				historyService.EndDate.ToString(format);

			if (historyService.IsDirty && !historyService.IsBusy)
				await historyService.RefreshData();
		}

		void HistoryService_Updated(object sender, HistoryUpdatedEventArgs e)
		{
			CreateGenderChart();
		}

		void CreateGenderChart()
		{
			var data = historyService.Data;
			if (data == null || data.Total == 0)
			{
				GenderChart.Entries = new ChartEntry[1]
				{
					new ChartEntry(1) { Color = SKColors.LightGray }
				};
				return;
			}

			var entries = new List<ChartEntry>();
			if (data.Males != null)
			{
				entries.Add(
				new ChartEntry((float)(data.Males.Total / data.Total))
				{
					Label = "Male",
					ValueLabel = data.Males.Total.ToString(),
					Color = CustomColors.DarkBlue
				});
			}

			if (data.Females != null)
			{
				entries.Add(
				new ChartEntry((float)(data.Females.Total / data.Total))
				{
					Label = "Female",
					ValueLabel = data.Females.Total.ToString(),
					Color = CustomColors.LightBlue
				});
			}

			GenderChart.Entries = entries;
		}

		void CreateDummyCharts()
		{
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
