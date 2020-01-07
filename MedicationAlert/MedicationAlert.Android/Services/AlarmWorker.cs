using Android.App;
using Android.Content;
using Android.OS;
using AndroidX.Work;
using Java.Util;
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
			var title = InputData.GetString("Title");
			var message = InputData.GetString("Message");
			var time = InputData.GetString("Time");

			MainActivity.AppLogger.D($"Title: {title}; Message: {message}; Time: {time}");

			var schedules = GetSchedules().Result;

			//MainActivity.AppLogger.D($"Schedules Count: {schedules.Count}");
			int i = 1;

			AlarmManager alarmManager = MainActivity.Instance.GetSystemService(Context.AlarmService) as AlarmManager;
			List<PendingIntent> intentArray = new List<PendingIntent>();

			schedules.ForEach(schedule =>
			{
				//MainActivity.AppLogger.D($"Schedule Medicine Name: {schedule.MedicineName}, Medication Time: {schedule.MedicationTime}");
				var bundle = new Bundle();
				bundle.PutString("Title", "Medication Reminder");
				bundle.PutString("Message", $"Please take {schedule.MedicineName} immediately.");


				Intent notificationIntent = new Intent(MainActivity.Instance, typeof(MedicationAlertReceiver));
				notificationIntent.PutExtra("NotificationData", bundle);

				PendingIntent alarmIntent = PendingIntent.GetBroadcast(
						MainActivity.Instance,
						i,
						notificationIntent,
						0
					);

				// TODO Need to calculate Alarm Time from Current Time.
				var alarmTime = SystemClock.ElapsedRealtime();

				var myTimeSpan = TimeSpan.Parse(schedule.MedicationTime.Replace(";", String.Empty));
				var myTotalMinutes = myTimeSpan.Hours * 60 + myTimeSpan.Minutes;
				var myTotalMilis = (myTotalMinutes * 60 + myTimeSpan.Seconds) * 1000;

				var currentTime = DateTime.Now;
				var currentTimeMilis = ((currentTime.Hour * 60 + currentTime.Minute) * 60 + currentTime.Second) * 1000;

				var anotherAlarmTime = alarmTime + (myTotalMilis - currentTimeMilis);

				MainActivity.AppLogger.D($"Alarm Time is {alarmTime} and Current Time - Elapsed Time {anotherAlarmTime}");

				alarmManager.SetAndAllowWhileIdle(
					AlarmType.ElapsedRealtimeWakeup,
					anotherAlarmTime,
					alarmIntent
				);
				++i;

				intentArray.Add(alarmIntent);
			});

			return Result.InvokeSuccess();
		}

		private async Task<List<Schedule>> GetSchedules()
		{
			return await App.Database.GetSchedulesAsync();
		}
	}
}