using System;
using System.ComponentModel;
using System.Threading.Tasks;
using MonkeyCache.FileStore;
using TargetAudience.Common.Messages;
using TargetAudienceClient.Constants;
using TargetAudienceClient.Models;
using TargetAudienceClient.Pages;
using TargetAudienceClient.Services;
using Xamarin.Forms;

namespace TargetAudienceClient.ViewModels
{
	public class CaptureViewModel : INotifyPropertyChanged
	{
		public INavigation Navigation { get; set; }
		public event PropertyChangedEventHandler PropertyChanged;

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

		public CaptureViewModel(INavigation navigation)
		{
			this.Navigation = navigation;
		}

		// TODO consider removing this UIResponse return.
		public async Task<UIResponse> Submit(byte[] bytes)
		{
			if (IsBusy)
				return new UIResponse();

			IsBusy = true;
			ImageData = bytes;

			var result = new UIResponse();
			AudienceResponse response = await AzureService.IdentifyAudience(location, imageData);
			if (!response.HasError)
			{
				//Saves to the cache with a timespan for expiration
				var capture = new AudienceData() { ImageData = imageData, Location = location, Audience = response.Audience };
				Barrel.Current.Add(StorageIds.CurrentCapture, capture, TimeSpan.FromMinutes(5));

				result.Result = true;
			}
			else
			{
				result.Message = response.Message;
			}

			IsBusy = false;
			return result;
		}

		public async Task Success()
		{
			await Navigation.PushAsync(new CaptureResultsPage());
		}

		protected void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
