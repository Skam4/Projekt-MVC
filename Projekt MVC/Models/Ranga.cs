using System.ComponentModel.DataAnnotations;

namespace Projekt_MVC.Models
{
    public class Ranga
    {
        [Key]
        public int IdRangi { get; set; }

        [Required]
        public string Nazwa { get; set; }

        [Required]
        public int PotrzebnaLiczbaWiadomosci { get; set; }
    }
}
