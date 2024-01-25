using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Projekt_MVC.Models
{
    public class Odpowiedz
    {
        public int OdpowiedzId { get; set; }

        [Required(ErrorMessage = "Napisz treść odpowiedzi")]
        public string Tresc { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DataOdpowiedzi { get; set; }

        public int DyskusjaId { get; set; }
        public virtual Dyskusja Dyskusja { get; set; }

        [Required(ErrorMessage = "Wybierz autora")]
        [ForeignKey("Autor")]
        public int AutorId { get; set; }
        public virtual User Autor { get; set; }

        public string? ZalacznikPath { get; set; }
    }
}
