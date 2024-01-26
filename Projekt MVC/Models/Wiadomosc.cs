using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projekt_MVC.Models
{
    public class Wiadomosc
    {
        public int Id { get; set; }
        public string Tresc { get; set; }
        public DateTime DataWyslania { get; set; }

        public int NadawcaId { get; set; }
        public int OdbiorcaId { get; set; }

        public User Nadawca { get; set; }
        public User Odbiorca { get; set; }
    }

}
