using LifeSure.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LifeSure.Controllers
{
    public class TeamController : Controller
    {
        private readonly LifeSureDbEntities db = new LifeSureDbEntities();
        public ActionResult Index()
        {
            var teamList = db.TblEmployee.ToList();
            return View(teamList);
        }

        [HttpGet]
        public ActionResult AddTeam()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddTeam(TblEmployee employee)
        {
            if (ModelState.IsValid)
            {
                db.TblEmployee.Add(employee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        public ActionResult Delete(int id)
        {
            var employee = db.TblEmployee.Find(id);
            if (employee != null)
            {
                db.TblEmployee.Remove(employee);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Admin");
        }

        [HttpGet]
        public ActionResult UpdateTeam(int id)
        {
            var employee = db.TblEmployee.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        [HttpPost]
        public ActionResult UpdateTeam(TblEmployee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(employee);
        }
    }
}