using System.ComponentModel.DataAnnotations;

namespace ProgrammingProject.Models
{
    public class Walks
    {
        [Required, Key]
        public int WalkerId { get; set; }
        [Required, Key]
        public int Postcode { get; set; }
        public virtual Suburb Suburb { get; set; }
        public virtual Walker Walker { get; set; }

    }
}
