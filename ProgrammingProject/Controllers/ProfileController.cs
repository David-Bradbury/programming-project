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


namespace ProgrammingProject.Controllers
{
    public class ProfileController : Controller
    {
        private readonly EasyWalkContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private int UserID => HttpContext.Session.GetInt32(nameof(Owner.UserId)).Value;


        [AuthorizeUser]
        public ProfileController(EasyWalkContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [AuthorizeUser]
        public async Task<IActionResult> Index()
        {
            var o = new Owner();
            o = await _context.Owners.FindAsync(UserID);

            var w = new Walker();
            w = await _context.Walkers.FindAsync(UserID);

            var viewModel = new EditProfileViewModel();
            viewModel.IsInsuredList = DropDownLists.GetInsuranceList();
            viewModel.ExperienceList = DropDownLists.GetExperienceLevel();
            viewModel.StatesList = DropDownLists.GetStates();


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
                viewModel.State = w.State;
                viewModel.Country = w.Country;
                viewModel.PhNumber = w.PhNumber;
                viewModel.SavedProfileImage = w.ProfileImage;
                if (w.IsInsured == true)
                {
                    viewModel.IsInsured = "Insured";
                }
                else
                {
                    viewModel.IsInsured = "Uninsured";
                }
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
                viewModel.State = o.State;
                viewModel.Country = o.Country;
                viewModel.PhNumber = o.PhNumber;
                viewModel.SavedProfileImage = o.ProfileImage;
            }

            return View(viewModel);

        }


        public async Task<IActionResult> EditProfile(EditProfileViewModel viewModel, int id)
        {          
            var o = new Owner();
            o = await _context.Owners.FindAsync(UserID);

            var w = new Walker();
            w = await _context.Walkers.FindAsync(UserID);

            viewModel.StatesList = DropDownLists.GetStates();
            viewModel.IsInsuredList = DropDownLists.GetInsuranceList();
            viewModel.ExperienceList = DropDownLists.GetExperienceLevel();
            viewModel.StatesList = DropDownLists.GetStates();

            viewModel.Password = "";
            viewModel.ConfirmPassword = "";

            if (id == 1)
                return View("EditPassword", viewModel);

            if (viewModel.FirstName == null)
                ModelState.AddModelError(nameof(viewModel.FirstName), "First Name is required");
            if (viewModel.LastName == null)
                ModelState.AddModelError(nameof(viewModel.LastName), "Last Name is required");
            if (viewModel.StreetAddress == null)
                ModelState.AddModelError(nameof(viewModel.StreetAddress), "Street Address is required");
            if (viewModel.State == null)
                ModelState.AddModelError(nameof(viewModel.State), "State is required");
            if (viewModel.Postcode == null)
                ModelState.AddModelError(nameof(viewModel.Postcode), "Postcode is required");
            if (viewModel.PhNumber == null)
                ModelState.AddModelError(nameof(viewModel.PhNumber), "Phone Number is required");
            if (viewModel.SuburbName == null)
                ModelState.AddModelError(nameof(viewModel.SuburbName), "Suburb Name is required");

            if (o == null)
            {
                if (viewModel.IsInsured == null)
                    ModelState.AddModelError(nameof(viewModel.IsInsured), "Insured Status is required");
                if (viewModel.ExperienceLevel == null)
                    ModelState.AddModelError(nameof(viewModel.ExperienceLevel), "Experience Level is required");
            }

            // ALL NEEDS TESTING. JC        
            if (!Regex.IsMatch(viewModel.Postcode, @"(^0[289][0-9]{2}\s*$)|(^[1-9][0-9]{3}\s*$)"))
                ModelState.AddModelError(nameof(viewModel.Postcode), "This postcode does not match any Australian postcode. Please enter an Australian 4 digit postcode");
            // Not perfect and needs updates for proper Australian phone numbers.
            if (!Regex.IsMatch(viewModel.PhNumber, @"^(\+?\(61\)|\(\+?61\)|\+?61|(0[1-9])|0[1-9])?( ?-?[0-9]){7,9}$"))
                ModelState.AddModelError(nameof(viewModel.PhNumber), "This is not a valid Australian mobile phone number. Please enter a valid Australian mobile phone number");

            // Checks the extension of the file to ensure a certain file format. Bring up Thurs meeting to see if extra verification needed. JC.
            if (viewModel.ProfileImage != null)
            {
                string filename = Path.GetFileName(viewModel.ProfileImage.FileName);
                string extension = Path.GetExtension(filename).ToLower();

                if (extension != ".jpg" && extension != ".jpeg" && extension != ".png")
                    ModelState.AddModelError(nameof(viewModel.ProfileImage), "Image must be of the jpg/jpeg, or png format");
            }


            // Checking to see if the state of the model is valid before continuing.
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }


