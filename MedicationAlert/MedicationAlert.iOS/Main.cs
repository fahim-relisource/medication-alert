using EventKit;
using MedicationAlert.Services;
using UIKit;

namespace MedicationAlert.iOS
{
	public class Application
	{
		public static ILogger AppLogger;
		public static EKEventStore EventStore;

		// This is the main entry point of the application.
		static void Main(string[] args)
		{
			// if you want to use a different Application Delegate class from "AppDelegate"
			// you can specify it here.
			AppLogger = new Logger();
			EventStore = new EKEventStore();
			
			UIApplication.Main(args, null, "AppDelegate");
		}
	}
}
