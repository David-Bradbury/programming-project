using System.ComponentModel.DataAnnotations;

namespace ProgrammingProject.Models
{
    public class Breed
    {
        [Required, StringLength(50), Key]
        public string BreedName { get; set; }

        public virtual List<Dog> Dogs { get; set; }
    }
}
