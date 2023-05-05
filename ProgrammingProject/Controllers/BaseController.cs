using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProgrammingProject.Data;
using ProgrammingProject.Helper;
using ProgrammingProject.Models;
using System.Text.RegularExpressions;

namespace ProgrammingProject.Controllers
{
    public class BaseController : Controller
    {
        private readonly EasyWalkContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public BaseController(EasyWalkContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;

        }
        // Ensures the Suburb is a valid Australian Suburb.
        public void CheckSuburbModelState(string SuburbName, string Postcode, string State)
        {

            var Suburb = _context.Suburbs.Where(s => s.SuburbName == SuburbName)
                                        .Where(s => s.Postcode == Postcode)
                                        .Where(s => s.State == State);
            if (Suburb.IsNullOrEmpty())
            {
                var sName = _context.Suburbs.Where(s => s.SuburbName == SuburbName);
                if (sName.IsNullOrEmpty())
                    ModelState.AddModelError(nameof(SuburbName), "Suburb Name does not exist in Australia");

                var sPostcode = _context.Suburbs.Where(s => s.Postcode == Postcode);
                if (sPostcode.IsNullOrEmpty())
                {
                    ModelState.AddModelError(nameof(Postcode), "Postcode does not exist in Australia");
                    return;
                }
                var sNamePostcodeMatch = _context.Suburbs.Where(s => s.SuburbName == SuburbName)
                                                             .Where(s => s.Postcode == Postcode);
                if (sNamePostcodeMatch.IsNullOrEmpty())
                {
                    ModelState.AddModelError(nameof(SuburbName), "There are no Suburbs with the Name and Postcode given");
                    return;
                }            
                    ModelState.AddModelError(nameof(State), "There are no Suburbs with the Name and Postcode given in this State/Territory");               
            }
        }

        // Check that password is valid.
        public void CheckValidPassword(string password, string confirmPassword)
        {
            if (!Regex.IsMatch(password, @"^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=]).*$"))
                ModelState.AddModelError(nameof(password), "Password is Invalid. Password must contain at least one upper case letter, a lower case letter, a special character, a number, and must be at least 8 characters in length");

            if (password != confirmPassword)
                ModelState.AddModelError(nameof(confirmPassword), "Passwords need to match.");
        }

        // Checks if a value is Null.
        public void CheckNull(string value,string nameOfValue, string message)
        {
            if (value == null)
                ModelState.AddModelError(nameOfValue, message);
        }

        // Checks to see if regex match.
        public void CheckRegex(string value, string nameOfValue, string regex, string message)
        {
            if (!Regex.IsMatch(value, regex))
                ModelState.AddModelError(nameof(value), message);
        }

        // Checks that image extensions are appropriate. Currently allowed extensions are jpg/jpeg and png.
        public void CheckImageExtension(IFormFile image, string nameOfValue)
        {
            string filename = Path.GetFileName(image.FileName);
            string extension = Path.GetExtension(filename).ToLower();

            if (extension != ".jpg" && extension != ".jpeg" && extension != ".png")
                ModelState.AddModelError(nameOfValue, "Image must be of the jpg/jpeg, or png format");
        }
    }
}
