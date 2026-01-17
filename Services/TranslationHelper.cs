using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace LifeSure.Services
{
    public static class TranslationHelper
    {
        public static string AutoTranslate(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return "";

            // Sitenin o anki dilini al (tr, en vb.)
            var targetLang = System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;

            // Eğer kullanıcı zaten İngilizce bakıyorsa veya metin çok kısaysa çevirme
            if (targetLang == "en") return text;

            try
            {
                // Google Translate Ücretsiz API Linki
                var url = $"https://translate.googleapis.com/translate_a/single?client=gtx&sl=en&tl={targetLang}&dt=t&q={HttpUtility.UrlEncode(text)}";

                using (var client = new HttpClient())
                {
                    // Senkron çalıştırmak için .Result kullanıyoruz (View içinde kolay kullanım için)
                    var response = client.GetStringAsync(url).Result;

                    // Gelen karmaşık JSON içinden çevrilmiş metni ayıkla
                    var startIndex = response.IndexOf("\"") + 1;
                    var endIndex = response.IndexOf("\"", startIndex);
                    return response.Substring(startIndex, endIndex - startIndex);
                }
            }
            catch
            {
                // Bir hata olursa (internet kesintisi vb.) orijinal metni bas
                return text;
            }
        }
    }
}