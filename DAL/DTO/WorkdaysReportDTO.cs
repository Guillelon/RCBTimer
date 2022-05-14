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
        public DateTime BeginningTimeDate { get; set; }
        public string BeginningTimeHour { get; set; }
        public string EndTime { get; set; }
        public string BreakBeginningTime { get; set; }
        public string BreakEndTime { get; set; }
        public string WorkedTime { get; set; }
        public string Comments { get; set; }
        public string EmployeeComments { get; set; }

        public WorkdayDTO()
        {
        }
    }
}
