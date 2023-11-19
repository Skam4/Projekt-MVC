using System.ComponentModel.DataAnnotations;

namespace Projekt_MVC.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Display(Name = "Nazwa")]
        [Required(ErrorMessage = "Nazwa konta jest wymagana")]
        public string NickName { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email jest wymagany")]
        public string Email { get; set; }

        [Display(Name = "Hasło")]
        [Required(ErrorMessage = "Hasło jest wymagane")]
        public string Password { get; set; }


    }
}
