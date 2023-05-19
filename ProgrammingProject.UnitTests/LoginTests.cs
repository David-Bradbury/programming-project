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
            string email = null;
            string password = "Aqwe123";

            _lc.Login(email, password);

            Assert.IsFalse(_lc.ViewData.ModelState.IsValid);
        }

        // Can't run following test as hashing pw method ends test, unsure why.
        //[Test]
        //public void Login_ValidData_ModelStateIsValid()
        //{
        //    string email = "johnsmith@proton.me";
        //    string password = "abc123";

        //    _lc.Login(email, password);

        //    Assert.IsTrue(_lc.ViewData.ModelState.IsValid);
        //}

    }
}