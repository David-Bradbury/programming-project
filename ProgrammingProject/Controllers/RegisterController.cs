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

        public IActionResult Register() => View();

        //[HttpPost]
        //public Task<IActionResult> Register()
        //{

        //}
    }
}
