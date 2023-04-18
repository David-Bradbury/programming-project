using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProgrammingProject.Models
{
    // A Walking Session includes a generated ID, A date, scheduled and actual walk times, between dogs and a walker.
    public class WalkingSession
    {

        [Required, Key]
        public int SessionID { get; set; }

        [Required]
        public DateTime Date { get; set; }
        [Required]
        public DateTime ScheduledStartTime { get; set; }
        [Required]
        public DateTime ScheduledEndTime { get; set; }

        public DateTime ActualStartTime { get; set; }
        public DateTime ActualEndTime { get; set; }

        public bool IsRecurring { get; set; }

        public virtual List<Dog> DogList { get; set; }
        [Required]
        public int WalkerID { get; set; }
        public virtual Walker Walker { get; set; }
    }
}


