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
        public string Position { get; set; }
        public string NationalId { get; set; }
        public bool HasTimeRunning { get; set; }
        public string TimeBeginning { get; set; }
        public int? LastWorkDayId { get; set; }
        public WorkdayDTO Workday { get; set; }
        public bool Active { get; set; }
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

        public DateTime BeginningTime { get; set; }
        public DateTime? BreakBeginningTime { get; set; }
        public DateTime? BreakEndTime { get; set; }
        public DateTime? EndTime { get; set; }

        public string BeginningTimeFormatted { 
            get { return BeginningTime.ToString("hh:mm tt"); }
            set { } 
        }

        public string BreakBeginningTimeFormatted
        {
            get {
                if (BreakBeginningTime.HasValue)
                    return BreakBeginningTime.Value.ToString("hh:mm tt");
                else
                    return "N/A";
            }
            set { }
        }

        public string BreakEndTimeFormatted
        {
            get
            {
                if (BreakEndTime.HasValue)
                    return BreakEndTime.Value.ToString("hh:mm tt");
                else
                    return "N/A";
            }
            set { }
        }

        public string EndTimeFormatted
        {
            get
            {
                if (EndTime.HasValue)
                    return EndTime.Value.ToString("hh:mm tt");
                else
                    return "N/A";
            }
            set { }
        }
    }

    public class WorkDayPost
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string Comments { get; set; }
    }

    public class EmployeeDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NationalId { get; set; }
        public string Position { get; set; }
    }
}
