using LifeSure.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LifeSure.Controllers
{
    public class AboutController : Controller
    {
        private readonly LifeSureDbEntities db = new LifeSureDbEntities();
        public ActionResult Index()
        {
            var aboutList = db.TblAbout.ToList();
            return View(aboutList);
        }

        [HttpGet]
        public ActionResult AddAbout()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddAbout(TblAbout about)
        {
            if (ModelState.IsValid)
            {
                db.TblAbout.Add(about);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(about);
        }

        public ActionResult Delete(int id)
        {
            var about = db.TblAbout.Find(id);
            if(about != null)
            {
                db.TblAbout.Remove(about);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Admin");
        }

        [HttpGet]
        public ActionResult UpdateAbout(int id)
        {
            var about = db.TblAbout.Find(id);
            if (about == null)
            {
                return HttpNotFound();
            }
            return View(about);
        }

        [HttpPost]
        public ActionResult UpdateAbout(TblAbout about)
        {
            if (ModelState.IsValid)
            {
                db.Entry(about).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(about);
        }
    }
}