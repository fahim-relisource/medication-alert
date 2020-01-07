using MedicationAlert.Data;
using MedicationAlert.Services;
using System;
using System.IO;
using Xamarin.Forms;

namespace MedicationAlert
{
	public partial class App : Application
	{
		static ScheduleDatabase database;
		private static ILogger _iLogger;

		public static ILogger AppLogger
		{
			get
			{
				_iLogger = DependencyService.Get<ILogger>();
				return _iLogger;
			}
		}

		public App()
		{
			InitializeComponent();

			MainPage = new NavigationPage(new MainPage());
		}

		public static ScheduleDatabase Database
		{
			get
			{
				if (database == null)
				{
					database = new ScheduleDatabase(
							Path.Combine(Environment.GetFolderPath(
									Environment.SpecialFolder.LocalApplicationData),
									"Schedule.db3")
							);
				}
				return database;
			}
		}

		protected override void OnStart()
		{
		}

		protected override void OnSleep()
		{
		}

		protected override void OnResume()
		{
		}
	}
}
