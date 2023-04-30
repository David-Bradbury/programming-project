using System.ComponentModel.DataAnnotations;

namespace ProgrammingProject.Models
{
    public class Breed
    {
        [Required, Key]
        public int BreedId { get; set; }

        [Required, StringLength(50)]
        public string BreedName { get; set; }

        public virtual List<Dog> Dogs { get; set; }
    }
}
