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
            var walker = await _context.Walkers.FindAsync(UserID);
            //return View(walker);
            ViewBag.Walker = walker;

            return View();

        }

    }
}
