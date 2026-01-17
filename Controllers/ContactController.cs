using LifeSure.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LifeSure.Controllers
{
    public class ContactController : Controller
    {
        private readonly LifeSureDbEntities db = new LifeSureDbEntities();
        public ActionResult Index()
        {
            var contactList = db.TblContact.ToList();
            return View(contactList);
        }

        [HttpGet]
        public ActionResult AddContact()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddContact(TblContact contact)
        {
            if (ModelState.IsValid)
            {
                db.TblContact.Add(contact);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(contact);
        }

        public ActionResult Delete(int id)
        {
            var contact = db.TblContact.Find(id);
            if (contact != null)
            {
                db.TblContact.Remove(contact);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Admin");
        }

        [HttpGet]
        public ActionResult UpdateContact(int id)
        {
            var contact = db.TblContact.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        [HttpPost]
        public ActionResult UpdateContact(TblContact contact)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contact).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(contact);
        }
    }
}