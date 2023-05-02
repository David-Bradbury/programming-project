using ProgrammingProject.Data;
using ProgrammingProject.Models;
using ProgrammingProject.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProgrammingProject.Helper;
using Microsoft.AspNetCore.WebUtilities;

namespace ProgrammingProject.Controllers
{

    [Route("/Register")]
    public class RegisterController : Controller
    {
        private readonly EasyWalkContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public RegisterViewModel viewModel = new RegisterViewModel();


        public RegisterController(EasyWalkContext context, IWebHostEnvironment webHostEnvironment)
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

    
        public async Task<IActionResult> Register(int id)
        {
            viewModel.AccountTypeSelection = id;
         
            viewModel.StatesList = DropDownLists.GetStates();
            viewModel.IsInsuredList = DropDownLists.GetInsuranceList();
            viewModel.ExperienceList = DropDownLists.GetExperienceLevel();
            ViewBag.SuburbsList = _context.Suburbs.ToList();

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel viewModel)
        {
      
            viewModel.StatesList = DropDownLists.GetStates();        
            viewModel.IsInsuredList = DropDownLists.GetInsuranceList();
            viewModel.ExperienceList = DropDownLists.GetExperienceLevel();

            if (viewModel.FirstName == null)
                ModelState.AddModelError(nameof(viewModel.FirstName), "Put the first name in idiot");
           // CheckModelState.CheckNull(firstName, "First Name is required Dumbs Dumbs.", ModelState);
            //CheckModelState.CheckNull(lastName, "Last Name is required.", ModelState);
            //CheckModelState.CheckNull(email, "Email is required.", ModelState);
            //CheckModelState.CheckNull(streetAddress, "The address is required.", ModelState);
            //CheckModelState.CheckNull(suburbName, "The suburb name is required.", ModelState);
            //CheckModelState.CheckNull(state, "The state is required.", ModelState);
            //CheckModelState.CheckNull(postcode, "The postcode is required.", ModelState);
            //CheckModelState.CheckNull(country, "The country is required.", ModelState);
            //CheckModelState.CheckNull(phNumber, "Phone number is required.", ModelState);
            //CheckModelState.CheckNull(password, "Password is required.", ModelState);

            if (viewModel.Password != viewModel.ConfirmPassword)
                ModelState.AddModelError(nameof(viewModel.ConfirmPassword), "Passwords need to match.");

            // Checking to see if email is already is the system.
            foreach (var l in _context.Logins)
                if (l.Email == viewModel.Email)
                    ModelState.AddModelError(nameof(viewModel.Email), "This email is already registered in the system. Please try with a different email address.");


            // ALL NEEDS TESTING. JC        
            if (!Regex.IsMatch(viewModel.Postcode, @"(^0[289][0-9]{2}\s*$)|(^[1-9][0-9]{3}\s*$)"))
                ModelState.AddModelError(nameof(viewModel.Postcode), "This postcode does not match any Australian postcode. Please enter an Australian 4 digit postcode");
            // Not perfect and needs updates for proper Australian phone numbers.
            if (!Regex.IsMatch(viewModel.PhNumber, @"^(\+?\(61\)|\(\+?61\)|\+?61|(0[1-9])|0[1-9])?( ?-?[0-9]){7,9}$"))
                ModelState.AddModelError(nameof(viewModel.PhNumber), "This is not a valid Australian mobile phone number. Please enter a valid Australian mobile phone number");
            if (!Regex.IsMatch(viewModel.Email, @"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+\s?$"))
                ModelState.AddModelError(nameof(viewModel.Email), "This is not a valid email address. Please enter a valid email address");
            if (!Regex.IsMatch(viewModel.Password, @"^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=]).*$"))
                ModelState.AddModelError(nameof(viewModel.Password), "Password is Invalid. Password must contain at least one upper case letter, a lower case letter, a special character, a number, and must be at least 8 characters in length");

            // Checks the extension of the file to ensure a certain file format. Bring up Thurs meeting to see if extra verification needed. JC.
            if (viewModel.ProfileImage != null)
            {
                string filename = Path.GetFileName(viewModel.ProfileImage.FileName);
                string extension = Path.GetExtension(filename).ToLower();

                if (extension != ".jpg" && extension != ".jpeg" && extension != ".png")
                    ModelState.AddModelError(nameof(viewModel.ProfileImage), "Image must be of the jpg/jpeg, or png format");
            }

            // Also add  stringlength regex checking here too.

            // Checking to see if the state of the model is valid before continuing.
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            var ImageHelper = new ImageHelper(_webHostEnvironment);
            string imageFileName = ImageHelper.UploadFile(viewModel.ProfileImage);

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


            // Create a new login from form submission
            var login = new Login();

            login.Email = viewModel.Email;
            login.PasswordHash = ControllerHelper.HashPassword(viewModel.Password);
            login.Locked = Locked.locked;

            _context.Logins.Add(login);

            SendEmailVerification(login, viewModel.FirstName);

            if (viewModel.AccountTypeSelection == 1)
            {
                var owner = new Owner();
                owner.FirstName = viewModel.FirstName;
                owner.LastName = viewModel.LastName;
                owner.Email = viewModel.Email;
                owner.State = viewModel.State;
                owner.StreetAddress = viewModel.StreetAddress;
                owner.Suburb = suburb;
                owner.Country = viewModel.Country;
                owner.PhNumber = viewModel.PhNumber;

                if (viewModel.ProfileImage != null)
                    owner.ProfileImage = imageFileName;
                else
                    owner.ProfileImage = "defaultProfile.png";

                _context.Add(owner);
                _context.SaveChanges();
            }
            else if (viewModel.AccountTypeSelection == 2)
            {
                var walker = new Walker();
                walker.FirstName = viewModel.FirstName;
                walker.LastName = viewModel.LastName;
                walker.Email = viewModel.Email;
                walker.StreetAddress = viewModel.StreetAddress;
                walker.State = viewModel.State;
                walker.Suburb = suburb;
                walker.Country = viewModel.Country;
                walker.PhNumber = viewModel.PhNumber;
                if (viewModel.IsInsured.Equals("Insured"))
                    walker.IsInsured = true;
                else if (viewModel.IsInsured.Equals("Uninsured"))
                    walker.IsInsured = false;

                if (viewModel.ExperienceLevel.Equals("Beginner"))
                    walker.ExperienceLevel = ExperienceLevel.Beginner;
                else if (viewModel.ExperienceLevel.Equals("Intermediate"))
                    walker.ExperienceLevel = ExperienceLevel.Intermediate;
                else if (viewModel.ExperienceLevel.Equals("Advanced"))
                    walker.ExperienceLevel = ExperienceLevel.Advanced;
                else if (viewModel.ExperienceLevel.Equals("Expert"))
                    walker.ExperienceLevel = ExperienceLevel.Expert;

                if (viewModel.ProfileImage != null)
                    walker.ProfileImage = imageFileName;
                else
                    walker.ProfileImage = "defaultProfile.png";

                _context.Add(walker);
                _context.SaveChanges();

            }

            _context.SaveChanges();

   
             // Parameters to send through to email method. Front End to modify messages.
            //string recipient = email;
            //string subject = "Thank you for Registering with EasyWalk";
            //string personName = firstName;
            //string htmlContent = GetRegisterEmailContent(personName);

            //Calling the method to send email.
            //Email.SendEmail(recipient, subject, htmlContent);
            // htmlContent = GetVerifyEmailContent(personName, )

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
