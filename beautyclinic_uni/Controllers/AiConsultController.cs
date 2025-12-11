using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using beautyclinic_uni.Data;
using System.Linq;

namespace beautyclinic_uni.Controllers
{
    public class AiConsultController : Controller
    {
        private readonly string _apiKey;
        private readonly string _model;
        private readonly HttpClient _client;
        private readonly ApplicationDbContext _db;

        public AiConsultController(
            IConfiguration config,
            IHttpClientFactory httpClientFactory,
            ApplicationDbContext db)
        {
            _apiKey = config["AI:ApiKey"];
            _model = config["AI:Model"];
            _client = httpClientFactory.CreateClient();
            _db = db;
        }

        [HttpPost]
        public async Task<IActionResult> Ask([FromBody] UserMessage msg)
        {
            if (msg == null || string.IsNullOrWhiteSpace(msg.Message))
                return Json(new { success = false, reply = "لطفاً پیام خود را وارد کنید." });

            // نمونه: خواندن یک دیتای تستی از دیتابیس
            // مثلا گرفتن نام اولین کاربر جهت استفاده در پرامپت
            var userName = _db.Users.FirstOrDefault()?.Name ?? "کاربر";

            var systemPrompt = $@"
تو دستیار تخصصی و رسمی «کلینیک زیبایی ملی مهارت» هستی.
نام دستیار: MeliMaharatAI

هویت تو:
- متخصص پوست، زیبایی و درمان‌های غیرجراحی
- تحلیل وضعیت پوست و مو و ارائه روتین اختصاصی
- نویسنده محتوای علمی و رسمی کلینیک
- پاسخ‌ها باید معتبر، طولانی و مناسب انتشار در وب‌سایت کلینیک باشند

اطلاعات پایگاه داده:
- نام کاربر نمونه دیتابیس: {userName}

قوانین:
- خودت را هیچگاه مدل یا ربات شرکت دیگری معرفی نکن
- پاسخ کوتاه و سطحی نده
- تشخیص پزشکی قطعی نده
- همیشه نام کلینیک را ذکر کن
- سوال خارج از حوزه: ""متأسفم، این سؤال خارج از حوزه خدمات کلینیک زیبایی ملی مهارت است.""
";

            var requestBody = new
            {
                model = _model,
                messages = new[]
                {
                    new { role = "system", content = systemPrompt },
                    new { role = "user", content = msg.Message }
                },
                temperature = 0.7,
                max_tokens = 2500
            };

            var jsonBody = JsonSerializer.Serialize(requestBody);

            try
            {
                _client.DefaultRequestHeaders.Clear();
                _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
                _client.DefaultRequestHeaders.Add("HTTP-Referer", "https://beautyclinic.com");
                _client.DefaultRequestHeaders.Add("X-Title", "BeautyClinicAI");
                _client.DefaultRequestHeaders.Add("User-Agent", "BeautyClinicUniApp");

                var response = await _client.PostAsync(
                    "https://openrouter.ai/api/v1/chat/completions",
                    new StringContent(jsonBody, Encoding.UTF8, "application/json")
                );

                var resultJson = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return Json(new { success = false, reply = $"خطا از OpenRouter: {response.StatusCode} - {resultJson}" });

                using var doc = JsonDocument.Parse(resultJson);
                var answer = doc.RootElement
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString();

                return Json(new { success = true, reply = answer });
            }
            catch (System.Exception ex)
            {
                return Json(new { success = false, reply = $"خطای سرور: {ex.Message}" });
            }
        }
    }

    public class UserMessage
    {
        public string Message { get; set; }
    }
}
