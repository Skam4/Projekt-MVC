using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projekt_MVC.Models
{
    public class RangaUzytkownika
    {
        [Key]
        public int IdRangiUzytkownika { get; set; }

        [Required]
        [ForeignKey("IdRangi")]
        public Ranga Ranga { get; set; }

        [Required]
        [ForeignKey("IdUzytkownika")]
        public User Uzytkownik { get; set; }
    }
}
