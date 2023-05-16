using ProgrammingProject.Data;
using ProgrammingProject.Models;
using ProgrammingProject.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using ProgrammingProject.Helper;
using System.Text.RegularExpressions;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using GeoCoordinatePortable;

namespace ProgrammingProject.Controllers
{
    //Mask URL
    [Route("/Owner/[action]")]


    public class OwnerController : BaseController
    {
        private readonly EasyWalkContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AddDogViewModel viewModel = new AddDogViewModel();
        private int OwnerID => HttpContext.Session.GetInt32(nameof(Owner.UserId)).Value;

        public OwnerController(EasyWalkContext context, IWebHostEnvironment webHostEnvironment) : base(context, webHostEnvironment)

        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // Owner Landing Page.
        [AuthorizeUser]

        public async Task<IActionResult> Index()
        {
            int? range = 1000;
            var owner = await _context.Owners.FindAsync(OwnerID);

            ViewBag.WalkingSessions = await GetSuitableWalkingSessions(owner, range);


            ViewBag.BookedSessions = await GetBookedWalkingSessions(owner);


            //  ViewBag.Walkers = await GetLocalWalkers();
            return View(owner);
        }

        public async Task<List<WalkingSession>> GetBookedWalkingSessions(Owner owner)
        {
            var bookedSessions = new List<WalkingSession>();
            foreach (Dog d in owner.Dogs)
            {
                var walks = _context.WalkingSessions.Where(w => w.DogList.Contains(d)).OrderBy(o => o.Date).ToList();
                foreach (WalkingSession walk in walks)
                {
                    if (!bookedSessions.Contains(walk))
                        bookedSessions.Add(walk);
                }
            }

            return bookedSessions;

        }

        public async Task<List<WalkingSession>> GetSuitableWalkingSessions(Owner owner, int? range = 1000)
        {
            var userLocation = new GeoCoordinate(double.Parse(owner.Suburb.Lat), double.Parse(owner.Suburb.Lon));
            var walkerLocation = new GeoCoordinate();

            var localWalkers = new List<Walker>();
            var suitableSessions = new List<WalkingSession>();

            foreach (Walker w in _context.Walkers)
            {
                if (w.Suburb != null)
                {
                    walkerLocation.Latitude = double.Parse(owner.Suburb.Lat);
                    walkerLocation.Longitude = double.Parse(owner.Suburb.Lon);

                    if (userLocation.GetDistanceTo(walkerLocation) < range)
                    {
                        localWalkers.Add(w);
                    }
                }

            }

            foreach (Walker w in localWalkers)
            {
                foreach (WalkingSession walkingSession in w.WalkingSessions)
                {
                    suitableSessions.Add(walkingSession);
                }
            }
            var orderedSuitableSessions = suitableSessions.OrderBy(s => s.Date).ToList();

            //  var walkingSessions = await _context.WalkingSessions.OrderBy(o => o.Date).ToListAsync();
            return orderedSuitableSessions;
        }

        public async Task<IActionResult> RemoveDogFromSession(int sessionID)
        {
            var owner = await _context.Owners.FindAsync(OwnerID);
            var walkingSession = await _context.WalkingSessions.FindAsync(sessionID);

            ViewBag.WalkingSession = walkingSession;
            return View(owner);
        }
        public async Task<IActionResult> AddDogToSession(int sessionID)
        {
            var owner = await _context.Owners.FindAsync(OwnerID);
            var walkingSession = await _context.WalkingSessions.FindAsync(sessionID);

            ViewBag.WalkingSession = walkingSession;
            return View(owner);
        }

        public async Task<IActionResult> UpdateNewDogInSession(int sessionID, int dogID)
        {
            var walkingSession = await _context.WalkingSessions.FindAsync(sessionID);
            var dog = await _context.Dogs.FindAsync(dogID);
            walkingSession.DogList.Add(dog);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }



        //Add a dog to the owner
        [Route("/Owner/AddDog",
     Name = "AddDog"), AuthorizeUser]
        public async Task<IActionResult> AddDog()
        {
            // Sets list needed in form.
            viewModel.DogSizeList = DropDownLists.GetDogSize();
            viewModel.TemperamentList = DropDownLists.GetTemperament();
            viewModel.TrainingLevelList = DropDownLists.GetTrainingLevel();
            viewModel.StatesList = DropDownLists.GetStates();
            viewModel.IsVaccinatedList = DropDownLists.GetVaccinatedList();

            ViewBag.BreedsList = _context.Breeds.ToList();
            ViewBag.SuburbsList = _context.Suburbs.ToList();

            return View(viewModel);
        }

