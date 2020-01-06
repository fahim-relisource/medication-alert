using System;
using System.Globalization;

namespace MedicationAlert.Utils
{
	class MedicationTimeConverter : Xamarin.Forms.IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string medicationTimes = (string)value;
			var times = medicationTimes.Split(';');
			// TODO Need to change the logic
			return String.Format("{0} Time{1}/day", times.Length - 1, times.Length > 2 ? "s" : "");
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
