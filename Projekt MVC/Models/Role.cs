using System.ComponentModel.DataAnnotations;

namespace Projekt_MVC.Models
{
    public class Role
    {
        [Key]
        [Required(ErrorMessage = "Identyfikator roli jest wymagany")]
        [Range(0, 5)]
        public int Id_roli { get; set; }

        [Required(ErrorMessage = "Nazwa roli jest wymagana")]
        [StringLength(10, ErrorMessage = "Nazwa roli nie może przekraczać 10 znaków")]
        public string Nazwa { get; set; }
        
        public virtual ICollection<User> Uzytkownicy { get; set; }
    }
}
