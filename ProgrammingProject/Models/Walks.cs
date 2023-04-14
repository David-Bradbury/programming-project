using System.ComponentModel.DataAnnotations;

namespace ProgrammingProject.Models
{
    public class Walks
    {
        [Required]
        public int WalkerId { get; set; }
        [Required]
        public string Postcode { get; set; }
        [Required]
        public string SuburbName { get; set; }
        public virtual Suburb Suburb { get; set; }
        public virtual Walker Walker { get; set; }

    }
}
