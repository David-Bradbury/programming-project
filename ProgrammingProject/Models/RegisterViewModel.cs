﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace ProgrammingProject.Models
{
    public class RegisterViewModel
    {
        public int AccountTypeSelection { get; set; }
        [Required, StringLength(50), Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required, StringLength(50), Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required, EmailAddress(ErrorMessage = "An email address is required")]
        public string Email { get; set; }
        [Display(Name = "Profile Image")]
        public IFormFile ProfileImage { get; set; }
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
        [Display(Name = "Are you insured?")]
        public string IsInsured { get; set; }
        [Display(Name = "Dog walking experience level")]
        public string ExperienceLevel { get; set; }
        [Required]
        [RegularExpression(@"^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=]).*$",
    ErrorMessage = "Password must include at least one upper case letter, a lower case letter, a special character, a number, and must be at least 8 characters in length")]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "The password does not match")]
        public string ConfirmPassword { get; set; }

        public List<SelectListItem> StatesList { get; set; }
        public List<SelectListItem> IsInsuredList { get; set; }
        public List<SelectListItem> ExperienceList { get; set; }
    }
}


