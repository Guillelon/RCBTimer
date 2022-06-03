using DAL.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
    public class WorkdaysReportDTO
    {
        public int Id { get; set; }
        public string EmployeeInfo { get; set; }
        public DateTime BeginningTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime? BreakBeginningTime { get; set; }
        public DateTime? BreakEndTime { get; set; }
        public string WorkedTime { get; set; }
        public string Comments { get; set; }
        public string EmployeeComments { get; set; }

        public WorkdaysReportDTO()
        {
        }

        public WorkdaysReportDTO(Workday model)
        {
            EmployeeInfo = model.Employee.FullInfo();
            BeginningTime = model.BeginningTime;
        }
    }
    public class WorkdayDTO
    {
        public int Id { get; set; }
        public string EmployeeInfo { get; set; }
        public string EmployeePosition { get; set; }
        public DateTime BeginningTime { get; set; }
        public DateTime BeginningTimeDate { get; set; }
        public string BeginningTimeHour { get; set; }
        public DateTime? EndTime { get; set; }
        public string BreakBeginningTime { get; set; }
        public string BreakEndTime { get; set; }
        public int BreakTime { get; set; }
        public string WorkedTime { get; set; }
        public string Comments { get; set; }
        public string EmployeeComments { get; set; }

        public int? BreakMinutes { get; set; }
        public TimeSpan? TotalWorkedTime { get; set; }

        public WorkdayDTO()
        {

        }

        public WorkdayDTO(Workday w)
        {
            BeginningTime = w.BeginningTime;
        }
    }
    public class WorkdayForEmployeeDTO
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeePosition { get; set; }
        public string CommentsfromEmployee { get; set; }
        public int EmployeeId { get; set; }
        public string TodaysInfo { get; set; }
        public string TodaysHourInfo { get; set; }
        public bool HasTimeRunning { get; set; }
        public bool HasBreakRunning { get; set; }

        public DateTime? BeginningTime { get; set; }
        public string BeginningTimeDate { get; set; }
        public string BeginningTimeHour {
            get
            {
                return BeginningTime.Value.ToString("hh:mm tt");
            }
        }
        public DateTime? EndTime { get; set; }
        public string EndTimeDate { get; set; }
        public string EndTimeHour
        {
            get
            {
                if (EndTime.HasValue)
                    return EndTime.Value.ToString("hh:mm tt");
                else
                    return string.Empty;
            }
        }
        public IList<BreakDTO> Breaks { get; set; }

        public WorkdayForEmployeeDTO()
        {
            Breaks = new List<BreakDTO>();
        }
    }
    public class WorkdayToEdit
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeePosition { get; set; }
        public DateTime BeginningTime { get; set; }
        public string BeginningTimeDate { get; set; }
        public string BeginningTimeHour { get; set; }
        public DateTime? EndTime { get; set; }
        public IList<BreakDTO> Breaks { get; set; }

        public WorkdayToEdit()
        {
            Breaks = new List<BreakDTO>();
        }
    }
    public class BreakDTO
    {
        public int Id { get; set; }
        public DateTime BeginningTime { get; set; }
        public string BeginningTimeDate { get; set; }
        public string BeginningTimeHour
        {
            get
            {
                return BeginningTime.ToString("hh:mm tt");
            }
        }
        public DateTime? EndTime { get; set; }
        public string EndTimeDate { get; set; }
        public string EndTimeHour
        {
            get
            {
                if (EndTime.HasValue)
                    return EndTime.Value.ToString("hh:mm tt");
                else
                    return string.Empty;
            }
        }
    }
}
