using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projekt_MVC.Models
{
    public class Ogloszenie
    {
        [Key]
        public int IdOgloszenia { get; set; }

        [Required]
        public string Tresc { get; set; }

        [Required]
        [ForeignKey("IdUzytkownika")]
        public virtual User Uzytkownik { get; set; }
    }
}
