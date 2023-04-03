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

        public string SuburbName  { get; set; }
        public string Postcode { get; set; }

        [StringLength(3)]
        [RegularExpression("NSW|QLD|SA|WA|TAS|VIC|NT|ACT",
            ErrorMessage = "Must be  2 or 3 letter Australian state or territory (in CAPS)")]
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


