using LifeSure.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LifeSure.Controllers
{
    public class TestimonialController : Controller
    {
        private readonly LifeSureDbEntities db = new LifeSureDbEntities();
        public ActionResult Index()
        {
            var testimonialList = db.TblTestimonial.ToList();
            return View(testimonialList);
        }


        [HttpGet]
        public ActionResult AddTestimonial()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddTestimonial(TblTestimonial testimonial)
        {
            if (ModelState.IsValid)
            {
                db.TblTestimonial.Add(testimonial);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(testimonial);
        }

        public ActionResult Delete(int id)
        {
            var testimonial = db.TblTestimonial.Find(id);
            if (testimonial != null)
            {
                db.TblTestimonial.Remove(testimonial);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Admin");
        }

        [HttpGet]
        public ActionResult UpdateTestimonial(int id)
        {
            var testimonial = db.TblTestimonial.Find(id);
            if (testimonial == null)
            {
                return HttpNotFound();
            }
            return View(testimonial);
        }

        [HttpPost]
        public ActionResult UpdateTestimonial(TblTestimonial testimonial)
        {
            if (ModelState.IsValid)
            {
                db.Entry(testimonial).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(testimonial);
        }
    }
}