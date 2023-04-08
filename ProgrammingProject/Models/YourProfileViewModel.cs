using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProgrammingProject.Models
{
    public class YourProfileViewModel
    {
        public int AccountTypeSelection { get; set; }
        [Required, StringLength(50), Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required, StringLength(50), Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required, EmailAddress(ErrorMessage = "An email address is required")]
        public string Email { get; set; }
        [Required, StringLength(200), Display(Name = "Street Address")]
        public string StreetAddress { get; set; }
        [Required, StringLength(100), Display(Name = "Suburb")]
        public string SuburbName { get; set; }

        [Required, StringLength(4)]
        [RegularExpression(@"(^0[289][0-9]{2}\s*$)|(^[1-9][0-9]{3}\s*$)",
    ErrorMessage = "Must be a valid Australia postcode with 4 digits")]
        public string Postcode { get; set; }
        [Required, StringLength(30)]
        public string State { get; set; }
        [Required, StringLength(100)]
        public string Country { get; set; }
        [Required, StringLength(25)]
        [RegularExpression(@"^(\+?\(61\)|\(\+?61\)|\+?61|(0[1-9])|0[1-9])?( ?-?[0-9]){7,9}$",
    ErrorMessage = "Must be an Australian phone number starting (Example 04XX XXX XXX")]
        [Display(Name = "Phone Number")]
        public string PhNumber { get; set; }
        [Display(Name = "Are you insured? Tick if yes")]
        public bool IsInsured { get; set; }
        [Range(1, 4), Display(Name = "Choose between 1= Beginner, 4 = Advanced")]
        public int ExperienceLevel { get; set; }
        [Required]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "The password does not match")]
        public string ConfirmPassword { get; set; }

    }
}
