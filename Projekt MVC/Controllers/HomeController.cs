using Microsoft.AspNetCore.Mvc;
using Projekt_MVC.Data;
using Projekt_MVC.Models;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

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

        public IActionResult Dyskusja(int id)
        {
            Dyskusja Dyskusja = null;

            foreach (var dyskusja in BazaDanych.dyskusja)
            {
                if(dyskusja.DyskusjaId == id)
                {
                    Dyskusja = dyskusja;
                    break;
                }
            }

            //Dyskusja Dyskusja = BazaDanych.dyskusja.FirstOrDefault(i => i.DyskusjaId == id);

            //Console.WriteLine("asdaidaids: " + Dyskusja.DyskusjaId);
            if(Dyskusja != null)
            {
                Console.WriteLine("WOOOWOWOWOWOWOWOW");
                User Użytkownik = Dyskusja.Owner;
                string Temat = Dyskusja.Temat;
                string Opis = Dyskusja.Opis;

                //Przekażemy te dane do widoku poprzez model
                Dyskusja model = new Dyskusja
                {
                    Owner = Użytkownik,
                    Temat = Temat,
                    Opis = Opis
                };

                return View("Dyskusja", model);
            }
            return View("Index");
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
            HttpContext.Session.Clear();
            return View("Logowanie");
        }

        [HttpPost]
        public IActionResult SprawdzanieDanychLogowania(string Email, string Password)
        {

                // Sprawdź, czy użytkownik o podanym emailu istnieje w bazie danych
                var existingUser = BazaDanych.user.FirstOrDefault(u => u.Email == Email);
                if (existingUser != null)
                {
                    // Jeśli użytkownik o podanym emailu istnieje, sprawdź, czy hasło jest poprawne
                    if (existingUser.Password == Password)
                    {
                        // Logowanie udane
                        HttpContext.Session.SetInt32("UserId", existingUser.UserId);

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

                BazaDanych.user.Add(user);
                BazaDanych.SaveChanges();
                HttpContext.Session.SetInt32("UserId", user.UserId);
                return RedirectToAction("Index");
            }

            return View("Rejestracja", user);
        }


        [HttpPost]
        public IActionResult TworzenieDyskusji(Dyskusja dyskusja)
        {
            Console.WriteLine("DISDKAIDJAIOS: " + dyskusja.DyskusjaId);

            if (ModelState.IsValid)
            {
                if (HttpContext.Session.GetInt32("UserId") != null)
                {
                    dyskusja.UserId = (int)HttpContext.Session.GetInt32("UserId");

                    BazaDanych.dyskusja.Add(dyskusja);
                    BazaDanych.SaveChanges();

                    var savedDyskusja = BazaDanych.dyskusja.FirstOrDefault(d => d.DyskusjaId == dyskusja.DyskusjaId);

                    return RedirectToAction("Dyskusja", new { id = savedDyskusja.DyskusjaId });
                }
                else
                {
                    return View("Index");
                }
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