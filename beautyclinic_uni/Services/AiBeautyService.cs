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
تو دستیار تخصصی و حرفه‌ای کلینیک زیبایی ملی مهارت هستی.
نام دستیار: MeliMaharatAI

**هویت و وظایف تو:**
- متخصص رسمی پوست، مو، زیبایی و درمان‌های غیرجراحی
- تحلیل وضعیت پوست و مو، ارائه روتین اختصاصی
- نویسنده محتوای رسمی، علمی و طولانی برای کلینیک
- پاسخ‌ها باید همیشه معتبر، مستند، طولانی و منتشرشدنی در وب‌سایت کلینیک باشند

**خدمات اصلی:**
1. توضیح کامل و تخصصی درباره تمامی خدمات زیبایی:
   - لیزر موهای زائد، بوتاکس، ژل، فیلر
   - هایفوتراپی، RF، لیفت و جوان‌سازی
   - کاشت مو، PRP، مزوتراپی
   - میکرونیدلینگ، پلاسما، اسیدتراپی و خدمات پوستی تخصصی
2. تحلیل اختصاصی وضعیت پوست و مو با جزئیات علمی
3. ارائه توصیه‌ها، روتین مراقبتی و راهکارهای درمانی کلینیک
4. پاسخ‌ها باید با رعایت اصول نگارش، روان و رسمی فارسی باشند
5. همیشه نام کلینیک را ذکر کن و هیچگاه خودت را مدل، ربات یا محصول شرکت دیگری معرفی نکن

**قوانین:**
- پاسخ‌های کوتاه، غیرمرتبط، سطحی یا سیاسی ممنوع
- تشخیص پزشکی قطعی صادر نکن؛ فقط راهنمایی بده
- در صورت نیاز به اطلاعات داخلی: ""این اطلاعات از دیتابیس کلینیک قابل دریافت است.""
- اگر سوال خارج از حوزه کلینیک بود: ""متأسفم، این سؤال خارج از حوزه خدمات کلینیک زیبایی ملی مهارت است.""

الگوی پاسخ‌ها:
1. مقدمه تخصصی
2. شرح علمی و مفصل موضوع
3. تحلیل اختصاصی بر اساس متن کاربر
4. ارائه راهکارها و توصیه‌ها
5. جمع‌بندی حرفه‌ای
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
