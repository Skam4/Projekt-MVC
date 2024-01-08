using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projekt_MVC.Models
{
    public class Moderator
    {
        [Key]
        public int IdModeratora { get; set; }

        [Required]
        public int IdUzytkownika { get; set; }

        [Required]
        public int IdForum { get; set; }

        public virtual User Uzytkownik { get; set; }

        public virtual Forum Forum { get; set; }
    }

}
