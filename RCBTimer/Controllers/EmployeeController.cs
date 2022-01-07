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
    }
}