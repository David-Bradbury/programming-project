﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ProgrammingProject.Models
{
    public class DogRating
    {
        public double Rating { get; set; }
        public DateTime RatingDate { get; set; }

        [Required, Key]
        public int DogID { get; set; }
        [Required, Key]
        public int WalkerID { get; set; }
        public virtual Walker Walker { get; set; }
        public virtual Dog Dog { get; set; }
    }
}
