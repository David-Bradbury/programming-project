using Microsoft.AspNetCore.Hosting;
using NUnit.Framework;
using ProgrammingProject.Controllers;
using ProgrammingProject.Data;
using ProgrammingProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingProject.UnitTests
{
    [TestFixture]
    internal class ProfileTests
    {
        private EasyWalkContext _context;
        private ProfileController _pc;
        private IWebHostEnvironment _webHostEnvironment;


        [SetUp]
        public void Setup()
        {
            var mt = new MasterTest();
            _context = mt.CreateContext();
            _pc = new ProfileController(_context, _webHostEnvironment);
        }

        [Test]
        public void EditProfile_WithValidData_ModelStateIsValid()
        {
            var testViewModel = new EditProfileViewModel();

            testViewModel.UserID = 2; //John
            testViewModel.FirstName = "Johnny";
            testViewModel.LastName = "Smith";
            testViewModel.StreetAddress = "12 Pine Road";
            testViewModel.SuburbName = "Barangaroo";
            testViewModel.Postcode = "2000";
            testViewModel.State = "NSW";
            testViewModel.PhNumber = "0400 000 000"; 

            _pc.EditProfile(testViewModel, 0);

            Assert.IsTrue(_pc.ViewData.ModelState.IsValid);
        }

    }
}
