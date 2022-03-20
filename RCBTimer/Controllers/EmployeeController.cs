using DAL.DTO;
using DAL.Models;
using DAL.Repository;
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

        [Authorize]
        public ActionResult ListForAdmin()
        {
            var model = employeeRepository.GetAllForAdmin();
            if (TempData["SuccessMessage"] != null)
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"].ToString();
            }
            return View(model);
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
                    employeeRepository.Edit(employee);
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

        public ActionResult Info(int id)
        {
            var employee = employeeRepository.GetEmployee(id);
            return View(employee);
        }

        public ActionResult InfoV2(int id)
        {
            var employee = employeeRepository.GetEmployee(id);
            return View(employee);
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

        [HttpPost]
        public string ProcessWorkDay(string query)
        {
            var dto = new JavaScriptSerializer().Deserialize<WorkDayPost>(query);
            var result = workdayRepository.ProcessWorkDay(dto.Id, dto.Comments);
            return result;
        }

        [HttpPost]
        public string ProcessBreak(string query)
        {
            var dto = new JavaScriptSerializer().Deserialize<WorkDayPost>(query);
            var result = workdayRepository.ProcessBreak(dto.Id, dto.Comments);
            return result;
        }

        public ActionResult Tester()
        {
            return View();
        }
    }
}