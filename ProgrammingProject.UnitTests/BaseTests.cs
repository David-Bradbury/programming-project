using Microsoft.AspNetCore.Hosting;
using NUnit.Framework;
using ProgrammingProject.Controllers;
using ProgrammingProject.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingProject.UnitTests
{
    internal class BaseTests
    {
        private  EasyWalkContext _context;
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
        public void CheckImageExtesion_WhenCalled_ReturnsModelStateError()
        {

        }
        public void CheckRegex_WhenCalled_ReturnsModelStateError()
        {

        }

    }
}
