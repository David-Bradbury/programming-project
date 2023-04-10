using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProgrammingProject.Models
{
    public class WalkingSession
    {

        // ID (Required but only key)

        // Add date (Required)

        // make scheduled
        [Required]
        public DateTime StartTime { get; set; }
        // make scheduled
        [Required]
        public DateTime EndTime { get; set; }
        
        // Add actual start

        // Add actual end

        public virtual List<Dog> DogList { get; set; }

        [Required]
        public int WalkerID { get; set; }
        public virtual Walker Walker { get; set; }

        // Boolean of recurring
    }
}


