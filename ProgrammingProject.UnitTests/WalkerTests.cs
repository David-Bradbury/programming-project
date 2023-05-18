using Microsoft.EntityFrameworkCore;
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
    internal class WalkerTests
    {
        private EasyWalkContext _context;
        private WalkerController _wc;

        [SetUp]
        public void SetUp()
        {
            var mt = new MasterTest();
            _context = mt.CreateContext();
            _wc = new WalkerController(_context);
        }

        [Test]
        public void FilterDogs_WhenPassedAListOfDogs_ReturnsAFilteredList()
        {
            //var walker = new Walker();
            //walker.UserId = 6;

            int UserID = 6;
            var walker = _context.Walkers.Find(UserID);

            Task<List<Dog>> task = _wc.MatchDogsToWalker(walker.UserId);
            //Task<List<Dog>> task = _wc.MatchDogsToWalker((int)walker.UserId);
            // task.Wait();

            var result = _wc.FilterDogs(task.Result);

            // Can improve once additional filters are provided.
            Assert.That(result.Count, Is.LessThanOrEqualTo(task.Result.Count));
        }

        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        [Test]
        //public void MatchDogsToWalker_WhenCalled_ReturnsListOfSuitableDogs(int a)
        public async Task MatchDogsToWalker_WhenCalled_ReturnsListOfSuitableDogs(int a)
        {
            var walker = new Walker();
            walker.UserId = a;
            walker.ExperienceLevel = ExperienceLevel.Advanced;

            //List<Dog> task = await _wc.MatchDogsToWalker((int)walker.UserId);
            var task = await _wc.MatchDogsToWalker(walker.UserId);
            //task.Wait();

            //var result = task.Result;

            // Can improve on this ensure exact amount is produced.
            Assert.That(task.ToList().FirstOrDefault, Is.InstanceOf<Dog>());
            //Assert.That(result.Count, Is.GreaterThanOrEqualTo(1));

            // Not sure why test is failing. For some reason the method when
            // called is not finding any walkers in context even though id is added.DP

        }

        [Test]
        public void GetListOfSuitableDogsToWalkers_WhenProvidedWalkerID_ReturnsListOfDogs()
        {
            var walker = new Walker();
            walker.ExperienceLevel = ExperienceLevel.Beginner;

            IEnumerable<Dog> dogs = _context.Dogs.AsEnumerable();

            Assert.That(dogs.Count, Is.GreaterThanOrEqualTo(1));

        }

        [Test]
        [TestCase(DogSize.Small, Temperament.Reactive, 3)]
        [TestCase(DogSize.Medium, Temperament.Calm, 2)]
        [TestCase(DogSize.Large, Temperament.NonReactive, 2)]
        public async Task GetDogTraitScore_WhenCalled_ReturnsTheDogTraitScoreAsync(DogSize a, Temperament b, int expectedResult)
        {
            var dog = new Dog();
            dog.DogSize = a;
            dog.Temperament = b;

            var result = await _wc.GetDogTraitScore(dog);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        //[Test]
        //public void UpdateWalkingSession_WithValidData_UpdatesSessionDetailsInDB()
        //{        
        //    var ws = _context.WalkingSessions.Find(2);
        //    DateTime date = new DateTime(2023, 06, 27, ws.Date.Hour, ws.Date.Minute, ws.Date.Second);
        //    DateTime startTime = new DateTime(date.Year, date.Month, date.Day, ws.ScheduledStartTime.Hour,
        //        ws.ScheduledStartTime.Minute, ws.ScheduledStartTime.Second);
        //    DateTime endTime = new DateTime(date.Year, date.Month, date.Day, ws.ScheduledEndTime.Hour,
        //        ws.ScheduledEndTime.Minute, ws.ScheduledEndTime.Second);

        //    _wc.UpdateWalkingSession(ws.SessionID, date, startTime, endTime);

        //    // Assert

        //}

        [Test]
        public void DeleteWalkingSession_WhenCalled_RemovesSessionFromDB()
        {
            int sessionID = 1;

            var task = _wc.DeleteWalkingSession(sessionID);
            task.Wait();

            Assert.IsNull(_context.WalkingSessions.Find(sessionID));
        }
    }
}
