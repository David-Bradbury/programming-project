using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProgrammingProject.Models
{
    public enum DogSize
    {
        Small = 0,
        MediumSmall = 1,
        MediumLarge = 2,
        Large = 3
    }
    public class Dog
    {
        [Required]
        public int ID { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string MicrochipNumber { get; set; }
        public bool IsVaccinated { get; set; }  


        public virtual Owner Owner { get; set; }
        



    }
}
