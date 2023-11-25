using System.ComponentModel.DataAnnotations;

namespace Projekt_MVC.Models
{
    public class Dyskusja
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Podaj temat dyskusji")]
        public string Temat { get; set;}

        [Required(ErrorMessage = "Napisz swój problem")]
        public string Opis { get; set;}

        public List<string>? Odpowiedzi { get; set;}
    }
}
