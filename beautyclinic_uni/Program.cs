using beautyclinic_uni.Services;
using beautyclinic_uni.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// ==============================
// Services
// ==============================

// MVC
builder.Services.AddControllersWithViews();

// HttpClient (AI و سایر سرویس‌ها)
builder.Services.AddHttpClient<AiBeautyService>();
builder.Services.AddHttpClient();

// DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// ==============================
// Authentication (Cookie-based)
// ==============================
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/Login";
        options.ExpireTimeSpan = TimeSpan.FromDays(1);
        options.SlidingExpiration = true;
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
    });

builder.Services.AddAuthorization();

// ==============================
// Session (اختیاری – برای موارد غیر Auth)
// ==============================
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// ==============================
// Build app
// ==============================
var app = builder.Build();

// ==============================
// Middleware
// ==============================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();        // اگر جایی Session استفاده می‌کنی
app.UseAuthentication(); // 🔴 حتماً قبل از Authorization
app.UseAuthorization();

// ==============================
// Routes
// ==============================
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();
