using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using ProgrammingProject.Controllers;
using ProgrammingProject.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingProject.UnitTests
{
    internal class BaseTests
    {
        private EasyWalkContext _context;
        private IWebHostEnvironment _webHostEnvironment;
        private BaseController _bc;

        [SetUp]
        public void SetUp()
        {
            var mt = new MasterTest();
            _context = mt.CreateContext();
            _bc = new BaseController(_context, _webHostEnvironment);
        }

        [Test]
        public void CheckSuburbModelState_IncorrectSuburbName_ReturnsModelErrorAndMessage()
        {
            string suburbName = "Docklandss";
            string postcode = "3008";
            string state = "VIC";

            _bc.CheckSuburbModelState(suburbName, postcode, state);

            Assert.IsTrue(_bc.ViewData.ModelState["SuburbName"].Errors.Count > 0);
            Assert.AreEqual(_bc.ModelState["SuburbName"].Errors[0].ErrorMessage, "Suburb Name does not exist in Australia");
        }

        [Test]
        public void CheckSuburbModelState_IncorrectPostcode_ReturnsModelErrorAndMessage()
        {
            string suburbName = "Docklands";
            string postcode = "0008";
            string state = "VIC";

            _bc.CheckSuburbModelState(suburbName, postcode, state);

            Assert.IsTrue(_bc.ViewData.ModelState["Postcode"].Errors.Count > 0);
            Assert.AreEqual(_bc.ModelState["Postcode"].Errors[0].ErrorMessage, "Postcode does not exist in Australia");
        }

        [Test]
        public void CheckSuburbModelState_NoPostcodeAndSuburbNameMatch_ReturnsModelErrorAndMessage()
        {
            string suburbName = "Docklands";
            string postcode = "2000";
            string state = "VIC";

            _bc.CheckSuburbModelState(suburbName, postcode, state);

            Assert.IsTrue(_bc.ViewData.ModelState["SuburbName"].Errors.Count > 0);
            Assert.AreEqual(_bc.ModelState["SuburbName"].Errors[0].ErrorMessage, "There are no Suburbs with the Name and Postcode given");
        }

        [Test]
        public void CheckSuburbModelState_NoPostcodeSuburbNameAndStateMatch_ReturnsModelErrorAndMessage()
        {
            string suburbName = "Docklands";
            string postcode = "3008";
            string state = "NSW";

            _bc.CheckSuburbModelState(suburbName, postcode, state);

            Assert.IsTrue(_bc.ViewData.ModelState["State"].Errors.Count > 0);
            Assert.AreEqual(_bc.ModelState["State"].Errors[0].ErrorMessage, "There are no Suburbs with the Name and Postcode given in this State/Territory");
        }

        [Test]
        public void CheckSuburbModelState_WithValidData_ModelStateIsValid()
        {
            string suburbName = "Docklands";
            string postcode = "3008";
            string state = "VIC";

            _bc.CheckSuburbModelState(suburbName, postcode, state);

            Assert.IsTrue(_bc.ViewData.ModelState.IsValid);

        }

        [Test]
        public void CheckValidPassword_PasswordInvalid_ReturnsModelErrorAndMessage()
        {
            string password = "abc123";
            string confirmPassword = "abc123";

            _bc.CheckValidPassword(password, confirmPassword);

            Assert.IsTrue(_bc.ModelState["Password"].Errors.Count > 0);
            Assert.AreEqual(_bc.ModelState["Password"].Errors[0].ErrorMessage, "Password is Invalid. Password must contain at least one upper case letter," +
                " a lower case letter, a special character, a number, and must be at least 8 characters in length");
        }

        [Test]
        public void CheckValidPassword_ConfirmPasswordDifferent_ReturnsModelErrorAndMessage()
        {
            string password = "Abcd123!";
            string confirmPassword = "AbcD123!";

            _bc.CheckValidPassword(password, confirmPassword);

            Assert.IsTrue(_bc.ModelState["confirmPassword"].Errors.Count > 0);
            Assert.AreEqual(_bc.ModelState["confirmPassword"].Errors[0].ErrorMessage, "Passwords need to match.");
        }

        [Test]
        public void CheckValidPassword_WithValidData_ModelStateIsValid()
        {
            string password = "Abcd123!";
            string confirmPassword = "Abcd123!";

            _bc.CheckValidPassword(password, confirmPassword);

            Assert.IsTrue(_bc.ViewData.ModelState.IsValid);
        }

        [Test]
        public void CheckNull_EmptyString_ReturnsModelErrorAndMessage()
        {
            string firstName = null;
            string value = nameof(firstName);
            string message = "First Name is Required";

            _bc.CheckNull(firstName, nameof(firstName), message);

            Assert.IsFalse(_bc.ViewData.ModelState.IsValid);
            Assert.AreEqual(_bc.ViewData.ModelState[value].Errors[0].ErrorMessage, message);

        }

        [Test]
        public void CheckNull_WithValidData_ModelStateIsValid()
        {
            string firstName = "David";
            string value = nameof(firstName);
            string message = "First Name is Required";

            _bc.CheckNull(firstName, nameof(firstName), message);

            Assert.IsTrue(_bc.ViewData.ModelState.IsValid);


        }

        [Test]
        public void CheckRegex_WhenCalled_ReturnsModelError()
        {
            string postcode = "0000";
            string regex = @"(^0[289][0-9]{2}\s*$)|(^[1-9][0-9]{3}\s*$)";
            string message = "This postcode does not match any Australian postcode. Please enter an Australian 4 digit postcode";

            _bc.CheckRegex(postcode, nameof(postcode), regex, message);

            Assert.IsFalse(_bc.ViewData.ModelState.IsValid);
        }

        [Test]
        public void CheckRegex_WhenCalled_ModelStateIsValid()
        {
            string postcode = "3008";
            string regex = @"(^0[289][0-9]{2}\s*$)|(^[1-9][0-9]{3}\s*$)";
            string message = "This postcode does not match any Australian postcode. Please enter an Australian 4 digit postcode";

            _bc.CheckRegex(postcode, nameof(postcode), regex, message);

            Assert.IsTrue(_bc.ViewData.ModelState.IsValid);
        }

        [Test]
        public void CheckImageExtesion_WhenCalled_ReturnsModelStateError()
        {
            //Arrange
            var content = "Hello World from a Fake File";
            var fileName = "test.pdf";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;

            //create FormFile with desired data
            IFormFile file = new FormFile(stream, 0, stream.Length, "id_from_form", fileName);

            //Act
            _bc.CheckImageExtension(file, nameof(file));

            //Assert
            Assert.IsFalse(_bc.ViewData.ModelState.IsValid);
        }

        [Test]
        public void CheckImageExtesion_WhenCalled_ModelStateIsValid()
        {
            //Arrange
            var content = "Hello World from a Fake File";
            var fileName = "test.png";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;

            //create FormFile with desired data
            IFormFile file = new FormFile(stream, 0, stream.Length, "id_from_form", fileName);

            //Act
            _bc.CheckImageExtension(file, nameof(file));

            //Assert
            Assert.IsTrue(_bc.ViewData.ModelState.IsValid);
        }
    }
}
