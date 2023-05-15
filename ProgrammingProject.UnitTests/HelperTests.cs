using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using NUnit.Framework;
using ProgrammingProject.Data;
using ProgrammingProject.Helper;
using ProgrammingProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ProgrammingProject.UnitTests
{
    [TestFixture]
    internal class HelperTests
    {
        private  EasyWalkContext _context;
        private IWebHostEnvironment _webHostEnvironment;

        [SetUp]
        public void SetUp()
        {
            var mt = new MasterTest();
            _context = mt.CreateContext();
            
        }

        //[Test]
        //public void GetStates_WhenCalled_ReturnsListOfStates()
        //{
        //    var states = DropDownLists.GetStates();

        //    List<SelectListItem> testStates = new List<SelectListItem>();

        //    testStates.Add(new SelectListItem { Text = "South Australia", Value = "South Australia" });
        //    testStates.Add(new SelectListItem { Text = "Victoria", Value = "Victoria" });
        //    testStates.Add(new SelectListItem { Text = "Western Australia", Value = "Western Australia" });
        //    testStates.Add(new SelectListItem { Text = "Northern Territory", Value = "Northern Territory" });
        //    testStates.Add(new SelectListItem { Text = "New South Wales", Value = "New South Wales" });
        //    testStates.Add(new SelectListItem { Text = "Australian Capital Territory", Value = "Australian Capital Territory" });
        //    testStates.Add(new SelectListItem { Text = "Queensland", Value = "Queensland" });
        //    testStates.Add(new SelectListItem { Text = "Tasmania", Value = "Tasmania" });

        //    Assert.That(states.ToList().First(), Is.EquivalentTo((System.Collections.IEnumerable)testStates.ToList().First()));
        //}

        [Test]
        public void CreateVet_NewVet_ReturnsVet()
        {
            // Arrange 
            var CreateHelper = new Create(_context, _webHostEnvironment);

            string businessName = "Vet";
            string phNumber = "0400 000 000";
            string email = "Vet@proton.me";
            string streetAddress = "13 test avenue";
            string country = "Australia";

            var suburb = _context.Suburbs.Where(s => s.SuburbName == "Barangaroo")
                                          .Where(s => s.Postcode == "2000")
                                          .Where(s => s.State == "NSW").FirstOrDefault();
      
            int vetID = 0;

            // Act
            var vet = CreateHelper.CreateVet(businessName, phNumber, email, streetAddress, country, suburb, vetID);

            // Assert        
            Assert.IsInstanceOf<Vet>(vet);
        }

        [Test]
        public void CreateVet_EditVet_ReturnsVet()
        {
            // Arrange 
            var CreateHelper = new Create(_context, _webHostEnvironment);
           
            string businessName = "Vet";
            string phNumber = "0400 000 000";
            string email = "Vet@proton.me";
            string streetAddress = "13 test avenue";
            string country = "Australia";

            var suburb = _context.Suburbs.Where(s => s.SuburbName == "Barangaroo")
                                          .Where(s => s.Postcode == "2000")
                                          .Where(s => s.State == "NSW").FirstOrDefault();

            int vetID = 3;

            // Act
            var vet = CreateHelper.CreateVet(businessName, phNumber, email, streetAddress, country, suburb, vetID);

            // Assert        
            Assert.IsInstanceOf<Vet>(vet);
        }
    }
}
