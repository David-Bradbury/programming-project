using System.ComponentModel.DataAnnotations;


namespace ProgrammingProject.Models
{
    public class RegisterViewModel
    {
        public int AccountTypeSelection { get; set; }

        [Required, StringLength(50)]
        public string FirstName { get; set; }

        [Required, StringLength(50)]
        public string LastName { get; set; }

        [Required, EmailAddress(ErrorMessage = "The email address is required")]
        public string Email { get; set; }

        [Required, StringLength(200)]
        public string StreetAddress { get; set; }
        [Required, StringLength(100)]
        public string Suburb { get; set; }
        public string State { get; set; }
        [Required, StringLength(100)]
        public string Country { get; set; }
        [Required, StringLength(50)]
        public string PhNumber { get; set; }

        [Display(Name = "Are you insured? Tick if yes")]
        public bool IsInsured { get; set; }

        [Range (1, 4), Display(Name ="Choose between 1= Beginner, 4 = Advanced")]
        public int ExperienceLevel { get; set; }
        public string Password{ get; set; }
    }
}


