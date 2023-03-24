namespace ProgrammingProject.Models
{
    public class WalkerRating
    {
        public int Id { get; set; }
        public double Rating { get; set; }
        public DateTime RatingDate { get; set; }

        public int OwnerID { get; set; }
        public int WalkerID { get; set; }
        public virtual Walker Walker { get; set; }
        public virtual Owner Owner { get; set; }
      
    }
}
