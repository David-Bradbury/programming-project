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
    internal class OwnerTests
    {

        private EasyWalkContext _context;
        private IWebHostEnvironment _webHostEnvironment;
        private OwnerController _oc;

        [SetUp]
        public void SetUp()
        {
            var mt = new MasterTest();
            _context = mt.CreateContext();
            _oc = new OwnerController(_context, _webHostEnvironment);
        }

        // Before Implementing this test, unittest DB,
        // Need to add list of suburbs to db of a 10km radius of target suburb.
        //[Test] 
        //public void GetSuitableWalkingSessions_() 
        //{
            
        //}

        [Test]
        public void UpdateNewDogInSession_WhenCalled_ReturnsWalkingSessionList()
        {
            var ws = _context.WalkingSessions.Find(2);
            var dog = _context.Dogs.Find(1);

            ws.DogList = new List<Dog>();

            _oc.UpdateNewDogInSession(ws.SessionID, dog.Id);

            Assert.IsNotEmpty(ws.DogList);
        }

        [Test]
        public void RemoveDogFromDBSession_WhenCalled_RemovesDogFromDB()
        {
            var ws = _context.WalkingSessions.Find(2);
            var dog = _context.Dogs.Find(1);

            ws.DogList = new List<Dog>();

            _oc.UpdateNewDogInSession(ws.SessionID, dog.Id);

            _oc.RemoveDogFromDBSession(ws.SessionID, dog.Id);

            Assert.IsEmpty(ws.DogList);
        }
    }

}

