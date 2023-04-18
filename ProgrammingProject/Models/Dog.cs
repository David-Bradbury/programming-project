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
        NonReactive = -1,
        Calm = 0,
        Friendly = 1,
        Reactive = 2,
        Aggressive = 3
    }
    public enum TrainingLevel
    {
        None = 1,
        Basic = 2,
        Fully = 3
    }
    public class Dog
    {
        [Required]
        public int Id { get; set; }
        [Required, StringLength(50)]
        public string Name { get; set; }
        [Required]
        public string Breed { get; set; }
        [StringLength(50)]
        public string MicrochipNumber { get; set; }
        [Required]
        public bool IsVaccinated { get; set; }  
        public Temperament Temperament { get; set; }
        public DogSize DogSize { get; set; }
        public TrainingLevel TrainingLevel { get; set; }
        public byte[] image { get; set; }

        public virtual Owner Owner { get; set; }
        public virtual Vet Vet { get; set; }
        public virtual List<WalkingSession> WalkingSessions { get; set; }
        public virtual List<DogRating> DogRatings { get; set;}

       //TODO
        // I also think the enums need to be tweaked to make
        // the matching algorithm easier.
        // E.g. NonReactive = -1. Calm = 0 etc.DP
        
        //NOTES
        // Added Training Level enum. Will we be using this
        // in matching algorithms??? JC



    }
}
