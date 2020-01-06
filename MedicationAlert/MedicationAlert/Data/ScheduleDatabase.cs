using MedicationAlert.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MedicationAlert.Data
{
    public class ScheduleDatabase
    {
        readonly SQLiteAsyncConnection _database;

        public ScheduleDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Schedule>().Wait();
        }

        public Task<List<Schedule>> GetSchedulesAsync()
        {
            return _database.Table<Schedule>().ToListAsync();
        }

        public Task<Schedule> GetSchedultAsync(int scheduleId)
        {
            return _database.Table<Schedule>().Where(schedule => schedule.ID == scheduleId).FirstOrDefaultAsync();
        }

        public Task<int> SaveScheduleAsync(Schedule schedule)
        {
            return schedule.ID != 0 ? _database.UpdateAsync(schedule) : _database.InsertAsync(schedule);
        }

        public Task<int> DeleteScheduleAsync(Schedule schedule)
        {
            return _database.DeleteAsync(schedule);
        }
    }
}
