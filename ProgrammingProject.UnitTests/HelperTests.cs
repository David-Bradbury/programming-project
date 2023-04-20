using Microsoft.AspNetCore.Mvc.Rendering;
using NUnit.Framework;
using ProgrammingProject.Helper;
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
        [Test]
        public void GetStates_WhenCalled_ReturnsListOfStates()
        {
            var states = DropDownLists.GetStates();

            List<SelectListItem> testStates = new List<SelectListItem>();

            testStates.Add(new SelectListItem { Text = "South Australia", Value = "South Australia" });
            testStates.Add(new SelectListItem { Text = "Victoria", Value = "Victoria" });
            testStates.Add(new SelectListItem { Text = "Western Australia", Value = "Western Australia" });
            testStates.Add(new SelectListItem { Text = "Northern Territory", Value = "Northern Territory" });
            testStates.Add(new SelectListItem { Text = "New South Wales", Value = "New South Wales" });
            testStates.Add(new SelectListItem { Text = "Australian Capital Territory", Value = "Australian Capital Territory" });
            testStates.Add(new SelectListItem { Text = "Queensland", Value = "Queensland" });
            testStates.Add(new SelectListItem { Text = "Tasmania", Value = "Tasmania" });

            Assert.That(states.ToList().First(), Is.EquivalentTo((System.Collections.IEnumerable)testStates.ToList().First()));
        }
    }
}
