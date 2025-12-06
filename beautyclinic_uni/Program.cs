using beautyclinic_uni.Services;
using Microsoft.EntityFrameworkCore;
using beautyclinic_uni.Data; // مطمئن شو اسم پروژه درست باشه

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

// HttpClient برای AiBeautyService
builder.Services.AddHttpClient<AiBeautyService>();
builder.Services.AddHttpClient(); // برای سایر درخواست‌ها

// Register DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Enable Session (for login, admin panel, etc.)
builder.Services.AddSession();

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

app.UseSession();         // Session فعال شد
app.UseAuthentication();  // برای سیستم ورود آینده
app.UseAuthorization();

// Map default controller route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
