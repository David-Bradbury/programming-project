using System.ComponentModel.DataAnnotations;

namespace ProgrammingProject.Models
{
    public class PlacesWalked
    {
        [Required, Key]
        public string Suburb { get; set; }
        public string Postcode { get; set; }

        public virtual Walker Walker { get; set; }
    }
}