        // Post method for adding dogs to the system. Validates entries.
        [Route("/Owner/AddDog",
    Name = "AddDog")]
        [HttpPost]
        public async Task<IActionResult> AddDog(AddDogViewModel viewModel)
        {
            // Creates lists in the case of model state failure.
            viewModel.DogSizeList = DropDownLists.GetDogSize();
            viewModel.TemperamentList = DropDownLists.GetTemperament();
            viewModel.TrainingLevelList = DropDownLists.GetTrainingLevel();
            viewModel.StatesList = DropDownLists.GetStates();
            viewModel.IsVaccinatedList = DropDownLists.GetVaccinatedList();

            var owner = await _context.Owners.FindAsync(OwnerID);

            // Checking if key values are Null.
            CheckNull(viewModel.Name, nameof(viewModel.Name), "Dogs Name is required.");
            CheckNull(viewModel.Breed, nameof(viewModel.Breed), "Dogs Breed is required.");
            CheckNull(viewModel.IsVaccinated, nameof(viewModel.IsVaccinated), "Dogs vaccinated status is required.");
            CheckNull(viewModel.Temperament, nameof(viewModel.Temperament), "Dogs Temperament is required.");
            CheckNull(viewModel.DogSize, nameof(viewModel.DogSize), "Dogs Size is required.");
            CheckNull(viewModel.TrainingLevel, nameof(viewModel.TrainingLevel), "Dogs Training Level is required.");
            CheckNull(viewModel.BusinessName, nameof(viewModel.BusinessName), "Vets Business Name is required.");
            CheckNull(viewModel.PhNumber, nameof(viewModel.PhNumber), "Vets Phone Number is required.");
            CheckNull(viewModel.Email, nameof(viewModel.Email), "Vets Email is required.");
            CheckNull(viewModel.StreetAddress, nameof(viewModel.StreetAddress), "Vets Street Address is required.");
            CheckNull(viewModel.SuburbName, nameof(viewModel.SuburbName), "Vets Suburb Name is required.");
            CheckNull(viewModel.Postcode, nameof(viewModel.Postcode), "Vets Postcode is required.");
            CheckNull(viewModel.State, nameof(viewModel.State), "Vets State is required.");

            // Checking regex values
            string regex = @"(^0[289][0-9]{2}\s*$)|(^[1-9][0-9]{3}\s*$)";
            CheckRegex(viewModel.Postcode, nameof(viewModel.Postcode), regex, "This postcode does not match any Australian postcode. Please enter an Australian 4 digit postcode");
            regex = @"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+\s?$";
            CheckRegex(viewModel.Email, nameof(viewModel.Email), regex, "This is not a valid email address. Please enter a valid email address");
            // Not perfect and needs updates for proper Australian phone numbers.
            regex = @"^(\+?\(61\)|\(\+?61\)|\+?61|(0[1-9])|0[1-9])?( ?-?[0-9]){7,9}$";
            CheckRegex(viewModel.PhNumber, nameof(viewModel.PhNumber), regex, "This is not a valid Australian mobile phone number.Please enter a valid Australian mobile phone number");

            if (viewModel.IsVaccinated.Equals("Unvaccinated"))
                ModelState.AddModelError(nameof(viewModel.IsVaccinated), "Sorry, All dogs must be vaccinated before being registered with EasyWalk");

            // Check Suburb is valid.
            CheckSuburbModelState(viewModel.SuburbName, viewModel.Postcode, viewModel.State);

            // Checks the extension of the file to ensure a certain file format.
            if (viewModel.ProfileImage != null)
                CheckImageExtension(viewModel.ProfileImage, nameof(viewModel.ProfileImage));

            // Checking to see if the state of the model is valid before continuing.
            if (!ModelState.IsValid)
            {
                ViewBag.BreedsList = _context.Breeds.ToList();
                ViewBag.SuburbsList = _context.Suburbs.ToList();

                return View(viewModel);
            }

            var suburb = new Suburb();
            suburb.SuburbName = viewModel.SuburbName;
            suburb.Postcode = viewModel.Postcode;
            suburb.State = viewModel.State;


            var CreateHelper = new Create(_context, _webHostEnvironment);

            // Create a new Vet from form submission.
            int vetId = 0;

            var vet = CreateHelper.CreateVet(viewModel.BusinessName, viewModel.PhNumber, viewModel.Email, viewModel.StreetAddress, viewModel.Country, suburb, vetId);

            // Checking BusinessName for now but this is wrong as BusinessName is not key. NEED TO RETHINK THIS!
            bool match = false;
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
            int DogId = 0;

            CreateHelper.CreateDog(viewModel.Name, viewModel.Breed, viewModel.MicrochipNumber, viewModel.Temperament,
                viewModel.DogSize, viewModel.TrainingLevel, viewModel.ProfileImage, vet, owner, DogId);


            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}


