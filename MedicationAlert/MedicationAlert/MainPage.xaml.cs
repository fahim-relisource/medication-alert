using MedicationAlert.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MedicationAlert
{
	// Learn more about making custom code visible in the Xamarin.Forms previewer
	// by visiting https://aka.ms/xamarinforms-previewer
	[DesignTimeVisible(false)]
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
			AddToolBarItems();
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();
			BindingContext = await App.Database.GetSchedulesAsync();
		}

		private void AddToolBarItems()
		{
			ToolbarItem toolbarItem = new ToolbarItem("", null, AddNewMedication_ClickHandler)
			{
				IconImageSource = ImageSource.FromResource("MedicationAlert.Assets.icons.add_circle.png"),
			};

			ToolbarItems.Add(toolbarItem);
		}

		private void AddNewMedication_ClickHandler()
		{
			Navigation.PushAsync(new ScheduleForm());
		}

		private void BtnEditSchedule_Clicked(object sender, EventArgs e)
		{
			var editButton = (Button)sender;
			var schedule = editButton.CommandParameter as Schedule;

			Navigation.PushAsync(new ScheduleForm(schedule));

			Console.WriteLine(schedule.MedicineName);
		}

		private async void BtnDeleteSchedule_Clicked(object sender, EventArgs e)
		{
			var deleteButton = (Button)sender;
			var schedule = deleteButton.CommandParameter as Schedule;

			var willDelete = await DisplayAlert("Delete", $"Do you want to delete {schedule.MedicineName}?", "Yes", "No");
			if (willDelete)
			{
				await App.Database.DeleteScheduleAsync(schedule);
				BindingContext = await App.Database.GetSchedulesAsync();
			}
		}
	}
}
