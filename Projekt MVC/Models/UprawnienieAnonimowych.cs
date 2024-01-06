using System.ComponentModel.DataAnnotations;

namespace Projekt_MVC.Models
{
    public class UprawnienieAnonimowych
    {
        [Key]
        public int IdUprawnienia { get; set; }

        [Required]
        public string Nazwa { get; set; }

        [Required]
        public ICollection<Forum> Forum { get; set; }
    }
}
