using Microsoft.AspNetCore.Mvc;
using ProgrammingProject.Data;
using ProgrammingProject.Models;
using ProgrammingProject.Filters;


namespace ProgrammingProject.Controllers
{
    public class ProfileController : Controller
    {
        private readonly EasyWalkContext _context;
        private bool isOwner;
        private int? OwnerID => HttpContext.Session.GetInt32(nameof(Owner.UserId)).Value;
        private int? WalkerID => HttpContext.Session.GetInt32(nameof(Walker.UserId)).Value;
       
        [AuthorizeUser]
        public ProfileController(EasyWalkContext context)
        {
            _context = context;
            if (OwnerID == null || WalkerID != null)
            {
                isOwner = false;
            }
        }

        public async Task<IActionResult> YourProfile()
        {

        }

    }
}
