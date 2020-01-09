using AndroidX.Work;
using Java.Util.Concurrent;
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
			PeriodicWorkRequest alarmRequest = PeriodicWorkRequest.Builder
				.From<AlarmWorker>(1, TimeUnit.Days)
				.Build();
			WorkManager.Instance.Enqueue(alarmRequest);

			return true;
		}
	}
}