using ProgrammingProject.Data;
using ProgrammingProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleHashing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingProject.Controllers
{
    //Mask URL
    [Route("/Login")]
    public class LoginController : Controller
    {
        //private readonly EasyWalkContext _context;

        //public LoginController(EasyWalkContext context)
        //{
        //    _context = context;
        //}


        //public IActionResult Login() => View();

        ////attempt loing
        //[HttpPost]
        //public async Task<IActionResult> Login(string loginID, string password)
        //{
        //    //find login id
        //    var login = await _context.Logins.FindAsync(loginID);

        //    //attempt password check
        //    if (login == null || !PBKDF2.Verify(login.PasswordHash, password))
        //    {
        //        ModelState.AddModelError("LoginFailure", "Login attempt failed, please try again");
        //        return View(new Login { LoginID = loginID });
        //    }

        //    //Customer login
        //    HttpContext.Session.SetInt32(nameof(User.id), login.id);
        //    HttpContext.Session.SetString(nameof(User.user_name), login.User.user_name);

        //    return RedirectToAction("Index", "User");
        //}

        //[Route("LoggingOut")]
        //public IActionResult Logout()
        //{
        //    HttpContext.Session.Clear();

        //    return RedirectToAction("Index", "Home");
        //}




    }
}

