using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projekt_MVC.Data;
using Projekt_MVC.Models;

namespace Projekt_MVC.Controllers
{
    public class ProfilUzytkownikaController : Controller
    {
        ForumDB BazaDanych = new ForumDB();

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
            
            return View();
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
/*
        [HttpPost]
        public IActionResult ZmienDane(string aktualneHaslo, string noweHaslo, string potwierdzNoweHaslo)
        {
            var userId = HttpContext.Session.GetString("UserId");

            if (int.TryParse(userId, out int userIdInt))
            {
                var uzytkownik = BazaDanych.User.FirstOrDefault(x => x.Id_uzytkownika == userIdInt);

                if (uzytkownik == null)
                {
                    return View(); //gdy użytkownik nie istnieje
                }
                else if (uzytkownik != null)
                {
                    uzytkownik.Nazwa = "NowaNazwaUzytkownika";

                    BazaDanych.SaveChanges();

                    TempData["SuccessMessage"] = "Dane zostały pomyślnie zmienione.";
                }
            }

            return View();
        }*/

    }
}
