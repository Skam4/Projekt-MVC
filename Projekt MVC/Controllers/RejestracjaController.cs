using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using Projekt_MVC.Data;
using Projekt_MVC.Models;
using System.ComponentModel.DataAnnotations;
using Projekt_MVC.ViewModels;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;

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

                    if (!BazaDanych.Role.Any(r => r.Nazwa == "admin"))
                    {
                        var adminRole = new Role { Nazwa = "admin" };
                        BazaDanych.Role.Add(adminRole);
                        BazaDanych.SaveChanges();
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

        public IActionResult PrzypomnijHaslo()
        {
            return View();
        }

        [HttpPost]
        public IActionResult PrzypomnijHaslo(string Email)
        {
            var existingUser = BazaDanych.User.FirstOrDefault(u => u.Email == Email);
            if (existingUser != null)
            {
                // Generowanie nowego hasła
                string newPassword = GenerateRandomPassword();

                // Aktualizacja hasła w bazie danych
                existingUser.Haslo = BCrypt.Net.BCrypt.EnhancedHashPassword(newPassword);
                BazaDanych.SaveChanges();

                // Wysyłanie e-maila z nowym hasłem
                SendPasswordResetEmail(Email, newPassword);

                TempData["Message"] = "Nowe hasło zostało wysłane na podany adres email.";
            }
            else
            {
                TempData["Error"] = "Podany adres email nie istnieje.";
            }

            return RedirectToAction("PrzypomnijHaslo");
        }

        private string GenerateRandomPassword()
        {
            // Generowanie losowego ciągu znaków jako nowe hasło
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var newPassword = new string(Enumerable.Repeat(chars, 8).Select(s => s[random.Next(s.Length)]).ToArray());
            return newPassword;
        }

        private void SendPasswordResetEmail(string recipientEmail, string newPassword)
        {
            string senderEmail = "trzebastworzycemaila@gmail.com"; // Twój adres e-mail
            string senderPassword = "niewpiszetuswojegohasla"; // Hasło do konta e-mail

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(senderEmail, senderPassword),
                EnableSsl = true,
            };

            // Utwórz wiadomość e-mail
            MailMessage mailMessage = new MailMessage(senderEmail, recipientEmail)
            {
                Subject = "Resetowanie hasła",
                Body = "Twoje nowe hasło: " + newPassword,
            };

            try
            {
                // Wyślij wiadomość
                smtpClient.Send(mailMessage);
                Console.WriteLine("Wiadomość e-mail została wysłana pomyślnie.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Wystąpił błąd podczas wysyłania wiadomości e-mail: " + ex.Message);
            }
        }


    }
}
