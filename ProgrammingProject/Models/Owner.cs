using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProgrammingProject.Models
{
    public class Owner : User
    {
        
        [Required, StringLength(200)]
        public string StreetAddress { get; set; }
        [Required, StringLength(100)]
        public string State { get; set; }
        [Required, StringLength(100)]
        public string Country { get; set; }
        [Required, StringLength(50)]
        public string PhNumber { get; set; }  
        public virtual Suburb Suburb { get; set; }
        public virtual List<Dog> Dogs { get; set; }
        public virtual List<WalkerRating> WalkerRatings { get; set;}
    }
}
