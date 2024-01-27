using HtmlAgilityPack;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Projekt_MVC.Data;
using Projekt_MVC.Models;
using Projekt_MVC.ViewModels;
using System.Diagnostics;
using HtmlAgilityPack;

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

            if (!BazaDanych.Forum.Any())
            {
                var kategoria = new Kategoria { Nazwa = "Kategoria1" };
                var uprawnienieAnonimowych = new UprawnienieAnonimowych { Nazwa = "Uprawnienie1" };

                var fora = new List<Forum>
                {
                    new Forum { Nazwa = "Ogrodnictwo", Opis = "Forum dla miłośników ogrodnictwa", LiczbaWatkow = 0, LiczbaWiadomosci = 0, UprawnienieAnonimowych = uprawnienieAnonimowych, Kategoria = kategoria },
                    new Forum { Nazwa = "Smartfony", Opis = "Dla miłośników telefonów", LiczbaWatkow = 0, LiczbaWiadomosci = 0, UprawnienieAnonimowych = uprawnienieAnonimowych, Kategoria = kategoria },
                    new Forum { Nazwa = "Piłka Nożna", Opis = "Dla miłośników piłki nożnej", LiczbaWatkow = 0, LiczbaWiadomosci = 0, UprawnienieAnonimowych = uprawnienieAnonimowych, Kategoria = kategoria },
                    new Forum { Nazwa = "Muzyka", Opis = "Dla miłośników muzyki / zmieni się te opisy", LiczbaWatkow = 0, LiczbaWiadomosci = 0, UprawnienieAnonimowych = uprawnienieAnonimowych, Kategoria = kategoria },

                };


                BazaDanych.Forum.AddRange(fora);
                BazaDanych.SaveChanges();
            }
            if (!BazaDanych.Skin.Any())
            {
                var style = new List<Skin>
                {
                    new Skin {Nazwa = "Jasny", CssPath = "~/css/light.css"},
                    new Skin {Nazwa = "Ciemny", CssPath = "~/css/dark.css"},
                    new Skin {Nazwa = "Niebieski", CssPath = "~/css/blue.css"}
                };

                BazaDanych.Skin.AddRange(style);
                BazaDanych.SaveChanges();
            }

            var uzytkownicy = BazaDanych.User.ToList();

            ViewBag.LiczbaUżytkownikow = uzytkownicy.Count;

            var listaZForami = BazaDanych.Forum.ToList();

            var userId = HttpContext.Session.GetInt32("UserId");

            ViewBag.UserId = userId;

            if (userId != null)
            {
                var user = BazaDanych.User.Include(x => x.Rola).FirstOrDefault(x => x.IdUzytkownika == userId);

                var wybranaSkorka = BazaDanych.Skin.FirstOrDefault(x => x.Id == user.SkinId);
                ViewBag.CurrentSkinCssFilePath = Url.Content(wybranaSkorka.CssPath);

                ViewBag.Rola = user.Rola.Nazwa;
                if (user.Rola.Nazwa == "admin")
                {
                    var kategorie = BazaDanych.Kategoria.ToList();
                    ViewBag.Kategorie = kategorie;
                }
            }

            var ostatnieOgloszenie = BazaDanych.Ogloszenie.OrderByDescending(o => o.DataDodania).LastOrDefault();
            ViewBag.Ogloszenie = ostatnieOgloszenie != null ? ostatnieOgloszenie.Tresc : string.Empty;
            if (ostatnieOgloszenie != null)
            {
                ViewBag.OgloszenieData = ostatnieOgloszenie.DataDodania;
            }


            return View("Index", listaZForami);
        }


        public IActionResult PrzejdzDoFora(int id)
        {
            var forum = BazaDanych.Forum.FirstOrDefault(f => f.IdForum == id);

            if (forum == null)
            {
                return NotFound();
            }

            var listaDyskusji = BazaDanych.Dyskusja
                .Where(d => d.Forum != null && d.Forum.IdForum == id)
                .ToList();

            var userId = HttpContext.Session.GetInt32("UserId");

            ViewBag.UserId = userId;
            ViewBag.ForumId = id;

            if (userId != null)
            {
                var user = BazaDanych.User.Include(x => x.Rola).FirstOrDefault(x => x.IdUzytkownika == userId);

                var wybranaSkorka = BazaDanych.Skin.FirstOrDefault(x => x.Id == user.SkinId);
                ViewBag.CurrentSkinCssFilePath = Url.Content(wybranaSkorka.CssPath);

                ViewBag.Rola = user.Rola.Nazwa;
            }

            return View("Forum", listaDyskusji);
        }

        public IActionResult Profil()
        {
            return View("Profil");
        }

        public IActionResult NapiszOdpowiedz(int IdDyskusji, string odpowiedz)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var user = BazaDanych.User.FirstOrDefault(x => x.IdUzytkownika == userId);

            var dyskusja = BazaDanych.Dyskusja
                .Include(x => x.Wlasciciel)
                .Include(x => x.Odpowiedzi)
                .Include(x => x.Forum)
                .FirstOrDefault(x => x.DyskusjaId == IdDyskusji);


            //BazaDanych.Entry(dyskusja).State = EntityState.Detached;

            Odpowiedz odp = new Odpowiedz
            {
                Tresc = FilterHtml(odpowiedz),
                Autor = user,
                Dyskusja = dyskusja,
                DataOdpowiedzi = DateTime.Now
            };

            if (dyskusja.LiczbaOdpowiedzi == null)
            {
                dyskusja.LiczbaOdpowiedzi = 1;
            }
            else
            {
                dyskusja.LiczbaOdpowiedzi++;
            }

            var forum = BazaDanych.Forum.FirstOrDefault(f => f.IdForum == dyskusja.Forum.IdForum);
            forum.LiczbaWiadomosci++;

            BazaDanych.Odpowiedz.Add(odp);

            BazaDanych.SaveChanges();


            return RedirectToAction("Dyskusja", new { id = IdDyskusji });
        }


        public IActionResult Dyskusja(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var user = BazaDanych.User.Include(x => x.Rola).FirstOrDefault(x => x.IdUzytkownika == userId);

            var wybranaSkorka = BazaDanych.Skin.FirstOrDefault(x => x.Id == user.SkinId);
            ViewBag.CurrentSkinCssFilePath = Url.Content(wybranaSkorka.CssPath);

            Dyskusja Dyskusja = BazaDanych.Dyskusja.Include(x => x.Wlasciciel).FirstOrDefault(i => i.DyskusjaId == id);

            var odpowiedzi = BazaDanych.Odpowiedz
                .Include(x => x.Autor)
                .Where(x => x.Dyskusja.DyskusjaId == id)
                .ToList();

            Dyskusja.Odpowiedzi = odpowiedzi;

            if (Dyskusja != null)
            {
                if (Dyskusja.LiczbaOdwiedzen == null)
                {
                    Dyskusja.LiczbaOdwiedzen = 1;
                }
                else
                {
                    Dyskusja.LiczbaOdwiedzen++;
                }

                BazaDanych.SaveChanges();
                return View("Dyskusja", Dyskusja);
            }
            return View("Index");
        }


        public IActionResult StworzDyskusje(int ForumId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var user = BazaDanych.User.Include(x => x.Rola).FirstOrDefault(x => x.IdUzytkownika == userId);

            var wybranaSkorka = BazaDanych.Skin.FirstOrDefault(x => x.Id == user.SkinId);
            ViewBag.CurrentSkinCssFilePath = Url.Content(wybranaSkorka.CssPath);

            ViewBag.ForumId = ForumId;
            return View("StwórzDyskusje");
        }

        public IActionResult Privacy()
        {
            return View();
        }


        [HttpPost]
        public IActionResult TworzenieDyskusji(DodawanieDyskusji dyskusja)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var user = BazaDanych.User.FirstOrDefault(x => x.IdUzytkownika == userId);

            if (ModelState.IsValid && HttpContext.Session.GetInt32("UserId") != null)
            {
                Dyskusja nowaDyskusja = new Dyskusja
                {
                    Temat = dyskusja.Temat,
                    Opis = FilterHtml(dyskusja.Opis),
                    Forum = BazaDanych.Forum.FirstOrDefault(f => f.IdForum == dyskusja.ForumId),
                    Wlasciciel = user
                };

                BazaDanych.Dyskusja.Add(nowaDyskusja);
                user.Dyskusje.Add(nowaDyskusja);

                var forum = BazaDanych.Forum.FirstOrDefault(f => f.IdForum == dyskusja.ForumId);
                forum.LiczbaWatkow++;

                BazaDanych.SaveChanges();

                return RedirectToAction("Dyskusja", new { id = nowaDyskusja.DyskusjaId });
            }
            else
            {
                return View("Index");
            }
            return View("StwórzDyskusje");
        }

        public IActionResult WyslijWiadomosc()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            var user = BazaDanych.User.Include(x => x.Rola).FirstOrDefault(x => x.IdUzytkownika == userId);

            var wybranaSkorka = BazaDanych.Skin.FirstOrDefault(x => x.Id == user.SkinId);
            ViewBag.CurrentSkinCssFilePath = Url.Content(wybranaSkorka.CssPath);

            var users = BazaDanych.User.Where(u => u.IdUzytkownika != userId).ToList();

            ViewBag.Users = users;

            return View();
        }

        [HttpPost]
        public IActionResult WykonajWiadomosc(int odbiorcaId, string tresc)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            var wiadomosc = new Wiadomosc
            {
                NadawcaId = (int)userId,
                OdbiorcaId = odbiorcaId,
                Tresc = tresc,
                DataWyslania = DateTime.Now
            };

            BazaDanych.Wiadomosci.Add(wiadomosc);
            BazaDanych.SaveChanges();

            return RedirectToAction("Wiadomosci");
        }

        public IActionResult Wiadomosci()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            var user = BazaDanych.User.Include(x => x.Rola).FirstOrDefault(x => x.IdUzytkownika == userId);

            var wybranaSkorka = BazaDanych.Skin.FirstOrDefault(x => x.Id == user.SkinId);
            ViewBag.CurrentSkinCssFilePath = Url.Content(wybranaSkorka.CssPath);

            var wiadomosci = BazaDanych.Wiadomosci
                                .Where(w => w.NadawcaId == userId || w.OdbiorcaId == userId)
                                .Include(w => w.Nadawca)
                                .Include(w => w.Odbiorca)
                                .OrderByDescending(w => w.DataWyslania)
                                .ToList();
            ViewBag.ZgloszoneOdpowiedzi = BazaDanych.Odpowiedz
                .Where(w => w.ZgloszenieModeracji == true)
                .ToList();

            return View(wiadomosci);
        }

        [HttpPost]
        public IActionResult ZglosOdpowiedzDoModeracji(int IdOdpowiedzi)
        {
            var odpowiedz = BazaDanych.Odpowiedz.FirstOrDefault(r => r.OdpowiedzId == IdOdpowiedzi);

            if (odpowiedz != null)
            {
                odpowiedz.ZgloszenieModeracji = true;
                BazaDanych.SaveChanges();
            }

            return RedirectToAction("Dyskusja", new { id = odpowiedz.DyskusjaId });
        }

        [HttpPost]
        public IActionResult PrzejdzDoZgloszonejDyskusji(int id)
        {
            return RedirectToAction("Dyskusja", id);

            //Można dodać usuwanie automatyczne przejrzanych odpowiedzi
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // Funkcja do filtrowania niedozwolonych znaczników HTML
        private string FilterHtml(string htmlContent)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlContent);

            // Lista dozwolonych znaczników HTML
            var allowedTags = new List<string> { "b", "i", "u", "p", "a" };

            // Usuwanie znaczników spoza listy dozwolonych
            foreach (var node in doc.DocumentNode.SelectNodes("//node()"))
            {
                if (node.NodeType == HtmlNodeType.Element && !allowedTags.Contains(node.Name.ToLower()))
                {
                    node.Remove();
                }
            }

            return doc.DocumentNode.OuterHtml;
        }

    }
}