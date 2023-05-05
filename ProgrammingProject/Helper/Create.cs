using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProgrammingProject.Data;
using ProgrammingProject.Models;
using System;


namespace ProgrammingProject.Helper
{
    public class Create
    {
       // private readonly IServiceProvider _serviceProvider;
        private readonly EasyWalkContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public Create(EasyWalkContext context, IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
           // _serviceProvider = serviceProvider;
            _context = context;
        
        }

        public Vet CreateVet(string BusinessName, string PhNumber, string Email, string StreetAddress, string Country, Suburb Suburb)
        {
       
            //using var context = new EasyWalkContext(
            //_serviceProvider.GetRequiredService<DbContextOptions<EasyWalkContext>>());
            Vet vet = new Vet();
          
            vet.BusinessName = BusinessName;
            vet.PhNumber = PhNumber;
            vet.Email = Email;
            vet.StreetAddress = StreetAddress;
            vet.Country = Country;
            vet.Suburb = _context.Suburbs.Where(s => s.SuburbName == Suburb.SuburbName)
                                          .Where(s => s.Postcode == Suburb.Postcode)
                                          .Where(s => s.State == Suburb.State).FirstOrDefault(); ;
     
            return vet;
        }

        public Dog CreateDog(string Name, string Breed, string MicrochipNumber, string StringTemperament,
            string StringDogSize, string StringTrainingLevel, IFormFile ProfileImage, Vet vet, Owner owner)
        {

            // Converting IFormFile to string.
            var ImageHelper = new ImageHelper(_webHostEnvironment);
            string imageFileName = ImageHelper.UploadFile(ProfileImage);

            var dog = new Dog();

            var breed = new Breed();
            breed.BreedName = Breed;

            dog.Name = Name;
            dog.Breed = breed;
            dog.MicrochipNumber = MicrochipNumber;
            dog.Owner = owner;
            dog.Vet = vet;
            dog.IsVaccinated = true;

            if (ProfileImage != null)
                dog.ProfileImage = imageFileName;
            else
                dog.ProfileImage = "dog-avatar.jpg";

            if (StringTemperament.Equals("NonReactive"))
                dog.Temperament = Temperament.NonReactive;
            if (StringTemperament.Equals("Calm"))
                dog.Temperament = Temperament.Calm;
            if (StringTemperament.Equals("Friendly"))
                dog.Temperament = Temperament.Friendly;
            if (StringTemperament.Equals("Reactive"))
                dog.Temperament = Temperament.Reactive;
            if (StringTemperament.Equals("Agressive"))
                dog.Temperament = Temperament.Aggressive;

            if (StringDogSize.Equals("Small"))
                dog.DogSize = DogSize.Small;
            if (StringDogSize.Equals("Medium"))
                dog.DogSize = DogSize.Medium;
            if (StringDogSize.Equals("Large"))
                dog.DogSize = DogSize.Large;
            if (StringDogSize.Equals("ExtraLarge"))
                dog.DogSize = DogSize.ExtraLarge;

            if (StringTrainingLevel.Equals("None"))
                dog.TrainingLevel = TrainingLevel.None;
            if (StringTrainingLevel.Equals("Basic"))
                dog.TrainingLevel = TrainingLevel.Basic;
            if (StringTrainingLevel.Equals("Fully"))
                dog.TrainingLevel = TrainingLevel.Fully;

            return dog;
        }

        public async void CreateOwner(string firstName, string lastName, string email, string streetAddress, 
            string country, string phNumber,IFormFile profileImage, Suburb suburb, int UserID, string savedProfileImage)
        {
            var owner = new Owner();
          
            if (UserID != 0)
                owner = _context.Owners.Find(UserID);
            
            // Converting IFormFile to string.
            var ImageHelper = new ImageHelper(_webHostEnvironment);
            string imageFileName = ImageHelper.UploadFile(profileImage);

           
            owner.FirstName = firstName;
            owner.LastName = lastName;
            owner.Email = email;
            owner.StreetAddress = streetAddress;           
            owner.Country = country;
            owner.PhNumber = phNumber;
            owner.Suburb = _context.Suburbs.Where(s => s.SuburbName == suburb.SuburbName)
                                           .Where(s => s.Postcode == suburb.Postcode)
                                           .Where(s => s.State == suburb.State).FirstOrDefault();

            if (UserID != 0)
            {
                if (profileImage != null)
                    owner.ProfileImage = imageFileName;
                else if (savedProfileImage != imageFileName)
                    owner.ProfileImage = "defaultProfile.png";

                //await _context.SaveChangesAsync();
            }
            else
            {
                if (profileImage != null)
                    owner.ProfileImage = imageFileName;
                else
                    owner.ProfileImage = "defaultProfile.png";

                _context.Add(owner);
                _context.SaveChanges();
            }
           



        }

        public Walker CreateWalker(string firstName, string lastName, string email, string streetAddress,
           string country, string phNumber, string insured, string experienceLevel, IFormFile profileImage, Suburb suburb)
        {
           
            // Converting IFormFile to string.
            var ImageHelper = new ImageHelper(_webHostEnvironment);
            string imageFileName = ImageHelper.UploadFile(profileImage);

            var walker = new Walker();

            walker.FirstName = firstName;
            walker.LastName = lastName;
            walker.Email = email;
            walker.StreetAddress = streetAddress;           
            walker.Country = country;
            walker.PhNumber = phNumber;
            walker.Suburb = _context.Suburbs.Where(s => s.SuburbName == suburb.SuburbName)
                                           .Where(s => s.Postcode == suburb.Postcode)
                                           .Where(s => s.State == suburb.State).FirstOrDefault();

            if (insured.Equals("Insured"))
                walker.IsInsured = true;
            else if (insured.Equals("Uninsured"))
                walker.IsInsured = false;

            if (experienceLevel.Equals("Beginner"))
                walker.ExperienceLevel = ExperienceLevel.Beginner;
            else if (experienceLevel.Equals("Intermediate"))
                walker.ExperienceLevel = ExperienceLevel.Intermediate;
            else if (experienceLevel.Equals("Advanced"))
                walker.ExperienceLevel = ExperienceLevel.Advanced;
            else if (experienceLevel.Equals("Expert"))
                walker.ExperienceLevel = ExperienceLevel.Expert;

            if (profileImage != null)
                walker.ProfileImage = imageFileName;
            else
                walker.ProfileImage = "defaultProfile.png";


            //if (viewModel.ProfileImage != null)
            //    w.ProfileImage = imageFileName;
            //else if (viewModel.SavedProfileImage != imageFileName)
            //    w.ProfileImage = "defaultProfile.png";

            return walker;
        }
    }
}
