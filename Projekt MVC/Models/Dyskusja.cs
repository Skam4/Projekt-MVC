using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Projekt_MVC.Models
{
    public class Dyskusja
    {
        [Key]
        public int DyskusjaId { get; set; }

        [Required(ErrorMessage = "Podaj temat dyskusji")]
        public string Temat { get; set; }

        [Required(ErrorMessage = "Napisz treść wiadomości")]
        public string Opis { get; set; }

        [Required]
        [ForeignKey("IdUzytkownika")]
        public virtual User Wlasciciel { get; set; }

        [Required]
        public virtual ICollection<Odpowiedz>? Odpowiedzi { get; set; }

        [Required]
        [ForeignKey("IdForum")]
        public virtual Forum? Forum { get; set; }

        public int? LiczbaOdwiedzen { get; set; }

        public int? LiczbaOdpowiedzi { get; set; }

        public bool? CzyPrzyklejony { get; set; }

        //public int LiczbaPolubien { get; set; }

        //public virtual ICollection<User>? PolubiajacyUzytkownicy { get; set; } // Użytkownicy, którzy polubili daną dyskusję
    }
}
