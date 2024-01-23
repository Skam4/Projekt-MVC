using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projekt_MVC.Models
{
    public class Forum
    {
        [Key]
        public int IdForum { get; set; }

        [Required]
        public string Nazwa { get; set; }

        public string? Opis { get; set; }

        public int? LiczbaWatkow { get; set; }

        public int? LiczbaWiadomosci { get; set; }

        public virtual ICollection<Moderator>? Moderatorzy { get; set; }

        [Required]
        [ForeignKey("IdUprawnien")]
        public virtual UprawnienieAnonimowych UprawnienieAnonimowych { get; set; }

        [Required]
        [ForeignKey("IdKategorii")]
        public virtual Kategoria Kategoria { get; set; }

    }
}
