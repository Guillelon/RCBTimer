using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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

        [NotMapped]
        public bool OnBreak { get; set; }

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
            if (!BreakBeginningTime.HasValue)
                BreakBeginningTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
            else
                BreakEndTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
        }

        public int GetMinutesInBreak()
        {
            var minutes = 0;
            var now = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
            if (BreakBeginningTime.HasValue)
            {
                if (BreakEndTime.HasValue)
                {
                    var totalTimeStamp = (BreakEndTime.Value - BreakBeginningTime.Value);
                    minutes = Convert.ToInt32(totalTimeStamp.TotalMinutes);
                }
                else
                {
                    var totalTimeStamp = (now - BreakBeginningTime.Value);
                    minutes = Convert.ToInt32(totalTimeStamp.TotalMinutes);
                    OnBreak = true;
                }
            }
            return minutes;
        }

        public string GetHoursWorking()
        {
            var hours = 0;
            TimeSpan timestampA;
            TimeSpan timestampB;
            var now = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
            if (BreakBeginningTime.HasValue)
            {
                if (BreakEndTime.HasValue)
                {
                    if (EndTime.HasValue)
                    {
                        timestampA = (BreakBeginningTime.Value - BeginningTime);
                        timestampB = (EndTime.Value - BreakEndTime.Value);                        
                    }
                    else {
                        timestampA = (BreakBeginningTime.Value - BeginningTime);
                        timestampB = (now - BreakEndTime.Value);                        
                    }                    
                }
                else
                {
                    timestampA = (BreakBeginningTime.Value - BeginningTime);
                    timestampB = (now - now);
                }
            }
            else
            {
                if (EndTime.HasValue)
                {
                    timestampA = (EndTime.Value - BeginningTime);
                    timestampB = (now - now);
                }
                else
                {
                    timestampA = (now - BeginningTime);
                    timestampB = (now - now);
                }
            }
            var totalTimeStamp = timestampA + timestampB;
            return totalTimeStamp.Hours.ToString("00") + ":" + totalTimeStamp.Minutes.ToString("00");
        }
    }
}
