    using DAL.DTO;
using DAL.Models;
using DAL.Repository;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RCBTimer.Controllers
{

    [Authorize]
    public class ReportController : Controller
    {
        private ReportRepository reportRepository;
        private EmployeeRepository employeeRepository;

        public ReportController()
        {
            reportRepository = new ReportRepository();
            employeeRepository = new EmployeeRepository();
        }

        public ActionResult Workdays()
        {
            return View();
        }

        public ActionResult GetWorkDays(string beginDate, string endDate, int employeeId)
        {
            var beginDateFormatted = new DateTime();
            var endDateFormatted = new DateTime();
            try
            {
                beginDateFormatted = DateTime.ParseExact(beginDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch
            {
                //Just remove years in case
                beginDateFormatted = DateTime.Now.AddYears(-1);
            }

            try
            {
                endDateFormatted = DateTime.ParseExact(endDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                endDateFormatted = endDateFormatted.AddDays(1);
                endDateFormatted = endDateFormatted.AddMinutes(-1);
            }
            catch
            {
                endDateFormatted = DateTime.Now;
            }

            var workdays = reportRepository.GetWorkdaysByDate(beginDateFormatted, endDateFormatted, employeeId);
            TempData["WorkdaysToDownload"] = workdays;
            return PartialView("_WorkDays", workdays);
        }

        public ActionResult DownloadWorkdays()
        {
            if (TempData["WorkdaysToDownload"] != null)
            {
                var dtos = (List<Workday>)TempData["WorkdaysToDownload"];
                var lines = "Colaborador,Inicio del turno,Inicio del break,Fin del break," +
                    "Fin del turno,Tiempo Laborado,Comentarios,Comentarios del Empleado"+ Environment.NewLine;
                foreach(var dto in dtos)
                {
                    var line = dto.Employee.FullName() + "," + dto.BeginningTime.ToString("dd/MM/yyyy hh:mm tt") + ",";
                    if (dto.BreakBeginningTime.HasValue)
                        line += dto.BreakBeginningTime.Value.ToString("dd/MM/yyyy hh:mm tt") + ",";
                    else
                        line += ",";
                    if (dto.BreakEndTime.HasValue)
                        line += dto.BreakEndTime.Value.ToString("dd/MM/yyyy hh:mm tt") + ",";
                    else
                        line += ",";
                    if (dto.EndTime.HasValue)
                        line += dto.EndTime.Value.ToString("dd/MM/yyyy hh:mm tt") + ",";
                    else
                        line += ",";

                    var workedTime = dto.GetHoursWorking();
                    line += workedTime;

                    line += ","+ dto.Comments + "," + dto.CommentsfromEmployee + Environment.NewLine;
                    lines += line;
                }
                return File(new System.Text.UTF8Encoding().GetBytes(lines), "text/csv", "Horas.csv");
            }
            return null;
        }        
    }
}