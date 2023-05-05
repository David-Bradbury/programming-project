using ProgrammingProject.Data;
using ProgrammingProject.Models;
using ProgrammingProject.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProgrammingProject.Helper;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.Metrics;
using System.Reflection;

namespace ProgrammingProject.Controllers
{

    [Route("/Register")]
    public class RegisterController : BaseController
    {
        private readonly EasyWalkContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public RegisterViewModel viewModel = new RegisterViewModel();

        public RegisterController(EasyWalkContext context, IWebHostEnvironment webHostEnvironment) : base(context, webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [Route("/Register/SelectAccountType",
       Name = "selectAccount")]
        public async Task<IActionResult> SelectAccountType()
        {
            return View(viewModel);
        }

        [Route("/Register/Register",
      Name = "Register")]
        public async Task<IActionResult> Register(int id)
        {
            viewModel.AccountTypeSelection = id;

            //Sets up the lists in the form
            viewModel.StatesList = DropDownLists.GetStates();
            viewModel.IsInsuredList = DropDownLists.GetInsuranceList();
            viewModel.ExperienceList = DropDownLists.GetExperienceLevel();
            ViewBag.SuburbsList = _context.Suburbs.ToList();

            return View(viewModel);
        }

        [HttpPost, Route("/Register/Register",
      Name = "Register")]
        public async Task<IActionResult> Register(RegisterViewModel viewModel)
        {

            // Sets up form in the case of invalid model state.
            viewModel.StatesList = DropDownLists.GetStates();
            viewModel.IsInsuredList = DropDownLists.GetInsuranceList();
            viewModel.ExperienceList = DropDownLists.GetExperienceLevel();
            ViewBag.SuburbsList = _context.Suburbs.ToList();


            // Checks if the viewmodel fields are null.
            CheckNull(viewModel.FirstName, nameof(viewModel.FirstName), "First Name is Required");
            CheckNull(viewModel.LastName, nameof(viewModel.LastName), "Last Name is Required");
            CheckNull(viewModel.Email, nameof(viewModel.Email), "Email is Required");
            CheckNull(viewModel.StreetAddress, nameof(viewModel.StreetAddress), "Street Address is Required");
            CheckNull(viewModel.SuburbName, nameof(viewModel.SuburbName), "Suburb Name is Required");
            CheckNull(viewModel.State, nameof(viewModel.State), "State is Required");
            CheckNull(viewModel.Postcode, nameof(viewModel.Postcode), "Postcode is Required");
            CheckNull(viewModel.PhNumber, nameof(viewModel.PhNumber), "Phone Number is Required");
            CheckNull(viewModel.Password, nameof(viewModel.Password), "Password is Required");

            if (viewModel.AccountTypeSelection == 2)
            {
                CheckNull(viewModel.ExperienceLevel, nameof(viewModel.ExperienceLevel), "Experience Level is Required");
                CheckNull(viewModel.IsInsured, nameof(viewModel.IsInsured), "Insurance Status is Required");
            }

            // Checking to see if email is already is the system.
            var checkEmail = _context.Logins.Where(l => l.Email == viewModel.Email);
            if (!checkEmail.IsNullOrEmpty())
                ModelState.AddModelError(nameof(viewModel.Email), "This email is already registered in the system. Please try with a different email address.");

            // Checking regex values
            string regex = @"(^0[289][0-9]{2}\s*$)|(^[1-9][0-9]{3}\s*$)";
            CheckRegex(viewModel.Postcode, nameof(viewModel.Postcode), regex, "This postcode does not match any Australian postcode. Please enter an Australian 4 digit postcode");
            regex = @"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+\s?$";
            CheckRegex(viewModel.Email, nameof(viewModel.Email), regex, "This is not a valid email address. Please enter a valid email address");
            // Not perfect and needs updates for proper Australian phone numbers.
            regex = @"^(\+?\(61\)|\(\+?61\)|\+?61|(0[1-9])|0[1-9])?( ?-?[0-9]){7,9}$";
            CheckRegex(viewModel.PhNumber, nameof(viewModel.PhNumber), regex, "This is not a valid Australian mobile phone number.Please enter a valid Australian mobile phone number");

            // Checks the extension of the file to ensure a certain file format.
            if (viewModel.ProfileImage != null)
                CheckImageExtension(viewModel.ProfileImage, nameof(viewModel.ProfileImage));

            // Checks password is valid.
            CheckValidPassword(viewModel.Password, viewModel.ConfirmPassword);

            // Checks Suburb is Valid.
            CheckSuburbModelState(viewModel.SuburbName, viewModel.Postcode, viewModel.State);

            // Checking to see if the state of the model is valid before continuing.
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
        
            // Creating suburb based on form details
            var suburb = new Suburb();
            suburb.SuburbName = viewModel.SuburbName;
            suburb.Postcode = viewModel.Postcode;
            suburb.State = viewModel.State;

            // Create a new login from form submission
            var login = new Login();

            login.Email = viewModel.Email;
            login.PasswordHash = ControllerHelper.HashPassword(viewModel.Password);
            login.Locked = Locked.locked;

            _context.Logins.Add(login);

            // Sends email out to new users for verification.
            SendEmailVerification(login, viewModel.FirstName);

            var CreateHelper = new Create(_context, _webHostEnvironment);
            int UserID = 0;
            string savedProfileImage = null;
            // Creates an Owner.
            if (viewModel.AccountTypeSelection == 1)
                CreateHelper.CreateOwner(viewModel.FirstName, viewModel.LastName, viewModel.Email, viewModel.StreetAddress,
                    viewModel.Country, viewModel.PhNumber, viewModel.ProfileImage, suburb, UserID, savedProfileImage);

            // Creates a Walker.
            else if (viewModel.AccountTypeSelection == 2)
            {
                var walker = CreateHelper.CreateWalker(viewModel.FirstName, viewModel.LastName, viewModel.Email, viewModel.StreetAddress,
                viewModel.Country, viewModel.PhNumber, viewModel.IsInsured, viewModel.ExperienceLevel, viewModel.ProfileImage, suburb);
            }
            _context.SaveChanges();

            return RedirectToAction("Login", "Login");
        }


        [Route("/Register/SendEmailVerification")]
        public void SendEmailVerification(Login login, string firstName)
        {
            //get token for verification 
            Random random = new Random();

            var token = ControllerHelper.HashPassword((random.Next().ToString()));
            login.EmailToken = token;
            _context.SaveChanges();


            // Parameters to send through to email method. Front End to modify messages.
            string recipient = login.Email;
            string subject = "Please verify your email address";

            //String for woring locally
            //const string url = "https://localhost:7199/Verification/Verify";

            //String for deployed version
            const string url = "https://programmingproject-easywalk.azurewebsites.net/Verification/Verify";

            var param = new Dictionary<string, string>() { { "emailToken", token } };

            var newUrl = new Uri(QueryHelpers.AddQueryString(url, param));

            string htmlContent = GetVerifyEmailContent(newUrl.ToString(), firstName);


            //Calling the method to send email.
            Email.SendEmail(recipient, subject, htmlContent);
        }


        private string GetRegisterEmailContent(string name)
        {
            string content = "";

            try
            {
                using (var sr = new StreamReader("./Helper/RegisterEmailContent.html"))
                {
                    string fileContent = sr.ReadToEnd();
                    content = String.Format(fileContent, name);
                }
            }
            catch (Exception e)
            {
                content = "You have successfully registered with EasyWalk";
            }

            return content;
        }


        private string GetVerifyEmailContent(string url, string name)
        {
            string content = "";

            try
            {
                using (var sr = new StreamReader("./Helper/VerifyEmailContent.html"))
                {
                    string fileContent = sr.ReadToEnd();
                    content = String.Format(fileContent, name, url);
                }
            }
            catch (Exception e)
            {
                content = "Please verify your email using this link: " + url;
            }

            return content;
        }
    }
}
