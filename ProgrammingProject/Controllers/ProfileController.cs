using Microsoft.AspNetCore.Mvc;
using ProgrammingProject.Data;
using ProgrammingProject.Models;
using ProgrammingProject.Filters;
using System.Text.RegularExpressions;


namespace ProgrammingProject.Controllers
{
    public class ProfileController : Controller
    {
        private readonly EasyWalkContext _context;
        private int UserID => HttpContext.Session.GetInt32(nameof(Owner.UserId)).Value;


        [AuthorizeUser]
        public ProfileController(EasyWalkContext context)
        {
            _context = context;

        }

        [AuthorizeUser]
        public async Task<IActionResult> Index()
        {
            var o = new Owner();
            o = await _context.Owners.FindAsync(UserID);

            var w = new Walker();
            w = await _context.Walkers.FindAsync(UserID);

            var viewModel = new EditProfileViewModel();

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
                viewModel.IsInsured = w.IsInsured;
                viewModel.ExperienceLevel = (int)w.ExperienceLevel;

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
                //default values for ExperienceLevel an IsInsured as are not used for owner.

            }

            return View(viewModel);

        }


        public async Task<IActionResult> EditProfile(int id)
        {
            //create view model and assign the selected field from the profile page.
            var viewModel = new EditProfileViewModel();


            var o = new Owner();
            o = await _context.Owners.FindAsync(UserID);

            var w = new Walker();
            w = await _context.Walkers.FindAsync(UserID);



            // Check usertype and create viewModel
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
                viewModel.IsInsured = w.IsInsured;
                viewModel.ExperienceLevel = (int)w.ExperienceLevel;

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
                //default values for ExperienceLevel an IsInsured as are not used for owner.

            }
            if (id == 1)
                viewModel.SelectedField = nameof(viewModel.FirstName);
            if (id == 2)
                viewModel.SelectedField = nameof(viewModel.LastName);
            if (id == 3)
                viewModel.SelectedField = nameof(viewModel.StreetAddress);
            if (id == 4)
                viewModel.SelectedField = nameof(viewModel.SuburbName);
            if (id == 5)
                viewModel.SelectedField = nameof(viewModel.State);
            if (id == 6)
                viewModel.SelectedField = nameof(viewModel.PhNumber);
            if (id == 7)
                viewModel.SelectedField = nameof(viewModel.IsInsured);
            if (id == 8)
                viewModel.SelectedField = nameof(viewModel.ExperienceLevel);

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(string email, string selectedField, string userType, string firstName, string lastName,
         string streetAddress, string suburbName, string postcode, string state, string phNumber, bool isInsured, int experienceLevel, string country)
        {

            var viewModel = new EditProfileViewModel
            {
                Email = email,
                SelectedField = selectedField,
                UserType = userType,
                Country= country,
            };

            if (selectedField == nameof(firstName) && firstName == null)
                ModelState.AddModelError(nameof(firstName), "First Name is required.");
            if (selectedField == nameof(lastName) && lastName == null)
                ModelState.AddModelError(nameof(lastName), "Last Name is required.");
            if (selectedField == nameof(streetAddress) && streetAddress == null)
                ModelState.AddModelError(nameof(streetAddress), "The address is required.");
            if (selectedField == nameof(suburbName) && suburbName == null)
                ModelState.AddModelError(nameof(suburbName), "The suburb name is required.");
            if (selectedField == nameof(state) && state == null)
                ModelState.AddModelError(nameof(state), "The state is required.");
            if (selectedField == nameof(postcode) && postcode == null)
                ModelState.AddModelError(nameof(postcode), "The postcode is required.");

            if (selectedField == nameof(postcode) && !Regex.IsMatch(postcode, @"(^0[289][0-9]{2}\s*$)|(^[1-9][0-9]{3}\s*$)"))
                ModelState.AddModelError(nameof(postcode), "This postcode does not match any Australian postcode. Please enter an Australian 4 digit postcode");
            // Will need to change to add different mobile entry options, such as 04xx xxx xxx or +614xx xxx xxx or various other combinations. JC
            if (selectedField == nameof(phNumber) && !Regex.IsMatch(phNumber, @"^(\+?\(61\)|\(\+?61\)|\+?61|(0[1-9])|0[1-9])?( ?-?[0-9]){7,9}$"))
                ModelState.AddModelError(nameof(phNumber), "This is not a valid Australian mobile phone number. Please enter a valid Australian mobile phone number");

            // Checking to see if the state of the model is valid before continuing.
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }





