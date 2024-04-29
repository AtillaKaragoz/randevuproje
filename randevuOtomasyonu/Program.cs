using Microsoft.EntityFrameworkCore;
using randevuOtomasyonu.Models;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<RandevuprojeContext>(option => option.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
    ));
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.Cookie.HttpOnly = true;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // �ste�e ba�l�: �erez s�resi
            options.LoginPath = "/Login/Login"; // Kullan�c� giri� yapmad�ysa y�nlendirilecek adres
            options.LogoutPath = "/Login/Logout"; // Kullan�c� ��k�� yaparsa y�nlendirilecek adres
            options.AccessDeniedPath = "/Home/AccessDenied"; // Yetkisiz eri�im durumunda y�nlendirilecek adres
        });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
    
app.Run();
