using Castle.Components.DictionaryAdapter;
using Microsoft.EntityFrameworkCore;

namespace ProgrammingProject.Models
{
    public class DogRating
    {
        public int Id { get; set; }
        public double Rating { get; set; }
        public DateTime RatingDate { get; set; }

        
        public int DogID { get; set; }
        public int WalkerID { get; set; }
        public virtual Walker Walker { get; set; }
        public virtual Dog Dog { get; set; }
    }
}
