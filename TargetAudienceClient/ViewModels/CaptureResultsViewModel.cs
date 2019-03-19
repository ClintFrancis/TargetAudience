using System.ComponentModel;
using TargetAudienceClient.Models;
using Xamarin.Forms;
using TargetAudience.Common.Models;
using TargetAudience.Common.Enums;
using MonkeyCache.FileStore;
using TargetAudienceClient.Constants;
using System.Runtime.CompilerServices;
using System;

namespace TargetAudienceClient.Forms.Models
{
	public class CaptureResultsViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		Audience customerData;
		INavigation navigation;

		string location;
		public string Location
		{
			get { return location; }
			protected set { SetProperty(ref location, value); }
		}

		double audienceTotal;
		public double AudienceTotal
		{
			get { return audienceTotal; }
			protected set { SetProperty(ref audienceTotal, value); }
		}

		GenderType averageGender;
		public GenderType AverageGender
		{
			get { return averageGender; }
			protected set { SetProperty(ref averageGender, value); }
		}

		double averageAge;
		public double AverageAge
		{
			get { return averageAge; }
			protected set { SetProperty(ref averageAge, value); }
		}

		MemberGroup maleAudience;
		public MemberGroup MaleAudience
		{
			get { return maleAudience; }
			protected set { SetProperty(ref maleAudience, value); }
		}

		MemberGroup femaleAudience;
		public MemberGroup FemaleAudience
		{
			get { return femaleAudience; }
			protected set { SetProperty(ref femaleAudience, value); }
		}

		byte[] imageData;
		public byte[] ImageData
		{
			get { return imageData; }
			protected set { SetProperty(ref imageData, value); }
		}

		bool isBusy;
		public bool IsBusy
		{
			get { return isBusy; }
			set { SetProperty(ref isBusy, value); }
		}

		public void Reset()
		{
			// TODO null fields
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:CustomerRecognition.Forms.Models.CreateOrderModel"/> class.
		/// </summary>
		public CaptureResultsViewModel(INavigation navigation)
		{
			this.navigation = navigation;

			var audienceData = Barrel.Current.Get<AudienceData>(StorageIds.CurrentCapture);
			if (audienceData != null)
			{
				Location = audienceData.Location;
				AudienceTotal = audienceData.Audience.Total;
				AverageGender = audienceData.Audience.AverageGender;
				AverageAge = audienceData.Audience.AverageAge;
				MaleAudience = audienceData.Audience.Males;
				FemaleAudience = audienceData.Audience.Females;
				ImageData = audienceData.ImageData;
			}
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
