using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace beautyclinic_uni.Services
{
    public class AiBeautyService
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _config;

        public AiBeautyService(HttpClient http, IConfiguration config)
        {
            _http = http;
            _config = config;
            _http.Timeout = TimeSpan.FromSeconds(30);
        }

        public async Task<string> AskBeautyAI(string userMessage)
        {
            var apiKey = _config["AI:ApiKey"];
            var model = _config["AI:Model"];

            _http.DefaultRequestHeaders.Clear();
            _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
            _http.DefaultRequestHeaders.Add("User-Agent", "BeautyClinicUniApp");
            _http.DefaultRequestHeaders.Add("HTTP-Referer", "https://beautyclinic.com");
            _http.DefaultRequestHeaders.Add("X-Title", "BeautyClinicAI");

            var systemPrompt = @"
شما دستیار رسمی کلینیک زیبایی ملی مهارت هستید.
نام شما: MeliMaharatAI

قوانین:
- هرگز خود را هوش مصنوعی، مدل یا محصول شرکت خاص معرفی نکن.
- فقط در حوزه خدمات زیبایی پاسخ بده: بوتاکس، فیلر، لیزر، پوست، مو، لیفت، جوانسازی، کاشت مو و ...
- پاسخ‌ها رسمی، علمی، بلند و قابل انتشار در وب‌سایت کلینیک باشند.
- اگر سؤال خارج از حوزه زیبایی بود، پاسخ بده: 
  «این سؤال خارج از خدمات کلینیک زیبایی ملی مهارت است.»
- هیچ‌وقت هویت خود را تغییر نده.
";

            var body = new
            {
                model = model,
                messages = new object[]
                {
                    new { role = "system", content = systemPrompt },
                    new { role = "user", content = userMessage }
                },
                temperature = 0.6,
                max_tokens = 2500
            };

            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _http.PostAsync(
                    "https://openrouter.ai/api/v1/chat/completions",
                    content
                );

                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                        return "توکن وارد شده نامعتبر است.";

                    return $"خطا در دریافت پاسخ: {response.StatusCode} — {result}";
                }

                using var doc = JsonDocument.Parse(result);
                var answer = doc.RootElement
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString();

                return string.IsNullOrWhiteSpace(answer) ? "پاسخی دریافت نشد." : answer;
            }
            catch (TaskCanceledException)
            {
                return "درخواست به سرور هوش مصنوعی تایم‌اوت شد.";
            }
            catch (Exception ex)
            {
                return $"خطای سیستم: {ex.Message}";
            }
        }
    }
}
