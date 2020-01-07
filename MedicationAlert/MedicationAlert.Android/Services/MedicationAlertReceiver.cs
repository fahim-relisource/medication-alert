using Android.Content;

namespace MedicationAlert.Droid.Services
{
	[BroadcastReceiver]
	public class MedicationAlertReceiver : BroadcastReceiver
	{
		public override void OnReceive(Context context, Intent intent)
		{
			MainActivity.NotificationBuildAndShow(context, intent);
		}

	}
}