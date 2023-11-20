using Microsoft.AspNetCore.Mvc;
using Projekt_MVC.Data;
using Projekt_MVC.Models;
using System.Diagnostics;

namespace Projekt_MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        ForumDB BazaDanych = new ForumDB();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult StwórzDyskusje()
        {
            return View("StwórzDyskusje");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Logowanie()
        {
            return View("Logowanie");
        }

        [HttpPost]
        public IActionResult SprawdzanieDanychLogowania(string Email, string Password)
        {

            Console.WriteLine("0");
                // Sprawdź, czy użytkownik o podanym emailu istnieje w bazie danych
                var existingUser = BazaDanych.User.FirstOrDefault(u => u.Email == Email);
                Console.WriteLine("1");
                if (existingUser != null)
                {
                    Console.WriteLine("2");
                    // Jeśli użytkownik o podanym emailu istnieje, sprawdź, czy hasło jest poprawne
                    if (existingUser.Password == Password)
                    {
                        Console.WriteLine("3");
                        // Logowanie udane
                        return View("Index"); // Przekierowanie do strony głównej po zalogowaniu
                    }
                    else
                    {
                        ModelState.AddModelError("Password", "Nieprawidłowe hasło.");
                    }
                }
                else
                {
                    ModelState.AddModelError("Email", "Użytkownik o podanym emailu nie istnieje.");
                }

            return View("Logowanie");
        }


        public IActionResult Rejestracja()
        {
            return View("Rejestracja");
        }

        [HttpPost]
        public IActionResult Rejestracja(User user, string confirmPassword)
        {
            if (ModelState.IsValid)
            {
                if (user.Password != confirmPassword)
                {
                    ModelState.AddModelError("ConfirmPassword", "Hasło i potwierdzenie hasła nie są identyczne.");
                    return View("Rejestracja", user);
                }

                BazaDanych.User.Add(user);
                BazaDanych.SaveChanges();
                return RedirectToAction("Index");
            }

            return View("Rejestracja", user);
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}