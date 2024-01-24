using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using Projekt_MVC.Data;
using Projekt_MVC.Models;
using System.ComponentModel.DataAnnotations;
using Projekt_MVC.ViewModels;
using Microsoft.Extensions.Options;

namespace Projekt_MVC.Controllers
{
    public class RejestracjaController : Controller
    {
        private readonly IOptionsMonitor<SessionOptions> _sessionOptionsMonitor;

        ForumDB BazaDanych = new ForumDB();

        public RejestracjaController(IOptionsMonitor<SessionOptions> sessionOptionsMonitor)
        {
            _sessionOptionsMonitor = sessionOptionsMonitor;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Logowanie");
        }

        [HttpPost]
        public IActionResult Rejestracja(UserRejestracja user)
        {
            var czyEmailJest = BazaDanych.User.FirstOrDefault(d => d.Email == user.Email);

            if (czyEmailJest == null)
            {
                if (!ModelState.IsValid)
                {
                    return View(user);
                }

                if (ModelState.IsValid)
                {
                    User uzytkownicy = new User();

                    if (BazaDanych.Role.FirstOrDefault(r => r.Nazwa == "uzytkownik") == null)
                    {
                        Role role = new Role();
                        role.Nazwa = "uzytkownik";
                        BazaDanych.Role.Add(role);
                        BazaDanych.SaveChanges();
                        uzytkownicy.Rola = role;
                    }
                    else
                    {
                        Role role = BazaDanych.Role.FirstOrDefault(r => r.Nazwa == "uzytkownik");
                        uzytkownicy.Rola = role;
                    }

                    uzytkownicy.Haslo = BCrypt.Net.BCrypt.EnhancedHashPassword(user.Haslo);
                    uzytkownicy.Nazwa = user.Nazwa;
                    uzytkownicy.Email = user.Email;
                    uzytkownicy.LogoutTimeSpan = 5;
                    BazaDanych.User.Add(uzytkownicy);
                    BazaDanych.SaveChanges();
                    HttpContext.Session.SetInt32("UserId", uzytkownicy.IdUzytkownika);

                    return RedirectToAction("Logowanie");
                }
            }
            else
            {
                ModelState.AddModelError("Email", "Podany email już istnieje.");
                return View(user);
            }

            return RedirectToAction("Rejestracja");
        }


        public IActionResult Rejestracja()
        {
            return View("Rejestracja");
        }

        public IActionResult Logowanie()
        {
            HttpContext.Session.Clear();
            return View("Logowanie");
        }

        [HttpPost]
        public IActionResult SprawdzanieDanychLogowania(string Email, string Password)
        {

            // Sprawdź, czy użytkownik o podanym emailu istnieje w bazie danych
            var existingUser = BazaDanych.User.Include(u => u.Rola).FirstOrDefault(u => u.Email == Email);
            if (existingUser != null)
            {
                // Jeśli użytkownik o podanym emailu istnieje, sprawdź, czy hasło jest poprawne
                if (BCrypt.Net.BCrypt.EnhancedVerify(Password, existingUser.Haslo) == true)
                {
                    // Logowanie udane
                    HttpContext.Session.SetInt32("UserId", existingUser.IdUzytkownika);
                    HttpContext.Session.SetString("UserRole", existingUser.Rola.Nazwa);

                    var minutes = existingUser.LogoutTimeSpan;
                    _sessionOptionsMonitor.CurrentValue.IdleTimeout = TimeSpan.FromMinutes((double)minutes);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["BladLogowania"] = "Nieprawidłowa nazwa użytkownika lub hasło.";
                    ModelState.AddModelError("Password", "Nieprawidłowe hasło.");
                }
            }
            else
            {
                TempData["BladLogowania"] = "Nieprawidłowa nazwa użytkownika lub hasło.";
                ModelState.AddModelError("Email", "Użytkownik o podanym emailu nie istnieje.");
            }

            return View("Logowanie");
        }


    }
}
