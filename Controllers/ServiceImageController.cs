using LifeSure.Models.DataModels;
using LifeSure.Models.ViewModels;
using LifeSure.Services;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace LifeSure.Controllers
{
    public class ServiceImageController : Controller
    {
        private readonly LifeSureDbEntities db = new LifeSureDbEntities();

        public ActionResult Index()
        {
            return RedirectToAction("GenerateImage");
        }

        [HttpGet]
        public ActionResult GenerateImage()
        {
            return View(new ServiceViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GenerateImage(ServiceViewModel model)
        {
            // 1. Seçilen Başlığa Göre Yapay Zekaya Gidecek İngilizce Promptu Belirliyoruz
            string aiPrompt = "";

            // model.ServiceTitle senin View'dan gelen "HomeInsurance", "CarInsurance" gibi değerin olmalı
            switch (model.ServiceTitle)
            {
                case "HomeInsurance":
                    aiPrompt = "Modern minimalist family house with a beautiful garden, warm sunlight, professional architectural photography, 8k";
                    break;
                case "HealthInsurance":
                    aiPrompt = "Friendly doctor consulting with a patient in a bright modern luxury clinic, medical care, high quality photography";
                    break;
                case "LifeInsurance":
                    aiPrompt = "Happy family playing in a sunny park, soft natural lighting, cinematic emotional atmosphere, realistic photorealistic";
                    break;
                case "CarInsurance":
                    aiPrompt = "Modern silver car driving on a scenic coastal road at sunset, high speed motion, automotive photography, realistic";
                    break;
                default:
                    // Eğer listede yoksa kullanıcının yazdığı açıklamayı kullan
                    aiPrompt = model.ServiceDescription;
                    break;
            }

            // URL Hazırlığı (Yapay Zekaya aiPrompt gidiyor)
            string encodedPrompt = Uri.EscapeDataString(aiPrompt);
            var apiUrl = $"https://image.pollinations.ai/prompt/{encodedPrompt}?width=1024&height=1024&nologo=true&seed={new Random().Next(1, 100000)}";

            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMinutes(2);
                try
                {
                    var response = await client.GetAsync(apiUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        var bytes = await response.Content.ReadAsByteArrayAsync();
                        var fileName = $"{Guid.NewGuid()}.png";
                        var relativePath = "/Content/AiImages/" + fileName;
                        var serverPath = Server.MapPath(relativePath);

                        if (!Directory.Exists(Path.GetDirectoryName(serverPath))) Directory.CreateDirectory(Path.GetDirectoryName(serverPath));

                        System.IO.File.WriteAllBytes(serverPath, bytes);
                        model.ServiceImage = relativePath;

                        // 2. Veritabanına Kayıt (Resim bitti, şimdi anahtarları kaydediyoruz)
                        using (var dbEntities = new LifeSure.Models.DataModels.LifeSureDbEntities())
                        {
                            var entity = new LifeSure.Models.DataModels.TblService
                            {
                                // ÖNEMLİ: Veritabanına uzun açıklamayı değil, .resx içindeki ANAHTARI yazıyoruz
                                Title = model.ServiceTitle,       // "HomeInsurance"
                                Description = model.ServiceDescription, // "HomeInsurance" (Açıklama anahtarın farklıysa onu yaz)
                                ImageUrl = relativePath
                            };

                            dbEntities.TblService.Add(entity);
                            await dbEntities.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("ServiceDescription", "Görsel üretim sunucusu meşgul.");
                    }
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                        foreach (var validationError in validationErrors.ValidationErrors)
                            ModelState.AddModelError("", $"Veritabanı Hatası: {validationError.PropertyName}");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Hata: {ex.Message}");
                }
            }

            return View(model);
















            //// 1. Temel Giriş Kontrolü
            //if (string.IsNullOrWhiteSpace(model.ServiceDescription))
            //{
            //    ModelState.AddModelError("ServiceDescription", "Lütfen bir açıklama giriniz.");
            //    return View(model);
            //}

            //// Pollinations URL Hazırlığı (Boşlukları URL formatına çeviriyoruz)
            //string encodedPrompt = Uri.EscapeDataString(model.ServiceDescription);
            //// Seed ekleyerek her seferinde benzersiz görsel gelmesini sağlıyoruz
            //var apiUrl = $"https://image.pollinations.ai/prompt/{encodedPrompt}?width=1024&height=1024&nologo=true&seed={new Random().Next(1, 100000)}";

            //using (var client = new HttpClient())
            //{
            //    client.Timeout = TimeSpan.FromMinutes(2);

            //    try
            //    {
            //        // Görseli API'den çekiyoruz
            //        var response = await client.GetAsync(apiUrl);

            //        if (response.IsSuccessStatusCode)
            //        {
            //            var bytes = await response.Content.ReadAsByteArrayAsync();
            //            var fileName = $"{Guid.NewGuid()}.png";
            //            var relativePath = "/Content/AiImages/" + fileName;
            //            var serverPath = Server.MapPath(relativePath);

            //            // Klasör yoksa oluşturuyoruz
            //            var directory = Path.GetDirectoryName(serverPath);
            //            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

            //            // Dosyayı kaydediyoruz
            //            System.IO.File.WriteAllBytes(serverPath, bytes);
            //            model.ServiceImage = relativePath;

            //            // 2. Veritabanı Kayıt Aşaması
            //            using (var dbEntities = new LifeSure.Models.DataModels.LifeSureDbEntities())
            //            {
            //                var entity = new LifeSure.Models.DataModels.TblService
            //                {
            //                    Description = model.ServiceDescription,
            //                    ImageUrl = relativePath,
            //                    Title = model.ServiceTitle ?? "Yeni Hizmet" // Başlık boşsa hata vermemesi için varsayılan değer
            //                };

            //                dbEntities.TblService.Add(entity);
            //                await dbEntities.SaveChangesAsync();
            //            }
            //        }
            //        else
            //        {
            //            ModelState.AddModelError("ServiceDescription", "Görsel üretim sunucusu şu an meşgul, lütfen tekrar deneyin.");
            //        }
            //    }
            //    // 3. ÖZEL HATA YAKALAMA (EntityValidationErrors Çözümü)
            //    catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            //    {
            //        foreach (var validationErrors in dbEx.EntityValidationErrors)
            //        {
            //            foreach (var validationError in validationErrors.ValidationErrors)
            //            {
            //                // Hangi kolonun hata verdiğini ekrana basar (Örn: Description çok uzun)
            //                ModelState.AddModelError("ServiceDescription",
            //                    $"Veritabanı Hatası: {validationError.PropertyName} - {validationError.ErrorMessage}");
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        ModelState.AddModelError("ServiceDescription", $"Sistemsel Hata: {ex.Message}");
            //    }
            //}

            //// İşlem bitince modeli View'a geri gönderiyoruz ki üretilen resim sağ tarafta görünsün
            //return View(model);
        }
    }
}