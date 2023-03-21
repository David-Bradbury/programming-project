using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProgrammingProject.Models
{
    public class Administrator
    {
        [Required]
        [Display(Name = "Admin ID")]
        public int AdministratorId { get; set; }

        [Required, StringLength(50)]
        public string FirstName { get; set; }

        [Required, StringLength(50)]
        public string LastName { get; set; }

        [Required, StringLength(50)]
        public string Email { get; set; }   
        public virtual Login Login { get; set; }

    }
}
