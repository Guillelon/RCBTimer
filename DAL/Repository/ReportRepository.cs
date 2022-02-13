using DAL.DTO;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class ReportRepository
    {
        private RCBTimerDBContext context;

        public ReportRepository()
        {
            context = new RCBTimerDBContext();
        }

        public IList<Workday> GetWorkdaysByDate(DateTime beginTime,  DateTime endTime, int employeeId)
        {
            var dtos = new List<Workday>();
            if (employeeId > 0)
                dtos = context.Workday.Where(w => w.BeginningTime >= beginTime &&
                                              w.BeginningTime <= endTime && w.EmployeeId == employeeId)
                                  .ToList();
            else
                dtos = context.Workday.Where(w => w.BeginningTime >= beginTime &&
                                              w.BeginningTime <= endTime)
                                  .ToList();
            var sorted = dtos.OrderBy(w => w.BeginningTime.Date).ThenBy(w => w.Employee.FirstName).ToList();
            return sorted;
        }
    }
}
