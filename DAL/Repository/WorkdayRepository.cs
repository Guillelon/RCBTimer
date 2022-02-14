using DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class WorkdayRepository
    {
        private RCBTimerDBContext context;

        public WorkdayRepository()
        {
            context = new RCBTimerDBContext();
        }

        public string ProcessWorkDay(int id, string commentsFromEmployee)
        {
            var employee = context.Employee.Where(e => e.Id == id).FirstOrDefault();
            var workDay = employee.Workdays.Where(w => w.EndTime == null).FirstOrDefault();
            if (workDay != null)
            {
                workDay.CommentsfromEmployee = commentsFromEmployee;
                workDay.EndWorkDay();
                context.Entry(workDay).State = EntityState.Modified;
                context.SaveChanges();
            }
            else
            {
                workDay = new Workday();
                workDay.EmployeeId = employee.Id;
                workDay.CommentsfromEmployee = commentsFromEmployee;
                context.Workday.Add(workDay);
                context.SaveChanges();
            }
            return employee.FirstName + " te has registrado con éxito";
        }

        public string ProcessBreak(int id, string commentsFromEmployee)
        {
            var employee = context.Employee.Where(e => e.Id == id).FirstOrDefault();
            var workDay = employee.Workdays.Where(w => w.EndTime == null).FirstOrDefault();
            if (workDay != null)
            {
                workDay.CommentsfromEmployee = commentsFromEmployee;
                workDay.Break();
                context.Entry(workDay).State = EntityState.Modified;
                context.SaveChanges();
                return employee.FirstName + " te has registrado con éxito";
            }
            return null;
        }

        public Workday Get(int id)
        {
            return context.Workday.Where(w => w.Id == id).Include(w => w.Employee).FirstOrDefault();
        }

        public void Edit(Workday model)
        {
            context.Entry(model).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void Add(Workday model)
        {
            context.Workday.Add(model);
            context.SaveChanges();
        }

        public void Remove(int id)
        {
            var workday = Get(id);
            context.Entry(workday).State = EntityState.Deleted;
            context.SaveChanges();
        }
    }
}
