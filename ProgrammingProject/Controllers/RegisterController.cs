//using ProgrammingProject.Data;
//using ProgrammingProject.Models;
//using ProgrammingProject.Utilities;
//using Microsoft.AspNetCore.Mvc;

//namespace ProgrammingProject.Controllers
//{

//    [Route("/Register")]
//    public class RegisterController : Controller
//    {
//        private readonly EasyWalkContext _context;
//        public RegisterViewModel viewModel = new RegisterViewModel();
       

//        public RegisterController(EasyWalkContext context)
//        {
//            _context = context;
//        }

//        [Route("/Register/SelectAccountType",
//       Name = "selectAccount")]
//        public async Task<IActionResult> SelectAccountType()
//        {
//            return View(viewModel);
//        }

//        public async Task<IActionResult> Register(int id)
//        {
//            viewModel.AccountTypeSelection = id;
            

//            return View(viewModel);
//        }
//        [HttpPost]
//        public async Task<IActionResult> Register(int accountTypeSelection, string firstName, string lastName, string email, string streetAddress,
//                                                                string suburbName, string postcode, string country, string phNumber, bool isInsured, int experienceLevel, string password)
//        {

//            //insert server side validation here.

//            var suburb = new Suburb();
//            suburb.SuburbName = suburbName;
//            suburb.Postcode = postcode;




//            bool match = false;
//            foreach (var s in _context.Suburbs)
//            {
//                if (s.Postcode == postcode)
//                {
//                    match = true;
//                    suburb = s;
//                }
//            }
//            if (!match)
//                _context.Suburbs.Add(suburb);
//            //var suburb = new Suburb();
//            //suburb.SuburbName = suburbName;
//            //suburb.Postcode = postcode;
//            //_context.Suburbs.Add(suburb);

//            bool inUse = true;
//            int randLoginId;
//            var rnd = new Random();
//            do
//            {
//                randLoginId = rnd.Next(10000000, 99999999);
//                var x = await _context.Logins.FindAsync(randLoginId.ToString());
//                if (x == null)
//                    inUse = false;

//            } while (inUse);
//            var login = new Login();

//            login.LoginID = randLoginId.ToString();
//            login.PasswordHash = ControllerHelper.HashPassword(password);
//            login.Locked = Locked.unlocked;

//            if (accountTypeSelection == 1)
//            {
//                var owner = new Owner();
//                owner.FirstName = firstName;
//                owner.LastName = lastName;
//                owner.Email = email;
//                owner.StreetAddress = streetAddress;
//                owner.Suburb = suburb;
//                owner.Country = country;
//                owner.PhNumber = phNumber;


//                _context.Add(owner);
//                _context.SaveChanges();

//                login.UserId = owner.UserId;

//            }
//            else if (accountTypeSelection == 2)
//            {
//                var walker = new Walker();
//                walker.FirstName = firstName;
//                walker.LastName = lastName;
//                walker.Email = email;
//                walker.StreetAddress = streetAddress;
//                walker.Suburb = suburb;
//                walker.Country = country;
//                walker.PhNumber = phNumber;
//                walker.IsInsured = isInsured;
//                if (experienceLevel == 1)
//                    walker.ExperienceLevel = ExperienceLevel.Beginner;
//                else if (experienceLevel == 2)
//                    walker.ExperienceLevel = ExperienceLevel.Intermediate;
//                else if (experienceLevel == 3)
//                    walker.ExperienceLevel = ExperienceLevel.Advanced;
//                else if (experienceLevel == 4)
//                    walker.ExperienceLevel = ExperienceLevel.Expert;
//                _context.Add(walker);
//                _context.SaveChanges();

//                login.UserId = walker.UserId;

//            }

//            _context.Logins.Add(login);

//           _context.SaveChanges();

//            return RedirectToAction("Login", "Login");
//        }


//    }
//}
