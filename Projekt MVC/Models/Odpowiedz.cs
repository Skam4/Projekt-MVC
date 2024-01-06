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

        /*[DataType(DataType.DateTime)]
        public DateTime DataOdpowiedzi { get; set; }*/

        [Required]
        [ForeignKey("IdDyskusji")]
        public virtual Dyskusja Dyskusja { get; set; }

        [Required]
        [ForeignKey("IdUzytkownika")]
        public virtual User Autor { get; set; }

        //ścieżka do załącznika
        public string ZalacznikPath { get; set; }

    }
}
