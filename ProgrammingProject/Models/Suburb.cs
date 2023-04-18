using System.ComponentModel.DataAnnotations;

namespace ProgrammingProject.Models
{
    public class Suburb
    {
        [Required, Key]
        public string Postcode { get; set; }
        [Required, Key]
        public string SuburbName { get; set; }

        public virtual List<Walks> Walks { get; set; }
        public virtual List<Walker> Walkers { get; set; } 
        public virtual List<Owner> Owners { get; set; }
    }
}
