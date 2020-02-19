using System;
using System.Collections.Generic;
using System.Linq;
using EventKit;
using Foundation;
using ObjCRuntime;
using UIKit;
using UserNotifications;
using Xamarin.Forms;

namespace MedicationAlert.iOS
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the 
	// User Interface of the application, as well as listening (and optionally responding) to 
	// application events from iOS.
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		// This method is invoked when the application has loaded and is ready to run. In this 
		// method you should instantiate the window, load the UI into it and then make the window
		// visible.
		//
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		//
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init();
			//Application.EventStore.Init();

			LoadApplication(new App());

			CheckNotificationPermission();


			#region Event Request Permission
			//Application.EventStore.RequestAccess(EKEntityType.Event,
			//	(bool granted, NSError e) =>
			//	{
			//		if (granted)
			//		{
			//			App.AppLogger.D("User gave access to calendar data");
			//		}
			//		else
			//		{
			//			App.AppLogger.E("User denied access to calendar data");
			//		}

			//	}); 
			#endregion

			#region Reminder Request Permission
			//Application.EventStore.RequestAccess(EKEntityType.Reminder,
			//	(bool granted, NSError e) =>
			//	{
			//		if (granted)
			//		{
			//			App.AppLogger.D("User gave access to reminder data");
			//		}
			//		else
			//		{
			//			App.AppLogger.E("User denied access to reminder data");
			//		}

			//	}); 
			#endregion

			return base.FinishedLaunching(app, options);
		}

		public void CheckNotificationPermission()
		{
			if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
			{
				UNUserNotificationCenter notificationCenter = UNUserNotificationCenter.Current;
				notificationCenter.RequestAuthorization(UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound, (approved, err) =>
				{
					if (approved)
					{
						App.AppLogger.D("User approved the notification center access");
					}
					else
					{
						App.AppLogger.D("User doesn't approved the notification center access");
					}
				});
				notificationCenter.Delegate = new NotificationDelegate();
			}
			else if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
			{
				var notificationSettings = UIUserNotificationSettings.GetSettingsForTypes(UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound, null);
				UIApplication.SharedApplication.RegisterUserNotificationSettings(notificationSettings);
			}
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

		public static void SendNotification(String medicineName, TimeSpan alarmTime)
		{
			string notificationBody = $"Its time to take {medicineName}";

			#region Local Notification
			// Deprecated Notification Method by Xamarin Doc
			//UILocalNotification notification = new UILocalNotification();
			//notification.FireDate = NSDate.FromTimeIntervalSinceNow(15);
			////notification.AlertTitle = "Alert Title"; // required for Apple Watch notifications
			//notification.AlertAction = "View Alert";
			//notification.AlertBody = "Your 15 second alert has fired!";
			//notification.SoundName = UILocalNotification.DefaultSoundName;
			//UIApplication.SharedApplication.ScheduleLocalNotification(notification); 
			#endregion

			#region Calender Local Notification
			var notificationContent = new UNMutableNotificationContent()
			{
				Title = "Medication Alert",
				Body = notificationBody,
				Sound = UNNotificationSound.GetSound("ringtone.caf"),
			};

			NSDateComponents dateComponents = new NSDateComponents
			{
				Hour = alarmTime.Hours,
				Minute = alarmTime.Minutes
			};

			var trigger = UNCalendarNotificationTrigger.CreateTrigger(dateComponents, false);
			var request = UNNotificationRequest.FromIdentifier(new Guid().ToString(), notificationContent, trigger);
			App.AppLogger.D(request.Identifier);

			UNUserNotificationCenter.Current.AddNotificationRequest(request, (err) =>
			{
				if (err != null)
				{
					throw new Exception("faild to schedule notification");
				}
			});
			#endregion

			#region EventKit Calender Event
			//NSDate nSDate = (NSDate)(DateTime.Today + alarmTime);

			//EKEvent newEvent = EKEvent.FromStore(Application.EventStore);
			//newEvent.AddAlarm(EKAlarm.FromDate(nSDate));
			//newEvent.StartDate = nSDate;
			//newEvent.EndDate = nSDate.AddSeconds(120.0);
			//newEvent.Title = "Hello";
			//newEvent.Notes = "Hello World";
			//newEvent.Calendar = Application.EventStore.DefaultCalendarForNewEvents;
			//try
			//{

			//	NSError e;
			//	Application.EventStore.SaveEvent(newEvent, EKSpan.ThisEvent, out e);
			//	App.AppLogger.D("EventID: " + newEvent.CalendarItemIdentifier);
			//	App.AppLogger.D("Event" + newEvent.ToString());
			//	if(e != null)
			//		App.AppLogger.E("Event Error" + e.ToString());

			//	DateTime startDate = DateTime.Now.AddDays(-7);
			//	DateTime endDate = DateTime.Now.AddDays(7);
			//	// the third parameter is calendars we want to look in, to use all calendars, we pass null
			//	NSPredicate query = Application.EventStore.PredicateForEvents((NSDate)startDate, (NSDate)endDate, null);
			//	// execute the query
			//	EKCalendarItem[] events = Application.EventStore.EventsMatching(query);

			//	foreach (var aEvent in events)
			//	{
			//		App.AppLogger.D(aEvent.ToString());
			//	}
			//}
			//catch (Exception e)
			//{
			//	App.AppLogger.E(e.Message);
			//}
			#endregion

			#region EventKit Reminder Event
			//EKReminder reminder = EKReminder.Create(Application.EventStore);
			//reminder.Title = notificationBody;
			//reminder.Calendar = Application.EventStore.DefaultCalendarForNewReminders;
			//reminder.AddAlarm(EKAlarm.FromDate((NSDate)(DateTime.Today + alarmTime)));

			//NSError e;
			//Application.EventStore.SaveReminder(reminder, true, out e);

			//if (e != null)
			//	App.AppLogger.E("Event Error" + e.ToString());

			//EKCalendarItem myReminder = Application.EventStore.GetCalendarItem(reminder.CalendarItemIdentifier);
			//App.AppLogger.D(myReminder.ToString());

			//// create our NSPredicate which we'll use for the query
			//NSPredicate query = Application.EventStore.PredicateForReminders(null);

			//// execute the query
			//Application.EventStore.FetchReminders(query, (EKReminder[] items) =>
			//		{
			//			foreach (var aReminder in items)
			//			{

			//				App.AppLogger.D(aReminder.ToString());
			//			}
			//		});
			#endregion

		}
	}

	class NotificationDelegate : UNUserNotificationCenterDelegate
	{
		public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
		{
			App.AppLogger.D(notification.ToString());
			completionHandler(UNNotificationPresentationOptions.Alert);
		}

	}

}
