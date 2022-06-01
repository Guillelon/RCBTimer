using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Break
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public DateTime? DeActivationDate { get; set; }
        public string DeActivationBy { get; set; }
        public DateTime BeginningTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string CommentsfromEmployee { get; set; }

        public virtual Workday Workday { get; set; }
        public int WorkdayId { get; set; }

        public Break()
        {
            IsActive = true;
            BeginningTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time"));
        }

        public Break(int workId)
        {
            BeginningTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time"));
            WorkdayId = workId;
        }

        public void Finish()
        {
            EndTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time"));
        }

        public void DeActivate(string userName)
        {
            DeActivationBy = userName;
            IsActive = false;
            DeActivationDate = DateTime.Now;
        }
    }
}
