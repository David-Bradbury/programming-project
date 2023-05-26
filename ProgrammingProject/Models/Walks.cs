using System.ComponentModel.DataAnnotations;

namespace ProgrammingProject.Models
{
    // Not currently is use, however will be in future.
    // This will be all the extra suburbs that a walker will walk that is not in their desired range. 
    public class Walks
    {
        [Required]
        public int WalkerId { get; set; }
        [Required]
        public string Postcode { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string SuburbName { get; set; }
        public virtual Suburb Suburb { get; set; }
        public virtual Walker Walker { get; set; }

    }
}
