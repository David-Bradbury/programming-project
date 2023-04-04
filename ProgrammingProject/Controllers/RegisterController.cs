using ProgrammingProject.Data;
using ProgrammingProject.Models;
using ProgrammingProject.Utilities;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> Register(int AccountTypeSelection, string FirstName, string LastName, string Email, string StreetAddress,
                                                                string SuburbName, string Postcode, string Country, string PhNumber, bool IsInsured, int experienceLevel, string Password)
        {
            var viewModel = new RegisterViewModel();
            //insert server side validation here.
            if (FirstName == null)
                ModelState.AddModelError(nameof(FirstName), "Firstname is test.");
            if (LastName == null)
                ModelState.AddModelError(nameof(LastName), "Lastname is required.");
            if (Email == null)
                ModelState.AddModelError(nameof(Email), "Email is required.");
            if (StreetAddress == null)
                ModelState.AddModelError(nameof(StreetAddress), "The address is required.");
            if (SuburbName == null)
                ModelState.AddModelError(nameof(SuburbName), "The suburb name is required.");
            if (Postcode == null)
                ModelState.AddModelError(nameof(Postcode), "The postcode is required.");
            if (Country == null)
                ModelState.AddModelError(nameof(Country), "The country is required.");
            if (PhNumber == null)
                ModelState.AddModelError(nameof(PhNumber), "Phone number is required.");
            if (Password == null)
                ModelState.AddModelError(nameof(Password), "Password is required.");
            
            
            if (!ModelState.IsValid)
            {
                
                ViewBag.email = Email;
                return View(viewModel);
            }

            var suburb = new Suburb();
            suburb.SuburbName = SuburbName;
            suburb.Postcode = Postcode;




            bool match = false;
            foreach (var s in _context.Suburbs)
            {
                if (s.Postcode == Postcode)
                {
                    match = true;
                    suburb = s;
                }
            }
            if (!match)
                _context.Suburbs.Add(suburb);

            bool inUse = true;
            int randLoginId;
            var rnd = new Random();
            do
            {
                randLoginId = rnd.Next(10000000, 99999999);
                var x = await _context.Logins.FindAsync(randLoginId.ToString());
                if (x == null)
                    inUse = false;

            } while (inUse);
            var login = new Login();

            login.LoginID = randLoginId.ToString();
            login.PasswordHash = ControllerHelper.HashPassword(Password);
            login.Locked = Locked.unlocked;

            if (AccountTypeSelection == 1)
            {
                var owner = new Owner();
                owner.FirstName = FirstName;
                owner.LastName = LastName;
                owner.Email = Email;
                owner.StreetAddress = StreetAddress;
                owner.Suburb = suburb;
                owner.Country = Country;
                owner.PhNumber = PhNumber;


                _context.Add(owner);
                _context.SaveChanges();

                login.UserId = owner.UserId;

            }
            else if (AccountTypeSelection == 2)
            {
                var walker = new Walker();
                walker.FirstName = FirstName;
                walker.LastName = LastName;
                walker.Email = Email;
                walker.StreetAddress = StreetAddress;
                walker.Suburb = suburb;
                walker.Country = Country;
                walker.PhNumber = PhNumber;
                walker.IsInsured = IsInsured;
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

                login.UserId = walker.UserId;

            }

            _context.Logins.Add(login);

           _context.SaveChanges();

            return RedirectToAction("Login", "Login");
        }


    }
}
