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
        public void CreateEditProfileViewModel_AsWalker_ReturnsViewModel()
        {
            var walker = _context.Walkers.Find(7);
            var owner = _context.Owners.Find(7); ;
            bool isAdmin = false;

            var Task = _pc.CreateEditProfileViewModel(walker, owner, isAdmin);

            Assert.That(Task.UserType == typeof(Walker).Name);
        }

        [Test]
        public void CreateEditProfileViewModel_AsOwner_ReturnsViewModel()
        {
            var walker = _context.Walkers.Find(3);
            var owner = _context.Owners.Find(3); ;
            bool isAdmin = false;

            var Task = _pc.CreateEditProfileViewModel(walker, owner, isAdmin);

            Assert.That(Task.UserType == typeof(Owner).Name);
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

            _pc.EditProfile(testViewModel);

            Assert.IsTrue(_pc.ViewData.ModelState.IsValid);
        }

        [Test]
        public void DeleteDog_WhenCalled_RemoveDogfromDB() 
        {
            int DogID = 5;

            var Task = _pc.DeleteDog(DogID);
            Task.Wait();

            Assert.IsNull(_context.Dogs.Find(DogID));
        }

    }
}
