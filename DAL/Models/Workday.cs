using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Workday
    {
        public int Id { get; set; }
        public DateTime BeginningTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime? BreakBeginningTime { get; set; }
        public DateTime? BreakEndTime { get; set; }
        public string Comments { get; set; }
        public string CommentsfromEmployee { get; set; }

        public virtual Employee Employee { get; set; }
        public int EmployeeId { get; set; }

        public Workday()
        {
            BeginningTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
        }

        public void EndWorkDay()
        {
            EndTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
        }

        public void Break()
        {
            if(!BreakBeginningTime.HasValue)
                BreakBeginningTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
            else
                BreakEndTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
        }
    }
}
