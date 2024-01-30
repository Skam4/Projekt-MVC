using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projekt_MVC.Data;
using Projekt_MVC.Models;

namespace Projekt_MVC.Controllers
{
    public class AdminController: Controller
    {

        ForumDB BazaDanych = new ForumDB();


        public IActionResult ZarzadzajUzytkownikami()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var user = BazaDanych.User.Include(x => x.Rola).FirstOrDefault(x => x.IdUzytkownika == userId);

            var wybranaSkorka = BazaDanych.Skin.FirstOrDefault(x => x.Id == user.SkinId);
            ViewBag.CurrentSkinCssFilePath = Url.Content(wybranaSkorka.CssPath);

            var listaUzytkownikow = BazaDanych.User.Include(x => x.Rola).ToList();

            return View("ZarzadzajUzytkownikami", listaUzytkownikow);
        }

        public IActionResult UsunUzytkownika(int id)
        {
            var userToDelete = BazaDanych.User.FirstOrDefault(r => r.IdUzytkownika == id);

            if (userToDelete != null)
            {
                BazaDanych.User.Remove(userToDelete);
                BazaDanych.SaveChanges();
            }
            return RedirectToAction("ZarzadzajUzytkownikami");
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
            return RedirectToAction("ZarzadzajUzytkownikami");
        }

        public IActionResult ZarzadzajKategoriami()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var user = BazaDanych.User.Include(x => x.Rola).FirstOrDefault(x => x.IdUzytkownika == userId);

            var wybranaSkorka = BazaDanych.Skin.FirstOrDefault(x => x.Id == user.SkinId);
            ViewBag.CurrentSkinCssFilePath = Url.Content(wybranaSkorka.CssPath);

            var listaKategorii = BazaDanych.Kategoria.ToList();

