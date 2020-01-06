using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MedicationAlert.Models
{
    public class Schedule
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string MedicineName { get; set; }

        public string MedicationTime { get; set; }
    }
}
