using Microsoft.AspNetCore.Hosting;
using NUnit.Framework;
using ProgrammingProject.Controllers;
using ProgrammingProject.Data;
using System.Threading.Tasks;

namespace ProgrammingProject.UnitTests
{
    [TestFixture]
    public class LoginTests
    {
        private EasyWalkContext _context;
        private LoginController _lc;

        [SetUp]
        public void Setup()
        {
            var mt = new MasterTest();
            _context = mt.CreateContext();
            _lc = new LoginController(_context);
        }
   
        [Test]
        public void Login_PasswordUnverified_ReturnsModelStateError()
        {
            string email = "ok";
            string password = "Aqwe123";

            _lc.Login(email, password);

            Assert.IsFalse(_lc.ModelState.IsValid);
        }
    }
}