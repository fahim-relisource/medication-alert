using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using ObjCRuntime;
using UIKit;
using UserNotifications;

namespace MedicationAlert.iOS
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the 
	// User Interface of the application, as well as listening (and optionally responding) to 
	// application events from iOS.
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		//
		// This method is invoked when the application has loaded and is ready to run. In this 
		// method you should instantiate the window, load the UI into it and then make the window
		// visible.
		//
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		//
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init();
			LoadApplication(new App());

			//UIApplication.SharedApplication.SetMinimumBackgroundFetchInterval(24 * 3600);

			var notificationSettings = UIUserNotificationSettings.GetSettingsForTypes(
				UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound, null
			);
			app.RegisterUserNotificationSettings(notificationSettings);

			return base.FinishedLaunching(app, options);
		}

		public override void PerformFetch(UIApplication application, Action<UIBackgroundFetchResult> completionHandler)
		{
			// TODO Fetch the datafrom the sqlite database and set the alarms
			completionHandler(UIBackgroundFetchResult.NewData);
		}

		public override void ReceivedLocalNotification(UIApplication application, UILocalNotification notification)
		{
			// show an alert
			UIAlertController okayAlertController = UIAlertController.Create(notification.AlertAction, notification.AlertBody, UIAlertControllerStyle.Alert);
			okayAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));

			Window.RootViewController.PresentViewController(okayAlertController, true, null);

			// reset our badge
			UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
		}

		public static void SendNotification()
		{
			App.AppLogger.D("Notification");

			// Deprecated Notification Method by Xamarin Doc
			UILocalNotification notification = new UILocalNotification();
			notification.FireDate = NSDate.FromTimeIntervalSinceNow(15);
			//notification.AlertTitle = "Alert Title"; // required for Apple Watch notifications
			notification.AlertAction = "View Alert";
			notification.AlertBody = "Your 15 second alert has fired!";
			notification.SoundName = UILocalNotification.DefaultSoundName;
			UIApplication.SharedApplication.ScheduleLocalNotification(notification);
		}
	}
}
