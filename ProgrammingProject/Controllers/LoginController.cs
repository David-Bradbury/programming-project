//using ProgrammingProject.Data;
//using ProgrammingProject.Models;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using SimpleHashing;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace ProgrammingProject.Controllers
//{
//    //Mask URL
//    [Route("/Login")]
//    public class LoginController : Controller
//    {
//        private readonly EasyWalkContext _context;

//        public LoginController(EasyWalkContext context)
//        {
//            _context = context;
//        }


//        public IActionResult login() => View();

//        //attempt loging
//        [HttpPost]
//        public async Task<IActionResult> login(string loginID, string password)
//        {
//            //find login id
//            var login = await _context.Logins.FindAsync(loginID);

//            //attempt password check
//            if (login == null || !PBKDF2.Verify(login.PasswordHash, password))
//            {
//                ModelState.AddModelError("loginfailure", "login attempt failed, please try again");
//                return View(new Login { LoginID = loginID });
//            }

//            //customer login
//            HttpContext.Session.SetInt32(nameof(User.Id), login.UserId);
//            HttpContext.Session.SetString(nameof(User.User_name), login.user.user_name);
             
//            return RedirectToAction("Index", "User");
//        }

//        [Route("loggingOut")] 
//        public IActionResult logout()
//        {
//            HttpContext.Session.Clear();

//            return RedirectToAction("Index", "Home");
//        }




//    }
//}

