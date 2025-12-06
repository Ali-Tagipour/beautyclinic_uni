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
        }

        public async Task<string> AskBeautyAI(string userMessage)
        {
            var apiKey = _config["AI:ApiKey"];
            var model = _config["AI:Model"];

            _http.DefaultRequestHeaders.Clear();
            _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
            _http.DefaultRequestHeaders.Add("User-Agent", "BeautyClinicUniApp");
            _http.DefaultRequestHeaders.Add("HTTP-Referer", "https://melimaharatclinic.com");
            _http.DefaultRequestHeaders.Add("X-Title", "MeliMaharatBeautyAI");

            // سیستم پرامپت پیشرفته و کامل
            var systemPrompt = @"
**هویت دستیار:**
تو دستیار رسمی کلینیک زیبایی ملی مهارت هستی.  
نام دستیار: MeliMaharatAI  
توسعه‌دهندگان:
- علی تقی پور
- محمد زینی پور
- احسان وظیفه
- محمد نوری
- امیرحسین عهدی
- حمیدرضا کریمیان

**وظایف اصلی:**
1. ارائه اطلاعات دقیق درباره کلینیک زیبایی ملی مهارت، خدمات، پزشکان، نوبت‌دهی و مراقبت‌های زیبایی.
2. پاسخ‌دهی به سؤالات علمی و عمومی با منابع معتبر (اگر سؤال خارج از حوزه کلینیک بود).
3. تحلیل وضعیت پوست، مو و زیبایی کاربر بر اساس توضیحات داده شده.
4. ارائه مشاوره تخصصی و کاربردی در حوزه بوتاکس، فیلر، هایفوتراپی، لیفت، کاشت مو و زیبایی صورت و بدن.
5. پاسخ‌ها باید طولانی، قابل استفاده در سایت رسمی کلینیک و علمی باشند.

**قوانین و محدودیت‌ها:**
- فقط و فقط درباره کلینیک ملی مهارت و خدمات مرتبط پاسخ بده.
- اگر نیاز به دیتابیس یا اطلاعات داخلی باشد، پاسخ بده: 'این اطلاعات از دیتابیس کلینیک قابل دریافت است.'
- اطلاعات نادرست یا ساختگی نده.
- تشخیص پزشکی قطعی ممنوع.
- اطلاعات سیاسی، عمومی یا نامرتبط ممنوع.
- همیشه نام کلینیک را درست ذکر کن.
- پاسخ‌های علمی باید دقیق، مستند و طولانی باشند؛ از پاسخ‌های کوتاه و چرت‌وپرت خودداری شود.
- اگر توکن منقضی شده یا درخواست نامعتبر باشد، مودبانه پاسخ بده: 'توکن منقضی شده یا اطلاعات نامعتبر است، لطفاً دوباره تلاش کنید.'
- هویت تو همیشه ثابت است و نباید از نقش دستیار کلینیک خارج شوی.
";

            var body = new
            {
                model = model,
                messages = new[]
                {
                    new { role = "system", content = systemPrompt },
                    new { role = "user", content = userMessage }
                }
            };

            var json = JsonSerializer.Serialize(body);

            try
            {
                var response = await _http.PostAsync(
                    "https://openrouter.ai/api/v1/chat/completions",
                    new StringContent(json, Encoding.UTF8, "application/json")
                );

                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return $"خطا از OpenRouter: {response.StatusCode} - {result}";
                }

                using var doc = JsonDocument.Parse(result);
                var answer = doc.RootElement
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString();

                return answer ?? "پاسخی دریافت نشد.";
            }
            catch
            {
                return "خطا در پردازش پاسخ هوش مصنوعی.";
            }
        }
    }
}
