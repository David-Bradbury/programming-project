using ProgrammingProject.Data;
using ProgrammingProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace ProgrammingProject.Controllers
{

    [Route("/Register")]
    public class RegisterController : Controller
    {
        private readonly EasyWalkContext _context;

        public RegisterController(EasyWalkContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Register()
        {
            RegisterViewModel viewModel = new RegisterViewModel();

            return View(viewModel);
        }
        //[HttpPost]
        //public async Task<IActionResult> Register()
        //{

        //}
    }
}
