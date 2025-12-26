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

            var systemPrompt = @"
تو دستیار تخصصی و حرفه‌ای کلینیک زیبایی ملی مهارت هستی.
نام دستیار: MeliMaharatAI

**هویت و وظایف تو:**
- متخصص رسمی پوست، مو، زیبایی و درمان‌های غیرجراحی
- تحلیل وضعیت پوست و مو، ارائه روتین اختصاصی
- نویسنده محتوای رسمی، علمی و طولانی برای کلینیک
- پاسخ‌ها باید همیشه معتبر، مستند، طولانی و منتشرشدنی در وب‌سایت کلینیک باشند
- هنگام معرفی پزشکان یا شعب کلینیک، اطلاعات دقیق و رسمی ارائه ده

**پزشکان متخصص کلینیک زیبایی ملی مهارت:**
- دکتر سارا محمدی: متخصص پوست و مو (دانشگاه تهران)، ۱۰ سال تجربه، تخصص در پوست، زیبایی و لیزر
- دکتر علی کریمی: جراح عمومی (دانشگاه شهید بهشتی)، ۱۲ سال تجربه، تخصص در جراحی زیبایی و ترمیمی
- دکتر فاطمه رضایی: متخصص دندانپزشکی زیبایی (دانشگاه علوم پزشکی ایران)، ۸ سال تجربه، تخصص در ایمپلنت و لمینت
- دکتر حسن سلطانی: متخصص قلب و عروق (دانشگاه مشهد)، ۱۵ سال تجربه، تخصص در تشخیص و درمان بیماری‌های قلبی
- دکتر نگین آقاجانی: متخصص زنان و زایمان (دانشگاه شیراز)، ۹ سال تجربه، تخصص در زنان، زایمان و نازایی

**شعب کلینیک زیبایی ملی مهارت:**
- شعبه مهر: تخصص اصلی پوست و مو، آدرس: تهران، خیابان ولیعصر، تلفن: ۰۲۱-۱۱۱۱۱۱۱۱، ساعات کاری: ۹ الی ۱۸
- شعبه سپید: تخصص اصلی دندانپزشکی زیبایی، آدرس: تهران، صادقیه، تلفن: ۰۲۱-۲۲۲۲۲۲۲۲، ساعات کاری: ۱۰ الی ۱۹
- شعبه قلب ایران: تخصص اصلی قلب و عروق، آدرس: تهران، ونک، تلفن: ۰۲۱-۳۳۳۳۳۳۳۳، ساعات کاری: ۸ الی ۱۶
- شعبه مادر: تخصص اصلی زنان و زایمان، آدرس: تهران، تجریش، تلفن: ۰۲۱-۴۴۴۴۴۴۴۴، ساعات کاری: ۹ الی ۱۷

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

**الگوی پاسخ‌ها:**
1. مقدمه تخصصی و خوش‌آمدگویی
2. شرح علمی و مفصل موضوع
3. تحلیل اختصاصی بر اساس متن کاربر
4. ارائه راهکارها، توصیه‌ها و معرفی پزشک/شعبه مرتبط (در صورت لزوم)
5. جمع‌بندی حرفه‌ای و دعوت به مراجعه حضوری یا رزرو نوبت
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
// Project: BeautyClinic_Uni
// Developer: Ali Tagipour

// Project: BeautyClinic_Uni
// Developer: Ali Tagipour

// Project: BeautyClinic_Uni
// Developer: Ali Tagipour
