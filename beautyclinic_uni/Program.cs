using beautyclinic_uni.Services;
using Microsoft.EntityFrameworkCore;
using beautyclinic_uni.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

// HttpClient برای AiBeautyService
builder.Services.AddHttpClient<AiBeautyService>();
builder.Services.AddHttpClient(); // برای سایر درخواست‌ها

// Register DbContext با Connection String درست
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Enable Session برای ذخیره اطلاعات کاربر
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(1); // زمان اعتبار Session
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Build app
var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();         // فعال شدن Session
app.UseAuthentication();  // برای ورود آینده
app.UseAuthorization();

// Map default controller route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
