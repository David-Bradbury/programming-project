using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProgrammingProject.Models
{
    public enum Locked
    {
        unlocked = 0,
        locked = 1
    }

    public record Login
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column(TypeName = "char")]
        [Required, StringLength(8)]
        [RegularExpression("[0-9]{8}",
    ErrorMessage = "Must be 8 digits"),
            Key]
        public string LoginID { get; set; }

        [Column(TypeName = "char")]
        [Required, StringLength(64)]
        public string PasswordHash { get; set; }
        public Locked Locked { get; set; }

        [Required]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        /*
      [Column(TypeName = "nchar")]
      [StringLength(8)]
      public string LoginID { get; init; }
      */


    }
}
