using System.ComponentModel;
using TargetAudienceClient.Models;
using Xamarin.Forms;
using TargetAudience.Common.Models;
using TargetAudience.Common.Enums;
using MonkeyCache.FileStore;
using TargetAudienceClient.Constants;

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
			set
			{
				if (location != value)
				{
					location = value;
					OnPropertyChanged("Location");
				}
			}
		}

		double audienceTotal;
		public double AudienceTotal
		{
			get { return audienceTotal; }
			set
			{
				if (audienceTotal != value)
				{
					audienceTotal = value;
					OnPropertyChanged("AudienceTotal");
				}
			}
		}

		GenderType averageGender;
		public GenderType AverageGender
		{
			get { return averageGender; }
			protected set
			{
				if (averageGender != value)
				{
					averageGender = value;
					OnPropertyChanged("AverageGender");
				}
			}
		}

		double averageAge;
		public double AverageAge
		{
			get { return averageAge; }
			protected set
			{
				if (averageAge != value)
				{
					averageAge = value;
					OnPropertyChanged("AverageAge");
				}
			}
		}

		MemberGroup maleAudience;
		public MemberGroup MaleAudience
		{
			get { return maleAudience; }
			protected set
			{
				if (maleAudience != value)
				{
					maleAudience = value;
					OnPropertyChanged("MaleAudience");
				}
			}
		}

		MemberGroup femaleAudience;
		public MemberGroup FemaleAudience
		{
			get { return femaleAudience; }
			protected set
			{
				if (femaleAudience != value)
				{
					femaleAudience = value;
					OnPropertyChanged("FemaleAudience");
				}
			}
		}

		byte[] imageData;
		public byte[] ImageData
		{
			get { return imageData; }
			protected set
			{
				if (imageData != value)
				{
					imageData = value;
					OnPropertyChanged("ImageData");
				}
			}
		}

		bool isBusy;
		public bool IsBusy
		{
			get { return isBusy; }
			set
			{
				if (isBusy == value)
					return;

				isBusy = value;
				OnPropertyChanged("IsBusy");
			}
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

		protected void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
