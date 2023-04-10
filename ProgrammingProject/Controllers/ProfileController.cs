using Microsoft.AspNetCore.Mvc;
using ProgrammingProject.Data;
using ProgrammingProject.Models;
using ProgrammingProject.Filters;


namespace ProgrammingProject.Controllers
{
    public class ProfileController : Controller
    {
        private readonly EasyWalkContext _context;
        private bool isOwner;
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
            if(o== null)
            {
                //User is Walker
                viewModel.UserType = typeof(Walker).Name;
                viewModel.FirstName = w.FirstName;
                viewModel.LastName = w.LastName;
                viewModel.Email = w.Email;
                viewModel.StreetAddress = w.StreetAddress;
                viewModel.SuburbName = w.Suburb.SuburbName;
                viewModel.Postcode = w.Suburb.SuburbName;
                viewModel.State = w.State;
                viewModel.Country = w.Country;
                viewModel.PhNumber = w.PhNumber;
                viewModel.IsInsured = w.IsInsured;
                viewModel.ExperienceLevel = (int) w.ExperienceLevel;

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
                viewModel.Postcode = o.Suburb.SuburbName;
                viewModel.State = o.State;
                viewModel.Country = o.Country;
                viewModel.PhNumber = o.PhNumber;
                //default values for ExperienceLevel an IsInsured as are not used for owner.

            }

            return View(viewModel);

        }
        //public async Task<IActionResult> EditProfile(int id)
        //{
        //    var user = await _context.Owners.FindAsync(UserID);
        // //   var viewModel = ControllerHelper.BuildEditProfileViewModel(customer, id);
        //    return View(viewModel);
        //}

    }
}
