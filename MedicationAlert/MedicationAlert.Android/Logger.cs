using Android.Util;
using MedicationAlert.Services;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: Xamarin.Forms.Dependency(typeof(MedicationAlert.Droid.Logger))]
namespace MedicationAlert.Droid
{
	class Logger : ILogger
	{
		public void D(object obj, [CallerFilePath] string file = "", [CallerMemberName] string member = "", [CallerLineNumber] int line = 0)
		{
			Log.Debug("MedicationAlert", "|---------------------------------------------------------------------");
			Log.Debug("MedicationAlert", $"|{member} @ {file.Split('\\').Last()}({line})");
			Log.Debug("MedicationAlert", "|---------------------------------------------------------------------");
			Log.Debug("MedicationAlert", $"|{obj?.ToString()}");
			Log.Debug("MedicationAlert", "|---------------------------------------------------------------------");
		}

		public void E(object obj, [CallerFilePath] string file = "", [CallerMemberName] string member = "", [CallerLineNumber] int line = 0)
		{
			Log.Error("MedicationAlert", "|---------------------------------------------------------------------");
			Log.Error("MedicationAlert", $"|{member} @ {file.Split('\\').Last()}({line})");
			Log.Error("MedicationAlert", "|---------------------------------------------------------------------");
			Log.Error("MedicationAlert", $"|{obj?.ToString()}");
			Log.Error("MedicationAlert", "|---------------------------------------------------------------------");
		}
	}
}