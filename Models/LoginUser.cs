using System.ComponentModel.DataAnnotations;

namespace iDeal.Models
{
    public class LoginUser
    {
        [Required(ErrorMessage = "Please enter a valid email.")]
        [Display(Name = "Email:")]
        public string LoginEmail {get; set; }

        [Required(ErrorMessage = "The password you entered is incorrect.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password:")]
        public string LoginPassword {get; set; }
    }
}