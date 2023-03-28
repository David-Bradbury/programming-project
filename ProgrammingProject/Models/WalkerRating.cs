using System.ComponentModel.DataAnnotations;

namespace ProgrammingProject.Models
{
    public class WalkerRating
    {
        public double Rating { get; set; }
        public DateTime RatingDate { get; set; }

       [Required, Key]
        public int OwnerID { get; set; }
        [Required, Key]
        public int WalkerID { get; set; }
        public virtual Walker Walker { get; set; }
        public virtual Owner Owner { get; set; }
      
    }
}
