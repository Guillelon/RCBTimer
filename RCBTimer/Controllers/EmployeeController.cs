using DAL.Models;
using DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RCBTimer.Controllers
{
    public class EmployeeController : Controller
    {
        private EmployeeRepository employeeRepository;

        public EmployeeController()
        {
            employeeRepository = new EmployeeRepository();
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

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Edit(int id)
        {
            var employee = employeeRepository.Get(id);
            return View("Create",employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Employee employee)
        {
            if(employee.FirstName != null && employee.FirstName.Length > 0 &&
               employee.LastName != null && employee.LastName.Length > 0 &&
               employee.Position != null && employee.Position.Length > 0 &&
               employee.NationalId != null && employee.NationalId.Length > 0)
            {
                if(employee.Id > 0)
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
            if(TempData["SuccessMessage"] != null)
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

        public ActionResult ProcessWorkDay(int id)
        {
            var result = employeeRepository.ProcessWorkDay(id);
            TempData["SuccessMessage"] = result;
            return RedirectToAction("Index");
        }

        public ActionResult ProcessBreak(int id)
        {
            var result = employeeRepository.ProcessBreak(id);
            TempData["SuccessMessage"] = result;
            return RedirectToAction("Index");
        }
    }
}