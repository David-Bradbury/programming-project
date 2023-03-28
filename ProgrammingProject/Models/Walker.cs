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
    public class Walker : User
    {
        [Required, StringLength(200)]
        public string StreetAddress { get; set; }
        [Required, StringLength(100)]
        public string State { get; set; }
        [Required, StringLength(100)]
        public string Country { get; set; }
        [Required]
        public string PhNumber { get; set; }     
        [Required]
        public bool IsInsured { get; set; }      
        [Required]
        public ExperienceLevel ExperienceLevel { get; set; }

        public virtual Suburb Suburb { get; set; }
        public virtual List<Walks> Walks { get; set; }
        public virtual List<WalkingSession> WalkingSessions { get; set; }
        public virtual List<DogRating> DogRatings { get; set; }
        public virtual List<WalkerRating> WalkerRatings { get; set; }
    }
}
