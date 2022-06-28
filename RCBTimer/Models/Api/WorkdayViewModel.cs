using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RCBTimer.Models.Api
{
    public class WorkdayViewModel
    {
        public int WorkdayId { get; set; }
        public int EmployeeId { get; set; }
        public bool ProcessBreak { get; set; }
        public string Comments { get; set; }
        public string ImageInBase64 { get; set; }
    }
}