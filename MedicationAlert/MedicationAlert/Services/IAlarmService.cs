using System;

namespace MedicationAlert.Services
{
	public interface IAlarmService
	{
		bool SetAlarmAt(string medicineName, TimeSpan medicationTime);
	}
}
