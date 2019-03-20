using System;
using System.IO;
using System.Threading.Tasks;
using System.Timers;
using Plugin.Media;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using TargetAudienceClient.Services;
using Xamarin.Forms;

namespace TargetAudienceClient.Pages
{
	public class MonitorPage : ContentPage, IStatefulContent
	{
		SKCanvasView canvasView;
		CameraPreview cameraPreview;
		string imagePath;
		Timer timer;

		public MonitorPage()
		{
			Title = "Monitor";

			var screenSize = new Size(App.ScreenWidth, App.ScreenHeight);

			if (Device.RuntimePlatform == Device.UWP || CrossMedia.Current.IsCameraAvailable)
			{
				// Canvas View
				canvasView = new SKCanvasView();
				canvasView.PaintSurface += OnCanvasViewPaintSurface;
				canvasView.IgnorePixelScaling = !(Device.RuntimePlatform == Device.UWP);

				AbsoluteLayout.SetLayoutBounds(canvasView, new Rectangle(0, 0, 1, 1));
				AbsoluteLayout.SetLayoutFlags(canvasView, AbsoluteLayoutFlags.All);

				// Capture preview window
				cameraPreview = new CameraPreview();
				cameraPreview.Filename = "capture";
				cameraPreview.CameraOption = Settings.CameraOption;
				cameraPreview.CapturePathCallback = new Action<string>(ProcessCameraPhoto);

				var padSize = 20;
				var padLeft = padSize / screenSize.Width;
				var padBottom = 1 - (padSize / screenSize.Height);

				AbsoluteLayout.SetLayoutBounds(cameraPreview, new Rectangle(padLeft, padBottom, screenSize.Width * .2, screenSize.Height * .2));
				AbsoluteLayout.SetLayoutFlags(cameraPreview, AbsoluteLayoutFlags.PositionProportional);

				var layout = new AbsoluteLayout();
				layout.Children.Add(canvasView);
				layout.Children.Add(cameraPreview);
				layout.SizeChanged += Layout_SizeChanged;
				Content = layout;

				this.ToolbarItems.Add(
					new ToolbarItem("Toggle Camera", null, () => ToggleCamera()) { Icon = "toggle.png" }
				);
			}

			else
			{
				var label = new Label()
				{
					Text = "Camera Not Available",
					HorizontalOptions = LayoutOptions.CenterAndExpand,
					VerticalOptions = LayoutOptions.CenterAndExpand
				};
				var layout = new StackLayout();
				layout.Children.Add(label);
				Content = layout;
			}
		}

		public void StartCapture()
		{
			if (cameraPreview == null)
				return;

			var ms = Settings.MonitorInterval * 1000;
			timer = new Timer(ms);
			timer.Elapsed += TimerElapsed;
			timer.Start();
		}

		public void StopCapture()
		{
			if (timer != null)
			{
				timer.Stop();
				timer.Elapsed -= TimerElapsed;
				timer = null;
			}
		}

		void ToggleCamera()
		{
			if (!CrossMedia.Current.IsCameraAvailable)
			{
				DisplayAlert("Camera Not Availalble", ":( Permission not granted to camera.", "OK");
				return;
			}

			if (cameraPreview != null && cameraPreview.Capture != null)
				cameraPreview.CameraOption = (cameraPreview.CameraOption == CameraOptions.Rear) ? CameraOptions.Front : CameraOptions.Rear;
		}

		void TimerElapsed(object sender, ElapsedEventArgs e)
		{
			Device.BeginInvokeOnMainThread(CaptureImage);
		}

		void CaptureImage()
		{
			if (cameraPreview != null && cameraPreview.Capture != null)
				cameraPreview.Capture.Execute(null);
		}

		void Layout_SizeChanged(object sender, EventArgs e)
		{
			var layout = (AbsoluteLayout)sender;
			var canvasBounds = canvasView.Bounds;

			canvasView.Scale = layout.Scale;
		}

		void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
		{
			SKImageInfo info = args.Info;
			SKSurface surface = args.Surface;
			SKCanvas canvas = surface.Canvas;

			if (string.IsNullOrEmpty(imagePath))
				return;

			canvas.Clear(SKColors.White);

			byte[] imageBytes = null;
			try
			{
				imageBytes = File.ReadAllBytes(imagePath);
			}
			catch (Exception)
			{
				// a dirty hack because its late
				return;
			}

			// decode the bitmap
			using (var bitmap = SKBitmap.Decode(imageBytes))
			{
				// Resizing
				var pictureFrame = canvasView.Bounds.ToSKRect();
				var imageSize = new SKSize(bitmap.Width, bitmap.Height);
				var dest = pictureFrame.AspectFill(imageSize);

				// draw the image
				var paint = new SKPaint
				{
					FilterQuality = SKFilterQuality.High // high quality scaling
				};

				// draw the modified bitmap to the screen
				canvas.DrawBitmap(bitmap, dest, paint);
			}
		}

		async void ProcessCameraPhoto(string path)
		{
			imagePath = path;

			byte[] imageBytes = File.ReadAllBytes(path);

			var results = await AzureService.CaptureAudience("TODO Location", imageBytes);

			if (results.ErrorCode == 0)
			{
				Device.BeginInvokeOnMainThread(() => canvasView.InvalidateSurface());
			}
		}

		protected override void OnDisappearing()
		{
			if (cameraPreview != null && cameraPreview.StopCamera != null)
				cameraPreview.StopCamera.Execute(null);

			base.OnDisappearing();
		}

		public void DidAppear()
		{
			if (cameraPreview != null && cameraPreview.StartCamera != null)
			{
				cameraPreview.StartCamera.Execute(null);
				StartCapture();
			}
			if (canvasView != null) canvasView.InvalidateSurface();
		}

		public void DidDisappear()
		{
			StopCapture();
		}
	}
}
