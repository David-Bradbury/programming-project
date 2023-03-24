using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProgrammingProject.Models
{
    public enum DogSize
    {
        Small = 1,
        Medium = 2,
        Large = 3,
        ExtraLarge = 4
    }
    public enum Temperament
    {
        NonReactive = 1,
        Calm = 2,
        Friendly = 3,
        Reactive = 4,
        Aggresive = 5
    }
    public class Dog
    {
        [Required]
        public int Id { get; set; }
        [Required, StringLength(50)]
        public string Name { get; set; }
        [StringLength(50)]
        public string MicrochipNumber { get; set; }
        [Required]
        public bool IsVaccinated { get; set; }  
       
        public virtual Owner Owner { get; set; }
        public virtual Vet Vet { get; set; }
        public virtual List<WalkingSession> WalkingSessions { get; set; }
        public virtual List<DogRating> DogRatings { get; set;}

    }
}
