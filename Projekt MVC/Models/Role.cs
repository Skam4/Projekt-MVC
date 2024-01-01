using System.ComponentModel.DataAnnotations;

namespace Projekt_MVC.Models
{
    public class Role
    {
        [Key]
        [Required(ErrorMessage = "Identyfikator roli jest wymagany")]
        [Range(0, 99999, ErrorMessage = "Identyfikator roli powinien być w zakresie od 0 do 99999")]
        public int IdRoli { get; set; }

        [Required(ErrorMessage = "Nazwa roli jest wymagana")]
        [StringLength(50, ErrorMessage = "Nazwa roli nie może przekraczać 50 znaków")]
        public string Nazwa { get; set; }

        public virtual ICollection<User> Uzytkownicy { get; set; }
    }
}
