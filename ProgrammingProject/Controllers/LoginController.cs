using ProgrammingProject.Data;
using ProgrammingProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace ProgrammingProject.Controllers
{
    //Mask URL
    [Route("/Login")]
    public class LoginController : Controller
    {
        private readonly EasyWalkContext _context;

        public LoginController(EasyWalkContext context)
        {
            _context = context;
        }


        public IActionResult Login() => View();

        //attempt loing
        [HttpPost]
        public async Task<IActionResult> Login(string loginID, string password)
        {
            //find login id
            var login = await _context.Logins.FindAsync(loginID);

            //attempt password check
            //if (login == null || !PBKDF2.Verify(login.PasswordHash, password))
            //{
            //    ModelState.AddModelError("LoginFailure", "Login attempt failed, please try again");
            //    return View(new Login { LoginID = loginID });
            //}

            //Customer login
            var o = await _context.Owners.FindAsync(login.UserId);
            var w = await _context.Walkers.FindAsync(login.UserId);
            string userType = "";


            if (o != null)
            {
                HttpContext.Session.SetInt32(nameof(o.UserId), login.UserId);
                HttpContext.Session.SetString(nameof(o.FirstName), login.User.FirstName);
                userType = "Owner";
            }
            else
            {
                HttpContext.Session.SetInt32(nameof(w.UserId), login.UserId);
                HttpContext.Session.SetString(nameof(w.FirstName), login.User.FirstName);
                userType = "Walker";
            }
           

            

            return RedirectToAction("Index", userType);
        }

        [Route("LoggingOut")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }




    }
}

