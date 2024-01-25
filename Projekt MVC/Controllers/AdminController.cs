using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projekt_MVC.Data;

namespace Projekt_MVC.Controllers
{
    public class AdminController: Controller
    {

        ForumDB BazaDanych = new ForumDB();


        public IActionResult ZarzadzajUżytkownikami()
        {
            var listaUzytkownikow = BazaDanych.User.Include(x => x.Rola).ToList();

            return View("ZarzadzajUżytkownikami", listaUzytkownikow);
        }

        public IActionResult UsunUzytkownika(int id)
        {
            var userToDelete = BazaDanych.User.FirstOrDefault(r => r.IdUzytkownika == id);

            if (userToDelete != null)
            {
                BazaDanych.User.Remove(userToDelete);
                BazaDanych.SaveChanges();
            }
            return RedirectToAction("ZarzadzajUżytkownikami");
        }

        public IActionResult MianujAdminem(int id)
        {
            var userToPromote = BazaDanych.User.FirstOrDefault(r => r.IdUzytkownika == id);
            var role = BazaDanych.Role.FirstOrDefault(r => r.Nazwa == "admin");

            if (userToPromote != null)
            {
                userToPromote.Rola = role;
                BazaDanych.SaveChanges();
            }
            return RedirectToAction("ZarzadzajUżytkownikami");
        }

        public IActionResult EdytujOdpowiedz(int IdOdpowiedzi)
        {
            return View("EdytujOdpowiedz", IdOdpowiedzi);
        }

        public IActionResult UsunOdpowiedz(int IdOdpowiedzi, int IdDyskusji)
        {
            var odpowiedzToDelete = BazaDanych.Odpowiedz.FirstOrDefault(r => r.OdpowiedzId == IdOdpowiedzi);

            if (odpowiedzToDelete != null)
            {
                BazaDanych.Odpowiedz.Remove(odpowiedzToDelete);
                BazaDanych.SaveChanges();
            }
            return RedirectToAction("Dyskusja", "Home", new { id = IdDyskusji });
        }

        public IActionResult ZedytowanaOdpowiedz(int IdOdpowiedzi, string Tresc)
        {
            var odpowiedz = BazaDanych.Odpowiedz.FirstOrDefault(x => x.OdpowiedzId == IdOdpowiedzi);

            odpowiedz.Tresc = Tresc;
            BazaDanych.SaveChanges();

            return View("Index");

            //return RedirectToAction("Dyskusja", ) TUTAJ TRZEBA POWRÓCIĆ DO DYSKUSJI
        }
    }
}
