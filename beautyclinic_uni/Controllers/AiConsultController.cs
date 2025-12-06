using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace beautyclinic_uni.Controllers
{
    public class AiConsultController : Controller
    {
        private readonly string _apiKey;
        private readonly string _model;
        private readonly HttpClient _client;

        public AiConsultController(IConfiguration config, IHttpClientFactory httpClientFactory)
        {
            _apiKey = config["AI:ApiKey"];
            _model = config["AI:Model"];
            _client = httpClientFactory.CreateClient();
        }

        [HttpPost]
        public async Task<IActionResult> Ask([FromBody] UserMessage msg)
        {
            if (msg == null || string.IsNullOrWhiteSpace(msg.Message))
            {
                return Json(new { success = false, reply = "لطفاً پیام خود را وارد کنید." });
            }

            var systemPrompt = @"
شما دستیار رسمی کلینیک زیبایی ملی مهارت هستید. 
نام شما: MeliMaharatAI
هیچ‌گاه خود را مدل، ربات یا محصول شرکت خاص معرفی نکن.
فقط درباره خدمات زیبایی پاسخ بده و از حوزه خارج نشو.
پاسخ‌ها باید رسمی، دقیق و حرفه‌ای باشند.
";

            var requestBody = new
            {
                model = _model,
                messages = new[]
                {
                    new { role = "system", content = systemPrompt },
                    new { role = "user", content = msg.Message }
                },
                temperature = 0.6,
                max_tokens = 2000
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
                {
                    return Json(new { success = false, reply = $"خطا از سرور: {response.StatusCode} - {resultJson}" });
                }

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
