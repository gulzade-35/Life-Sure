using LifeSure.Models.DataModels;
using LifeSure.Models.ViewModels;
using LifeSure.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace LifeSure.Controllers
{
    public class HomeController : Controller
    {
        private readonly LifeSureDbEntities db = new LifeSureDbEntities();

        public async Task<ActionResult> Index()
        {
            //var serviceImages = db.TblService.ToList();

            var instagramService = new InstagramService();
            var twitterService = new TwitterService();
            try
            {
                var instagramFollowers = await instagramService.GetInstagramFollowerAsync("allianzturkiye");
                var twitterFollowers = await twitterService.GetTwitterFollowerAsync("AllianzTurkiye");

                ViewBag.InstagramFollower = instagramFollowers;
                ViewBag.TwitterFollower = twitterFollowers;
            }
            catch (Exception ex)
            {
                ViewBag.InstagramFollower = 0;
                ViewBag.Error = ex.Message;
            }
            

            return View();
        }
        public PartialViewResult PartialHead()
        {
            return PartialView();
        }
        public PartialViewResult PartialTopbar()
        {
            return PartialView();
        }
        public PartialViewResult PartialNavbar()
        {
            return PartialView();
        }
        public PartialViewResult PartialSlider()
        {
            return PartialView();
        }
        public PartialViewResult PartialFeature()
        {
            return PartialView();
        }
        public PartialViewResult PartialAbout()
        {
            return PartialView();
        }
        public PartialViewResult PartialService()
        {
            var values = db.TblService
                .OrderByDescending(x => x.ServiceId)
                .Take(4)
                .ToList();
            return PartialView(values);
        }
        public PartialViewResult PartialFAQ()
        {
            var faqs = db.TblFaq
                .OrderBy(f => f.FaqId)
                .Take(3)
                .Select(f => new FaqViewModel
                {
                    Question = f.FaqQuestion,
                    Answer = f.FaqAnsver
                })
                .ToList();
            return PartialView(faqs);
        }
        public PartialViewResult PartialTeam()
        {
            return PartialView();
        }
        public PartialViewResult PartialTestimonial()
        {
            return PartialView();
        }
        public PartialViewResult PartialFooter()
        {
            return PartialView();
        }
        public PartialViewResult PartialScript()
        {
            return PartialView();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}