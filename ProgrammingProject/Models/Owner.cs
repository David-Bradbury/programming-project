using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProgrammingProject.Models
{
    public class Owner
    {
        [Required]
        public int OwnerID { get; set; }

        [Required, StringLength(50)]
        public string FirstName { get; set; }

        [Required, StringLength(50)]
        public string LastName { get; set; }

        [Required, StringLength(200)]
        public string Address { get; set; }

        [Required, StringLength(50)]
        public string PhNumber { get; set; }

        [Required, StringLength(50)]
        public string Email { get; set; }
        public virtual List<Dog> Dogs { get; set; }
        public virtual Login Login { get; set; }





    }
}
