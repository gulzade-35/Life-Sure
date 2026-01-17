using LifeSure.Models.DataModels;
using LifeSure.Models.ViewModels;
using LifeSure.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace LifeSure.Controllers
{
    public class FaqController : Controller
    {
        private readonly RapidApiChatGptService _chatService;
        private readonly LifeSureDbEntities db = new LifeSureDbEntities();

        public FaqController()
        {
            string apiKey = ConfigurationManager.AppSettings["ChatGptRapidApi_Key"];
            _chatService = new RapidApiChatGptService(apiKey);
        }

        [HttpGet]
        public ActionResult Generate()
        {
            return View(new FaqViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Generate(FaqViewModel model, string submitType)
        {
            if (submitType == "SoruOlustur")
            {
                var promptForQuestion = $"Sadece kısa ve net bir soru oluştur, başka bir açıklama yapma: {model.Prompt}";
                var question = await _chatService.SendMessageAsync(promptForQuestion);
                model.Question = question;

                var entity = new TblFaq { FaqQuestion = question };
                db.TblFaq.Add(entity);
                db.SaveChanges();
            }
            else if (submitType == "CevapOlustur")
            {
                // 1. En son eklenen soruyu al
                var entity = db.TblFaq.OrderByDescending(x => x.FaqId).FirstOrDefault();
                if (entity != null)
                {
                    // 2. Soruya göre cevap üret
                    var promptForAnswer = $"Sadece soruya cevap ver, tekrar soruyu veya açıklamayı yazma: {entity.FaqQuestion}";
                    var answer = await _chatService.SendMessageAsync(promptForAnswer);
                    model.Answer = answer;

                    // 3. Cevabı veritabanına yaz
                    entity.FaqAnsver = answer;
                    db.SaveChanges();
                }
            }

            ModelState.Clear();
            return View("Generate", model);
        }



    }
}