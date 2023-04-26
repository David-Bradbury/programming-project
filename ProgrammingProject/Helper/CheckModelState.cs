using Microsoft.AspNetCore.Mvc;
using ProgrammingProject.Data;
using ProgrammingProject.Models;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProgrammingProject.Helper;
using Microsoft.AspNetCore.WebUtilities;
using System.Web.Mvc;

namespace ProgrammingProject.Helper
{
    public class CheckModelState
    {
        private readonly EasyWalkContext _context;
        public CheckModelState(EasyWalkContext context)
        {
            _context = context;
        }
        public static void CheckRegex(string value, string regex, string message, Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary modelState) 
        {
            if (!Regex.IsMatch(value, regex))
                modelState.AddModelError(nameof(value), message);
        }

        public static void CheckNull(string value, string message, Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary modelState)
        {
            if (value == null)
                modelState.AddModelError(nameof(value), message);
        }  
        
        public void CheckEmailMatch(string email,  Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary modelState)
        {
            foreach(var i in _context.Logins)
            {
                if(i.Email == email)
                {
                    modelState.AddModelError(nameof(email), "This email is already registered in the system. Please try with a different email address.");
                }
            }
        }
    }
}
