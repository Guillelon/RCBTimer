using DAL.DTO;
using DAL.Models;
using DAL.Repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace RCBTimer.Controllers
{
    public class EmployeeController : Controller
    {
        private EmployeeRepository employeeRepository;
        private WorkdayRepository workdayRepository;

        public EmployeeController()
        {
            employeeRepository = new EmployeeRepository();
            workdayRepository = new WorkdayRepository();
        }

        public ActionResult Landing()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("ListForAdmin");
            else
                return RedirectToAction("Today","Workday",null);
        }

        [Authorize]
        public ActionResult ListForAdmin()
        {
            if (TempData["SuccessMessage"] != null)
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"].ToString();
            }
            return View();
        }

        public string GetDataforListForAdmin()
        {
            var model = employeeRepository.GetAllForAdmin();
            return JsonConvert.SerializeObject(model, Formatting.Indented,
                    new JsonSerializerSettings
                    {
                        DateFormatHandling = DateFormatHandling.IsoDateFormat
                    });
        }

        [Authorize]
        public string GetEmployeesForAutoComplete(string query)
        {
            var employees = employeeRepository.GetEmployeesByAutocomplete(query);
            return new JavaScriptSerializer().Serialize(employees.Select(e => new
            {
                e.Id,
                Name =
                e.FirstName + " " + e.LastName
            }));
        }

        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            var employee = employeeRepository.Get(id);
            return View("Create", employee);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Employee employee)
        {
            if (employee.FirstName != null && employee.FirstName.Length > 0 &&
               employee.LastName != null && employee.LastName.Length > 0 &&
               employee.Position != null && employee.Position.Length > 0 &&
               employee.NationalId != null && employee.NationalId.Length > 0)
            {
                if (employee.Id > 0)
                {
                    //employeeRepository.Edit(employee);
                    TempData["SuccessMessage"] = "Se editó el colaborador con éxito!";
                    return RedirectToAction("ListForAdmin");
                }
                else
                {
                    employeeRepository.Create(employee);
                    TempData["SuccessMessage"] = "Se creó el colaborador con éxito!";
                    return RedirectToAction("ListForAdmin");
                }
            }
            ViewBag.ErrorMessage = "Todos los campos son requeridos";
            return View(employee);
        }

        public ActionResult Index()
        {
            var model = employeeRepository.GetAll();
            if (TempData["SuccessMessage"] != null)
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"].ToString();
            }
            return View(model);
        }

        public string GetData(int id)
        {
            var employee = employeeRepository.GetEmployee(id);
            var json = new JavaScriptSerializer().Serialize(employee);
            return json;
        }

        public ActionResult DeActivate(int id)
        {
            var employee = employeeRepository.Get(id);
            return View(employee);
        }

        [Authorize]
        [HttpPost]
        public ActionResult DeActivate(Employee employee)
        {
            employeeRepository.DeActivate(employee.Id, User.Identity.Name);
            TempData["SuccessMessage"] = "Se eliminó el empleado con éxito";
            return RedirectToAction("ListForAdmin");
        }        

        public ActionResult Tester()
        {
            return View();
        }

        public string GetEmployee(int id)
        {
            var employee = employeeRepository.Get(id);
            var dto = new EmployeeDTO { 
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Position = employee.Position,
                NationalId = employee.NationalId
             };
            var json = new JavaScriptSerializer().Serialize(dto);
            return json;
        }

        [HttpPost]
        public string EditEmployee(string query)
        {
            var dto = new JavaScriptSerializer()
                      .Deserialize<EmployeeDTO>(query);
            employeeRepository.Edit(dto.Id, dto.FirstName, dto.LastName,
                                    dto.Position, dto.NationalId);
            return "200";
        }
    }
}