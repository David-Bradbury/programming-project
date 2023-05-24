using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using ProgrammingProject.Data;
using ProgrammingProject.Models;
using System.Text.RegularExpressions;
using ProgrammingProject.Utilities;
using ProgrammingProject.Helper;


namespace ProgrammingProject.Controllers
{
    public class VerificationController : Controller
    {

        private readonly EasyWalkContext _context;

        public VerificationController(EasyWalkContext context)
        {
            _context = context;
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
                    login.EmailToken = null;
                    _context.SaveChanges();
                }


            }
            if (verified == true)
                return View();
            else
                return RedirectToAction("Index", "Home");
        }


        [Route("/Verification/ForgotPassword",
            Name = "forgotPassword")]
        public IActionResult ForgotPassword()
        {
            var login = new Login();
            return View(login);

        }



        [Route("/Verification/ForgotPassword"), HttpPost]
        public IActionResult ForgotPassword(string email)
        {
            email = email.ToLower();
            var login = new Login();
            login.Email = email;

            if (email == null)
                ModelState.AddModelError(nameof(email), "Email is required.");

            bool emailExists = false;

            foreach (var l in _context.Logins)
                if (l.Email == email)
                {
                    if(l.Locked == Locked.locked)
                    {
                        ModelState.AddModelError(nameof(email), "The email entered has not been verified. " +
                                                                "Please verify email address prior to recovering password.");
                    }
                    else
                    {
                        emailExists = true;
                        l.EmailToken = ControllerHelper.GetToken();
                        login.EmailToken = l.EmailToken;
                        _context.SaveChanges();
                    }


                }

            if (!emailExists)
                ModelState.AddModelError(nameof(email), "The email entered does not exist in the system. " +
                                                                "Please try with a different email address.");


            if (!ModelState.IsValid)
            {
                return View(login);
            }

            SendPasswordRecovery(login);

            return RedirectToAction("Login", "Login");

        }

        public void SendPasswordRecovery(Login login)
        {

            string name = null;

            foreach (Owner o in _context.Owners)
                if (o.Email == login.Email)
                    name = o.FirstName;


            if (name == null)
                foreach (Walker w in _context.Walkers)
                    if (w.Email == login.Email)
                        name = w.FirstName;



            var recipient = login.Email;
            var subject = "Password Recovery";

            //String for woring locally
            //const string url = "https://localhost:7199/Verification/NewPassword";

            //String for deployed version
               const string url = "https://programmingproject-easywalk.azurewebsites.net/Verification/NewPassword";

            var param = new Dictionary<string, string>() { { "emailToken", login.EmailToken } };

            var newUrl = new Uri(QueryHelpers.AddQueryString(url, param)).ToString();

            var htmlContent = GetPasswordRecoveryContent(newUrl, name);

            //Calling the method to send email.
            Email.SendEmail(recipient, subject, htmlContent);
        }

        private string GetPasswordRecoveryContent(string url, string name)
        {
            string content = "";

            try
            {
                using (var sr = new StreamReader("./Helper/PasswordRecoveryContent.html"))
                {
                    string fileContent = sr.ReadToEnd();
                    content = String.Format(fileContent, name, url);
                }
            }
            catch (Exception e)
            {
                content = "Please verify your email using this link: " + url;
            }

            return content;
        }

        [Route("/Verification/NewPassword")]
        public IActionResult NewPassword(string emailToken)
        {
            var viewModel = new RegisterViewModel();

            foreach (Login l in _context.Logins)
            {
                if (emailToken == l.EmailToken)
                {
                    viewModel.Email = l.Email;
                    return View(viewModel);
                }

            }

            return RedirectToAction("Index", "Home");

        }

        [Route("/Verification/NewPassword")]
        [HttpPost]
        public IActionResult NewPassword(string email, string password, string confirmPassword)
        {
            var viewModel = new RegisterViewModel();
            viewModel.Email = email;

            if (password == null)
                ModelState.AddModelError(nameof(password), "Password is required.");

            if (password != confirmPassword)
                ModelState.AddModelError(nameof(confirmPassword), "Passwords need to match.");

            if (!Regex.IsMatch(password, @"^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=]).*$"))
                ModelState.AddModelError(nameof(password), "Password is Invalid. Password must contain at least one upper case letter, a lower case letter, " +
                    "a special character, a number, and must be at least 8 characters in length");

            if (!ModelState.IsValid)
                return View(viewModel);

            foreach (Login l in _context.Logins)
                if (l.Email == email)
                {
                    l.PasswordHash = ControllerHelper.HashPassword(password);
                    l.EmailToken = null;
                    _context.SaveChanges();
                    return RedirectToAction("Login", "Login");
                }
            return RedirectToAction("Index", "Home");

        }

    }
}
