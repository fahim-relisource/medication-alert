using System;
using System.Globalization;
using System.Linq;

namespace MedicationAlert.Utils
{
	class NextMedicationTimeConverter : Xamarin.Forms.IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string medicationTimes = (string)value;
			var times = medicationTimes.Split(';');
			times = times.Where(time => !string.IsNullOrWhiteSpace(time)).ToArray();

			var currentTime = DateTime.Now;

			foreach (String time in times)
			{
				var parsedTime = DateTime.Today + TimeSpan.Parse(time);
				if (parsedTime > currentTime)
				{
					return $"Next at {parsedTime.ToString("hh:mm tt")}";
				}
			}
			var onlyTime = DateTime.Today + TimeSpan.Parse(times[0]);
			return $"Next at {onlyTime.ToString("hh:mm tt")}";
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
