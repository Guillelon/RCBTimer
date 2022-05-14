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
        public DateTime BeginningTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string CommentsfromEmployee { get; set; }

        public virtual Workday Workday { get; set; }
        public int WorkdayId { get; set; }
    }
}
