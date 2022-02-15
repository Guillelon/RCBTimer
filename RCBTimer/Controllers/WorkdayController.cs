using DAL.Models;
using DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RCBTimer.Controllers
{
    public class WorkdayController : Controller
    {
        private EmployeeRepository employeeRepository;
        private WorkdayRepository workdayRepository;

        public WorkdayController()
        {
            employeeRepository = new EmployeeRepository();
            workdayRepository = new WorkdayRepository();
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
    }
}