            return View("ZarzadzajKategoriami", listaKategorii);
        }

        public IActionResult UsunKategorie(int id)
        {
            var kategoriaToDelete = BazaDanych.Kategoria.FirstOrDefault(r => r.IdKategorii == id);

            if (kategoriaToDelete != null)
            {
                BazaDanych.Kategoria.Remove(kategoriaToDelete);
                BazaDanych.SaveChanges();
            }
            return RedirectToAction("ZarzadzajKategoriami");
        }

        public IActionResult DodajKategorie(string nazwa)
        {
            var kategoria = new Kategoria
            {
                Nazwa = nazwa,
            };

            BazaDanych.Kategoria.Add(kategoria);
            BazaDanych.SaveChanges();

            return RedirectToAction("ZarzadzajKategoriami");
        }

        public IActionResult UsunForum(int id)
        {
            var forumToDelete = BazaDanych.Forum.FirstOrDefault(r => r.IdForum == id);

            if(forumToDelete != null)
            {
                BazaDanych.Forum.Remove(forumToDelete);
                BazaDanych.SaveChanges();
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult DodajForum(string nazwa, string opis, int idKategorii)
        {
            var kategoria = BazaDanych.Kategoria.FirstOrDefault(r => r.IdKategorii == idKategorii);
            var uprawnienie = BazaDanych.UprawnienieAnonimowych.FirstOrDefault(r => r.IdUprawnienia == 1);

            var forum = new Forum
            {
                Nazwa = nazwa,
                Opis = opis,
                Kategoria = kategoria,
                UprawnienieAnonimowych = uprawnienie,
                LiczbaWatkow = 0,
                LiczbaWiadomosci = 0
            };

            BazaDanych.Forum.Add(forum);
            BazaDanych.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult EdytujForum(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var user = BazaDanych.User.Include(x => x.Rola).FirstOrDefault(r => r.IdUzytkownika == userId);
            var rola = user.Rola.Nazwa;
            var kategorie = BazaDanych.Kategoria.ToList();

            var forum = BazaDanych.Forum.Include(x => x.Kategoria).FirstOrDefault(r => r.IdForum == id);

            var wybranaSkorka = BazaDanych.Skin.FirstOrDefault(x => x.Id == user.SkinId);
            ViewBag.CurrentSkinCssFilePath = Url.Content(wybranaSkorka.CssPath);

            ViewBag.Rola = rola;
            ViewBag.Kategorie = kategorie;

            return View("EdytujForum", forum);
        }

        public IActionResult EdycjaForum(int id, string nazwa, string opis, int idKategorii)
        {
            var forum = BazaDanych.Forum.FirstOrDefault(r => r.IdForum == id);
            var kategoria = BazaDanych.Kategoria.FirstOrDefault(r => r.IdKategorii == idKategorii);

            forum.Nazwa = nazwa;
            forum.Opis = opis;
            forum.Kategoria = kategoria;

            BazaDanych.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult EdytujOdpowiedz(int IdOdpowiedzi)
        {
            return View("EdytujOdpowiedz", IdOdpowiedzi);
        }

        public IActionResult UsunOdpowiedz(int IdOdpowiedzi, int IdDyskusji)
        {
            var odpowiedzToDelete = BazaDanych.Odpowiedz.FirstOrDefault(r => r.OdpowiedzId == IdOdpowiedzi);
            var dyskusja = BazaDanych.Dyskusja.FirstOrDefault(r => r.DyskusjaId == IdDyskusji);

            if (odpowiedzToDelete != null)
            {
                dyskusja.LiczbaOdpowiedzi--;
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



        public IActionResult ZarzadzajOgloszeniami()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var user = BazaDanych.User.Include(x => x.Rola).FirstOrDefault(x => x.IdUzytkownika == userId);

            var wybranaSkorka = BazaDanych.Skin.FirstOrDefault(x => x.Id == user.SkinId);
            ViewBag.CurrentSkinCssFilePath = Url.Content(wybranaSkorka.CssPath);

            var ogloszenie = BazaDanych.Ogloszenie.ToList();
            return View(ogloszenie);
        }

        [HttpPost]
        public IActionResult DodajOgloszenie(string tresc)
        {
            var ogloszenie = new Ogloszenie
            {
                Tresc = tresc,
                DataDodania = DateTime.Now
            };

            BazaDanych.Ogloszenie.Add(ogloszenie);
            BazaDanych.SaveChanges();

            return RedirectToAction("ZarzadzajOgloszeniami");
        }

        public IActionResult UsunOgloszenie(int id)
        {
            var ogloszenie = BazaDanych.Ogloszenie.Find(id);
            if (ogloszenie != null)
            {
                BazaDanych.Ogloszenie.Remove(ogloszenie);
                BazaDanych.SaveChanges();
            }
            return RedirectToAction("ZarzadzajOgloszeniami");
        }

        public IActionResult ZarzadzajZakazanymiSlowami()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var user = BazaDanych.User.Include(x => x.Rola).FirstOrDefault(x => x.IdUzytkownika == userId);

            var wybranaSkorka = BazaDanych.Skin.FirstOrDefault(x => x.Id == user.SkinId);
            ViewBag.CurrentSkinCssFilePath = Url.Content(wybranaSkorka.CssPath);

            var forbiddenWords = BazaDanych.ZakazaneSlowa.ToList();
            return View("ZarzadzajZakazanymiSlowami", forbiddenWords);
        }

        public IActionResult DodajZakazaneSlowo(string slowo)
        {
            var newForbiddenWord = new ZakazaneSlowo { Slowo = slowo };
            BazaDanych.ZakazaneSlowa.Add(newForbiddenWord);
            BazaDanych.SaveChanges();
            return RedirectToAction("ZarzadzajZakazanymiSlowami");
        }

        public IActionResult UsunZakazaneSlowo(int ZakazaneSlowoId)
        {
            var wordToRemove = BazaDanych.ZakazaneSlowa.FirstOrDefault(zs => zs.ZakazaneSlowoId == ZakazaneSlowoId);
            if (wordToRemove != null)
            {
                BazaDanych.ZakazaneSlowa.Remove(wordToRemove);
                BazaDanych.SaveChanges();
            }
            return RedirectToAction("ZarzadzajZakazanymiSlowami");
        }

        [HttpPost]
        public IActionResult PrzejdzDoZgloszonejDyskusji(int id)
        {
            return RedirectToAction("Dyskusja", "Home", new { id });

            //Można dodać usuwanie automatyczne przejrzanych odpowiedzi
        }

        [HttpPost]
        public IActionResult PrzypiszModeratora(int idForum, int idUzytkownika)
        {
            var forum = BazaDanych.Forum.Include(f => f.Moderatorzy).FirstOrDefault(f => f.IdForum == idForum);
            var user = BazaDanych.User.FirstOrDefault(u => u.IdUzytkownika == idUzytkownika);

            if (forum != null && user != null)
            {
                if (forum.Moderatorzy == null)
                    forum.Moderatorzy = new List<Moderator>();

                Moderator moderator = new Moderator();
                moderator.IdForum = idForum;
                moderator.Forum = forum;
                moderator.Uzytkownik = user;
                moderator.IdUzytkownika = idUzytkownika;


                forum.Moderatorzy.Add(moderator);
                BazaDanych.SaveChanges();
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult UsunDyskusje(int id)
        {
            var dyskusjaToDelete = BazaDanych.Dyskusja.FirstOrDefault(r => r.DyskusjaId == id);

/*            var forum = dyskusjaToDelete.Forum;

            forum.LiczbaWatkow = forum.LiczbaWatkow - 1;

            BazaDanych.Update(forum);*/

            if (dyskusjaToDelete != null)
            {
                BazaDanych.Dyskusja.Remove(dyskusjaToDelete);
                BazaDanych.SaveChanges();
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult UsunWiadomosc(int id)
        {
            var wiadomoscToDelete = BazaDanych.Wiadomosci.FirstOrDefault(w => w.Id == id);

            if (wiadomoscToDelete != null)
            {
                BazaDanych.Wiadomosci.Remove(wiadomoscToDelete);
                BazaDanych.SaveChanges();
            }

            return RedirectToAction("Wiadomosci", "Home");
        }

        [HttpPost]
        public IActionResult UsunZgloszonaOdpowiedz(int IdOdpowiedzi)
        {
            var odpowiedzToDelete = BazaDanych.Odpowiedz.FirstOrDefault(r => r.OdpowiedzId == IdOdpowiedzi);
            if (odpowiedzToDelete != null)
            {
                BazaDanych.Odpowiedz.Remove(odpowiedzToDelete);
                BazaDanych.SaveChanges();
            }
            return RedirectToAction("Wiadomosci", "Home");
        }



    }
}
