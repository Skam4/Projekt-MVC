using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projekt_MVC.Models
{
    public class User
    {
        [Key]
        [Required(ErrorMessage = "Identyfikator jest wymagany")]
        [Range(0, 9999)]
        public int Id_uzytkownika { get; set; }

        [Required(ErrorMessage = "Nazwa roli jest wymagana")]
        [StringLength(25, ErrorMessage = "Nazwa nie może przekraczać 25 znaków")]
        public string Nazwa { get; set; }

        [Required(ErrorMessage = "Nazwa roli jest wymagana")]
        [StringLength(255, ErrorMessage = "Hasło nie może przekraczać 255 znaków")]
        [MinLength(5, ErrorMessage = "Hasło jest za krótkie")]
        public string Haslo { get; set; }


        [Required(ErrorMessage = "Nazwa roli jest wymagana")]
        [StringLength(50, ErrorMessage = "Email nie może przekraczać 50 znaków")]
        public string Email { get; set; }


        [ForeignKey("DyskusjaId")]
        public ICollection<Dyskusja>? Dyskusje { get; set; }

        [ForeignKey("AutorId_uzytkownika")]
        public ICollection<Odpowiedz>? Odpowiedzi { get; set; }


        /*        [InverseProperty("PolubiajacyUzytkownicy")]
                public ICollection<Dyskusja>? PolubioneDyskusje { get; set; }*/

        public virtual Role Role { get; set; }
    }

}
