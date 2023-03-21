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
        [Column(TypeName = "nchar")]
        [StringLength(8)]
        public string LoginID { get; init; }

        public virtual User User { get; set; }

        [Column(TypeName = "nchar")]
        [Required, StringLength(64)]
        public string PasswordHash { get; set; }

        public Locked Locked { get; init; }
    }
}
