using MedicationAlert.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MedicationAlert.ViewModels
{
  class ScheduleViewModel : INotifyPropertyChanged
  {
    private Schedule _schedule;

    public Schedule Schedule
    {
      get { return _schedule; }
      set
      {
        _schedule = value;
        OnPropertyChanged("Schedule");
      }
    }

    public ScheduleViewModel(Schedule schedule = null)
    {
      Schedule = schedule ?? new Schedule()
      {
        MedicineName = "",
        MedicationTime = ""
      };
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
