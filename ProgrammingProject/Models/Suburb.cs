using System.ComponentModel.DataAnnotations;

namespace ProgrammingProject.Models
{
    public class Suburb
    {
        [Required, Key]
        public string Postcode { get; set; }
        [Required]
        public string SuburbName { get; set; }

        public virtual List<Walks> Walks { get; set; }
    }
}
