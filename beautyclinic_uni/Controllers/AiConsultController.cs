using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using beautyclinic_uni.Data;
using System.Linq;
using System.Reflection; // اضافه شد برای Reflection

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

            string userName = "کاربر گرامی";

            try
            {
                var firstUser = _db.Users.FirstOrDefault();
                if (firstUser != null)
                {
                    // با Reflection هر فیلدی که شبیه نام بود رو پیدا می‌کنه
                    var userType = firstUser.GetType();
                    var nameProperty = userType.GetProperty("Name") ??
                                       userType.GetProperty("FullName") ??
                                       userType.GetProperty("FirstName") ??
                                       userType.GetProperty("LastName") ??
                                       userType.GetProperty("UserName") ??
                                       userType.GetProperty("Username");

                    if (nameProperty != null)
                    {
                        var value = nameProperty.GetValue(firstUser)?.ToString();
                        if (!string.IsNullOrWhiteSpace(value))
                        {
                            if (nameProperty.Name.Contains("First") || nameProperty.Name.Contains("Last"))
                            {
                                var first = userType.GetProperty("FirstName")?.GetValue(firstUser)?.ToString() ?? "";
                                var last = userType.GetProperty("LastName")?.GetValue(firstUser)?.ToString() ?? "";
                                userName = $"{first} {last}".Trim();
                                if (string.IsNullOrWhiteSpace(userName) || userName == " ")
                                    userName = value;
                            }
                            else
                            {
                                userName = value;
                            }
                        }
                    }
                }
            }
            catch
            {
                // اگه هر خطایی داد، بی‌خیال نام کاربر شو
                userName = "کاربر گرامی";
            }

            var systemPrompt = $@"
تو دستیار هوشمند و رسمی کلینیک زیبایی ملی مهارت هستی.
نام دستیار: ملی‌مهارت AI
لطفاً با نام کاربر شروع کن: {userName}

قوانین مهم:
- خودت را هیچگاه مدل هوش مصنوعی دیگر معرفی نکن
- پاسخ کامل، علمی و حداقل ۳۰۰-۴۰۰ کلمه بده
- همیشه نام کلینیک «کلینیک زیبایی ملی مهارت» را ذکر کن
- تشخیص پزشکی نده، فقط راهنمایی کن
- اگر سوال خارج از حوزه زیبایی و پوست بود بگو: متأسفم، این سوال خارج از حوزه خدمات کلینیک زیبایی ملی مهارت است.
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

            var json = JsonSerializer.Serialize(requestBody);

            try
            {
                _client.DefaultRequestHeaders.Clear();
                _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
                _client.DefaultRequestHeaders.Add("HTTP-Referer", "https://beautyclinic-uni.ir");

                var response = await _client.PostAsync(
                    "https://openrouter.ai/api/v1/chat/completions",
                    new StringContent(json, Encoding.UTF8, "application/json"));

                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return Json(new { success = false, reply = $"خطای API: {response.StatusCode}" });

                using var doc = JsonDocument.Parse(result);
                var answer = doc.RootElement
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString();

                return Json(new { success = true, reply = answer?.Trim() });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, reply = "خطای ارتباط با هوش مصنوعی: " + ex.Message });
            }
        }
    }

    public class UserMessage
    {
        public string Message { get; set; }
    }
}