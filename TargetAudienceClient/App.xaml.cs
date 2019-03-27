using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TargetAudienceClient.Pages;
using TargetAudienceClient.Forms.Models;
using MonkeyCache.FileStore;
using TargetAudienceClient.Models;
using TargetAudienceClient.Services;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace TargetAudienceClient
{
	public partial class App : Application
	{
		public static int ScreenWidth;
		public static int ScreenHeight;
		public static int CameraRatio;

		public App()
		{
			InitializeComponent();

			Barrel.ApplicationId = "TargetAudience";

			ServiceContainer.Register<IHistoryService>(new HistoryService());

			MainPage = new MainPage();
		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}
