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
        [StringLength(50)]
        public string Address { get; set; }
        [Required]
        public string PhNumber { get; set; }
        [Required]
        public bool IsInsured { get; set; }
        [Required]
        public ExperienceLevel ExperienceLevel { get; set; }

        //public virtual List<PlacesWalked> PlacesWalked { get; set; }
        public virtual List<WalkingSession> WalkingSessions { get; set; }
        //public virtual List<DogRating> DogRatings { get; set; }
        //public virtual List<WalkerRating> WalkerRatings { get; set; }
    }
}
