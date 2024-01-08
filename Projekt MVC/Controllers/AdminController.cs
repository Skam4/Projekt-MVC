using Microsoft.AspNetCore.Mvc;
using Projekt_MVC.Data;

namespace Projekt_MVC.Controllers
{
    public class AdminController: Controller
    {

        ForumDB BazaDanych = new ForumDB();


        public IActionResult ZarzadzajDyskusjami()
        {
            var listaDyskusji = BazaDanych.Dyskusja.ToList();

            return View("ZarzadzajDyskusjami", listaDyskusji);
        }

        public IActionResult UsunDyskusje(int id)
        {
            var dyskusjaToDelete = BazaDanych.Dyskusja.FirstOrDefault(r => r.DyskusjaId == id);

            if (dyskusjaToDelete != null)
            {
                BazaDanych.Dyskusja.Remove(dyskusjaToDelete);
                BazaDanych.SaveChanges();
            }
            return RedirectToAction("ZarzadzajDyskusjami");
        }

        public IActionResult EdytujOdpowiedz(int IdOdpowiedzi)
        {
            return View("EdytujOdpowiedz", IdOdpowiedzi);
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
