using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projekt_MVC.Models
{
    public class Moderator
    {
        [Key]
        public int IdModeratora { get; set; }

        [Required]
        [ForeignKey("IdUzytkownika")]
        public User Uzytkownik { get; set; }

        [Required]
        [ForeignKey("IdForum")]
        public Forum Forum { get; set; }
    }
}
