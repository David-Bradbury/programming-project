using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ProgrammingProject.Models;

namespace ProgrammingProject.Filters
{
    public class AuthorizeUserAttribute : Attribute, IAuthorizationFilter
    {

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var ownerID = context.HttpContext.Session.GetInt32(nameof(Owner.UserId));
            var walkerID = context.HttpContext.Session.GetInt32(nameof(Walker.UserId));
            var adminID = context.HttpContext.Session.GetInt32(nameof(Administrator.UserId));

            if(!ownerID.HasValue || !walkerID.HasValue || !adminID.HasValue)
            {
                context.Result = new RedirectToActionResult("Index", "Home", null);
            }
        }
    }
}
