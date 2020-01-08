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
			PeriodicWorkRequest alarmRequest = PeriodicWorkRequest.Builder
				.From<AlarmWorker>(TimeSpan.FromDays(1))
				.Build();
			WorkManager.Instance.Enqueue(alarmRequest);

			return true;
		}
	}
}