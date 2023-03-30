using System.ComponentModel.DataAnnotations;

namespace ProgrammingProject.Models
{
    public class WalkerRating
    {
        public double Rating { get; set; }
        public DateTime RatingDate { get; set; }

       [Required]
        public int OwnerID { get; set; }
        [Required]
        public int WalkerID { get; set; }
        public virtual Walker Walker { get; set; }
        public virtual Owner Owner { get; set; }
      
    }
}
