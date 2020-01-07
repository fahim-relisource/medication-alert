using AndroidX.Work;
using MedicationAlert.Services;
using System;
using Xamarin.Forms;

[assembly: Dependency(typeof(MedicationAlert.Droid.Services.AlarmService))]
namespace MedicationAlert.Droid.Services
{
	class AlarmService : IAlarmService
	{
		public bool SetAlarmAt(string medicineName,TimeSpan medicationTime)
		{

			MainActivity.AppLogger.D($"Medicine Name-{medicineName}; Medication Time-{medicationTime}");

			var dataBuilder = new AndroidX.Work.Data.Builder()
				.PutString("Title", "Medication Reminder")
				.PutString("Message", $"Please take {medicineName} immediately.")
				.PutString("Time", medicationTime.ToString())
				.Build();

			PeriodicWorkRequest alarmRequest = PeriodicWorkRequest.Builder.From<AlarmWorker>(TimeSpan.FromDays(1))
				.SetInputData(dataBuilder)
				.Build();
			WorkManager.Instance.Enqueue(alarmRequest);

			return true;
		}
	}
}