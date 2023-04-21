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
    internal class OwnerTests
    {

        private EasyWalkContext _context;
        private OwnerController _oc;

        [SetUp]
        public void SetUp()
        {
            var mt = new MasterTest();
            _context = mt.CreateContext();
            //_oc = new OwnerController(_context);
        }



    }
}
