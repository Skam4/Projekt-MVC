using System.ComponentModel.DataAnnotations;

namespace Projekt_MVC.Models
{
    public class ZmianaHasla
    {
        [Required(ErrorMessage = "Podaj aktualne hasło")]
        [DataType(DataType.Password)]
        public string AktualneHaslo { get; set; }

        [Required(ErrorMessage = "Podaj nowe hasło")]
        [DataType(DataType.Password)]
        public string NoweHaslo { get; set; }

        [Compare("NewPassword", ErrorMessage = "Potwierdzenie hasła nie zgadza się z nowym hasłem")]
        [DataType(DataType.Password)]
        public string PotwierdzHaslo { get; set; }
    }

}
