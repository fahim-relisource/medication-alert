
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using MedicationAlert.Services;

namespace MedicationAlert.Droid
{
	[Activity(Label = "MedicationAlert", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		public static MainActivity Instance;
		public const string APP_CHANNEL = "com.companyname.medicationalert";
		public static ILogger AppLogger;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(savedInstanceState);

			Instance = this;
			AppLogger = new Logger();

			Xamarin.Essentials.Platform.Init(this, savedInstanceState);
			global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
			LoadApplication(new App());
		}
		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
		{
			Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

			base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
		}

		public static void NotificationBuildAndShow(Context context, Intent intent)
		{
			var dataBundle = intent.GetBundleExtra("NotificationData");
			var message = dataBundle.GetString("Message") ?? "My Message";
			var title = dataBundle.GetString("Title") ?? "My Title";

			MainActivity.AppLogger.D($"Title: {title}, Message: {message}");

			var resultIntent = new Intent(context, typeof(MainActivity));
			intent.AddFlags(ActivityFlags.ClearTop);
			var pendingIntent = PendingIntent.GetActivity(context, 0, resultIntent, PendingIntentFlags.UpdateCurrent);

			var importance = NotificationImportance.High;
			NotificationChannel notificationChannel = new NotificationChannel(APP_CHANNEL, "Important", importance);
			notificationChannel.EnableVibration(true);
			notificationChannel.LockscreenVisibility = NotificationVisibility.Public;

			var audioAttributes = new AudioAttributes.Builder()
				.SetContentType(AudioContentType.Sonification)
				.SetUsage(AudioUsageKind.Alarm)
				.Build();

			notificationChannel.SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Ringtone), audioAttributes);

			var notificationBuilder = new NotificationCompat.Builder(context, APP_CHANNEL)
				.SetSmallIcon(Resource.Mipmap.icon_round)
				.SetContentTitle(title)
				.SetContentText(message)
				.SetContentIntent(pendingIntent)
				.SetAutoCancel(false)
				.SetChannelId(APP_CHANNEL);
			;

			NotificationManager notificationManager = (NotificationManager)context.GetSystemService(NotificationService);
			notificationManager.CreateNotificationChannel(notificationChannel);
			notificationManager.Notify(6461, notificationBuilder.Build());
		}
	}
}