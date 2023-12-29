using System.ComponentModel.DataAnnotations;

namespace Projekt_MVC.ViewModels
{
    public class UserRejestracja
    {

        [Required(ErrorMessage = "Nazwa roli jest wymagana")]
        [StringLength(25, ErrorMessage = "Nazwa nie może przekraczać 25 znaków")]
        [MinLength(3, ErrorMessage = "Nazwa jest za krótka")]
        public string Nazwa { get; set; }

        [Required(ErrorMessage = "Nazwa roli jest wymagana")]
        [StringLength(255, ErrorMessage = "Hasło nie może przekraczać 255 znaków")]
        [MinLength(5, ErrorMessage = "Hasło jest za krótkie")]
        public string Haslo { get; set; }

        [Required(ErrorMessage = "Nazwa roli jest wymagana")]
        [StringLength(255, ErrorMessage = "Hasło nie może przekraczać 255 znaków")]
        [Compare("Haslo", ErrorMessage = "Hasło i potwierdzenie hasła nie są identyczne")]
        public string HasloConfirm { get; set; }


        [Required(ErrorMessage = "Nazwa roli jest wymagana")]
        [StringLength(50, ErrorMessage = "Email nie może przekraczać 50 znaków")]
        [EmailAddress]
        public string Email { get; set; }


    }
}
