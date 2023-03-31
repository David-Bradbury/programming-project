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
    }
}
