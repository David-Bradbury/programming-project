﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProgrammingProject.Models
{
    public record Listing
    {
        [Column(TypeName = "nchar")]
        [Required, StringLength(64)]
        public string subject { get; init; }

        public virtual User User { get; init; }

        [Column(TypeName = "nchar")]
        [Required]
        public string messageText { get; init; }

        public DateTime postTimeUTC { get; init; }

        public string Image { get; init; }
    }
}
