using ProgrammingProject.Data;
using ProgrammingProject.Models;
using SimpleHashing;
using X.PagedList;

namespace ProgrammingProject.Utilities
{
    public static class ControllerHelper
    {

        public static string HashPassword(string password)
        {
            var hashedPassword = PBKDF2.Hash(password);
            return hashedPassword;
        }
        public static string GetToken()
        {
            Random random = new Random();

            string token = ControllerHelper.HashPassword((random.Next().ToString()));
            return token;
        }

        public async static Task<UserAdminViewModel> BuildUserAdminViewModel(EasyWalkContext context, int page)
        {
            var viewModel = new UserAdminViewModel();

            var userList = new List<User>();
            const int pageSize = 20;

            foreach (Owner o in context.Owners)
            {
                userList.Add(o);
            }
            foreach(Walker w in context.Walkers)
            {
                userList.Add(w);
            }
            userList = userList.OrderBy(x => x.UserId).ToList();
            viewModel.PagedList= userList.ToPagedList(page, pageSize);

            return viewModel;


        }





    }
}
