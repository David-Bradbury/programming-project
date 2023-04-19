﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ProgrammingProject.Models
{
    public class AddDogViewModel
    {
   
        [Required, StringLength(50), Display(Name = "Dogs Name")]
        public string Name { get; set; }
        [Required, StringLength(50), Display(Name = "Dogs Breed")]
        public string Breed { get; set; }
        [Required, StringLength(50), Display(Name = "Dogs Microchip Number")]
        public string MicrochipNumber { get; set; }
        [Required, Display(Name = "Dogs Vaccination Status")]
        public string IsVaccinated { get; set; }
        [Required, Display(Name = "Dogs Temperament")]
        public string Temperament { get; set; }
        [Required, Display(Name = "Size of the Dog")]
        public string DogSize { get; set; }
        [Required, Display(Name = "Dogs Level of Training")]
        public string TrainingLevel { get; set; }
        [Display(Name = "Image of the Dog")]
        public IFormFile DogImage { get; set; }

        [Required, StringLength(50), Display(Name = "Dogs Vet Business Name")]
        public string BusinessName { get; set; }
        [Required, StringLength(50), Display(Name = "Vets Phone Number")]
        [RegularExpression(@"^(\+?\(61\)|\(\+?61\)|\+?61|(0[1-9])|0[1-9])?( ?-?[0-9]){7,9}$",
    ErrorMessage = "Must be an Australian phone number starting (Example 04XX XXX XXX")]
        public string PhNumber { get; set; }
        [Required, StringLength(320), EmailAddress(ErrorMessage = "An email address is required"), Display(Name = "Vets Email Address")]
        public string Email { get; set; }
        [Required, Display(Name = "Vets Street Address")]
        public string StreetAddress { get; set; }
        [Required, StringLength(100), Display(Name = "Vets Suburb")]
        public string SuburbName { get; set; }

        [Required, StringLength(4), Display(Name = "Vets Postcode")]
        [RegularExpression(@"(^0[289][0-9]{2}\s*$)|(^[1-9][0-9]{3}\s*$)",
    ErrorMessage = "Must be a valid Australia postcode with 4 digits")]
        public string Postcode { get; set; }
        [Required, StringLength(30)]
        public string State { get; set; }
        [Required, StringLength(100)]
        public string Country { get; set; }

        public List<SelectListItem> DogSizeList { get; set; }
        public List<SelectListItem> TemperamentList { get; set; }
        public List<SelectListItem> TrainingLevelList { get; set; }
        public List<SelectListItem> StatesList { get; set; }
        public List<SelectListItem> IsVaccinatedList { get; set; }
    }
}
