using Android.App;
using Android.Content;
using Android.OS;
using AndroidX.Work;
using MedicationAlert.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicationAlert.Droid.Services
{
	public class AlarmWorker : Worker
	{
		public AlarmWorker(Context context, WorkerParameters workerParameters) : base(context, workerParameters) { }

		public override Result DoWork()
		{
			List<Schedule> schedules = GetSchedules().Result;

			AlarmManager alarmManager = MainActivity.Instance.GetSystemService(Context.AlarmService) as AlarmManager;
			List<PendingIntent> intentArray = new List<PendingIntent>();

			int alarmCounter = 1;
			schedules.ForEach(schedule =>
			{
				Bundle notificationDataBundle = new Bundle();
				notificationDataBundle.PutString("Title", "Medication Reminder");
				notificationDataBundle.PutString("Message", $"Please take {schedule.MedicineName} immediately.");

				Intent notificationIntent = new Intent(MainActivity.Instance, typeof(MedicationAlertReceiver));
				notificationIntent.PutExtra("NotificationData", notificationDataBundle);

				string[] medicationTimes = schedule.MedicationTime.Split(";");

				foreach (string medicationTime in medicationTimes)
				{
					if (string.IsNullOrWhiteSpace(medicationTime))
						continue;

					int alarmRequestCode = schedule.ID * 10 + alarmCounter;
					PendingIntent alarmIntent = PendingIntent.GetBroadcast(
							MainActivity.Instance,
							alarmCounter,
							notificationIntent,
							PendingIntentFlags.Immutable
						);

					TimeSpan myTimeSpan = TimeSpan.Parse(medicationTime);
					DateTime scheduleTime = DateTime.Today + myTimeSpan;

					long alarmTimeEpoch = new DateTimeOffset(scheduleTime).ToUnixTimeMilliseconds();
					long currentTimeEpoch = new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds();

					if (alarmTimeEpoch > currentTimeEpoch)
					{
						alarmManager.SetAlarmClock(
							new AlarmManager.AlarmClockInfo(
								alarmTimeEpoch,
								alarmIntent
							),
							alarmIntent
						);

						intentArray.Add(alarmIntent);
					}

					++alarmCounter;
				}
			});

			return Result.InvokeSuccess();
		}

		private async Task<List<Schedule>> GetSchedules()
		{
			return await App.Database.GetSchedulesAsync();
		}

		private long GetTimeOfDayInMilis(DateTime dateTime)
		{
			long inMinutes = dateTime.Hour * 60 + dateTime.Minute;
			long inSeconds = inMinutes * 60 + dateTime.Second;
			return inSeconds * 1000;
		}
	}
}