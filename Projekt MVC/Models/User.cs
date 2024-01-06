using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projekt_MVC.Models
{
    public class User
    {
        [Key]
        [Required(ErrorMessage = "Identyfikator użytkownika jest wymagany")]
        [Range(0, int.MaxValue, ErrorMessage = "Identyfikator użytkownika powinien być większy niż 0")]
        public int IdUzytkownika { get; set; }

        [Required(ErrorMessage = "Nazwa użytkownika jest wymagana")]
        [StringLength(25, ErrorMessage = "Nazwa użytkownika nie może przekraczać 25 znaków")]
        public string Nazwa { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane")]
        [StringLength(255, ErrorMessage = "Hasło nie może przekraczać 255 znaków")]
        [MinLength(5, ErrorMessage = "Hasło jest za krótkie")]
        public string Haslo { get; set; }

        [Required(ErrorMessage = "Adres email jest wymagany")]
        [StringLength(50, ErrorMessage = "Adres email nie może przekraczać 50 znaków")]
        [EmailAddress(ErrorMessage = "Nieprawidłowy format adresu email")]
        public string Email { get; set; }

        public virtual ICollection<Dyskusja> Dyskusje { get; set; }

        public virtual ICollection<Odpowiedz> Odpowiedzi { get; set; }

        public virtual ICollection<Ogloszenie> Ogloszenia { get; set; }


        //public int LiczbaPolubien { get; set; }

        [Required]
        [ForeignKey("IdRoli")]
        public virtual Role Rola { get; set; }

        public string? AvatarPath { get; set; }
    }

}
