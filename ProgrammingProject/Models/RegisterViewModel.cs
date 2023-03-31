using System.ComponentModel.DataAnnotations;


namespace ProgrammingProject.Models
{
    public class RegisterViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
        [Required, EmailAddress(ErrorMessage = "The email address is required")]
        public string Email { get; set; }

        [Required, StringLength(200)]
        public string StreetAddress { get; set; }
        [Required, StringLength(100)]
        public string State { get; set; }
        [Required, StringLength(100)]
        public string Country { get; set; }
        [Required, StringLength(50)]
        public string PhNumber { get; set; }

        public string Suburb { get; set; }

        public int Postcode { get; set; }
   
        public bool IsInsured { get; set; }
        public int ExperienceLevel { get; set; }
        public string Password { get; set; }
        public int LoginID { get; set; }

        
        }
}

}
}
