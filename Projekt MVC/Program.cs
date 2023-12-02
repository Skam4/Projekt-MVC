using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Projekt_MVC.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSession(options =>
{
    //options.IdleTimeout = TimeSpan.FromSeconds(5);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.MaxAge = TimeSpan.FromMinutes(30);
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
    var path = context.Request.Path;

    // Jeœli u¿ytkownik jest ju¿ na stronie logowania, nie wykonuj przekierowania
    if (path.StartsWithSegments("/Home/Logowanie"))
    {
        await next();
        return;
    }

    if (userId == null && !path.StartsWithSegments("/Home/Logowanie") && !path.StartsWithSegments("/Home/SprawdzanieDanychLogowania") && !path.StartsWithSegments("/Home/Rejestracja"))
    {
        // Brak sesji, przekieruj do strony logowania
        context.Response.Redirect("/Home/Logowanie");
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
