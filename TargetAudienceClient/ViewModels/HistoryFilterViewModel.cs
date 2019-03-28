using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TargetAudienceClient.Services;
using Xamarin.Forms;

namespace TargetAudienceClient.ViewModels
{
	public class HistoryFilterViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		IHistoryService historyService;

		DateTime mainStartDate;
		DateTime mainEndDate;

		public DateTime StartDate
		{
			get { return mainStartDate; }
			set
			{
				mainStartDate = value;
				OnPropertyChanged("StartDate");
				VerifyTimes();
			}
		}

		public TimeSpan StartTime
		{
			get { return mainStartDate.TimeOfDay; }
			set
			{
				mainStartDate = new DateTime(mainStartDate.Year, mainStartDate.Month, mainStartDate.Day).Add(value);
				OnPropertyChanged("StartTime");
				VerifyTimes();
			}
		}

		public DateTime EndDate
		{
			get { return mainEndDate; }
			set
			{
				mainEndDate = value;
				OnPropertyChanged("EndDate");
				VerifyTimes();
			}
		}

		public TimeSpan EndTime
		{
			get { return mainEndDate.TimeOfDay; }
			set
			{
				mainEndDate = new DateTime(mainEndDate.Year, mainEndDate.Month, mainEndDate.Day).Add(value);
				OnPropertyChanged("EndTime");
				VerifyTimes();
			}
		}

		bool uniqueMembersOnly;
		public bool UniqueMembersOnly
		{
			get { return uniqueMembersOnly; }
			set
			{
				if (uniqueMembersOnly == value)
					return;

				uniqueMembersOnly = historyService.UniqueMembersOnly = value;
				OnPropertyChanged("UniqueMembersOnly");
			}
		}

		bool useAllLocations;
		public bool UseAllLocations
		{
			get { return useAllLocations; }
			set
			{
				if (useAllLocations == value)
					return;

				// TODO implement this

				useAllLocations = value;
				OnPropertyChanged("UseAllLocations");
			}
		}

		public string TotalTimeSpan
		{
			get
			{
				var span = mainEndDate.Subtract(mainStartDate);
				var days = span.Days;
				var hours = span.Hours;
				var minutes = span.Minutes;

				string response = "";
				if (days > 0)
					response += (days == 1) ? days + " Day" : days + " Days";

				if (hours > 0)
				{
					if (response.Length > 0) response += ", ";
					response += (hours == 1) ? hours + " Hour" : hours + " Hours";
				}

				if (minutes > 0)
				{
					if (response.Length > 0) response += ", ";
					response += (minutes == 1) ? minutes + " Minute" : minutes + " Minutes";
				}

				return response;
			}
		}

		public HistoryFilterViewModel()
		{
			historyService = ServiceContainer.Resolve<IHistoryService>();
			mainStartDate = historyService.StartDate;
			mainEndDate = historyService.EndDate = DateTime.Now;
			UniqueMembersOnly = historyService.UniqueMembersOnly;
		}

		void VerifyTimes()
		{
			if (mainEndDate > DateTime.Now.Subtract(TimeSpan.FromSeconds(10)))
			{
				mainEndDate = mainEndDate.Subtract(TimeSpan.FromSeconds(10));
				OnPropertyChanged("EndDate");
				OnPropertyChanged("EndTime");
			}

			if (mainStartDate > mainEndDate)
			{
				mainStartDate = mainEndDate.Subtract(TimeSpan.FromMinutes(1));
				OnPropertyChanged("StartDate");
				OnPropertyChanged("StartTime");
			}

			OnPropertyChanged("TotalTimeSpan");

			historyService.StartDate = mainStartDate;
			historyService.EndDate = mainEndDate;
		}

		protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}

