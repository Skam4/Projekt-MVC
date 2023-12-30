using System.ComponentModel.DataAnnotations;

namespace Projekt_MVC.ViewModels
{
    public class DodawanieDyskusji
    {

        [Required(ErrorMessage = "Podaj temat dyskusji")]
        public string Temat { get; set; }

        [Required(ErrorMessage = "Napisz treść wiadomości")]
        public string Opis { get; set; }

    }
}
