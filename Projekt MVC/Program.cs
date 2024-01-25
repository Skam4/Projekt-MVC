using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Projekt_MVC.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    //options.Cookie.MaxAge = TimeSpan.FromMinutes(30);
    //options.Cookie.MaxAge = TimeSpan.FromSeconds(1);
    options.IdleTimeout = TimeSpan.FromMinutes(10);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();

app.Use(async (context, next) =>
{
    var userId = context.Session.GetInt32("UserId");


    if (userId != null)
    {
        context.Session.SetString("LastActivityTime", DateTime.UtcNow.ToString("o"));
    }

    var lastActivityTime = context.Session.GetString("LastActivityTime");

    if (!string.IsNullOrEmpty(lastActivityTime))
    {
        var sessionOptions = context.RequestServices.GetRequiredService<IOptions<SessionOptions>>().Value;
        var idleTimeout = sessionOptions.IdleTimeout;
        var lastActivity = DateTime.Parse(lastActivityTime);

        if (DateTime.UtcNow - lastActivity > idleTimeout)
        {
            // Wyloguj u¿ytkownika
            context.Session.Clear(); // Wyczyœæ sesjê
            context.Response.Redirect("/Rejestracja/Logowanie");
            return;
        }
    }

    await next();
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    //pattern: "{controller=Home}/{action=Logowanie}/{id?}");
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
