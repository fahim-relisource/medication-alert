using MedicationAlert.Services;
using UIKit;

namespace MedicationAlert.iOS
{
	public class Application
	{
		public static ILogger AppLogger;

		// This is the main entry point of the application.
		static void Main(string[] args)
		{
			// if you want to use a different Application Delegate class from "AppDelegate"
			// you can specify it here.
			UIApplication.Main(args, null, "AppDelegate");
			AppLogger = new Logger();
		}
	}
}
