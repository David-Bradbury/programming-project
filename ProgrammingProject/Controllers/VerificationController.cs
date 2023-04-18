using Microsoft.AspNetCore.Mvc;
using ProgrammingProject.Data;
using ProgrammingProject.Models;


namespace ProgrammingProject.Controllers
{
    public class VerificationController : Controller
    {

        private readonly EasyWalkContext _context;

        public VerificationController(EasyWalkContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Route("/Verification/Verify")]
        public IActionResult Verify(string emailToken)
        {
            bool verified = false;
   

            foreach (var login in _context.Logins)
            {
                if (login.EmailToken == emailToken)
                {
                    verified = true;
                    login.Locked = Locked.unlocked;
                    _context.SaveChanges();
                }


            }
            if(verified == true)
                    return View();
            else
               return View("Home", "Index");
        }


    }
}
