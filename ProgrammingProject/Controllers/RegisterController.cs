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
        public RegisterViewModel viewModel = new RegisterViewModel();


        public RegisterController(EasyWalkContext context)
        {
            _context = context;
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

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Register(int accountTypeSelection, string firstName, string lastName, string email, string streetAddress, string state,
                                                                string suburbName, string postcode, string country, string phNumber, string isInsured, string experienceLevel, string password, string confirmPassword)
        {
            var viewModel = new RegisterViewModel();
            viewModel.AccountTypeSelection = accountTypeSelection;
        
            viewModel.StatesList = DropDownLists.GetStates();        
            viewModel.IsInsuredList = DropDownLists.GetInsuranceList();
            viewModel.ExperienceList = DropDownLists.GetExperienceLevel();


            if (firstName == null)
                ModelState.AddModelError(nameof(firstName), "First Name is required.");
            if (lastName == null)
                ModelState.AddModelError(nameof(lastName), "Last Name is required.");
            if (email == null)
                ModelState.AddModelError(nameof(email), "Email is required.");
            if (streetAddress == null)
                ModelState.AddModelError(nameof(streetAddress), "The address is required.");
            if (suburbName == null)
                ModelState.AddModelError(nameof(suburbName), "The suburb name is required.");
            if (state == null)
                ModelState.AddModelError(nameof(state), "The state is required.");
            if (postcode == null)
                ModelState.AddModelError(nameof(postcode), "The postcode is required.");
            if (country == null)
                ModelState.AddModelError(nameof(country), "The country is required.");
            if (phNumber == null)
                ModelState.AddModelError(nameof(phNumber), "Phone number is required.");
            if (password == null)
                ModelState.AddModelError(nameof(password), "Password is required.");
            if (password != confirmPassword)
                ModelState.AddModelError(nameof(confirmPassword), "Passwords need to match.");

            // Checking to see if email is already is the system.
            foreach (var l in _context.Logins)
                if (l.Email == email)
                    ModelState.AddModelError(nameof(email), "This email is already registered in the system. Please try with a different email address.");


            // ALL NEEDS TESTING. JC        
            if (!Regex.IsMatch(postcode, @"(^0[289][0-9]{2}\s*$)|(^[1-9][0-9]{3}\s*$)"))
                ModelState.AddModelError(nameof(postcode), "This postcode does not match any Australian postcode. Please enter an Australian 4 digit postcode");
            // Not perfect and needs updates for proper Australian phone numbers.
            if (!Regex.IsMatch(phNumber, @"^(\+?\(61\)|\(\+?61\)|\+?61|(0[1-9])|0[1-9])?( ?-?[0-9]){7,9}$"))
                ModelState.AddModelError(nameof(phNumber), "This is not a valid Australian mobile phone number. Please enter a valid Australian mobile phone number");
            if (!Regex.IsMatch(email, @"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+\s?$"))
                ModelState.AddModelError(nameof(email), "This is not a valid email address. Please enter a valid email address");
            if (!Regex.IsMatch(password, @"^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=]).*$"))
                ModelState.AddModelError(nameof(password), "Password is Invalid. Password must contain at least one upper case letter, a lower case letter, a special character, a number, and must be at least 8 characters in length");

            // Also add  stringlength regex checking here too.

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


            // Create a new login from form submission
            var login = new Login();

            login.Email = email;
            login.PasswordHash = ControllerHelper.HashPassword(password);
            login.Locked = Locked.locked;

            _context.Logins.Add(login);

            SendEmailVerification(login, firstName);

            if (accountTypeSelection == 1)
            {
                var owner = new Owner();
                owner.FirstName = firstName;
                owner.LastName = lastName;
                owner.Email = email;
                owner.State = state;
                owner.StreetAddress = streetAddress;
                owner.Suburb = suburb;
                owner.Country = country;
                owner.PhNumber = phNumber;


                _context.Add(owner);
                _context.SaveChanges();
            }
            else if (accountTypeSelection == 2)
            {
                var walker = new Walker();
                walker.FirstName = firstName;
                walker.LastName = lastName;
                walker.Email = email;
                walker.StreetAddress = streetAddress;
                walker.State = state;
                walker.Suburb = suburb;
                walker.Country = country;
                walker.PhNumber = phNumber;
                if (isInsured.Equals("Insured"))
                    walker.IsInsured = true;
                else if (isInsured.Equals("Uninsured"))
                    walker.IsInsured = false;

                if (experienceLevel.Equals("Beginner"))
                    walker.ExperienceLevel = ExperienceLevel.Beginner;
                else if (experienceLevel.Equals("Intermediate"))
                    walker.ExperienceLevel = ExperienceLevel.Intermediate;
                else if (experienceLevel.Equals("Advanced"))
                    walker.ExperienceLevel = ExperienceLevel.Advanced;
                else if (experienceLevel.Equals("Expert"))
                    walker.ExperienceLevel = ExperienceLevel.Expert;
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
            //generate token for verification 
            Random random = new Random();

            string token = ControllerHelper.HashPassword((random.Next().ToString()));
            login.EmailToken = token;
            _context.SaveChanges();


            // Parameters to send through to email method. Front End to modify messages.
            string recipient = login.Email;
            string subject = "Please verify your email address";

            //String for woring locally
            const string url = "https://localhost:7199/Verification/Verify";

           //String for deployed version
           //  const string url = "https://programmingproject-easywalk.azurewebsites.net/Verification/Verify";

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
