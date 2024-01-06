using System.ComponentModel.DataAnnotations;

namespace Projekt_MVC.Models
{
    public class Kategoria
    {
        [Key]
        public int IdKategorii { get; set; }

        [Required]
        public string Nazwa { get; set; }
    }
}
