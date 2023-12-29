using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Projekt_MVC.Models
{
    public class Dyskusja
    {
        [Key]
        public int DyskusjaId { get; set; }

        [ForeignKey("Id_uzytkownika")]
        public int? UserId { get; set; }

        [Required(ErrorMessage = "Podaj temat dyskusji")]
        public string Temat { get; set; }

        [Required(ErrorMessage = "Napisz treść wiadomości")]
        public string Opis { get; set; }

        public virtual User Owner { get; set; }

        [InverseProperty("PolubioneDyskusje")]
        public ICollection<User> PolubiajacyUzytkownicy { get; set; } // Użytkownicy, którzy polubili daną dyskusję
    }
}
