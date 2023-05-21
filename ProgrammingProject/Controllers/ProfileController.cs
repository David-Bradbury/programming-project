using Microsoft.AspNetCore.Mvc;
using ProgrammingProject.Data;
using ProgrammingProject.Models;
using ProgrammingProject.Filters;
using System.Text.RegularExpressions;
using ProgrammingProject.Utilities;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProgrammingProject.Helper;
using Microsoft.AspNetCore.Hosting;
using System.Web.Helpers;
using System.Dynamic;


namespace ProgrammingProject.Controllers
{
    public class ProfileController : BaseController
    {
        private readonly EasyWalkContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private int UserID => HttpContext.Session.GetInt32(nameof(Owner.UserId)).Value;


        [AuthorizeUser]
        public ProfileController(EasyWalkContext context, IWebHostEnvironment webHostEnvironment) : base(context, webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // Method for users to view/edit their profile information.
        [AuthorizeUser]
        public async Task<IActionResult> Index()
        {
            var isAdmin = false;

            var o = new Owner();
            o = await _context.Owners.FindAsync(UserID);

            var w = new Walker();
            w = await _context.Walkers.FindAsync(UserID);

            var viewModel = CreateEditProfileViewModel(w, o, isAdmin);
            viewModel.UserID = UserID;
            // new
            ViewBag.ActiveView = "Index";

            return View(viewModel);

        }

        // Method for Admins to view a users profile.
        [AuthorizeUser]
        public async Task<IActionResult> AdminIndex(int id)
        {
            var isAdmin = true;
            var o = new Owner();
            o = await _context.Owners.FindAsync(id);

            var w = new Walker();
            w = await _context.Walkers.FindAsync(id);
            var viewModel = CreateEditProfileViewModel(w, o, isAdmin);
          //  viewModel.UserType = "Administrator";
            viewModel.UserID = id;

            return View("Index", viewModel);

        }

        public EditProfileViewModel CreateEditProfileViewModel(Walker w, Owner o, bool isAdmin)
        {
            var viewModel = new EditProfileViewModel();

            viewModel.IsAdmin = isAdmin;


            viewModel.IsInsuredList = DropDownLists.GetInsuranceList();
            viewModel.ExperienceList = DropDownLists.GetExperienceLevel();
            viewModel.StatesList = DropDownLists.GetStates();
            ViewBag.SuburbsList = _context.Suburbs.ToList();

            //Check usertype and create viewModel
            if (o == null)
            {
                //User is Walker

                viewModel.UserType = typeof(Walker).Name;

                viewModel.FirstName = w.FirstName;
                viewModel.LastName = w.LastName;
                viewModel.Email = w.Email;
                viewModel.StreetAddress = w.StreetAddress;
                viewModel.SuburbName = w.Suburb.SuburbName;
                viewModel.Postcode = w.Suburb.Postcode;
                viewModel.State = w.Suburb.State;
                viewModel.Country = w.Country;
                viewModel.PhNumber = w.PhNumber;
                viewModel.SavedProfileImage = w.ProfileImage;

                if (w.IsInsured == true)
                    viewModel.IsInsured = "Insured";
                else
                    viewModel.IsInsured = "Uninsured";

                if (w.ExperienceLevel == ExperienceLevel.Beginner)
                    viewModel.ExperienceLevel = "Beginner";
                else if (w.ExperienceLevel == ExperienceLevel.Intermediate)
                    viewModel.ExperienceLevel = "Intermediate";
                else if (w.ExperienceLevel == ExperienceLevel.Advanced)
                    viewModel.ExperienceLevel = "Advanced";
                else
                    viewModel.ExperienceLevel = "Expert";
            }
            else
            {
                //User is Owner

                viewModel.UserType = typeof(Owner).Name;

                viewModel.FirstName = o.FirstName;
                viewModel.LastName = o.LastName;
                viewModel.Email = o.Email;
                viewModel.StreetAddress = o.StreetAddress;
                viewModel.SuburbName = o.Suburb.SuburbName;
                viewModel.Postcode = o.Suburb.Postcode;
                viewModel.State = o.Suburb.State;
                viewModel.Country = o.Country;
                viewModel.PhNumber = o.PhNumber;
                viewModel.SavedProfileImage = o.ProfileImage;
            }

            return viewModel;
        }

        // Here the changes made in the profile index view are checked and changes saved to the db.
        [Route("/Profile/EditProfile")]
        
        public async Task<IActionResult> EditProfile(EditProfileViewModel viewModel, int id)
        {
            // Finds the profile holder from db.
            var o = new Owner();
            o = await _context.Owners.FindAsync(viewModel.UserID);

            var w = new Walker();
            w = await _context.Walkers.FindAsync(viewModel.UserID);

            // Sets up lists in case of model state failure.
            viewModel.StatesList = DropDownLists.GetStates();
            viewModel.IsInsuredList = DropDownLists.GetInsuranceList();
            viewModel.ExperienceList = DropDownLists.GetExperienceLevel();
            viewModel.StatesList = DropDownLists.GetStates();
            ViewBag.SuburbsList = _context.Suburbs.ToList();

            viewModel.Password = "";
            viewModel.ConfirmPassword = "";

            // return user to edit password view if Edit Password button pressed.
            if (id == 1)
            {
                var userO = await _context.Owners.FindAsync(viewModel.UserID);
                var userW = await _context.Walkers.FindAsync(viewModel.UserID);
                viewModel.UserType = userW == null ? typeof(Owner).Name : typeof(Walker).Name;
                viewModel.FirstName = userW == null ? userO.FirstName : userW.FirstName;
                viewModel.LastName = userW == null ? userO.LastName : userW.LastName;
                viewModel.SavedProfileImage = userW == null ? userO.ProfileImage : userW.ProfileImage;
                ViewBag.ActiveView = "EditPassword";
                return View("EditPassword", viewModel);
            }

            // Check if key values are null.
            CheckNull(viewModel.FirstName, nameof(viewModel.FirstName), "First Name is Required");
            CheckNull(viewModel.LastName, nameof(viewModel.LastName), "Last Name is Required");
            CheckNull(viewModel.StreetAddress, nameof(viewModel.StreetAddress), "Street Address is Required");
            CheckNull(viewModel.SuburbName, nameof(viewModel.SuburbName), "Suburb Name is Required");
            CheckNull(viewModel.State, nameof(viewModel.State), "State is Required");
            CheckNull(viewModel.Postcode, nameof(viewModel.Postcode), "Postcode is Required");
            CheckNull(viewModel.PhNumber, nameof(viewModel.PhNumber), "Phone Number is Required");

            if (o == null)
            {
                CheckNull(viewModel.ExperienceLevel, nameof(viewModel.ExperienceLevel), "Experience Level is Required");
                CheckNull(viewModel.IsInsured, nameof(viewModel.IsInsured), "Insurance Status is Required");
            }

            // Checking regex values
            string regex = @"(^0[289][0-9]{2}\s*$)|(^[1-9][0-9]{3}\s*$)";
            CheckRegex(viewModel.Postcode, nameof(viewModel.Postcode), regex, "This postcode does not match any Australian postcode. Please enter an Australian 4 digit postcode");
            // Not perfect and needs updates for proper Australian phone numbers.
            regex = @"^(\+?\(61\)|\(\+?61\)|\+?61|(0[1-9])|0[1-9])?( ?-?[0-9]){7,9}$";
            CheckRegex(viewModel.PhNumber, nameof(viewModel.PhNumber), regex, "This is not a valid Australian mobile phone number.Please enter a valid Australian mobile phone number");

            // Checks Suburb is a valid Australian Suburb.
            CheckSuburbModelState(viewModel.SuburbName, viewModel.Postcode, viewModel.State);

            // Checks the extension of the file to ensure a certain file format.
            if (viewModel.ProfileImage != null)
                CheckImageExtension(viewModel.ProfileImage, nameof(viewModel.ProfileImage));

            // Checking to see if the state of the model is valid before continuing.
            if (!ModelState.IsValid)
            {
                return View("Index", viewModel);
            }

            // Creating suburb based on form details
            var suburb = new Suburb();
            suburb.SuburbName = viewModel.SuburbName;
            suburb.Postcode = viewModel.Postcode;
            suburb.State = viewModel.State;

            var CreateHelper = new Create(_context, _webHostEnvironment);

            // Check usertype and create viewModel
            if (o == null)
            {
                //User is Walker
                CreateHelper.CreateWalker(viewModel.FirstName, viewModel.LastName, viewModel.Email, viewModel.StreetAddress,
                viewModel.Country, viewModel.PhNumber, viewModel.IsInsured, viewModel.ExperienceLevel, viewModel.ProfileImage, suburb, viewModel.UserID);

                viewModel.UserType = typeof(Walker).Name;
                HttpContext.Session.SetString(nameof(w.FirstName), w.FirstName);
                viewModel.SavedProfileImage = w.ProfileImage;
            }
            else
            {
                //User is Owner
                CreateHelper.CreateOwner(viewModel.FirstName, viewModel.LastName, viewModel.Email, viewModel.StreetAddress,
                    viewModel.Country, viewModel.PhNumber, viewModel.ProfileImage, suburb, viewModel.UserID);

                viewModel.UserType = typeof(Owner).Name;
                HttpContext.Session.SetString(nameof(o.FirstName), o.FirstName);
                viewModel.SavedProfileImage = o.ProfileImage;
            }

            await _context.SaveChangesAsync();

            if (viewModel.IsAdmin)
                return RedirectToAction("Index", "Administrator");

            else if (w == null)
                return RedirectToAction("Index", "Owner");

            else
                return RedirectToAction("Index", "Walker");
        }


        // Here password checks are completed and changes made to the db.
        public async Task<IActionResult> EditPassword(string password, string confirmPassword)
        {
            // Finds profile holder.
            var o = new Owner();
            o = await _context.Owners.FindAsync(UserID);

            var w = new Walker();
            w = await _context.Walkers.FindAsync(UserID);

            // Checks if password is Null.
            CheckNull(password, nameof(password), "Password is Required");
            // Checks password is valid.
            CheckValidPassword(password, confirmPassword);

            // Checking to see if the state of the model is valid before continuing.
            if (!ModelState.IsValid)
            {
                return View();
            }

            //Check if walker or Owner and update fields accordingly.
            if (w == null)
                o.Login.PasswordHash = ControllerHelper.HashPassword(password);
            else
                w.Login.PasswordHash = ControllerHelper.HashPassword(password);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // Returns the view that shows a list of an owners dogs.
        public async Task<IActionResult> ViewDogs()
        {
            var owner = await _context.Owners.FindAsync(UserID);

            //creates the owners profile view model.
            EditProfileViewModel viewModel = SetViewModel();
            ViewBag.EditProfileViewModel = viewModel;

            return View(owner);
        }

        // Shows dog information to be viewed/edited.
        public async Task<IActionResult> EditDogProfile(int dogId)
        {
            //creates the owners profile view model.
            EditProfileViewModel vm = SetViewModel();
            ViewBag.EditProfileViewModel = vm;

            // Finds dog and vat information.
            var dog = new Dog();
            dog = await _context.Dogs.FindAsync(dogId);

            var vet = new Vet();
            vet = await _context.Vets.FindAsync(dog.Vet.Id);

            // creates the view model for editing a dogs information.
            var viewModel = new EditDogProfileViewModel();

            // Sets the lists needed in the form.
            viewModel.DogSizeList = DropDownLists.GetDogSize();
            viewModel.TemperamentList = DropDownLists.GetTemperament();
            viewModel.TrainingLevelList = DropDownLists.GetTrainingLevel();
            viewModel.StatesList = DropDownLists.GetStates();
            viewModel.IsVaccinatedList = DropDownLists.GetVaccinatedList();

            // Sets the forms variable based on the dog. 
            viewModel.DogId = dog.Id;
            viewModel.Name = dog.Name;
            viewModel.Breed = dog.Breed.BreedName;
            viewModel.MicrochipNumber = dog.MicrochipNumber;
            viewModel.SavedProfileImage = dog.ProfileImage;

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

            // Sets the forms data based on the dogs vet.
            viewModel.SuburbName = vet.Suburb.SuburbName;
            viewModel.Postcode = vet.Suburb.Postcode;
            viewModel.BusinessName = vet.BusinessName;
            viewModel.PhNumber = vet.PhNumber;
            viewModel.Email = vet.Email;
            viewModel.StreetAddress = vet.StreetAddress;
            viewModel.State = vet.Suburb.State;
            viewModel.Country = vet.Country;

            ViewBag.BreedsList = _context.Breeds.ToList();

            return View(viewModel);
        }

        // This is called when the data of a dog is changed in the view EditDogProfile.
        public async Task<IActionResult> EditDogProfileSave(EditDogProfileViewModel viewModel)
        {
            // creates the owners profile view model.
            EditProfileViewModel vm = SetViewModel();
            ViewBag.EditProfileViewModel = vm;

            // retrievs the dog from the db.
            var dog = new Dog();
            dog = await _context.Dogs.FindAsync(viewModel.DogId);
            var vet = new Vet();
            vet = await _context.Vets.FindAsync(dog.Vet.Id);
            var owner = new Owner();
            owner = await _context.Owners.FindAsync(UserID);


            // Sets the list needed in case of model state error.
            viewModel.StatesList = DropDownLists.GetStates();
            viewModel.TemperamentList = DropDownLists.GetTemperament();
            viewModel.DogSizeList = DropDownLists.GetDogSize();
            viewModel.TrainingLevelList = DropDownLists.GetTrainingLevel();

            // Check if key values are null.
            CheckNull(viewModel.Name, nameof(viewModel.Name), "Dogs Name is required.");
            CheckNull(viewModel.Breed, nameof(viewModel.Breed), "Dogs Breed is required.");
            CheckNull(viewModel.Temperament, nameof(viewModel.Temperament), "Dogs Temperament is required.");
            CheckNull(viewModel.DogSize, nameof(viewModel.DogSize), "Dogs Size is required.");
            CheckNull(viewModel.TrainingLevel, nameof(viewModel.TrainingLevel), "Dogs Training Level is required.");

            // Check if Image extension acceptable
            if (viewModel.ProfileImage != null)
                CheckImageExtension(viewModel.ProfileImage, nameof(viewModel.ProfileImage));

            // Checking to see if the state of the model is valid before continuing.
            if (!ModelState.IsValid)
            {
                ViewBag.BreedsList = _context.Breeds.ToList();
                return View("EditDogProfile", viewModel);
            }

            var CreateHelper = new Create(_context, _webHostEnvironment);

            viewModel.SavedProfileImage = dog.ProfileImage;

            // creating the dog.
            CreateHelper.CreateDog(viewModel.Name, viewModel.Breed, viewModel.MicrochipNumber, viewModel.Temperament,
                viewModel.DogSize, viewModel.TrainingLevel, viewModel.ProfileImage, vet, owner, viewModel.DogId);

            await _context.SaveChangesAsync();

            return RedirectToAction("ViewDogs");
        }


        // Sets up the view EditVet.
        public async Task<IActionResult> EditVet(EditDogProfileViewModel viewModel)
        {
            // Sets the list needed in the next view.
            viewModel.StatesList = DropDownLists.GetStates();

            // creates the owners profile view model.
            EditProfileViewModel vm = SetViewModel();
            ViewBag.EditProfileViewModel = vm;
            ViewBag.SuburbsList = _context.Suburbs.ToList();

            return View(viewModel);
        }


        // The changes made to a vet in EditVet view are validated and saved to the db.
        public async Task<IActionResult> EditVetSave(EditDogProfileViewModel viewModel)
        {

            // Retrieving dog and vet info in db.
            var dog = new Dog();
            dog = await _context.Dogs.FindAsync(viewModel.DogId);
            var vet = new Vet();
            vet = await _context.Vets.FindAsync(dog.Vet.Id);

            // Checking if key fields are Null.
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

            // Check Suburb is valid.
            CheckSuburbModelState(viewModel.SuburbName, viewModel.Postcode, viewModel.State);

            // Checks the extension of the file to ensure a certain file format.
            if (viewModel.ProfileImage != null)
                CheckImageExtension(viewModel.ProfileImage, nameof(viewModel.ProfileImage));

            // Checking to see if the state of the model is valid before continuing.
            if (!ModelState.IsValid)
            {
                viewModel.StatesList = DropDownLists.GetStates();
                ViewBag.SuburbsList = _context.Suburbs.ToList();
                return View("EditVet", viewModel);
            }

            // Converting IFormFile to string.
            var ImageHelper = new ImageHelper(_webHostEnvironment);
            string imageFileName = ImageHelper.UploadFile(viewModel.ProfileImage);

            if (viewModel.ProfileImage != null)
                dog.ProfileImage = imageFileName;

            viewModel.SavedProfileImage = dog.ProfileImage;

            var CreateHelper = new Create(_context, _webHostEnvironment);

            var suburb = new Suburb();
            suburb.SuburbName = viewModel.SuburbName;
            suburb.Postcode = viewModel.Postcode;
            suburb.State = viewModel.State;

            int vetId = vet.Id;
            viewModel.Country = vet.Country;

            vet = CreateHelper.CreateVet(viewModel.BusinessName, viewModel.PhNumber, viewModel.Email, viewModel.StreetAddress, viewModel.Country, suburb, vetId);

            // Checking BusinessName for now but this is wrong as BusinessName is not key.
            bool match = false;
            foreach (var v in _context.Vets)
            {
                if (v.BusinessName == viewModel.BusinessName)
                {
                    match = true;
                    vet = v;
                }
            }
            if (!match & viewModel.BusinessName != null)
                _context.Vets.Add(vet);

            dog.Vet = vet;

            await _context.SaveChangesAsync();

            return RedirectToAction("EditDogProfile", new { dogId = viewModel.DogId });
        }

        // Deletes a dog from the db.
        public async Task<IActionResult> DeleteDog(int id)
        {

            var dog = await _context.Dogs.FindAsync(id);

            _context.Dogs.Remove(dog);

            _context.SaveChanges();
            ;
            return RedirectToAction("ViewDogs");
        }

        // Helper method only required in profile controller.
        public EditProfileViewModel SetViewModel()
        {
            var owner = _context.Owners.Find(UserID);

            EditProfileViewModel vm = new EditProfileViewModel();

            vm.UserType = typeof(Owner).Name;
            vm.FirstName = owner.FirstName;
            vm.LastName = owner.LastName;
            vm.Email = owner.Email;
            vm.StreetAddress = owner.StreetAddress;
            vm.SuburbName = owner.Suburb.SuburbName;
            vm.Postcode = owner.Suburb.Postcode;
            vm.State = owner.Suburb.State;
            vm.Country = owner.Country;
            vm.PhNumber = owner.PhNumber;
            vm.SavedProfileImage = owner.ProfileImage;

            return vm;
        }
    }
}