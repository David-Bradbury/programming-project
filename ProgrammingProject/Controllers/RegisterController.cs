using ProgrammingProject.Data;
using ProgrammingProject.Models;
using ProgrammingProject.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

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

            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Register(int accountTypeSelection, string firstName, string lastName, string email, string streetAddress, string state,
                                                                string suburbName, string postcode, string country, string phNumber, bool isInsured, int experienceLevel, string password, string confirmPassword)
        {
              var viewModel = new RegisterViewModel();
            //insert server side validation here.
            if (firstName == null)
                ModelState.AddModelError(nameof(firstName), "Firstname is required.");
            if (lastName == null)
                ModelState.AddModelError(nameof(lastName), "Lastname is required.");
            if (email == null)
                ModelState.AddModelError(nameof(email), "Email is required.");
            if (streetAddress == null)
                ModelState.AddModelError(nameof(streetAddress), "The address is required.");
            if (suburbName == null)
                ModelState.AddModelError(nameof(suburbName), "The suburb name is required.");
            if (postcode == null)
                ModelState.AddModelError(nameof(postcode), "The postcode is required.");
            if (country == null)
                ModelState.AddModelError(nameof(country), "The country is required.");
            if (phNumber == null)
                ModelState.AddModelError(nameof(phNumber), "Phone number is required.");
            if (password == null)
                ModelState.AddModelError(nameof(password), "Password is required.");
            if(password != confirmPassword)
                ModelState.AddModelError(nameof(confirmPassword), "Passwords need to match.");

            foreach (var l in _context.Logins)
                if (l.Email == email)
                    ModelState.AddModelError(nameof(email), "This email is already registered in the system. Please try with a different email address.");
           
            // ALL NEEDS TESTING. JC 
            // Might be worth changing to add full state/territory names. discuss with group JC
            if (!Regex.IsMatch(state, "NSW|QLD|SA|WA|TAS|VIC|NT|ACT"))
                ModelState.AddModelError(nameof(state), "This is not a valid Australia State or Territory. Please enter an Australian State or Territory");
            if (!Regex.IsMatch(postcode, @"(^0[289][0-9]{2}\s*$)|(^[1-9][0-9]{3}\s*$)"))
                ModelState.AddModelError(nameof(postcode), "This postcode does not match any Australian postcode. Please enter an Australian 4 digit postcode");
            // Will need to change to add different mobile entry options, such as 04xx xxx xxx or +614xx xxx xxx or various other combinations. JC
            if (!Regex.IsMatch(phNumber, "04[0-9]{8}"))
                ModelState.AddModelError(nameof(phNumber), "This is not a valid Australian mobile phone number. Please enter a valid Australian mobile phone number");
            // Add Email REGEX test here, needs to at the least match what the data annotation for EmailAddress accepts.

            // Also add  stringlength regex checking here too.


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
                if (s.Postcode == postcode)
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
            login.Locked = Locked.unlocked;

            _context.Logins.Add(login);
           

            if (accountTypeSelection == 1)
            {
                var owner = new Owner();
                owner.FirstName = firstName;
                owner.LastName = lastName;
                owner.Email = email;
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
                walker.Suburb = suburb;
                walker.Country = country;
                walker.PhNumber = phNumber;
                walker.IsInsured = isInsured;
                if (experienceLevel == 1)
                    walker.ExperienceLevel = ExperienceLevel.Beginner;
                else if (experienceLevel == 2)
                    walker.ExperienceLevel = ExperienceLevel.Intermediate;
                else if (experienceLevel == 3)
                    walker.ExperienceLevel = ExperienceLevel.Advanced;
                else if (experienceLevel == 4)
                    walker.ExperienceLevel = ExperienceLevel.Expert;
                _context.Add(walker);
                _context.SaveChanges();

             

            }
        
            _context.SaveChanges();

            return RedirectToAction("Login", "Login");
        }


    }
}
