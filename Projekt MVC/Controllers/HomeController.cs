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

        public IActionResult Profil()
        {
            return View("Profil");
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
                var existingUser = BazaDanych.user.FirstOrDefault(u => u.Email == Email);
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
            Console.WriteLine("4");
            if (ModelState.IsValid)
            {
                Console.WriteLine("5");
                if (user.Password != confirmPassword)
                {
                    ModelState.AddModelError("ConfirmPassword", "Hasło i potwierdzenie hasła nie są identyczne.");
                    return View("Rejestracja", user);
                }

                BazaDanych.user.Add(user);
                BazaDanych.SaveChanges();
                return RedirectToAction("Index");
            }

            return View("Rejestracja", user);
        }


        [HttpPost]
        public IActionResult TworzenieDyskusji(Dyskusja dyskusja)
        {
            if (ModelState.IsValid)
            {
                BazaDanych.dyskusja.Add(dyskusja);
                BazaDanych.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("StwórzDyskusje");
        }



            [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}