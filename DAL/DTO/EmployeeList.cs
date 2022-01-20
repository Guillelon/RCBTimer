using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
    public class EmployeeList
    {
        public int Id { get; set; }
        public string Info { get; set; }
        public string NationalId { get; set; }
        public bool HasTimeRunning { get; set; }
        public string TimeBeginning { get; set; }
    }

    public class EmployeeInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public bool HasTimeRunning { get; set; }
        public bool HasBreakRunning { get; set; }
        public bool TimeEnded { get; set; }
        public bool BreakEnded { get; set; }
        public Workday Workday { get; set; }
        public string TodaysInfo { get; set; }
        public string TodaysHourInfo { get; set; }
    }
}
