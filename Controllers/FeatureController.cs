using LifeSure.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LifeSure.Controllers
{
    public class FeatureController : Controller
    {
        private readonly LifeSureDbEntities db = new LifeSureDbEntities();
        public ActionResult Index()
        {
            var featureList = db.TblFeature.ToList();
            return View(featureList);
        }

        [HttpGet]
        public ActionResult AddFeature()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddFeature(TblFeature feature)
        {
            if (ModelState.IsValid)
            {
                db.TblFeature.Add(feature);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(feature);
        }

        public ActionResult Delete(int id)
        {
            var feature = db.TblFeature.Find(id);
            if (feature != null)
            {
                db.TblFeature.Remove(feature);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Admin");
        }

        [HttpGet]
        public ActionResult UpdateFeature(int id)
        {
            var feature = db.TblFeature.Find(id);
            if (feature == null)
            {
                return HttpNotFound();
            }
            return View(feature);
        }

        [HttpPost]
        public ActionResult UpdateFeature(TblFeature feature)
        {
            if (ModelState.IsValid)
            {
                db.Entry(feature).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(feature);
        }
    }
}