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

            var dyskusja = BazaDanych.Dyskusja.FirstOrDefault(x => x.DyskusjaId == IdDyskusji);

            Odpowiedz odp = new Odpowiedz();
            odp.Tresc = odpowiedz;
            odp.Autor = user;
            odp.Dyskusja = dyskusja;
            odp.DataOdpowiedzi = DateTime.Now;

            BazaDanych.Odpowiedz.Add(odp);
            BazaDanych.SaveChanges();

            return RedirectToAction("Dyskusja", IdDyskusji);
        }

        public IActionResult Dyskusja(int id)
        {
            //Dyskusja Dyskusja = null;


            //po cholere to robić, skoro można to zrobić w jednej linijce
            /*foreach (var dyskusja in BazaDanych.dyskusja)
            {
                if(dyskusja.DyskusjaId == id)
                {
                    Dyskusja = dyskusja;
                    break;
                }
            }*/

            Dyskusja Dyskusja = BazaDanych.Dyskusja.Include(x => x.Wlasciciel).FirstOrDefault(i => i.DyskusjaId == id);

            if (Dyskusja != null)
            {

                //to też jest niepotrzebne
                /*User Użytkownik = Dyskusja.Owner;
                string Temat = Dyskusja.Temat;
                string Opis = Dyskusja.Opis;

                //Przekażemy te dane do widoku poprzez model
                Dyskusja model = new Dyskusja
                {
                    Owner = Użytkownik,
                    Temat = Temat,
                    Opis = Opis
                };*/

                return View("Dyskusja", Dyskusja);
            }
            return View("Index");
        }

        public IActionResult StwórzDyskusje(int ForumId)
        {
            ViewBag.ForumId = ForumId;
            return View("StwórzDyskusje");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        /*        public IActionResult Logowanie()
                {
                    HttpContext.Session.Clear();
                    return View("Logowanie");
                }

                [HttpPost]
                public IActionResult SprawdzanieDanychLogowania(string Email, string Password)
                {

                        // Sprawdź, czy użytkownik o podanym emailu istnieje w bazie danych
                        var existingUser = BazaDanych.User.FirstOrDefault(u => u.Email == Email);
                        if (existingUser != null)
                        {
                            // Jeśli użytkownik o podanym emailu istnieje, sprawdź, czy hasło jest poprawne
                            if (existingUser.Password == Password)
                            {
                                // Logowanie udane
                                HttpContext.Session.SetInt32("UserId", existingUser.UserId);

                                return RedirectToAction("Index"); // Przekierowanie do strony głównej po zalogowaniu
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
                }*/

        /*        [HttpPost]
                public IActionResult Rejestracja(User user, string confirmPassword)
                {
                    var czyEmailByl = BazaDanych.User.FirstOrDefault(d => d.Email == user.Email);

                    if (ModelState.IsValid && czyEmailByl == null)
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


                    return View("Rejestracja");
                }*/


        [HttpPost]
        public IActionResult TworzenieDyskusji(DodawanieDyskusji dyskusja)
        {

            if (ModelState.IsValid)
            {
                if (HttpContext.Session.GetInt32("UserId") != null)
                {
                    Dyskusja NowaDyskusja = new Dyskusja();

                    NowaDyskusja.Temat = dyskusja.Temat;
                    NowaDyskusja.Opis = dyskusja.Opis;
                    NowaDyskusja.Forum = BazaDanych.Forum.FirstOrDefault(f => f.IdForum == dyskusja.ForumId);

                    NowaDyskusja.Wlasciciel = BazaDanych.User.FirstOrDefault(u => u.IdUzytkownika == (int)HttpContext.Session.GetInt32("UserId"));

                    BazaDanych.Dyskusja.Add(NowaDyskusja);
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