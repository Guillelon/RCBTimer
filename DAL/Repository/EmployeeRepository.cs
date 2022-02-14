using DAL.DTO;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class EmployeeRepository
    {
        private RCBTimerDBContext context;

        public EmployeeRepository()
        {
            context = new RCBTimerDBContext();
        }

        public IList<EmployeeList> GetAll()
        {
            var employees = context.Employee.Where(e=> e.IsActive).ToList();
            var dtos = new List<EmployeeList>();
            foreach (var employee in employees)
            {
                var dto = new EmployeeList();
                dto.NationalId = employee.NationalId;
                dto.Info = employee.FullInfo();
                dto.Id = employee.Id;
                dto.HasTimeRunning = employee.Workdays.Where(w => w.EndTime == null).Any();
                dtos.Add(dto);
            }
            return dtos;
        }

        public Employee Create(Employee employee)
        {
            context.Employee.Add(employee);
            context.SaveChanges();
            return employee;
        }

        public Employee Edit(Employee employee)
        {
            context.Entry(employee).State = EntityState.Modified;
            context.SaveChanges();
            return employee;
        }

        public IList<EmployeeList> GetAllForAdmin()
        {
            var employees = context.Employee.ToList();
            var dtos = new List<EmployeeList>();
            foreach (var employee in employees)
            {
                var dto = new EmployeeList();
                dto.Info = employee.FullName();
                dto.Active = employee.IsActive;
                dto.Id = employee.Id;
                dto.HasTimeRunning = employee.Workdays.Where(w => w.EndTime == null).Any();
                if (dto.HasTimeRunning)
                {
                    var workday = employee.Workdays.Where(w => w.EndTime == null).FirstOrDefault();
                    dto.Workday = workday;
                    dto.TimeBeginning = workday.BeginningTime.ToString("hh:mm tt");
                    dto.LastWorkDayId = workday.Id;
                }
                else
                {
                    var lastWorkDay = employee.Workdays.OrderByDescending(w => w.Id).FirstOrDefault();
                    if (lastWorkDay != null)
                    {
                        dto.LastWorkDayId = lastWorkDay.Id;
                        dto.Workday = lastWorkDay;
                    }
                        
                }
                dtos.Add(dto);
            }
            var sorted = dtos.OrderBy(w => w.Info).ToList();
            return sorted;
        }

        public Employee Get(int id)
        {
            return context.Employee.Where(e => e.Id == id).FirstOrDefault();
        }

        public IList<Employee> GetEmployeesByAutocomplete(string query)
        {

            return context.Employee.Where(w => w.IsActive && (w.FirstName.Contains(query) ||
                                               w.LastName.Contains(query))).ToList();
        }

        public EmployeeInfo GetEmployee(int id)
        {
            var employee = context.Employee.Where(e => e.Id == id).FirstOrDefault();
            var dto = new EmployeeInfo();
            var today = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
            if (employee != null)
            {
                dto.Name = employee.FullName();
                dto.Position = employee.Position;
                dto.Id = employee.Id;
                dto.HasTimeRunning = employee.Workdays.Where(w => w.EndTime == null).Any();
                dto.Workday = employee.Workdays.Where(w => w.EndTime == null).FirstOrDefault();
                if (dto.Workday != null)
                {
                    if (dto.Workday.BreakBeginningTime.HasValue)
                    {
                        if (!dto.Workday.BreakEndTime.HasValue)
                        {
                            dto.BreakEnded = false;
                            dto.HasBreakRunning = true;
                        }
                        else
                            dto.BreakEnded = true;
                    }
                    else
                        dto.HasBreakRunning = false;
                }
                else
                {
                    dto.Workday = employee.Workdays.Where(c => c.EndTime != null
                                                          && c.EndTime.Value.Date == today.Date)
                                                   .OrderByDescending(w => w.Id)
                                                   .FirstOrDefault();
                    if (dto.Workday != null)
                    {
                        dto.TimeEnded = true;
                        if (dto.Workday.BreakBeginningTime.HasValue)
                        {
                            if (!dto.Workday.BreakEndTime.HasValue)
                            {
                                dto.BreakEnded = false;
                                dto.HasBreakRunning = true;
                            }
                            else
                                dto.BreakEnded = true;
                        }
                        else
                            dto.HasBreakRunning = false;
                    }
                }

                var ci = new CultureInfo("es-ES");
                string todaysInfo = today.Day + " " + today.ToString("MMMM", ci) + " del " + today.ToString("yyyy");
                dto.TodaysInfo = todaysInfo;
                dto.TodaysHourInfo = today.ToShortTimeString();
                return dto;
            }

            return null;
        }
        
        public Employee DeActivate(int id, string userName)
        {
            var employee = Get(id);
            employee.DeActivate(userName);
            context.Entry(employee).State = EntityState.Modified;
            context.SaveChanges();
            return employee;
        }
    }
}
