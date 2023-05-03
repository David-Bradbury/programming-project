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
        public ProfileController(EasyWalkContext context, IWebHostEnvironment webHostEnvironment) : base(context) 
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
            viewModel.UserType = "Administrator";
            viewModel.UserID = id;

            return View("Index", viewModel);

        }

        public EditProfileViewModel CreateEditProfileViewModel(Walker w, Owner o, bool isAdmin)
        {
            var viewModel = new EditProfileViewModel();

            viewModel.IsAdmin = isAdmin;
            if (isAdmin)
                viewModel.UserType = "Administrator";

            viewModel.IsInsuredList = DropDownLists.GetInsuranceList();
            viewModel.ExperienceList = DropDownLists.GetExperienceLevel();
            viewModel.StatesList = DropDownLists.GetStates();
            ViewBag.SuburbsList = _context.Suburbs.ToList();

            //Check usertype and create viewModel
            if (o == null)
            {
                //User is Walker
                if (!isAdmin)
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
                if (!isAdmin)
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
                return View("EditPassword", viewModel);

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
            CheckImageExtension(viewModel.ProfileImage, nameof(viewModel.ProfileImage));

            // Checking to see if the state of the model is valid before continuing.
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            // Converting IFormFile to string.
            var ImageHelper = new ImageHelper(_webHostEnvironment);
            string imageFileName = ImageHelper.UploadFile(viewModel.ProfileImage);

            // Creating suburb based on form details
            var suburb = new Suburb();
            suburb.SuburbName = viewModel.SuburbName;
            suburb.Postcode = viewModel.Postcode;
            suburb.State = viewModel.State;

            // Check usertype and create viewModel
            if (o == null)
            {
                //User is Walker
                viewModel.UserType = typeof(Walker).Name;
                w.FirstName = viewModel.FirstName;
                HttpContext.Session.SetString(nameof(w.FirstName), w.FirstName);
                w.LastName = viewModel.LastName;
                w.Email = viewModel.Email;
                w.StreetAddress = viewModel.StreetAddress;
                w.Suburb = suburb;
                w.Country = viewModel.Country;
                w.PhNumber = viewModel.PhNumber;

                if (viewModel.ProfileImage != null)
                    w.ProfileImage = imageFileName;
                else if (viewModel.SavedProfileImage != imageFileName)
                    w.ProfileImage = "defaultProfile.png";

                viewModel.SavedProfileImage = w.ProfileImage;

                if (viewModel.IsInsured == "true")
                    w.IsInsured = true;
                else
                    w.IsInsured = false;

                if (viewModel.ExperienceLevel == "Beginner")
                    w.ExperienceLevel = ExperienceLevel.Beginner;
                else if (viewModel.ExperienceLevel == "Intermediate")
                    w.ExperienceLevel = ExperienceLevel.Intermediate;
                else if (viewModel.ExperienceLevel == "Advanced")
                    w.ExperienceLevel = ExperienceLevel.Advanced;
                else
                    w.ExperienceLevel = ExperienceLevel.Expert;
            }
            else
            {
                //User is Owner
                viewModel.UserType = typeof(Owner).Name;
                o.FirstName = viewModel.FirstName;
                HttpContext.Session.SetString(nameof(o.FirstName), o.FirstName);
                o.LastName = viewModel.LastName;
                o.Email = viewModel.Email;
                o.StreetAddress = viewModel.StreetAddress;
                o.Suburb = suburb;
                o.Country = viewModel.Country;
                o.PhNumber = viewModel.PhNumber;

                if (viewModel.ProfileImage != null)
                    o.ProfileImage = imageFileName;
                else if (viewModel.SavedProfileImage != imageFileName)
                    o.ProfileImage = "defaultProfile.png";

                viewModel.SavedProfileImage = o.ProfileImage;
            }

            await _context.SaveChangesAsync();

            if (viewModel.IsAdmin)
                return RedirectToAction("EditUser", "Administrator");

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
            viewModel.State = vet.State;
            viewModel.Country = vet.Country;

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
            CheckImageExtension(viewModel.ProfileImage, nameof(viewModel.ProfileImage));

            // Checking to see if the state of the model is valid before continuing.
            if (!ModelState.IsValid)
            {
                return View("EditDogProfile", viewModel);
            }

            // Converting IFormFile to string.
            var ImageHelper = new ImageHelper(_webHostEnvironment);
            string imageFileName = ImageHelper.UploadFile(viewModel.ProfileImage);

            // Creating breed.
            var breed = new Breed();
            breed.BreedName = viewModel.Breed;

            // Setting the dogs variables to be saved to the db.
            dog.Name = viewModel.Name;
            dog.Breed = breed;
            dog.MicrochipNumber = viewModel.MicrochipNumber;

            if (viewModel.ProfileImage != null)
                dog.ProfileImage = imageFileName;
            else if (viewModel.SavedProfileImage != imageFileName)
                dog.ProfileImage = "dog-avatar.jpg";

            viewModel.SavedProfileImage = dog.ProfileImage;

            // Set dogs vaccinations status.
            if (dog.IsVaccinated == true)
                viewModel.IsVaccinated = "True";

            // Set dog temperament level enum.
            if (viewModel.Temperament.Equals("NonReactive"))
                dog.Temperament = Temperament.NonReactive;
            else if (viewModel.Temperament.Equals("Calm"))
                dog.Temperament = Temperament.Calm;
            else if (viewModel.Temperament.Equals("Friendly"))
                dog.Temperament = Temperament.Friendly;
            else if (viewModel.Temperament.Equals("Reactive"))
                dog.Temperament = Temperament.Reactive;
            else
                dog.Temperament = Temperament.Aggressive;

            // Set dog size enum.
            if (viewModel.DogSize.Equals("Small"))
                dog.DogSize = DogSize.Small;
            else if (viewModel.DogSize.Equals("Medium"))
                dog.DogSize = DogSize.Medium;
            else if (viewModel.DogSize.Equals("Large"))
                dog.DogSize = DogSize.Large;
            else
                dog.DogSize = DogSize.ExtraLarge;

            // Set dog training level enum.
            if (viewModel.TrainingLevel.Equals("None"))
                dog.TrainingLevel = TrainingLevel.None;
            else if (viewModel.TrainingLevel.Equals("Basic"))
                dog.TrainingLevel = TrainingLevel.Basic;
            else
                dog.TrainingLevel = TrainingLevel.Fully;

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
            CheckNull(viewModel.BusinessName, nameof(viewModel.BusinessName), "Vets Business Name is required.");
            CheckNull(viewModel.BusinessName, nameof(viewModel.BusinessName), "Vets Business Name is required.");
            CheckNull(viewModel.BusinessName, nameof(viewModel.BusinessName), "Vets Business Name is required.");
            CheckNull(viewModel.BusinessName, nameof(viewModel.BusinessName), "Vets Business Name is required.");
            CheckNull(viewModel.BusinessName, nameof(viewModel.BusinessName), "Vets Business Name is required.");
            CheckNull(viewModel.BusinessName, nameof(viewModel.BusinessName), "Vets Business Name is required.");

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

            if (viewModel.SelectedField == nameof(viewModel.Postcode) && !Regex.IsMatch(viewModel.Postcode, @"(^0[289][0-9]{2}\s*$)|(^[1-9][0-9]{3}\s*$)"))
                ModelState.AddModelError(nameof(viewModel.Postcode), "This postcode does not match any Australian postcode. Please enter an Australian 4 digit postcode");
            // Not perfect and needs updates for proper Australian phone numbers.
            if (viewModel.SelectedField == nameof(viewModel.PhNumber) && !Regex.IsMatch(viewModel.PhNumber, @"^(\+?\(61\)|\(\+?61\)|\+?61|(0[1-9])|0[1-9])?( ?-?[0-9]){7,9}$"))
                ModelState.AddModelError(nameof(viewModel.PhNumber), "This is not a valid Australian mobile phone number. Please enter a valid Australian mobile phone number");
            if (viewModel.SelectedField == nameof(viewModel.Email) && !Regex.IsMatch(viewModel.Email, @"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+\s?$"))
                ModelState.AddModelError(nameof(viewModel.Email), "This is not a valid email address. Please enter a valid email address");

            CheckSuburbModelState(viewModel.SuburbName, viewModel.Postcode, viewModel.State);

            if (viewModel.SelectedField == nameof(viewModel.SavedProfileImage) && viewModel.ProfileImage != null)
            {
                string filename = Path.GetFileName(viewModel.ProfileImage.FileName);
                string extension = Path.GetExtension(filename).ToLower();

                if (extension != ".jpg" && extension != ".jpeg" && extension != ".png")
                    ModelState.AddModelError(nameof(viewModel.ProfileImage), "Image must be of the jpg/jpeg, or png format");
            }

            if (!ModelState.IsValid)
            {
                return View("EditVet", viewModel);
            }

            var ImageHelper = new ImageHelper(_webHostEnvironment);
            string imageFileName = ImageHelper.UploadFile(viewModel.ProfileImage);

            if (viewModel.ProfileImage != null)
                dog.ProfileImage = imageFileName;
            else if (viewModel.SavedProfileImage != imageFileName)
                dog.ProfileImage = "dog-avatar.jpg";

            viewModel.SavedProfileImage = dog.ProfileImage;


            vet.BusinessName = viewModel.BusinessName;
            vet.PhNumber = viewModel.PhNumber;
            vet.Email = viewModel.Email;
            vet.StreetAddress = viewModel.StreetAddress;
            vet.State = viewModel.State;

            var suburb = new Suburb();

            suburb.SuburbName = viewModel.SuburbName;
            suburb.Postcode = viewModel.Postcode;
            suburb.State = viewModel.State;


            vet.Suburb = suburb;

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

        public async Task<IActionResult> DeleteDog(int id)
        {

            var dog = await _context.Dogs.FindAsync(id);

            _context.Dogs.Remove(dog);

            _context.SaveChanges();
            ;
            return RedirectToAction("ViewDogs");
        }

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

