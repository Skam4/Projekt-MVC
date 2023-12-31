using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Projekt_MVC.Models
{
    public class Odpowiedz
    {
        [Key]
        public int OdpowiedzId { get; set; }

        [Required(ErrorMessage = "Napisz treść odpowiedzi")]
        public string Tresc { get; set; }
        public DateTime DataOdpowiedzi { get; set; }


        /*        [ForeignKey("Dyskusja")]
                public int DyskusjaId { get; set; }*/

        public virtual Dyskusja Dyskusja { get; set; }

        public virtual User Autor { get; set; }
    }
}
