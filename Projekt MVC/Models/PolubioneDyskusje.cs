using System.ComponentModel.DataAnnotations;

namespace Projekt_MVC.Models
{
    public class PolubioneDyskusje
    {
        [Key]
        public int PolubionaDyskusjaId { get; set; }

        public virtual User Owner { get; set; }

        public virtual Dyskusja Dyskusja { get; set;}
    }
}
