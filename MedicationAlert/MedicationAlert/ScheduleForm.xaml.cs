using MedicationAlert.Models;
using MedicationAlert.ViewModels;
using System;
using System.Text;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MedicationAlert
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class ScheduleForm : ContentPage
  {
    ScheduleViewModel scheduleViewModel;
    public ScheduleForm(Schedule schedule = null)
    {
      InitializeComponent();

      scheduleViewModel = new ScheduleViewModel(schedule);
      BindingContext = scheduleViewModel;

      if(schedule != null)
      {
        var times = schedule.MedicationTime.Split(';');
        foreach(string time in times)
        {
          if (!string.IsNullOrWhiteSpace(time))
          {
            TimePicker newTimeEntry = new TimePicker()
            {
              Time = TimeSpan.Parse(time)
            };
            timeContainer.Children.Add(newTimeEntry);
          }
        }
      }
      else
      {
        TimePicker newTimeEntry = new TimePicker();
        timeContainer.Children.Add(newTimeEntry);
      }
    }

    private void BtnScheduleSave_Clicked(object sender, EventArgs e)
    {
      Console.WriteLine(scheduleViewModel.Schedule.MedicineName);
      StringBuilder timeBuilder = new StringBuilder();

      foreach (TimePicker timePicker in timeContainer.Children)
      {
        timeBuilder.Append(timePicker.Time);
        timeBuilder.Append(";");
      }
      App.Database.SaveScheduleAsync(new Schedule()
      {
        ID = scheduleViewModel.Schedule.ID,
        MedicineName = scheduleViewModel.Schedule.MedicineName,
        MedicationTime = timeBuilder.ToString()
      });
      Navigation.PopAsync();
    }

    private void BtnNewTime_Clicked(object sender, EventArgs e)
    {
      TimePicker newTimeEntry = new TimePicker();
      timeContainer.Children.Add(newTimeEntry);
    }
  }
}