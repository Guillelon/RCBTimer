using DAL.DTO;
using DAL.Models;
using DAL.Repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace RCBTimer.Controllers
{
    [Authorize]
    public class WorkdayController : Controller
    {
        private EmployeeRepository employeeRepository;
        private WorkdayRepository workdayRepository;
        private JsonSerializerSettings jsonConverterSettings;

        public WorkdayController()
        {
            employeeRepository = new EmployeeRepository();
            workdayRepository = new WorkdayRepository();
            var dateConverter = new Newtonsoft.Json.Converters.IsoDateTimeConverter
            {
                DateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm"
            };
            jsonConverterSettings = new JsonSerializerSettings();
            jsonConverterSettings.Converters.Add(dateConverter);
        }

        [AllowAnonymous]
        public ActionResult Today()
        {
            var model = employeeRepository.GetAll();
            if (TempData["SuccessMessage"] != null)
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"].ToString();
            }
            var json = new JavaScriptSerializer().Serialize(model);
            return View(model);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit(int id)
        {
            var workday = workdayRepository.Get(id);
            if(workday != null)
                return View(workday);
            else
                return View("DeleteSuccess");
        }

        public ActionResult Add(int? id = null)
        {
            var model = new Workday();
            if (id.HasValue)
            {
                var employee = employeeRepository.Get(id.Value);
                model.Employee = employee;
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Workday model)
        {
            var workday = new Workday();
            if (model.Id > 0)
            {
                workdayRepository.Edit(model);
                ViewBag.SuccessMessage = "Se editó con éxito el turno";
                workday = workdayRepository.Get(model.Id);
            }
            else
            {
                workdayRepository.Add(model);
                ViewBag.SuccessMessage = "Se agregó con éxito el turno";
                workday = workdayRepository.Get(model.Id);
            }
            return View(workday);
        }

        public ActionResult Remove(int id)
        {
            var model = workdayRepository.Get(id);
            if (model != null)
                return View(model);
            else
                return View("DeleteSuccess");
        }

        [HttpPost]
        public ActionResult Remove(Workday model)
        {
            workdayRepository.Remove(model.Id);
            return View("DeleteSuccess");
        }

        [HttpPost]
        [AllowAnonymous]
        public string ProcessWorkDay(string query)
        {
            var dto = new JavaScriptSerializer().Deserialize<WorkDayPost>(query);
            var result = workdayRepository.ProcessWorkDay(dto.Id, dto.EmployeeId, dto.Comments);
            return result;
        }

        [HttpPost]
        [AllowAnonymous]
        public string ProcessBreak(string query)
        {
            var dto = new JavaScriptSerializer().Deserialize<WorkDayPost>(query);
            var result = workdayRepository.ProcessBreak(dto.Id, dto.EmployeeId, dto.Comments);
            return result;
        }

        public string GetData()
        {
            var workdays = workdayRepository.GetLast();            
            return JsonConvert.SerializeObject(workdays, Formatting.Indented, jsonConverterSettings);
        }

        public string Search(string date, int? id)
        {

            DateTime? dateFormated = null;
            try
            {
                dateFormated = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch
            {
                //Today just in case
                dateFormated = null;
            }
            var workdays = workdayRepository.GetByDateOrEmployeeId(dateFormated, id);
            return JsonConvert.SerializeObject(workdays, Formatting.Indented, jsonConverterSettings);
        }

        [AllowAnonymous]
        public ActionResult FromEmployee(int id)
        {
            return View("FromEmployeeV2", id);
        }

        [AllowAnonymous]
        public string GetDataForEmployee(int id)
        {
            var employee = workdayRepository.GetEmployeeV2(id);
            var json = JsonConvert.SerializeObject(employee, Formatting.Indented,
                    new JsonSerializerSettings
                    {
                        DateFormatHandling = DateFormatHandling.IsoDateFormat
                    });
            return json;
        }

        public string GetWorkDayByEdit(int id)
        {
            var model = workdayRepository.Get(id);
            var dto = new WorkdayToEdit();
            dto.Id = model.Id;
            dto.EmployeeName = model.Employee.FullName();
            dto.BeginningTime = model.BeginningTime;
            dto.EndTime = model.EndTime;
            foreach(var tBreak in model.Breaks.Where(b => b.IsActive))
            {
                var breakDto = new BreakDTO();
                breakDto.Id = tBreak.Id;
                breakDto.BeginningTime = tBreak.BeginningTime;
                breakDto.EndTime = tBreak.EndTime;
                breakDto.ToDelete = false;
                dto.Breaks.Add(breakDto);
            }
            return JsonConvert.SerializeObject(dto, Formatting.Indented, jsonConverterSettings);
        }

        [HttpPost]
        public string EditWorkday(string query)
        {
            var userName = User.Identity.Name;
            var dto = new JavaScriptSerializer()
                      .Deserialize<WorkdayToEdit>(query);
            workdayRepository.EditV2(dto, userName);
            var toReturn = workdayRepository.GetById(dto.Id);
            return JsonConvert.SerializeObject(toReturn, Formatting.Indented, jsonConverterSettings);
        }

        public string DeleteWorkday(int id)
        {
            workdayRepository.Delete(id, User.Identity.Name);
            return id.ToString();
        }
    }
}