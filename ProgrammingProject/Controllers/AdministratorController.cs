using ProgrammingProject.Data;
using ProgrammingProject.Models;
using Microsoft.AspNetCore.Mvc;
using ProgrammingProject.Utilities;

namespace ProgrammingProject.Controllers
{
    [Route("/Administrator")]
    public class AdministratorController : Controller
    {
        private readonly EasyWalkContext _context;

        private int AdminID => HttpContext.Session.GetInt32(nameof(Administrator.UserId)).Value;

        public AdministratorController(EasyWalkContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var admin = await _context.Administrators.FindAsync(AdminID);
            var viewModel = new AdminIndexViewModel();
            viewModel.OwnerCount = _context.Owners.Count();
            viewModel.WalkerCount = _context.Walkers.Count();
            foreach (var o in _context.Owners)
            {
                viewModel.DogCount = viewModel.DogCount + o.Dogs.Count();
            }

            return View(viewModel);
        }

        [Route("/Administrator/EditUser")]
        public async Task<IActionResult> EditUser(int page = 1)
        {
            var viewModel = await ControllerHelper.BuildUserAdminViewModel(_context, page);
            return View(viewModel);

        }

        [Route("/Administrator/DeleteUser")]
        public async Task<IActionResult> DeleteUser(int id)
        {

            var owner = await _context.Owners.FindAsync(id);
            if (owner != null)
            {
                _context.Logins.Remove(owner.Login);
                var dogList = owner.Dogs.ToList();

                foreach (Dog d in dogList)
                    _context.Dogs.Remove(d);

                _context.Owners.Remove(owner);

            }

            if (owner == null)
            {
                var walker = await _context.Walkers.FindAsync(id);

                if (walker != null)
                {
                    _context.Logins.Remove(walker.Login);
                    _context.Walkers.Remove(walker);
                }
            }


            _context.SaveChanges();

            return RedirectToAction("EditUser");
        }

        [Route("/Administrator/UpdateUserProfile")]
        public async Task<IActionResult> UpdateUserProfile(int id) => RedirectToAction("AdminIndex", "Profile", new { id = id });

    }
}
