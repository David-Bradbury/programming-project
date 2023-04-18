using ProgrammingProject.Data;
using ProgrammingProject.Models;
using Microsoft.AspNetCore.Mvc;
using SimpleHashing;

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
        public async Task<IActionResult> Login(string Email, string password)
        {
            //find login id
            var login = await _context.Logins.FindAsync(Email);



            //attempt password check
            if (login == null || string.IsNullOrEmpty(password) || !PBKDF2.Verify(login.PasswordHash, password))
            {
                ModelState.AddModelError("LoginFailure", "Login attempt failed, please try again");
                return View(new Login { Email = Email }); ;
            }

            //attempt verified email
            if (login.Locked == Locked.locked)
            {
                ModelState.AddModelError("LoginFailure", "Email has not been validated. Please check inbox or junk folder to validate");
                return View(new Login { Email = Email }); ;
            }

            Owner o = new Owner();
            Walker w = new Walker();
            Administrator a = new Administrator();

            //Find Userids
            foreach (Owner owner in _context.Owners)
            {
                if (owner.Email == Email)
                    o = await _context.Owners.FindAsync(owner.UserId);

            }

            foreach (Walker walker in _context.Walkers)
            {
                if (walker.Email == Email)
                    w = await _context.Walkers.FindAsync(walker.UserId);
            }
            foreach (Administrator admin in _context.Administrators)
            {
                if (admin.Email == Email)
                    a = await _context.Administrators.FindAsync(admin.UserId);
            }

            string userType = "";


            if (o.Email != null)
            {
                HttpContext.Session.SetInt32(nameof(o.UserId), login.User.UserId);
                HttpContext.Session.SetString(nameof(o.FirstName), login.User.FirstName);
                HttpContext.Session.SetString("AccountType", "Owner");
                userType = "Owner";
            }
            else if (w.Email != null)
            {
                HttpContext.Session.SetInt32(nameof(w.UserId), login.User.UserId);
                HttpContext.Session.SetString(nameof(w.FirstName), login.User.FirstName);
                HttpContext.Session.SetString("AccountType", "Walker");
                userType = "Walker";
            }
            else if (a.Email != null)
            {
                HttpContext.Session.SetInt32(nameof(a.UserId), login.User.UserId);
                HttpContext.Session.SetString(nameof(a.FirstName), login.User.FirstName);
                HttpContext.Session.SetString("AccountType", "Administrator");
                userType = "Administrator";

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
