using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProgrammingProject.Models
{
    public enum ExperienceLevel
    {
        Beginner = 1,
        Intermediate = 2,
        Advanced = 3,
        Expert = 4
        
    }
    public class Walker
    {
        [Required]
        public int WalkerID { get; set; }

        [Required, StringLength(50)]
        public string FirstName { get; set; }

        [Required, StringLength(50)]
        public string LastName { get; set; }

        [ StringLength(50)]
        public string Address { get; set; }

        [Required]
        public string PhNumber { get; set; }
        public string Email { get; set; }

        [StringLength(50)]
        public List<string> WalkingSuburbs { get; set; }
        [Required]
        public bool IsInsured { get; set; }
        [Required]
        public ExperienceLevel ExperienceLevel { get; set; }





   
    }
}
