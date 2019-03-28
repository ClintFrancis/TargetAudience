using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TargetAudience.Common.Models;

namespace TargetAudienceClient.Services
{
	public interface IHistoryService
	{
		Task<Audience> RefreshData();
		event EventHandler<HistoryUpdatedEventArgs> Updated;

		string[] Locations { get; set; }
		DateTime StartDate { get; set; }
		DateTime EndDate { get; set; }
		bool UniqueMembersOnly { get; set; }
		Audience Data { get; }
		bool IsBusy { get; }
		bool IsDirty { get; }
	}

	public class HistoryUpdatedEventArgs : EventArgs
	{
		public Audience Data { get; private set; }

		public HistoryUpdatedEventArgs(Audience data)
		{
			Data = data;
		}
	}

	public class HistoryService : IHistoryService
	{
		public event EventHandler<HistoryUpdatedEventArgs> Updated;

		#region Properties
		string[] locations;
		public string[] Locations
		{
			get { return locations; }
			set { SetProperty(ref locations, value); }
		}

		DateTime startDate;
		public DateTime StartDate
		{
			get { return startDate; }
			set { SetProperty(ref startDate, value); }
		}

		DateTime endDate;
		public DateTime EndDate
		{
			get { return endDate; }
			set { SetProperty(ref endDate, value); }
		}

		bool uniqueMembersOnly;
		public bool UniqueMembersOnly
		{
			get { return uniqueMembersOnly; }
			set { SetProperty(ref uniqueMembersOnly, value); }
		}

		public Audience Data { get; private set; }

		public bool IsBusy { get; private set; }

		public bool IsDirty { get; private set; } = true;
		#endregion

		public HistoryService()
		{
			endDate = DateTime.Now;
			startDate = endDate.Subtract(TimeSpan.FromHours(12));
			uniqueMembersOnly = false;
			locations = null;
		}

		public async Task<Audience> RefreshData()
		{
			IsBusy = true;
			var response = await AzureService.AudienceHistory(locations, startDate, endDate, uniqueMembersOnly);
			if (response.HasError)
			{
				Console.WriteLine(response.Message);
				IsBusy = false;
				return null;
			}

			Data = response.Audience;
			IsBusy = IsDirty = false;
			Updated?.Invoke(this, new HistoryUpdatedEventArgs(Data));

			return response.Audience;
		}

		bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
		{
			if (Object.Equals(storage, value))
				return false;

			storage = value;
			IsDirty = true;
			return true;
		}
	}
}
