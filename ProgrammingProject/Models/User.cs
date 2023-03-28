using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProgrammingProject.Models
{
    public class User
    {
        [Required]
        public int UserId { get; set; }
        [Required, StringLength(50)]
        public string FirstName { get; set; }
        [Required, StringLength(50)]
        public string LastName { get; set; }
        [Required, StringLength(320)]
        public string Email { get; set; }
        
        public virtual Login Login { get; set; }

        public virtual Login Login { get; set; }
    }
}
