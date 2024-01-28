﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Projekt_MVC.Data;
using Projekt_MVC.Models;

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

        public IActionResult ZarządzajKategoriami()
        {
            var listaKategorii = BazaDanych.Kategoria.ToList();

            return View("ZarządzajKategoriami", listaKategorii);
        }

        public IActionResult UsunKategorie(int id)
        {
            var kategoriaToDelete = BazaDanych.Kategoria.FirstOrDefault(r => r.IdKategorii == id);

            if (kategoriaToDelete != null)
            {
                BazaDanych.Kategoria.Remove(kategoriaToDelete);
                BazaDanych.SaveChanges();
            }
            return RedirectToAction("ZarządzajKategoriami");
        }

        public IActionResult DodajKategorie(string nazwa)
        {
            var kategoria = new Kategoria
            {
                Nazwa = nazwa,
            };

            BazaDanych.Kategoria.Add(kategoria);
            BazaDanych.SaveChanges();

            return RedirectToAction("ZarządzajKategoriami");
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
    }
}
