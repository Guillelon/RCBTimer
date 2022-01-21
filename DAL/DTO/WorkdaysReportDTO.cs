using System;
using System.Collections.Generic;
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
    }
}
