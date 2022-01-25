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

        public ActionResult GetWorkDays(string beginDate, string endDate)
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

            var workdays = reportRepository.GetWorkdaysByDate(beginDateFormatted, endDateFormatted);
            TempData["WorkdaysToDownload"] = workdays;
            return PartialView("_WorkDays", workdays);
        }

        public ActionResult DownloadWorkdays()
        {
            if (TempData["WorkdaysToDownload"] != null)
            {
                var dtos = (List<WorkdaysReportDTO>)TempData["WorkdaysToDownload"];
                var lines = "Colaborador,Inicio del turno,Inicio del break,Fin del break,Fin del turno,Tiempo Laborado"+ Environment.NewLine;
                foreach(var dto in dtos)
                {
                    var line = dto.EmployeeInfo + "," + dto.BeginningTime.ToString("dd/MM/yyyy HH:mm") + ",";
                    if (dto.BreakBeginningTime.HasValue)
                        line += dto.BreakBeginningTime.Value.ToString("dd/MM/yyyy HH:mm") + ",";
                    else
                        line += ",";
                    if (dto.BreakEndTime.HasValue)
                        line += dto.BreakEndTime.Value.ToString("dd/MM/yyyy HH:mm") + ",";
                    else
                        line += ",";
                    if (dto.EndTime.HasValue)
                        line += dto.EndTime.Value.ToString("dd/MM/yyyy HH:mm") + ",";
                    else
                        line += ",";

                    if(dto.BreakBeginningTime.HasValue && dto.BreakEndTime.HasValue && dto.EndTime.HasValue)
                    {
                        var timestampA = (dto.BreakBeginningTime.Value - dto.BeginningTime);
                        var timestampB = (dto.EndTime.Value - dto.BreakEndTime.Value);
                        var totalTimeStamp = timestampA + timestampB;
                        dto.WorkedTime = totalTimeStamp.Hours.ToString() + ":" + totalTimeStamp.Minutes.ToString();
                        line += dto.WorkedTime;
                    }

                    line += Environment.NewLine;
                    lines += line;
                }
                return File(new System.Text.UTF8Encoding().GetBytes(lines), "text/csv", "Horas.csv");
            }
            return null;
        }

        public ActionResult EditWorkDay(int id)
        {
            var workday = employeeRepository.GetWorkday(id);
            return View(workday);
        }

        [HttpPost]
        public ActionResult EditWorkDay(Workday model)
        {
            employeeRepository.EditWorkDay(model);
            ViewBag.SuccessMessage = "Se editó con éxito el turno";
            var workday = employeeRepository.GetWorkday(model.Id);
            return View(workday);
        }

        public ActionResult RemoveWorkDay(int id)
        {
            var model = employeeRepository.GetWorkday(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult RemoveWorkDay(Workday model)
        {
            employeeRepository.RemoveWorkDay(model.Id);
            return View("DeleteSuccess");
        }
    }
}