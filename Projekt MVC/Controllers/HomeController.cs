﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projekt_MVC.Data;
using Projekt_MVC.Models;
using Projekt_MVC.ViewModels;
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

            var listaZForami = BazaDanych.Forum.ToList();

            var userId = HttpContext.Session.GetInt32("UserId");

            ViewBag.UserId = userId;

            if(userId != null)
            {
                var user = BazaDanych.User.Include(x => x.Rola).FirstOrDefault(x => x.IdUzytkownika == userId);
                ViewBag.Rola = user.Rola.Nazwa;
                if(user.Rola.Nazwa == "admin")
                {
                    var kategorie = BazaDanych.Kategoria.ToList();
                    ViewBag.Kategorie = kategorie;
                }
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
                .FirstOrDefault(x => x.DyskusjaId == IdDyskusji);


            //BazaDanych.Entry(dyskusja).State = EntityState.Detached;

            Odpowiedz odp = new Odpowiedz
            {
                Tresc = odpowiedz,
                Autor = user,
                Dyskusja = dyskusja,
                DataOdpowiedzi = DateTime.Now
            };

            BazaDanych.Odpowiedz.Add(odp);

            dyskusja.LiczbaOdpowiedzi++;

            BazaDanych.SaveChanges();


            return RedirectToAction("Dyskusja", new { id = IdDyskusji });
        }


        public IActionResult Dyskusja(int id)
        {
            Dyskusja Dyskusja = BazaDanych.Dyskusja.Include(x => x.Wlasciciel).Include(x => x.Odpowiedzi).FirstOrDefault(i => i.DyskusjaId == id);

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

            if (ModelState.IsValid)
            {
                if (HttpContext.Session.GetInt32("UserId") != null)
                {
                    Dyskusja NowaDyskusja = new Dyskusja();

                    NowaDyskusja.Temat = dyskusja.Temat;
                    NowaDyskusja.Opis = dyskusja.Opis;
                    NowaDyskusja.Forum = BazaDanych.Forum.FirstOrDefault(f => f.IdForum == dyskusja.ForumId);
                    NowaDyskusja.Wlasciciel = user;

                    NowaDyskusja.Wlasciciel = BazaDanych.User.FirstOrDefault(u => u.IdUzytkownika == (int)HttpContext.Session.GetInt32("UserId"));

                    BazaDanych.Dyskusja.Add(NowaDyskusja);

                    user.Dyskusje.Add(NowaDyskusja);

                    BazaDanych.SaveChanges();



                    var savedDyskusja = BazaDanych.Dyskusja.FirstOrDefault(d => d.DyskusjaId == NowaDyskusja.DyskusjaId);

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