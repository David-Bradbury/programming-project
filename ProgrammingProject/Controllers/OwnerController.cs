using ProgrammingProject.Data;
using ProgrammingProject.Models;
using ProgrammingProject.Filters;
using Microsoft.AspNetCore.Mvc;
using ProgrammingProject.Helper;
using System.Text.RegularExpressions;

namespace ProgrammingProject.Controllers
{
    //Mask URL
    [Route("/Owner")]

   
    public class OwnerController : Controller
    {
        private readonly EasyWalkContext _context;
        public AddDogViewModel viewModel = new AddDogViewModel();
        private int OwnerID => HttpContext.Session.GetInt32(nameof(Owner.UserId)).Value;

        public OwnerController(EasyWalkContext context)
        {
            _context = context;
        }


        [AuthorizeUser]
        public async Task<IActionResult> Index()
        {
            var owner = await _context.Owners.FindAsync(OwnerID);
            return View(owner);
        }

        //Add a dog to the owner
        [Route("/Owner/AddDog",
     Name = "AddDog")]
        public async Task<IActionResult> AddDog()
        {
            viewModel.DogSizeList = DropDownLists.GetDogSize();
            viewModel.TemperamentList = DropDownLists.GetTemperament();
            viewModel.TrainingLevelList = DropDownLists.GetTrainingLevel();
            viewModel.StatesList = DropDownLists.GetStates();
            viewModel.IsVaccinatedList =DropDownLists.GetVaccinatedList();
            
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddDog(string name, string breed, string microchipNumber, string isVaccinated, string temperament,
            string dogSize, string trainingLevel, string businessName, string phNumber, string email, string streetAddress,
            string suburbName, string postcode, string state, string country)
        {
            var viewModel = new AddDogViewModel();

            viewModel.DogSizeList = DropDownLists.GetDogSize();
            viewModel.TemperamentList = DropDownLists.GetTemperament();
            viewModel.TrainingLevelList = DropDownLists.GetTrainingLevel();
            viewModel.StatesList = DropDownLists.GetStates();
            viewModel.IsVaccinatedList = DropDownLists.GetVaccinatedList();

            var owner = new Owner();
            foreach (var o in _context.Owners)
                if (o.UserId == OwnerID)
                    owner = o;

            if (name == null)
                ModelState.AddModelError(nameof(name), "Dogs Name is required.");
            if (breed == null)
                ModelState.AddModelError(nameof(breed), "Dogs Breed is required.");
            if (microchipNumber == null)
                ModelState.AddModelError(nameof(microchipNumber), "Dogs Microchip Number is required.");
            if (isVaccinated == null)
                ModelState.AddModelError(nameof(isVaccinated), "Dogs Vaccination Status is required.");
            if (temperament == null)
                ModelState.AddModelError(nameof(temperament), "Dogs Temperament is required.");
            if (dogSize == null)
                ModelState.AddModelError(nameof(dogSize), "Dogs Size is required.");
            if (trainingLevel == null)
                ModelState.AddModelError(nameof(trainingLevel), "Dogs Training Level is required.");
            //if (image == null)
            //   ModelState.AddModelError(nameof(image), "Dogs Image is required.");
            if (businessName == null)
                ModelState.AddModelError(nameof(businessName), "Vets Business Name is required.");
            if (phNumber == null)
                ModelState.AddModelError(nameof(phNumber), "Vets Phone Number is required.");
            if (email == null)
                ModelState.AddModelError(nameof(email), "Vets Email is required.");
            if (streetAddress == null)
                ModelState.AddModelError(nameof(streetAddress), "Vets Street Address is required.");
            if (suburbName == null)
                ModelState.AddModelError(nameof(suburbName), "Vets Suburb is required.");
            if (postcode == null)
                ModelState.AddModelError(nameof(postcode), "Vets Postcode is required.");
            if (state == null)
                ModelState.AddModelError(nameof(state), "Vets State is required.");
            if (country == null)
                ModelState.AddModelError(nameof(country), "Vets Country is required.");

            if (!Regex.IsMatch(postcode, @"(^0[289][0-9]{2}\s*$)|(^[1-9][0-9]{3}\s*$)"))
                ModelState.AddModelError(nameof(postcode), "This postcode does not match any Australian postcode. Please enter an Australian 4 digit postcode");
            // Not perfect and needs updates for proper Australian phone numbers.
            if (!Regex.IsMatch(phNumber, @"^(\+?\(61\)|\(\+?61\)|\+?61|(0[1-9])|0[1-9])?( ?-?[0-9]){7,9}$"))
                ModelState.AddModelError(nameof(phNumber), "This is not a valid Australian mobile phone number. Please enter a valid Australian mobile phone number");
            if (!Regex.IsMatch(email, @"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+\s?$"))
                ModelState.AddModelError(nameof(email), "This is not a valid email address. Please enter a valid email address");
            // Maybe This Error Message IS Not Enough, maybe a pop up or something needs to occur. Discuss at nextr meeting. JC
            if (isVaccinated.Equals("Unvaccinated"))
                ModelState.AddModelError(nameof(isVaccinated), "Sorry, All dogs must be vaccinated before being registered with EasyWalk");

            // Checking to see if the state of the model is valid before continuing.
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            // Creating suburb based on form details
            var suburb = new Suburb();
            suburb.SuburbName = suburbName;
            suburb.Postcode = postcode;

            // Check is Suburb is already known to Easy Walk DB, and rejects entry if known.
            bool match = false;
            foreach (var s in _context.Suburbs)
            {
                if (s.Postcode == postcode && s.SuburbName == suburbName)
                {
                    match = true;
                    suburb = s;
                }
            }
            if (!match)
                _context.Suburbs.Add(suburb);


            // Create a new Vet from form submission.
            var vet = new Vet();

            vet.BusinessName = businessName;
            vet.PhNumber = phNumber;
            vet.Email = email;
            vet.StreetAddress = streetAddress;
            vet.Suburb = suburb;
            vet.State = state;
            vet.Country = country;

            // Checking BusinessName for now but this is wrong as BusinessName is not key.
            match = false;
            foreach(var v in _context.Vets)
            {
                if (v.BusinessName == businessName)
                {
                    match = true;
                    vet = v;
                }
            }
            if (!match)
                _context.Vets.Add(vet);


            // Create a new dog from form submission.
            var dog = new Dog();

            dog.Name = name;
            dog.Breed = breed;
            dog.MicrochipNumber = microchipNumber;
           // dog.Image = image;
            dog.Owner = owner;
            dog.Vet = vet;
            dog.IsVaccinated = true;

            if (temperament.Equals("NonReactive"))
                dog.Temperament = Temperament.NonReactive;
            if (temperament.Equals("Calm"))
                dog.Temperament = Temperament.Calm;
            if (temperament.Equals("Friendly"))
                dog.Temperament = Temperament.Friendly;
            if (temperament.Equals("Reactive"))
                dog.Temperament = Temperament.Reactive;
            if (temperament.Equals("Agressive"))
                dog.Temperament = Temperament.Aggressive;

            if (dogSize.Equals("Small"))
                dog.DogSize = DogSize.Small;
            if (dogSize.Equals("Medium"))
                dog.DogSize = DogSize.Medium;
            if (dogSize.Equals("Large"))
                dog.DogSize = DogSize.Large;
            if (dogSize.Equals("ExtraLarge"))
                dog.DogSize = DogSize.ExtraLarge;

            if (trainingLevel.Equals("None"))
                dog.TrainingLevel = TrainingLevel.None;
            if (trainingLevel.Equals("Basic"))
                dog.TrainingLevel = TrainingLevel.Basic;
            if (trainingLevel.Equals("Fully"))
                dog.TrainingLevel = TrainingLevel.Fully;

            _context.Add(dog);

            _context.SaveChanges();


            return View("Index", viewModel);
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


