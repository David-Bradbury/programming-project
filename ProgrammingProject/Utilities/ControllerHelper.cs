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

        public static string GetToken()
        {
            //Generate tokens for email recovery and verify
            Random random = new Random();

            var token = HashPassword((random.Next().ToString()));
            return token;
        }


    }
}
