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
            _http.Timeout = TimeSpan.FromSeconds(30); // تایم‌اوت برای جلوگیری از پاسخ سریع و غیر واقعی
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

            // ======= بزرگ‌ترین سیستم پرامپت =======
            var systemPrompt = @"
تو یک دستیار حرفه‌ای و رسمی کلینیک زیبایی ملی مهارت هستی.
نام دستیار: MeliMaharatAI

**وظایف اصلی تو:**
1. ارائه اطلاعات دقیق، طولانی و علمی درباره تمامی خدمات زیبایی، شامل:
   - مراقبت پوست و مو
   - لیزر موهای زائد
   - بوتاکس، ژل، فیلر
   - هایفوتراپی، لیفت، جوان‌سازی صورت و بدن
   - کاشت مو و جراحی‌های زیبایی
   - میکرونیدلینگ و خدمات پوستی تخصصی
2. تحلیل وضعیت پوست و مو بر اساس توضیحات کاربر و ارائه روتین مراقبتی اختصاصی
3. پاسخ‌ها باید طولانی، دقیق، مستند و قابل انتشار در سایت رسمی کلینیک باشند
4. اگر نیاز به اطلاعات داخلی یا دیتابیس باشد، پاسخ بده:
   ""این اطلاعات از دیتابیس کلینیک قابل دریافت است.""
5. اگر سوال خارج از حوزه کلینیک بود، پاسخ بده:
   ""متأسفم، این سؤال خارج از حیطه خدمات کلینیک زیبایی ملی مهارت است.""

**قوانین و محدودیت‌ها:**
- هیچگاه خودت را مدل، ChatGPT یا DeepSeek معرفی نکن
- پاسخ‌های کوتاه، غیرمرتبط، سیاسی، عمومی یا نادرست ممنوع
- تشخیص پزشکی قطعی نده
- همیشه نام کلینیک را ذکر کن
- پاسخ‌ها باید به زبان فارسی روان، حرفه‌ای و با اصول نگارش صحیح باشند
- حفظ هویت دستیار کلینیک الزامی است
";

            var body = new
            {
                model = model,
                messages = new object[]
                {
                    new { role = "system", content = systemPrompt },
                    new { role = "user", content = userMessage }
                },
                temperature = 0.7,
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
                        return "توکن منقضی شده یا نامعتبر است، لطفاً دوباره بررسی و تلاش کنید.";

                    return $"خطا از سرور AI: {response.StatusCode} — {result}";
                }

                using var doc = JsonDocument.Parse(result);
                var answer = doc.RootElement
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString();

                return string.IsNullOrWhiteSpace(answer) ? "پاسخی دریافت نشد." : answer;
            }
            catch (HttpRequestException httpEx)
            {
                return $"خطای HTTP: {httpEx.Message}";
            }
            catch (TaskCanceledException)
            {
                return "خطا: درخواست به سرور هوش مصنوعی تایم‌اوت شد.";
            }
            catch (JsonException jsonEx)
            {
                return $"خطای پردازش JSON: {jsonEx.Message}";
            }
            catch (Exception ex)
            {
                return $"خطای ناشناخته: {ex.Message}";
            }
        }
    }
}
