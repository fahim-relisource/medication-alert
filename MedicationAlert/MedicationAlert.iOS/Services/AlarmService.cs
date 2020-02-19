using System;
using MedicationAlert.Services;


[assembly: Xamarin.Forms.Dependency(typeof(MedicationAlert.iOS.Services.AlarmService))]
namespace MedicationAlert.iOS.Services
{
	public class AlarmService : IAlarmService
	{
		public bool SetAlarmAt(string medicineName, TimeSpan medicationTime)
		{
			App.AppLogger.D($"Medicine Name: {medicineName}, Medication Time {medicationTime}");
			AppDelegate.SendNotification(medicineName, medicationTime);
			return true;
		}
	}
}
