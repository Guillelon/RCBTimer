using DAL.DTO;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Globalization;
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

        public string ProcessWorkDay(int id, int employeeId,string commentsFromEmployee)
        {
            var employee = context.Employee.Where(e => e.Id == employeeId).FirstOrDefault();
            var workDay = employee.Workdays.Where(w => w.IsActive && w.Id == id)
                                           .FirstOrDefault();
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

        public string ProcessBreak(int id, int employeeId,string commentsFromEmployee)
        {
            var employee = context.Employee.Where(e => e.Id == employeeId).FirstOrDefault();
            var workDay = context.Workday.Where(w => w.Id == id).FirstOrDefault();
            if (workDay != null)
            {
                var tBreak = workDay.GetActiveBreak();
                if (tBreak != null)
                {
                    tBreak.Finish();
                    context.Entry(tBreak).State = EntityState.Modified;
                }
                else
                {
                    tBreak = new Break(workDay.Id);
                    context.Break.Add(tBreak);
                }
                workDay.CommentsfromEmployee = commentsFromEmployee;
                context.Entry(workDay).State = EntityState.Modified;
                context.SaveChanges();
                return employee.FirstName + " te has registrado con éxito";
            }
            return null;
        }

        public Workday Get(int id)
        {
            return context.Workday.Where(w => w.Id == id)
                                  .Include(w => w.Breaks)
                                  .Include(w => w.Employee).FirstOrDefault();
        }

        public IList<WorkdayDTO> GetLast()
        {
            var query = "SELECT TOP 10  W.Id, E.FIRSTNAME + ' ' + E.LASTNAME as EmployeeInfo," +
                        "E.Position as EmployeePosition, W.BeginningTime, W.EndTime, W.Comments," +
                        "SUM(DATEDIFF(MINUTE, B.BeginningTime, B.EndTime)) AS BreakMinutes," +
                        "CONVERT(TIME, W.EndTime - W.BeginningTime) AS 'TotalWorkedTime' " +
                        "FROM Workdays W " +
                        "LEFT JOIN BREAKS B ON W.ID = B.WorkdayId and B.ISACTIVE = 1 " +
                        "LEFT JOIN Employees E ON W.EmployeeId = E.Id " +
                        "WHERE W.ISACTIVE = 1 " +
                        "GROUP BY W.Id, E.FIRSTNAME, E.LASTNAME,E.Position, " +
                                 "W.BeginningTime, W.EndTime, W.Comments," +
                                 "CONVERT(TIME, W.EndTime - W.BeginningTime) " +
                        "ORDER BY W.ID desc";
            var result = context.Database.SqlQuery<WorkdayDTO>(query).ToList();
            return result;
        }

        public WorkdayDTO GetById(int id)
        {
            var query = "SELECT W.Id, E.FIRSTNAME + ' ' + E.LASTNAME as EmployeeInfo," +
                        "E.Position as EmployeePosition, W.BeginningTime, W.EndTime, W.Comments," +
                        "SUM(DATEDIFF(MINUTE, B.BeginningTime, B.EndTime)) AS BreakMinutes," +
                        "CONVERT(TIME, W.EndTime - W.BeginningTime) AS 'TotalWorkedTime' " +
                        "FROM Workdays W " +
                        "LEFT JOIN BREAKS B ON W.ID = B.WorkdayId and B.ISACTIVE = 1 " +
                        "LEFT JOIN Employees E ON W.EmployeeId = E.Id " +                        
                        "WHERE W.ID = " + id + " AND W.ISACTIVE = 1 " +
                        "GROUP BY W.Id, E.FIRSTNAME, E.LASTNAME,E.Position, " +
                                 "W.BeginningTime, W.EndTime, W.Comments," +
                                 "CONVERT(TIME, W.EndTime - W.BeginningTime) ";
            var result = context.Database.SqlQuery<WorkdayDTO>(query).FirstOrDefault();
            return result;
        }

        public IList<WorkdayDTO> GetByDate(DateTime date)
        {
            var nextDay = date.AddDays(1).AddMinutes(-1);
            var query = "SELECT  W.Id, E.FIRSTNAME + ' ' + E.LASTNAME as EmployeeInfo," +
                       "E.Position as EmployeePosition, W.BeginningTime, W.EndTime, W.Comments," +
                       "SUM(DATEDIFF(MINUTE, B.BeginningTime, B.EndTime)) AS BreakMinutes," +
                       "CONVERT(TIME, W.EndTime - W.BeginningTime) AS 'TotalWorkedTime' " +
                       "FROM Workdays W " +
                       "LEFT JOIN BREAKS B ON W.ID = B.WorkdayId and B.ISACTIVE = 1 " +
                       "LEFT JOIN Employees E ON W.EmployeeId = E.Id " +
                       "WHERE W.ISACTIVE = 1 AND W.BeginningTime >= '" + date.ToString("s") + "' AND " +
                             "W.BeginningTime <= '" + nextDay.ToString("s") + "' " +
                       "GROUP BY W.Id, E.FIRSTNAME, E.LASTNAME,E.Position, " +
                                "W.BeginningTime, W.EndTime, W.Comments," +
                                "CONVERT(TIME, W.EndTime - W.BeginningTime) ";
            var result = context.Database.SqlQuery<WorkdayDTO>(query).ToList();
            return result;
        }

        public IList<WorkdayDTO> GetByEmployee(int id)
        {
            var query = "SELECT  W.Id, E.FIRSTNAME + ' ' + E.LASTNAME as EmployeeInfo," +
                       "E.Position as EmployeePosition, W.BeginningTime, W.EndTime, W.Comments," +
                       "SUM(DATEDIFF(MINUTE, B.BeginningTime, B.EndTime)) AS BreakMinutes," +
                       "CONVERT(TIME, W.EndTime - W.BeginningTime) AS 'TotalWorkedTime' " +
                       "FROM Workdays W " +
                       "LEFT JOIN BREAKS B ON W.ID = B.WorkdayId and B.ISACTIVE = 1 " +
                       "LEFT JOIN Employees E ON W.EmployeeId = E.Id " +
                       "WHERE W.ISACTIVE = 1 AND W.EmployeeId = " + id + " " +
                       "GROUP BY W.Id, E.FIRSTNAME, E.LASTNAME,E.Position, " +
                                "W.BeginningTime, W.EndTime, W.Comments," +
                                "CONVERT(TIME, W.EndTime - W.BeginningTime) " +
                       " ORDER BY W.ID DESC";
            var result = context.Database.SqlQuery<WorkdayDTO>(query).ToList();
            return result;
        }

        public void Edit(Workday model)
        {
            context.Entry(model).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void Add(Workday model)
        {
            if (model.BreakBeginningTime.HasValue)
            {
                model.Breaks = new List<Break>();
                var tBreak = new Break();
                tBreak.BeginningTime = model.BreakBeginningTime.Value;
                if (model.BreakEndTime.HasValue)
                    tBreak.EndTime = model.BreakEndTime.Value;
                model.Breaks.Add(tBreak);
            }               
            context.Workday.Add(model);
            context.SaveChanges();
        }

        public void Remove(int id)
        {
            var workday = Get(id);
            context.Entry(workday).State = EntityState.Deleted;
            context.SaveChanges();
        }

        public WorkdayForEmployeeDTO GetEmployeeV2(int id)
        {
            var employee = context.Employee.Where(e => e.Id == id).FirstOrDefault();
            var dto = new WorkdayForEmployeeDTO();
            var today = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time"));
            if (employee != null)
            {
                dto.EmployeeName = employee.FullName();
                dto.EmployeePosition = employee.Position;
                dto.EmployeeId = employee.Id;

                var workday = employee.Workdays.Where(w => w.IsActive && w.EndTime == null).FirstOrDefault();
                if (workday != null)
                {
                    dto.Id = workday.Id; 
                    dto.CommentsfromEmployee = workday.CommentsfromEmployee;
                    dto.BeginningTime = workday.BeginningTime;
                    dto.Breaks = workday.Breaks.Where(b => b.IsActive).Select(b => new BreakDTO
                    {
                        BeginningTime = b.BeginningTime,
                        EndTime = b.EndTime
                    }).ToList();
                    dto.HasTimeRunning = true;
                    if (dto.Breaks.Where(b => !b.EndTime.HasValue).Any())
                        dto.HasBreakRunning = true;
                }
                else
                {
                    dto.HasTimeRunning = false;
                    dto.BeginningTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time")); ;
                }

                var ci = new CultureInfo("es-ES");
                string todaysInfo = today.Day + " " + today.ToString("MMMM", ci) + " del " + today.ToString("yyyy");
                dto.TodaysInfo = todaysInfo;
                dto.TodaysHourInfo = today.ToShortTimeString();
                return dto;
            }

            return null;
        }

        public void EditV2(WorkdayToEdit dto)
        {
            var model = Get(dto.Id);
            model.BeginningTime = dto.BeginningTime;
            model.EndTime = dto.EndTime;
            foreach (var tBreak in dto.Breaks)
            {
                var mBreak = model.Breaks.Where(b => b.Id == tBreak.Id).FirstOrDefault();
                if (mBreak != null)
                {
                    mBreak.BeginningTime = tBreak.BeginningTime;
                    mBreak.EndTime = tBreak.EndTime;
                }
            }
            context.Entry(model).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void Delete(int id, string userName)
        {
            var model = Get(id);
            model.DeActivate(userName);
            foreach(var tBreak in model.Breaks)
            {
                tBreak.DeActivate(userName);
                context.Entry(tBreak).State = EntityState.Modified;
            }
            context.Entry(model).State = EntityState.Modified;
            context.SaveChanges();
        }
    }
}