            var o = new Owner();
            o = await _context.Owners.FindAsync(UserID);

            var w = new Walker();
            w = await _context.Walkers.FindAsync(UserID);

            //Check if walker or Owner and update fields accordingly
            if (w == null)
            {
                if (selectedField.Equals(nameof(viewModel.FirstName)))
                {
                    viewModel.FirstName = firstName;
                    o.FirstName = viewModel.FirstName;
                    //set session data
                    HttpContext.Session.SetString(nameof(o.FirstName), o.FirstName);
                }
                else
                {
                    viewModel.FirstName = o.FirstName;
                }

                if (selectedField.Equals(nameof(viewModel.LastName)))
                {
                    viewModel.LastName = lastName;
                    o.LastName = viewModel.LastName;
                }
                else
                {
                    viewModel.LastName = o.LastName;
                }


                if (selectedField.Equals(nameof(viewModel.StreetAddress)))
                {
                    viewModel.StreetAddress = streetAddress;
                    o.StreetAddress = viewModel.StreetAddress;
                }

                else
                {
                    viewModel.StreetAddress = o.StreetAddress;
                }

                if (selectedField.Equals(nameof(viewModel.State)))
                {
                    viewModel.State = state;
                    o.State = viewModel.State;
                }
                else
                {
                    viewModel.State = o.State;
                }

                if (selectedField.Equals(nameof(viewModel.PhNumber)))
                {
                    viewModel.PhNumber = phNumber;
                    o.PhNumber = viewModel.PhNumber;
                }
                else
                {
                    viewModel.PhNumber = o.PhNumber;
                }

                // Creating suburb based on form details
                if (selectedField.Equals(nameof(viewModel.SuburbName)))
                {
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
                    o.Suburb = suburb;
                    viewModel.SuburbName = o.Suburb.SuburbName;
                    viewModel.Postcode = o.Suburb.Postcode;

                }
                else
                {
                    viewModel.SuburbName = o.Suburb.SuburbName;
                    viewModel.Postcode = o.Suburb.Postcode;
                }

            }
            else
            {
                if (selectedField.Equals(nameof(viewModel.FirstName)))
                {
                    viewModel.FirstName = firstName;
                    w.FirstName = viewModel.FirstName;
                    //set session data
                    HttpContext.Session.SetString(nameof(o.FirstName), w.FirstName);
                }


                if (selectedField.Equals(nameof(viewModel.LastName)))
                {
                    viewModel.LastName = lastName;
                    w.LastName = viewModel.LastName;
                }
                else
                {
                    viewModel.LastName = w.LastName;
                }

                if (selectedField.Equals(nameof(viewModel.StreetAddress)))
                {
                    viewModel.StreetAddress = streetAddress;
                    w.StreetAddress = viewModel.StreetAddress;
                }
                else
                {
                    viewModel.StreetAddress = w.StreetAddress;
                }

                if (selectedField.Equals(nameof(viewModel.State)))
                {
                    viewModel.State = state;
                    w.State = viewModel.State;
                }
                else
                {
                    viewModel.State = w.State;
                }

                if (selectedField.Equals(nameof(viewModel.PhNumber)))
                {
                    viewModel.PhNumber = phNumber;
                    w.PhNumber = viewModel.PhNumber;
                }
                else
                {
                    viewModel.PhNumber = w.PhNumber;
                }

                if (selectedField.Equals(nameof(viewModel.IsInsured)))
                {
                    viewModel.IsInsured = isInsured;
                    w.IsInsured = viewModel.IsInsured;
                }
                else
                {
                    viewModel.IsInsured = w.IsInsured;
                }

                if (selectedField.Equals(nameof(viewModel.ExperienceLevel)))
                {
                    viewModel.ExperienceLevel = experienceLevel;
                    w.ExperienceLevel = (ExperienceLevel)experienceLevel;
                }
                else
                {
                    viewModel.ExperienceLevel = (int)w.ExperienceLevel;
                }

                // Creating suburb based on form details
                if (selectedField.Equals(nameof(viewModel.SuburbName)))
                {
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
                    w.Suburb = suburb;
                    viewModel.SuburbName = w.Suburb.SuburbName;
                    viewModel.Postcode = w.Suburb.Postcode;

                }
                else
                {
                    viewModel.SuburbName = w.Suburb.SuburbName;
                    viewModel.Postcode = w.Suburb.Postcode;
                }

            }

            await _context.SaveChangesAsync();

            return View("Index", viewModel);
        }

    }
}

