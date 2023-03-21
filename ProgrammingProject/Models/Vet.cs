using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProgrammingProject.Models
{
    public class Vet
    {
        [Required]
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string BusinessName { get; set; }


        [Required, StringLength(50)]
        public string PhNumber { get; set; }


        [Required, StringLength(95)]
        public string Address { get; set; }



    }
}
