using ProgrammingProject.Data;
using ProgrammingProject.Models;
using ProgrammingProject.Filters;
using Microsoft.AspNetCore.Mvc;
using ProgrammingProject.Helper;
using System.Text.RegularExpressions;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace ProgrammingProject.Controllers
{
    //Mask URL
    [Route("/Owner")]


    public class OwnerController : Controller
    {
        private readonly EasyWalkContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AddDogViewModel viewModel = new AddDogViewModel();
        private int OwnerID => HttpContext.Session.GetInt32(nameof(Owner.UserId)).Value;

        public OwnerController(EasyWalkContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }


        [AuthorizeUser]
        public async Task<IActionResult> Index()
        {
            var owner = await _context.Owners.FindAsync(OwnerID);
            return View(owner);
        }

        //Add a dog to the owner
        [Route("/Owner/AddDog",
     Name = "AddDog"), AuthorizeUser]
        public async Task<IActionResult> AddDog()
        {
            viewModel.DogSizeList = DropDownLists.GetDogSize();
            viewModel.TemperamentList = DropDownLists.GetTemperament();
            viewModel.TrainingLevelList = DropDownLists.GetTrainingLevel();
            viewModel.StatesList = DropDownLists.GetStates();
            viewModel.IsVaccinatedList = DropDownLists.GetVaccinatedList();

            return View(viewModel);
        }

        [Route("/Owner/AddDog",
    Name = "AddDog")]
        [HttpPost]
        public async Task<IActionResult> AddDog(AddDogViewModel viewModel)
        {
            //var viewModel = new AddDogViewModel();

            viewModel.DogSizeList = DropDownLists.GetDogSize();
            viewModel.TemperamentList = DropDownLists.GetTemperament();
            viewModel.TrainingLevelList = DropDownLists.GetTrainingLevel();
            viewModel.StatesList = DropDownLists.GetStates();
            viewModel.IsVaccinatedList = DropDownLists.GetVaccinatedList();

            var owner = new Owner();
            foreach (var o in _context.Owners)
                if (o.UserId == OwnerID)
                    owner = o;

            if (viewModel.Name == null)
                ModelState.AddModelError(nameof(viewModel.Name), "Dogs Name is required.");
            if (viewModel.Breed == null)
                ModelState.AddModelError(nameof(viewModel.Breed), "Dogs Breed is required.");
            if (viewModel.IsVaccinated == null)
                ModelState.AddModelError(nameof(viewModel.IsVaccinated), "Dogs Vaccination Status is required.");
            if (viewModel.Temperament == null)
                ModelState.AddModelError(nameof(viewModel.Temperament), "Dogs Temperament is required.");
            if (viewModel.DogSize == null)
                ModelState.AddModelError(nameof(viewModel.DogSize), "Dogs Size is required.");
            if (viewModel.TrainingLevel == null)
                ModelState.AddModelError(nameof(viewModel.TrainingLevel), "Dogs Training Level is required.");
            if (viewModel.BusinessName == null)
                ModelState.AddModelError(nameof(viewModel.BusinessName), "Vets Business Name is required.");
            if (viewModel.PhNumber == null)
                ModelState.AddModelError(nameof(viewModel.PhNumber), "Vets Phone Number is required.");
            if (viewModel.Email == null)
                ModelState.AddModelError(nameof(viewModel.Email), "Vets Email is required.");
            if (viewModel.StreetAddress == null)
                ModelState.AddModelError(nameof(viewModel.StreetAddress), "Vets Street Address is required.");
            if (viewModel.SuburbName == null)
                ModelState.AddModelError(nameof(viewModel.SuburbName), "Vets Suburb is required.");
            if (viewModel.Postcode == null)
                ModelState.AddModelError(nameof(viewModel.Postcode), "Vets Postcode is required.");
            if (viewModel.State == null)
                ModelState.AddModelError(nameof(viewModel.State), "Vets State is required.");
            if (viewModel.Country == null)
                ModelState.AddModelError(nameof(viewModel.Country), "Vets Country is required.");

            if (!Regex.IsMatch(viewModel.Postcode, @"(^0[289][0-9]{2}\s*$)|(^[1-9][0-9]{3}\s*$)"))
                ModelState.AddModelError(nameof(viewModel.Postcode), "This postcode does not match any Australian postcode. Please enter an Australian 4 digit postcode");
            // Not perfect and needs updates for proper Australian phone numbers.
            if (!Regex.IsMatch(viewModel.PhNumber, @"^(\+?\(61\)|\(\+?61\)|\+?61|(0[1-9])|0[1-9])?( ?-?[0-9]){7,9}$"))
                ModelState.AddModelError(nameof(viewModel.PhNumber), "This is not a valid Australian mobile phone number. Please enter a valid Australian mobile phone number");
            if (!Regex.IsMatch(viewModel.Email, @"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+\s?$"))
                ModelState.AddModelError(nameof(viewModel.Email), "This is not a valid email address. Please enter a valid email address");
            // Maybe This Error Message IS Not Enough, maybe a pop up or something needs to occur. Discuss at nextr meeting. JC
            if (viewModel.IsVaccinated.Equals("Unvaccinated"))
                ModelState.AddModelError(nameof(viewModel.IsVaccinated), "Sorry, All dogs must be vaccinated before being registered with EasyWalk");

            // Checks the extension of the file to ensure a certain file format. Bring up Thurs meeting to see if extra verification needed. JC.
            if (viewModel.DogImage != null)
            {
                string filename = Path.GetFileName(viewModel.DogImage.FileName);
                string extension = Path.GetExtension(filename).ToLower();

                if (extension != ".jpg" && extension != ".jpeg" && extension != ".png")
                    ModelState.AddModelError(nameof(viewModel.DogImage), "Image must be of the jpg/jpeg, or png format");
            }

            // Checking to see if the state of the model is valid before continuing.
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }


            string imageFileName = UploadFile(viewModel);

            // Creating suburb based on form details
            var suburb = new Suburb();
            suburb.SuburbName = viewModel.SuburbName;
            suburb.Postcode = viewModel.Postcode;

            // Check is Suburb is already known to Easy Walk DB, and rejects entry if known.
            bool match = false;
            foreach (var s in _context.Suburbs)
            {
                if (s.Postcode == viewModel.Postcode && s.SuburbName == viewModel.SuburbName)
                {
                    match = true;
                    suburb = s;
                }
            }
            if (!match)
                _context.Suburbs.Add(suburb);


            // Create a new Vet from form submission.
            var vet = new Vet();

            vet.BusinessName = viewModel.BusinessName;
            vet.PhNumber = viewModel.PhNumber;
            vet.Email = viewModel.Email;
            vet.StreetAddress = viewModel.StreetAddress;
            vet.Suburb = suburb;
            vet.State = viewModel.State;
            vet.Country = viewModel.Country;

            // Checking BusinessName for now but this is wrong as BusinessName is not key.
            match = false;
            foreach (var v in _context.Vets)
            {
                if (v.BusinessName == viewModel.BusinessName)
                {
                    match = true;
                    vet = v;
                }
            }
            if (!match)
                _context.Vets.Add(vet);


            // Create a new dog from form submission.
            var dog = new Dog();

            dog.Name = viewModel.Name;
            dog.Breed = viewModel.Breed;
            dog.MicrochipNumber = viewModel.MicrochipNumber;
            dog.Owner = owner;
            dog.Vet = vet;
            dog.IsVaccinated = true;

            if (viewModel.DogImage != null)
                dog.DogImage = imageFileName;
            else
                dog.DogImage = "dog-avatar.jpg";

            if (viewModel.Temperament.Equals("NonReactive"))
                dog.Temperament = Temperament.NonReactive;
            if (viewModel.Temperament.Equals("Calm"))
                dog.Temperament = Temperament.Calm;
            if (viewModel.Temperament.Equals("Friendly"))
                dog.Temperament = Temperament.Friendly;
            if (viewModel.Temperament.Equals("Reactive"))
                dog.Temperament = Temperament.Reactive;
            if (viewModel.Temperament.Equals("Agressive"))
                dog.Temperament = Temperament.Aggressive;

            if (viewModel.DogSize.Equals("Small"))
                dog.DogSize = DogSize.Small;
            if (viewModel.DogSize.Equals("Medium"))
                dog.DogSize = DogSize.Medium;
            if (viewModel.DogSize.Equals("Large"))
                dog.DogSize = DogSize.Large;
            if (viewModel.DogSize.Equals("ExtraLarge"))
                dog.DogSize = DogSize.ExtraLarge;

            if (viewModel.TrainingLevel.Equals("None"))
                dog.TrainingLevel = TrainingLevel.None;
            if (viewModel.TrainingLevel.Equals("Basic"))
                dog.TrainingLevel = TrainingLevel.Basic;
            if (viewModel.TrainingLevel.Equals("Fully"))
                dog.TrainingLevel = TrainingLevel.Fully;

            _context.Add(dog);

            _context.SaveChanges();


            return RedirectToAction(nameof(Index));
        }

        // Uploads the file to the img directory in wwwroot and returns string that is saved to the DB.
        private string UploadFile(AddDogViewModel viewModel)
        {
            string fileName = null;
            if (viewModel.DogImage != null)
            {
                string uploadDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "img");
                fileName = Guid.NewGuid().ToString() + "-" + viewModel.DogImage.FileName;
                string filePath = Path.Combine(uploadDirectory, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    viewModel.DogImage.CopyTo(fileStream);
                }
            }
            return fileName;
        }

        [Route("/Owner/EditDogProfile",
   Name = "EditDogProfile")]
        public async Task<IActionResult> EditDogProfile(int dogID)
        {
            var dog = new Dog();
            dog = await _context.Dogs.FindAsync(dogID);

            var vet = new Vet();
            vet = await _context.Vets.FindAsync(dog.Vet.Id);

            var viewModel = new EditDogProfileViewModel();


            viewModel.DogSizeList = DropDownLists.GetDogSize();
            viewModel.TemperamentList = DropDownLists.GetTemperament();
            viewModel.TrainingLevelList = DropDownLists.GetTrainingLevel();
            viewModel.StatesList = DropDownLists.GetStates();
            viewModel.IsVaccinatedList = DropDownLists.GetVaccinatedList();

            viewModel.DogId = dog.Id;
            viewModel.Name = dog.Name;
            viewModel.Breed = dog.Breed;
            viewModel.MicrochipNumber = dog.MicrochipNumber;
            viewModel.SavedDogImage = dog.DogImage;

            //if(dog.IsVaccinated == true)
            //    viewModel.IsVaccinated = "true";

            if (dog.Temperament == Temperament.NonReactive)
                viewModel.Temperament = "NonReactive";
            if (dog.Temperament == Temperament.Calm)
                viewModel.Temperament = "Calm";
            if (dog.Temperament == Temperament.Friendly)
                viewModel.Temperament = "Friendly";
            if (dog.Temperament == Temperament.Reactive)
                viewModel.Temperament = "Reactive"; ;
            if (dog.Temperament == Temperament.Aggressive)
                viewModel.Temperament = "Agressive";

            if (dog.DogSize == DogSize.Small)
                viewModel.DogSize = "Small";
            if (dog.DogSize == DogSize.Medium)
                viewModel.DogSize = "Medium";
            if (dog.DogSize == DogSize.Large)
                viewModel.DogSize = "Large";
            if (dog.DogSize == DogSize.ExtraLarge)
                viewModel.DogSize = "ExtraLarge";

            if (dog.TrainingLevel == TrainingLevel.None)
                viewModel.TrainingLevel = "None";
            if (dog.TrainingLevel == TrainingLevel.Basic)
                viewModel.TrainingLevel = "Basic";
            if (dog.TrainingLevel == TrainingLevel.Fully)
                viewModel.TrainingLevel = "Fully";

            var sub = new Suburb();
            sub.SuburbName = viewModel.SuburbName;
            sub.Postcode = viewModel.Postcode;

            vet.Suburb = sub;

            viewModel.BusinessName = vet.BusinessName;
            viewModel.PhNumber = vet.PhNumber;
            viewModel.Email = vet.Email;
            viewModel.StreetAddress = vet.StreetAddress;
            viewModel.State = vet.State;
            viewModel.Country = vet.Country;

            return View(viewModel);
        }
        [Route("/Owner/EditDogProfileVariable",
   Name = "EditDogProfileVariable")]
        public async Task<IActionResult> EditDogProfileVariable(int id, int DogID)
        {
            var dog = new Dog();
            dog = await _context.Dogs.FindAsync(DogID);

            var vet = new Vet();
            vet = await _context.Vets.FindAsync(dog.Vet.Id);

            var viewModel = new EditDogProfileViewModel();

            viewModel.DogSizeList = DropDownLists.GetDogSize();
            viewModel.TemperamentList = DropDownLists.GetTemperament();
            viewModel.TrainingLevelList = DropDownLists.GetTrainingLevel();
            viewModel.StatesList = DropDownLists.GetStates();
            viewModel.IsVaccinatedList = DropDownLists.GetVaccinatedList();

            viewModel.DogId = dog.Id;
            viewModel.DogSizeList = DropDownLists.GetDogSize();
            viewModel.TemperamentList = DropDownLists.GetTemperament();
            viewModel.TrainingLevelList = DropDownLists.GetTrainingLevel();
            viewModel.StatesList = DropDownLists.GetStates();
            viewModel.IsVaccinatedList = DropDownLists.GetVaccinatedList();

            viewModel.Name = dog.Name;
            viewModel.Breed = dog.Breed;
            viewModel.MicrochipNumber = dog.MicrochipNumber;
            viewModel.SavedDogImage = dog.DogImage;

            if (dog.IsVaccinated == true)
                viewModel.IsVaccinated = "true";

            if (dog.Temperament == Temperament.NonReactive)
                viewModel.Temperament = "NonReactive";
            if (dog.Temperament == Temperament.Calm)
                viewModel.Temperament = "Calm";
            if (dog.Temperament == Temperament.Friendly)
                viewModel.Temperament = "Friendly";
            if (dog.Temperament == Temperament.Reactive)
                viewModel.Temperament = "Reactive"; ;
            if (dog.Temperament == Temperament.Aggressive)
                viewModel.Temperament = "Agressive";

            if (dog.DogSize == DogSize.Small)
                viewModel.DogSize = "Small";
            if (dog.DogSize == DogSize.Medium)
                viewModel.DogSize = "Medium";
            if (dog.DogSize == DogSize.Large)
                viewModel.DogSize = "Large";
            if (dog.DogSize == DogSize.ExtraLarge)
                viewModel.DogSize = "ExtraLarge";

            if (dog.TrainingLevel == TrainingLevel.None)
                viewModel.TrainingLevel = "None";
            if (dog.TrainingLevel == TrainingLevel.Basic)
                viewModel.TrainingLevel = "Basic";
            if (dog.TrainingLevel == TrainingLevel.Fully)
                viewModel.TrainingLevel = "Fully";

            var sub = new Suburb();
            vet.Suburb = sub;

            viewModel.BusinessName = vet.BusinessName;
            viewModel.PhNumber = vet.PhNumber;
            viewModel.Email = vet.Email;
            viewModel.StreetAddress = vet.StreetAddress;
            viewModel.SuburbName = sub.SuburbName;
            viewModel.Postcode = sub.Postcode;
            viewModel.State = vet.State;
            viewModel.Country = vet.Country;

            if (id == 1)
                viewModel.SelectedField = nameof(viewModel.Name);
            if (id == 2)
                viewModel.SelectedField = nameof(viewModel.Breed);
            if (id == 3)
                viewModel.SelectedField = nameof(viewModel.MicrochipNumber);
            if (id == 4)
                viewModel.SelectedField = nameof(viewModel.IsVaccinated);
            if (id == 5)
                viewModel.SelectedField = nameof(viewModel.Temperament);
            if (id == 6)
                viewModel.SelectedField = nameof(viewModel.DogSize);
            if (id == 7)
                viewModel.SelectedField = nameof(viewModel.TrainingLevel);
            if (id == 8)
                viewModel.SelectedField = nameof(viewModel.BusinessName);
            if (id == 9)
                viewModel.SelectedField = nameof(viewModel.PhNumber);
            if (id == 10)
                viewModel.SelectedField = nameof(viewModel.Email);
            if (id == 11)
                viewModel.SelectedField = nameof(viewModel.StreetAddress);
            if (id == 12)
                viewModel.SelectedField = nameof(viewModel.SuburbName);
            if (id == 13)
                viewModel.SelectedField = nameof(viewModel.Postcode);
            if (id == 14)
                viewModel.SelectedField = nameof(viewModel.State);

            return View(viewModel);

        }

        [HttpPost]
        public async Task<IActionResult> EditDogProfileVariable(EditDogProfileViewModel viewModel)
        {

            if (viewModel.Name == null)
                ModelState.AddModelError(nameof(viewModel.Name), "Dogs Name is required.");
            if (viewModel.Breed == null)
                ModelState.AddModelError(nameof(viewModel.Breed), "Dogs Breed is required.");
            //if (viewModel.IsVaccinated == null)
            //    ModelState.AddModelError(nameof(viewModel.IsVaccinated), "Dogs Vaccination Status is required.");
            if (viewModel.Temperament == null)
                ModelState.AddModelError(nameof(viewModel.Temperament), "Dogs Temperament is required.");
            if (viewModel.DogSize == null)
                ModelState.AddModelError(nameof(viewModel.DogSize), "Dogs Size is required.");
            if (viewModel.TrainingLevel == null)
                ModelState.AddModelError(nameof(viewModel.TrainingLevel), "Dogs Training Level is required.");
            if (viewModel.BusinessName == null)
                ModelState.AddModelError(nameof(viewModel.BusinessName), "Vets Business Name is required.");
            if (viewModel.PhNumber == null)
                ModelState.AddModelError(nameof(viewModel.PhNumber), "Vets Phone Number is required.");
            if (viewModel.Email == null)
                ModelState.AddModelError(nameof(viewModel.Email), "Vets Email is required.");
            if (viewModel.StreetAddress == null)
                ModelState.AddModelError(nameof(viewModel.StreetAddress), "Vets Street Address is required.");
            if (viewModel.SuburbName == null)
                ModelState.AddModelError(nameof(viewModel.SuburbName), "Vets Suburb is required.");
            if (viewModel.Postcode == null)
                ModelState.AddModelError(nameof(viewModel.Postcode), "Vets Postcode is required.");
            if (viewModel.State == null)
                ModelState.AddModelError(nameof(viewModel.State), "Vets State is required.");

            if (!Regex.IsMatch(viewModel.Postcode, @"(^0[289][0-9]{2}\s*$)|(^[1-9][0-9]{3}\s*$)"))
                ModelState.AddModelError(nameof(viewModel.Postcode), "This postcode does not match any Australian postcode. Please enter an Australian 4 digit postcode");
            // Not perfect and needs updates for proper Australian phone numbers.
            if (!Regex.IsMatch(viewModel.PhNumber, @"^(\+?\(61\)|\(\+?61\)|\+?61|(0[1-9])|0[1-9])?( ?-?[0-9]){7,9}$"))
                ModelState.AddModelError(nameof(viewModel.PhNumber), "This is not a valid Australian mobile phone number. Please enter a valid Australian mobile phone number");
            if (!Regex.IsMatch(viewModel.Email, @"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+\s?$"))
                ModelState.AddModelError(nameof(viewModel.Email), "This is not a valid email address. Please enter a valid email address");

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var dog = new Dog();
            dog = await _context.Dogs.FindAsync(viewModel.DogId);
            var vet = new Vet();
            vet = await _context.Vets.FindAsync(dog.Vet.Id);


            if (viewModel.SelectedField.Equals(nameof(viewModel.Name)))
                dog.Name = viewModel.Name;
            if (viewModel.SelectedField.Equals(nameof(viewModel.Breed)))
                dog.Breed = viewModel.Breed;
            if (viewModel.SelectedField.Equals(nameof(viewModel.MicrochipNumber)))
                dog.MicrochipNumber = viewModel.MicrochipNumber;

            if (viewModel.SelectedField.Equals(nameof(viewModel.Temperament)))
            {
                if (viewModel.Temperament.Equals("NonReactive"))
                    dog.Temperament = Temperament.NonReactive;
                if (viewModel.Temperament.Equals("Calm"))
                    dog.Temperament = Temperament.Calm;
                if (viewModel.Temperament.Equals("Friendly"))
                    dog.Temperament = Temperament.Friendly;
                if (viewModel.Temperament.Equals("Reactive"))
                    dog.Temperament = Temperament.Reactive;
                if (viewModel.Temperament.Equals("Agressive"))
                    dog.Temperament = Temperament.Aggressive;
            }

            if (viewModel.SelectedField.Equals(nameof(viewModel.DogSize)))
            {
                if (viewModel.DogSize.Equals("Small"))
                    dog.DogSize = DogSize.Small;
                if (viewModel.DogSize.Equals("Medium"))
                    dog.DogSize = DogSize.Medium;
                if (viewModel.DogSize.Equals("Large"))
                    dog.DogSize = DogSize.Large;
                if (viewModel.DogSize.Equals("ExtraLarge"))
                    dog.DogSize = DogSize.ExtraLarge;
            }

            if (viewModel.SelectedField.Equals(nameof(viewModel.TrainingLevel)))
            {
                if (viewModel.TrainingLevel.Equals("None"))
                    dog.TrainingLevel = TrainingLevel.None;
                if (viewModel.TrainingLevel.Equals("Basic"))
                    dog.TrainingLevel = TrainingLevel.Basic;
                if (viewModel.TrainingLevel.Equals("Fully"))
                    dog.TrainingLevel = TrainingLevel.Fully;
            }

            if (viewModel.SelectedField.Equals(nameof(viewModel.BusinessName)))
                vet.BusinessName = viewModel.BusinessName;
            if (viewModel.SelectedField.Equals(nameof(viewModel.PhNumber)))
                vet.PhNumber = viewModel.PhNumber;
            if (viewModel.SelectedField.Equals(nameof(viewModel.Email)))
                vet.Email = viewModel.Email;
            if (viewModel.SelectedField.Equals(nameof(viewModel.StreetAddress)))
                vet.StreetAddress = viewModel.StreetAddress;
            if (viewModel.SelectedField.Equals(nameof(viewModel.State)))
                vet.State = viewModel.State;

            bool match = false;
            // Creating suburb based on form details
            if (viewModel.SelectedField.Equals(nameof(viewModel.SuburbName)) || viewModel.SelectedField.Equals(nameof(viewModel.Postcode)))
            {
                var suburb = new Suburb();

                suburb.SuburbName = viewModel.SuburbName;
                suburb.Postcode = viewModel.Postcode;

                // Check is Suburb is already known to Easy Walk DB, and rejects entry if known.
                foreach (var s in _context.Suburbs)
                {
                    if (s.Postcode == viewModel.Postcode && s.SuburbName == viewModel.SuburbName)
                    {
                        match = true;
                        suburb = s;
                    }
                }
                if (!match)
                    _context.Suburbs.Add(suburb);

                vet.Suburb = suburb;

            }

            // Checking BusinessName for now but this is wrong as BusinessName is not key.
            match = false;
            foreach (var v in _context.Vets)
            {
                if (v.BusinessName == viewModel.BusinessName)
                {
                    match = true;
                    vet = v;
                }
            }
            if (!match)
                _context.Vets.Add(vet);

            dog.Vet = vet;

            await _context.SaveChangesAsync();

            return View("EditDogProfile", viewModel);

        }


        //    owner.Dogs.Add(
        //    new Dog
        //    {
        //        DogSize = size,
        //        Temperament = temperament,
        //        ID = id,
        //        Name = name,
        //        MicrochipNumber = microchip,
        //        IsVaccinated = IsVaccinated,
        //        Owner = owner,
        //        Vet = vet
        //    });

        //    await _context.SaveChangesAsync();

        //    return RedirectToAction(nameof(Index));
        //}

        //// Match suitable walkers to the dog

        //public async Task<IActionResult> MatchWalkersToDog(int id) => View(await _context.Dogs.FindAsync(id));

        //[HttpPost]
        //public async Task MatchWalkersToDog(int id)
        //{
        //    var dog = await _context.Dogs.FindAsync(id);


        //    var walkers = await _context.Walker.FindAsync();


        //    var tempList = new List<Walker>();

        //    var score = 0;

        //    // Sets a difficulty score to the dog
        //    if (dog != null)
        //    {
        //        score += (int)dog.DogSize;
        //        score += (int)dog.Temperament;
        //    }
        //    foreach (var walker in walkers)
        //    {
        //        // Below matches dog to suitable Walkers
        //        if ((int)walker.ExperienceLevel == 4)
        //        {
        //            tempList.Add(walker);
        //        }
        //        else if ((int)walker.ExperienceLevel == 3 && score < 7)
        //        {
        //            tempList.Add(walker);
        //        }
        //        else if ((int)walker.ExperienceLevel == 2 && score < 5)
        //        {
        //            tempList.Add(walker);
        //        }
        //        else if ((int)walker.ExperienceLevel == 1 && score < 3)
        //        {
        //            tempList.Add(walker);
        //        }
        //    }

        //    // Could add logic here to filter list down based on user preferences.
        //    // E.g.Location or dates/times. Some filter work started below...

        //    // Notes to discuss: AllowUninsured + min experiencelevel as a question
        //    // when adding dog (saved to model). Allows user to set requirements and
        //    // makes filtering easy (as per below).

        //    foreach (var temp in tempList)
        //    {
        //        if (temp.IsInsured == false /*&& dog.Owner.AllowUninsured == false */)
        //            tempList.Remove(temp);
        //        //if (temp.ExperienceLevel > dog.MinimumAllowedExperienceLevel)
        //        //    tempList.Remove(temp);
        //    }


        //    // Return tempList to View. View to list suitable walkers
        //    // with contact details/button for the owner to select.
        //    // Might be worth returning an IPagedList .DP
        //    ViewBag.Walker = tempList;

        //}


    }

}


