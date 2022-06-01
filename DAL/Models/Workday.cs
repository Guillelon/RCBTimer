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
        public bool IsActive { get; set; }
        public DateTime? DeActivationDate { get; set; }
        public string DeActivationBy { get; set; }
        public DateTime BeginningTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime? BreakBeginningTime { get; set; }
        public DateTime? BreakEndTime { get; set; }
        public string Comments { get; set; }
        public string CommentsfromEmployee { get; set; }

        public virtual Employee Employee { get; set; }
        public int EmployeeId { get; set; }

        public virtual IList<Break> Breaks { get; set; }

        public Workday()
        {
            BeginningTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time"));
            IsActive = true;
        }

        public bool OnActiveBreak()
        {
            if (Breaks != null && Breaks.Count > 0)
            {
                if (Breaks.Where(b => b.EndTime.HasValue).Any())
                    return true;
            }

            return false;
        }

        public Break GetActiveBreak()
        {
            if (Breaks != null && Breaks.Count > 0)
            {
                var tBreak = Breaks.Where(b => !b.EndTime.HasValue).FirstOrDefault();
                if (tBreak != null)
                    return tBreak;
            }
            return null;
        }

        public void EndWorkDay()
        {
            EndTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time"));
        }

        public void Break()
        {
            if (!BreakBeginningTime.HasValue)
                BreakBeginningTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time"));
            else
                BreakEndTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time"));
        }

        public int GetMinutesInBreak()
        {
            var minutes = 0;
            var now = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time"));
            foreach (var tBreak in Breaks.Where(b => b.IsActive))
            {
                if (tBreak.EndTime.HasValue)
                {
                    var totalTimeStamp = (tBreak.EndTime.Value - tBreak.BeginningTime);
                    minutes += Convert.ToInt32(totalTimeStamp.TotalMinutes);
                }
                else
                {
                    var totalTimeStamp = (now - tBreak.BeginningTime);
                    minutes += Convert.ToInt32(totalTimeStamp.TotalMinutes);
                }
            }

            return minutes;
        }

        public string GetHoursWorking()
        {
            var hours = 0;
            TimeSpan timestampA;
            TimeSpan timestampB;
            var now = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time"));
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
            var totalTimeStamp = timestampA + timestampB;
            var breakTimeinMinutes = GetMinutesInBreak();
            totalTimeStamp = totalTimeStamp.Add(new TimeSpan(0, -(breakTimeinMinutes), 0));
            return totalTimeStamp.Hours.ToString("00") + ":" + totalTimeStamp.Minutes.ToString("00");
        }

        public void DeActivate(string userName)
        {
            DeActivationBy = userName;
            IsActive = false;
            DeActivationDate = DateTime.Now;
        }
    }
}
