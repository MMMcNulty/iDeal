using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace iDeal.Models
{
    public class User
    {
        [Key]
        public int UserId {get; set; }

        
        [Required(ErrorMessage = "Please enter your first name.")]
        [MinLength(2)]
        [RegularExpression("^[A-Za-z ]+$")]
        [Display(Name = "First Name: ")]
        public string FirstName {get; set; }

        [Required(ErrorMessage = "Please enter your last name.")]
        [MinLength(2)]
        [RegularExpression("^[A-Za-z ]+$", ErrorMessage="Name may only contain letters and spaces.")]
        [Display(Name = "Last Name: ")]
        public string LastName {get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Please enter a valid email address.")]
        [Display(Name = "Email: ")]
        public string Email {get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "The password field is required.")]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage="Name must contain at least one capital letter, and special character, and one number. The number must come after the special character." )]
        // [RegularExpression(" ^(?=.*[a-z])(?=.*[A-Z])(?=.*)(?=.*[$@$!%*?&])[A-Za-z$@$!%*?&]{8,}", ErrorMessage="Name must contain at least one special character." )]
        [MinLength(8, ErrorMessage ="Your password must be 8 characters or longer!")]
        [Display(Name = "Password: ")]
        public string Password {get; set; }

        public DateTime CreatedAt {get; set; } = DateTime.Now;
        public DateTime UpdatedAt {get; set; } = DateTime.Now;

        public int ChipValue {get; set; } =0;
        
        [NotMapped]
        [Compare("Password", ErrorMessage = "Password and password confirmation must match.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password Confirmation: ")]
        public string ConfirmPassword {get; set; }

        

    }
}