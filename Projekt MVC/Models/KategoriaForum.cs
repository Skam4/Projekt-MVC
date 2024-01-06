using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projekt_MVC.Models
{
    public class KategoriaForum
    {
        [Key]
        public int IdKategoriiForum { get; set; }

        [Required]
        [ForeignKey("IdKategorii")]
        public Kategoria Kategoria { get; set; }

        [Required]
        [ForeignKey("IdForum")]
        public Forum Forum { get; set; }
    }
}
