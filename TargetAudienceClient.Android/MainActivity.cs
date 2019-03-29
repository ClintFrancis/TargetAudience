
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Lottie.Forms.Droid;
using Plugin.Permissions;

namespace TargetAudienceClient.Droid
{
	[Activity(Label = "TargetAudienceClient", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			App.ScreenWidth = (int)Resources.DisplayMetrics.WidthPixels; // real pixels
			App.ScreenHeight = (int)Resources.DisplayMetrics.HeightPixels; // real pixels

			base.OnCreate(savedInstanceState);

			global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
			Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, savedInstanceState);
			AnimationViewRenderer.Init();
			LoadApplication(new App());
		}

		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
		{
			PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
			base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
		}
	}
}