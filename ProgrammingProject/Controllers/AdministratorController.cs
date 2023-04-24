using ProgrammingProject.Data;
using ProgrammingProject.Models;
using Microsoft.AspNetCore.Mvc;

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


            return View(admin);
        }

        public async Task<IActionResult> DeleteUser()
        {
            var userList = new List<DeleteUserViewModel>();
            var tempUser = new DeleteUserViewModel();

            foreach(var o in _context.Owners)
            {
                tempUser.Email = o.Email;
                tempUser.FirstName = o.FirstName;
                tempUser.LastName = o.LastName;
                tempUser.UserType = "Owner";
                userList.Add(tempUser);

            }
            foreach (var w in _context.Walkers)
            {
                tempUser.Email = w.Email;
                tempUser.FirstName = w.FirstName;
                tempUser.LastName = w.LastName;
                tempUser.UserType = "Walker";
                userList.Add(tempUser);


            }
            return View(userList);
        }
    }
}
