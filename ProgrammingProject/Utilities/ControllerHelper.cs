using SimpleHashing;

namespace ProgrammingProject.Utilities
{
    public static class ControllerHelper
    {

        public static string HashPassword(string password)
        {
            var hashedPassword = PBKDF2.Hash(password);
            return hashedPassword;
        }
    }
}