            var ImageHelper = new ImageHelper(_webHostEnvironment);
            string imageFileName = ImageHelper.UploadFile(viewModel.ProfileImage);

            var suburb = new Suburb();

            suburb.SuburbName = viewModel.SuburbName;
            suburb.Postcode = viewModel.Postcode;

            // Check is Suburb is already known to Easy Walk DB, and rejects entry if known.
            bool match = false;
            foreach (var s in _context.Suburbs)
            {
                if (s.Postcode == suburb.Postcode && s.SuburbName == suburb.SuburbName)
                {
                    match = true;
                    suburb = s;
                }
            }
            if (!match)
                _context.Suburbs.Add(suburb);


            // Check usertype and create viewModel
            if (o == null)
            {
                //User is Walker
                viewModel.UserType = typeof(Walker).Name;
                w.FirstName = viewModel.FirstName;
                w.LastName = viewModel.LastName;
                w.Email = viewModel.Email;
                w.StreetAddress = viewModel.StreetAddress;
                w.Suburb = suburb;
                w.State = viewModel.State;
                w.Country = viewModel.Country;
                w.PhNumber = viewModel.PhNumber;

                if (viewModel.SavedProfileImage != w.ProfileImage)
                    w.ProfileImage = imageFileName;


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
                o.LastName = viewModel.LastName;
                o.Email = viewModel.Email;
                o.StreetAddress = viewModel.StreetAddress;
                o.Suburb = suburb;
                o.State = viewModel.State;
                o.Country = viewModel.Country;
                o.PhNumber = viewModel.PhNumber;

                if (viewModel.ProfileImage != null)
                    o.ProfileImage = imageFileName;
                else if (viewModel.SavedProfileImage != imageFileName)
                    o.ProfileImage = "defaultProfile.png";

                viewModel.SavedProfileImage = o.ProfileImage;
            }

            await _context.SaveChangesAsync();
        
            return RedirectToAction("Index", "Owner");
        }


        public async Task<IActionResult> EditPassword(string password, string confirmPassword)
        {
            var o = new Owner();
            o = await _context.Owners.FindAsync(UserID);

            var w = new Walker();
            w = await _context.Walkers.FindAsync(UserID);

            if (password == null)
                ModelState.AddModelError(nameof(password), "Password is required.");
            if (!Regex.IsMatch(password, @"^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=]).*$"))
                ModelState.AddModelError(nameof(password), "Password is Invalid. Password must contain at least one upper case letter, a lower case letter, a special character, a number, and must be at least 8 characters in length");
            if (password != confirmPassword)
                ModelState.AddModelError(nameof(confirmPassword), "Passwords need to match.");

            if (!ModelState.IsValid)
            {
                return View();
            }

            //Check if walker or Owner and update fields accordingly
            if (w == null)
                o.Login.PasswordHash = ControllerHelper.HashPassword(password);
            else
                w.Login.PasswordHash = ControllerHelper.HashPassword(password);

            await _context.SaveChangesAsync();
         
            return RedirectToAction("Index");
        }

    }
}

