using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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

			AgeChart = new PointChart()
			{
				BackgroundColor = SKColor.Empty,
				PointSize = 20,
				LabelTextSize = 30

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
			CreateAgeChart();
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
					Color = CustomColors.SKDarkBlue
				});
			}

			if (data.Females != null)
			{
				entries.Add(
				new ChartEntry((float)(data.Females.Total / data.Total))
				{
					Label = "Female",
					ValueLabel = data.Females.Total.ToString(),
					Color = CustomColors.SKLightBlue
				});
			}

			GenderChart.Entries = entries;
		}

		void CreateAgeChart()
		{
			var data = historyService.Data;
			if (data == null || data.Total == 0)
			{
				AgeChart.Entries = new ChartEntry[1]
				{
					new ChartEntry(1) { Color = SKColors.LightGray }
				};
				return;
			}

			var entries = new List<ChartEntry>();
			foreach (var group in data.GetAllIndividuals().GroupBy(x => x.Age).OrderBy(c => c.Key))
			{
				var age = group.Key;
				var total = group.Count();
				entries.Add(
						new ChartEntry(total)
						{
							Label = age.ToString(),
							ValueLabel = total.ToString(),
							Color = CustomColors.SKDarkBlue,
							TextColor = SKColors.LightGray
						}
					);
			}

			AgeChart.Entries = entries;
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
