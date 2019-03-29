using Plugin.Media;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TargetAudienceClient.Pages
{
	public class MainPage : TabbedPage
	{
		IStatefulContent currentStatefulPage;

		public MainPage()
		{
			CheckPermissions();
		}

		async Task CheckPermissions()
		{
			var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
			if (status != PermissionStatus.Granted)
			{
				Application.Current.ModalPopping += HandleModalPopping;
				await Navigation.PushModalAsync(new PermissionsPage(), false);
			}

			else
			{
				CreateContent();
			}
		}

		private void HandleModalPopping(object sender, ModalPoppingEventArgs e)
		{
			Application.Current.ModalPopping -= HandleModalPopping;
			CreateContent();
		}

		void CreateContent()
		{
			var textColor = Color.White;
			var backgroundColor = Color.FromRgb(73, 113, 175);

			// Bug where UWP crashes if the camera has been requested in two locations
			if (Device.RuntimePlatform != Device.UWP)
			{
				var captureNavigationPage = new NavigationPage(new CapturePage());
				captureNavigationPage.Title = "Capture";
				//captureNavigationPage.Icon = "person.png";
				captureNavigationPage.BarTextColor = textColor;
				captureNavigationPage.BarBackgroundColor = backgroundColor;
				Children.Add(captureNavigationPage);
			}

			var monitorNavigationPage = new NavigationPage(new MonitorPage());
			monitorNavigationPage.Title = "Monitor";
			//monitorNavigationPage.Icon = "capture.png";
			monitorNavigationPage.BarTextColor = textColor;
			monitorNavigationPage.BarBackgroundColor = backgroundColor;
			Children.Add(monitorNavigationPage);

			var historyNavigationPage = new NavigationPage(new HistoryPage());
			historyNavigationPage.Title = "History";
			//historyNavigationPage.Icon = "orders.png";
			historyNavigationPage.BarTextColor = textColor;
			historyNavigationPage.BarBackgroundColor = backgroundColor;
			Children.Add(historyNavigationPage);

			var settingsNavigationPage = new NavigationPage(new SettingsPage());
			settingsNavigationPage.Title = "Settings";
			//settingsNavigationPage.Icon = "settings.png";
			settingsNavigationPage.BarTextColor = textColor;
			settingsNavigationPage.BarBackgroundColor = backgroundColor;
			Children.Add(settingsNavigationPage);
		}


		protected override void OnCurrentPageChanged()
		{
			base.OnCurrentPageChanged();

			var navPage = (NavigationPage)CurrentPage;
			if (currentStatefulPage != null && currentStatefulPage != navPage.CurrentPage)
			{
				currentStatefulPage.DidDisappear();
				currentStatefulPage = null;
			}

			if (navPage.CurrentPage is IStatefulContent)
			{
				var stateful = (IStatefulContent)navPage.CurrentPage;
				stateful.DidAppear();
				currentStatefulPage = stateful;
			}
		}
	}
}

