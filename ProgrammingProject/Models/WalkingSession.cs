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
    }
}
