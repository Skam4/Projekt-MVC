﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projekt_MVC.Models
{
    public class Dyskusja
    {
        [Key]
        public int DyskusjaId { get; set; }

        [ForeignKey("UserId")]
        public int? UserId { get; set; }

        [Required(ErrorMessage = "Podaj temat dyskusji")]
        public string Temat { get; set;}

        [Required(ErrorMessage = "Napisz treść wiadomości")]
        public string Opis { get; set;}

        //public List<string>? Odpowiedzi { get; set;}

        public virtual User? Owner { get; set; }
    }
}
