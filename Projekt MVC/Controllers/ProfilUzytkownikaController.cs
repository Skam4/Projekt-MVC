using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Projekt_MVC.Data;
using Projekt_MVC.Models;

namespace Projekt_MVC.Controllers
{
    public class ProfilUzytkownikaController : Controller
    {
        ForumDB BazaDanych = new ForumDB();
        /*private readonly IOptionsMonitor<SessionOptions> _sessionOptionsMonitor;

        public ProfilUzytkownikaController(IOptionsMonitor<SessionOptions> sessionOptionsMonitor)
        {
            _sessionOptionsMonitor = sessionOptionsMonitor;
        }*/

        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            var uzytkownik = BazaDanych.User.Include(u => u.Dyskusje).FirstOrDefault(x => x.IdUzytkownika == userId);

            if(uzytkownik != null)
            {
                ViewBag.UserId = userId;
                ViewBag.Nazwa = uzytkownik.Nazwa;
                ViewBag.Email = uzytkownik.Email;
                ViewBag.Rola = uzytkownik.Rola;
                ViewBag.Dyskusje = uzytkownik.Dyskusje;
            }

            return View("../Home/Profil");
        }

        public IActionResult WczytajDyskusje(Dyskusja dyskusja)
        {
            return View("Dyskusja", dyskusja);
        }

        [HttpPost]
        public IActionResult ZmienHaslo(string aktualneHaslo, string noweHaslo)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            var uzytkownik = BazaDanych.User.FirstOrDefault(x => x.IdUzytkownika == userId);

            if(uzytkownik == null) 
            {
               return View(); //W sumie nwm co tu dać, bo to nie powinno sie wydarzyc                
            }
            else if(uzytkownik != null)
            {
                if(uzytkownik.Haslo == aktualneHaslo) 
                {
                    //Aktualizacja nowego hasła
                    uzytkownik.Haslo = noweHaslo;

                    BazaDanych.SaveChanges();

                    TempData["SuccessMessage"] = "Hasło zostało pomyślnie zmienione.";
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Błędne aktualne hasło");
                }
            }

            return View();
        }

        /*[HttpPost]
        public IActionResult ZmienDane(string nowaNazwa, int timeSpan)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            var uzytkownik = BazaDanych.User.FirstOrDefault(x => x.IdUzytkownika == userId);

            var sessionOptions = _sessionOptionsMonitor.CurrentValue;

            if (uzytkownik == null)
            {
                return View(); //gdy użytkownik nie istnieje
            }
            else if (uzytkownik != null)
            {
                if(nowaNazwa != null)
                    uzytkownik.Nazwa = "NowaNazwaUzytkownika";
                else if(timeSpan != null)
                {
                    var minutes = Math.Max(1, timeSpan);
                    sessionOptions.IdleTimeout = TimeSpan.FromMinutes(minutes);
                }


                BazaDanych.SaveChanges();

                TempData["SuccessMessage"] = "Dane zostały pomyślnie zmienione.";
            }

            RedirectToAction("Index");
        }*/

    }
}
