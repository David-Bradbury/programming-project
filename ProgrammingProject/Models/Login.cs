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
        [Required, StringLength(320), Key]
        public string Email { get; set; }

        [Column(TypeName = "char")]
        [Required, StringLength(64)]
        public string PasswordHash { get; set; }
        public Locked Locked { get; set; }

        public virtual User User { get; set; }
    
    }
}
