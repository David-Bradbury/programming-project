using X.PagedList;

namespace ProgrammingProject.Models
{
    public class UserAdminViewModel
    {
        public string Action { get; set; }
        public string UserEmail { get; set; }
        public IPagedList<User> PagedList { get; set; }
    }
}
