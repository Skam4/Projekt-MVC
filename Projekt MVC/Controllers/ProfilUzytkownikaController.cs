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
            var userId = HttpContext.Session.GetString("UserId");

            if (int.TryParse(userId, out int userIdInt))
            {
                var uzytkownik = BazaDanych.user.Include(u => u.Dyskusje).FirstOrDefault(x => x.UserId == userIdInt);

                ViewBag.User = uzytkownik;
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
            var userId = HttpContext.Session.GetString("UserId");

            if (int.TryParse(userId, out int userIdInt))
            {
                var uzytkownik = BazaDanych.user.FirstOrDefault(x => x.UserId == userIdInt);

                if(uzytkownik == null) 
                {
                    return View(); //W sumie nwm co tu dać, bo to nie powinno sie wydarzyc                
                }
                else if(uzytkownik != null)
                {
                    if(uzytkownik.Password == aktualneHaslo) 
                    {
                        //Aktualizacja nowego hasła
                        uzytkownik.Password = noweHaslo;

                        BazaDanych.SaveChanges();

                        TempData["SuccessMessage"] = "Hasło zostało pomyślnie zmienione.";
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Błędne aktualne hasło");
                    }
                }
            }

            return View();
        }

        [HttpPost]
        public IActionResult ZmienDane(string aktualneHaslo, string noweHaslo, string potwierdzNoweHaslo)
        {


            return View();
        }
    }
}
