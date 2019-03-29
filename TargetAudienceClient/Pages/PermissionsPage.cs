using System;
using System.Threading.Tasks;
using Plugin.Media;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using TargetAudienceClient.Constants;
using Xamarin.Forms;

namespace TargetAudienceClient.Pages
{
	public class PermissionsPage : ContentPage
	{
		Button cameraButton;
		Button photosButton;

		bool hasCameraPermission;
		bool hasPhotoPermission;

		public PermissionsPage()
		{
			var titleLabel = new Label()
			{
				Text = "Permissions Please",
				FontSize = 30,
				TextColor = Color.DarkSlateGray,
				HorizontalOptions = LayoutOptions.CenterAndExpand
			};

			var messageLabel = new Label()
			{
				Text = "This app requires access to the camera and photos. Please grant access before continuing.",
				WidthRequest = 300,
				TextColor = Color.SlateGray,
				HorizontalOptions = LayoutOptions.Center,
				HorizontalTextAlignment = TextAlignment.Center
			};

			cameraButton = new Button()
			{
				Text = "Allow Camera Access",
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				BackgroundColor = CustomColors.XFDarkBlue,
				TextColor = Color.White,
				FontAttributes = FontAttributes.Bold,
				WidthRequest = 180,
				HeightRequest = 40,
				CornerRadius = 10,
				Margin = new Thickness(0, 20, 0, 5)
			};
			cameraButton.Clicked += async (sender, e) => await CheckCameraPermissions();

			photosButton = new Button()
			{
				Text = "Allow Photos Access",
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				BackgroundColor = CustomColors.XFDarkBlue,
				TextColor = Color.White,
				FontAttributes = FontAttributes.Bold,
				WidthRequest = 180,
				HeightRequest = 40,
				CornerRadius = 10
			};
			photosButton.Clicked += async (sender, e) => await CheckPhotosPermissions();

			var centerLayout = new StackLayout();
			centerLayout.HorizontalOptions = LayoutOptions.CenterAndExpand;
			centerLayout.VerticalOptions = LayoutOptions.CenterAndExpand;
			centerLayout.Children.Add(titleLabel);
			centerLayout.Children.Add(messageLabel);
			centerLayout.Children.Add(cameraButton);
			centerLayout.Children.Add(photosButton);

			Content = centerLayout;
		}

		async Task CheckCameraPermissions()
		{
			var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
			if (status != PermissionStatus.Granted)
			{
				if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Camera))
					await DisplayAlert("Camera Please", "This application requires access to the camera.", "OK");

				var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Camera);
				if (results.ContainsKey(Permission.Camera))
					status = results[Permission.Camera];
			}

			if (status == PermissionStatus.Granted)
			{
				cameraButton.IsEnabled = false;
				cameraButton.Text = "Camera Granted";
				cameraButton.BackgroundColor = Color.LightGray;
				hasCameraPermission = true;
				await CheckComplete();
			}

			else if (status != PermissionStatus.Unknown)
				await DisplayAlert("Camera Denied", "Can not continue, try again.", "OK");
		}

		async Task CheckPhotosPermissions()
		{
			var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Photos);
			if (status != PermissionStatus.Granted)
			{
				if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Photos))
					await DisplayAlert("Photos Please", "This application requires access to photos.", "OK");

				var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Photos);
				if (results.ContainsKey(Permission.Photos))
					status = results[Permission.Photos];
			}

			if (status == PermissionStatus.Granted)
			{
				photosButton.IsEnabled = false;
				photosButton.Text = "Photos Granted";
				photosButton.BackgroundColor = Color.LightGray;
				hasPhotoPermission = true;
				await CheckComplete();
			}

			else if (status != PermissionStatus.Unknown)
				await DisplayAlert("Photos Denied", "Can not continue, try again.", "OK");
		}

		async Task CheckComplete()
		{
			if (hasCameraPermission && hasPhotoPermission)
				await Navigation.PopModalAsync();
		}

	}
}
