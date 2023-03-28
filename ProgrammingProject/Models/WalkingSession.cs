using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProgrammingProject.Models
{
    public class WalkingSession
    {
        [Required, Key]
        public DateTime StartTime { get; set; }
        [Required]
        public DateTime EndTime { get; set; }

        public virtual List<Dog> DogList { get; set; }

        public int WalkerID { get; set; }
        public virtual Walker Walker { get; set; }


        // TODO: Is it worth having the walking session as a
        // virtual property in walker? i'm thinking
        // that it would make it easier to add a dog to a WalkingSession
        // from the WalkerController class with only the SessionID.DP

    }
}


