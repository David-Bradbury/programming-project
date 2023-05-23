using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using ProgrammingProject.Controllers;
using ProgrammingProject.Data;
using ProgrammingProject.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            int UserID = 6;
            var walker = _context.Walkers.Find(UserID);

            Task<List<Dog>> task = _wc.MatchDogsToWalker(walker.UserId);
       
            var result = _wc.FilterDogs(task.Result);

            // Can improve once additional filters are provided.
            Assert.That(result.Count, Is.LessThanOrEqualTo(task.Result.Count));
        }

        [TestCase(5)]
        [TestCase(6)]
        [TestCase(7)]
        [Test]
        public async Task MatchDogsToWalker_WhenCalled_ReturnsListOfSuitableDogs(int a)
        {
            var walker = new Walker();
            walker.UserId = a;
            walker.ExperienceLevel = ExperienceLevel.Advanced;

            var dogs = await _wc.MatchDogsToWalker(walker.UserId);
          
            Assert.That(dogs, Is.InstanceOf<List<Dog>>());
        }

        // Need to add list of suburbs to db of a 10km radius of target suburb.
        //[Test]
        //public void FilterLocationByRadius_DefaultRange_ReturnsListOfDogs()
        //{

        //}

        [Test]
        public void FilterDogsByTrainingLevel_FullyTrained_ReturnsListOfDogs()
        {
            List<Dog> dogs = _context.Dogs.ToList();
            TrainingLevel tl = TrainingLevel.Fully;

            var Task = _wc.FilterDogsByTrainingLevel(dogs, tl);

            Assert.IsNotNull(Task);
        }

        [Test]
        public void FilterDogsByTrainingLevel_BasicTraining_ReturnsListOfDogs()
        {
            List<Dog> dogs = _context.Dogs.ToList();
            TrainingLevel tl = TrainingLevel.Fully;

            var Task = _wc.FilterDogsByTrainingLevel(dogs, tl);

            Assert.IsNotNull(Task);
        }

        [Test]
        public void FilterDogsByTrainingLevel_NoTraining_ReturnsListOfDogs()
        {
            List<Dog> dogs = _context.Dogs.ToList();
            TrainingLevel tl = TrainingLevel.Fully;

            var Task = _wc.FilterDogsByTrainingLevel(dogs, tl);

            Assert.IsNotNull(Task);
        }

        [Test]
        public void FilterDogsByMinimumTrainingLeveL_FullyTrained_ReturnsListOfDogs()
        {
            List<Dog> dogs = _context.Dogs.ToList();
            TrainingLevel tl = TrainingLevel.Fully;

            var Task = _wc.FilterDogsByMinimumTrainingLevel(dogs, tl);

            Assert.IsNotNull(Task);
        }

        [Test]
        public void FilterDogsByMinimumTrainingLeveL_BasicTraining_ReturnsListOfDogs()
        {
            List<Dog> dogs = _context.Dogs.ToList();
            TrainingLevel tl = TrainingLevel.Basic;

            var Task = _wc.FilterDogsByMinimumTrainingLevel(dogs, tl);

            Assert.IsNotNull(Task);
        }

        [Test]
        public void FilterDogsByMinimumTrainingLeveL_NoTraining_ReturnsListOfDogs()
        {
            List<Dog> dogs = _context.Dogs.ToList();
            TrainingLevel tl = TrainingLevel.None;

            var Task = _wc.FilterDogsByMinimumTrainingLevel(dogs, tl);

            Assert.IsNotNull(Task);
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

        [Test]
        public void AddDogToWalkingSession_WhenCalled_AdditionSavedToDB()
        {
            var ws = _context.WalkingSessions.Find(2);
            var dog = _context.Dogs.Find(1);
            ws.DogList = new List<Dog>();

            var Task = _wc.AddDogToWalkingSession(dog.Id, ws.SessionID, ws.ScheduledStartTime, ws.ScheduledEndTime);

            Assert.That(ws.DogList.Count > 0);
        }

        [Test]
        public void AddDogToWalkingSession_OverSixDogs_ReturnsModelErrorAndMessage()
        {
            var ws = _context.WalkingSessions.Find(3);
            var dog1 = new Dog();
            var dog2 = new Dog();
            var dog3 = new Dog();
            var dog4 = new Dog();
            var dog5 = new Dog();
            var dog6 = new Dog();

            ws.DogList = new List<Dog>();
            ws.DogList.Add(dog1);
            ws.DogList.Add(dog2);
            ws.DogList.Add(dog3);
            ws.DogList.Add(dog4);
            ws.DogList.Add(dog5);
            ws.DogList.Add(dog6);

            var addedDog = _context.Dogs.Find(1);

            var Task = _wc.AddDogToWalkingSession(addedDog.Id, ws.SessionID, ws.ScheduledStartTime, ws.ScheduledEndTime);

            DateTime StartTime;

            Assert.IsFalse(_wc.ViewData.ModelState.IsValid);
            Assert.AreEqual(_wc.ViewData.ModelState[nameof(StartTime)].Errors[0].ErrorMessage,
                "Too many dogs on this walk.");
        }

        [Test]
        public void AddDogToWalkingSession_DogAlreadyOnWalk_ReturnsModelErrorAndMessage()
        {
            var ws = _context.WalkingSessions.Find(3);
            ws.DogList = new List<Dog>();

            var dog = _context.Dogs.Find(1);
            ws.DogList.Add(dog);

            var Task = _wc.AddDogToWalkingSession(dog.Id, ws.SessionID, ws.ScheduledStartTime, ws.ScheduledEndTime);

            DateTime StartTime;

            Assert.IsFalse(_wc.ViewData.ModelState.IsValid);
            Assert.AreEqual(_wc.ViewData.ModelState[nameof(StartTime)].Errors[0].ErrorMessage,
                "Dog is already on this walk.");
        }

        [Test]
        public void RemoveDogFromWalk_WhenCalled_DogIsRemovedFromWalkInDB()
        {
            var ws = _context.WalkingSessions.Find(2);
            var dog = _context.Dogs.Find(1);
            ws.DogList = new List<Dog>();

            _wc.AddDogToWalkingSession(dog.Id, ws.SessionID, ws.ScheduledStartTime, ws.ScheduledEndTime);

            Assert.That(ws.DogList.Count > 0);

            _wc.RemoveDogFromWalk(dog.Id, ws.SessionID);

            Assert.That(ws.DogList.Count == 0);
        }

        [Test]
        public void RemoveDogFromWalk_DogNotOnWalk_ReturnsModelErrorAndMessage()
        {
            var ws = _context.WalkingSessions.Find(2);
            var dog = _context.Dogs.Find(3);
            ws.DogList = new List<Dog>();

            int SessionID;

            _wc.RemoveDogFromWalk(dog.Id, ws.SessionID);

            Assert.False(_wc.ViewData.ModelState.IsValid);
            Assert.AreEqual(_wc.ViewData.ModelState[nameof(SessionID)].Errors[0].ErrorMessage,
                "Dog is not on this walk");
        }

        [Test]
        public void RemoveDogFromWalk_SessionNotFound_ReturnsModelErrorAndMessage()
        {
            var ws = _context.WalkingSessions.Find(2);
            var dog = _context.Dogs.Find(3);
            ws.DogList = new List<Dog>();

            int SessionID = 200;

            var Task = _wc.RemoveDogFromWalk(dog.Id, SessionID);
            Task.Wait();

            Assert.False(_wc.ViewData.ModelState.IsValid);
            Assert.AreEqual(_wc.ViewData.ModelState[nameof(SessionID)].Errors[0].ErrorMessage,
                "Can't find walking Session");
        }

        [Test]
        public void StartWalkingSession_WhenCalled_AddsActualStartTimeToDB()
        {
            int sessionID = 2;

            DateTime dt = new DateTime();

            var Task = _wc.StartWalkingSession(sessionID);
            Task.Wait();

            Assert.AreNotEqual(dt, _context.WalkingSessions.Find(sessionID).ActualStartTime);
        }

        [Test]
        public void EndWalkingSession_WhenCalled_AddsActualEndTimeToDB()
        {
            int sessionID = 2;

            DateTime dt = new DateTime();

            var Task = _wc.EndWalkingSession(sessionID);
            Task.Wait();

            Assert.AreNotEqual(dt, _context.WalkingSessions.Find(sessionID).ActualEndTime);
        }

        [Test]
        public void EditWalkingSession_EndTimeBeforeStartTime_ReturnsModelErrorAndMessage()
        {

            var ws = _context.WalkingSessions.Find(2);
            DateTime date = new DateTime(ws.Date.Year, ws.Date.Month, ws.Date.Day, ws.Date.Hour, ws.Date.Minute, ws.Date.Second);
            DateTime startTime = new DateTime(date.Year, date.Month, date.Day, ws.ScheduledStartTime.Hour,
                ws.ScheduledStartTime.Minute, ws.ScheduledStartTime.Second);
            DateTime endTime = new DateTime(date.Year, date.Month, date.Day, 12,
                ws.ScheduledEndTime.Minute, ws.ScheduledEndTime.Second);

            _wc.EditWalkingSession(ws.SessionID, date, startTime, endTime);

            Assert.IsFalse(_wc.ViewData.ModelState.IsValid);
            Assert.AreEqual(_wc.ViewData.ModelState[nameof(WalkingSession.ScheduledEndTime)].Errors[0].ErrorMessage, "Valid End Time needs to be selected");
        }

        [Test]
        public void EditWalkingSession_DateChangedToDateInPast_ReturnsModelErrorAndMessage()
        {

            var ws = _context.WalkingSessions.Find(2);
            DateTime date = new DateTime(2020, 06, 27, ws.Date.Hour, ws.Date.Minute, ws.Date.Second);
            DateTime startTime = new DateTime(date.Year, date.Month, date.Day, ws.ScheduledStartTime.Hour,
                ws.ScheduledStartTime.Minute, ws.ScheduledStartTime.Second);
            DateTime endTime = new DateTime(date.Year, date.Month, date.Day, ws.ScheduledEndTime.Hour,
                ws.ScheduledEndTime.Minute, ws.ScheduledEndTime.Second);

            _wc.EditWalkingSession(ws.SessionID, date, startTime, endTime);

            Assert.IsFalse(_wc.ViewData.ModelState.IsValid);
            Assert.AreEqual(_wc.ViewData.ModelState[nameof(WalkingSession.Date)].Errors[0].ErrorMessage, "Date cannot be in the past");
        }

        [Test]
        public void EditWalkingSession_ChangeDay_UpdatesSessionDetailsInDB()
        {
            var ws = _context.WalkingSessions.Find(2);
            DateTime date = new DateTime(2023, 06, 27, ws.Date.Hour, ws.Date.Minute, ws.Date.Second);
            DateTime startTime = new DateTime(date.Year, date.Month, date.Day, ws.ScheduledStartTime.Hour,
                ws.ScheduledStartTime.Minute, ws.ScheduledStartTime.Second);
            DateTime endTime = new DateTime(date.Year, date.Month, date.Day, ws.ScheduledEndTime.Hour,
                ws.ScheduledEndTime.Minute, ws.ScheduledEndTime.Second);

            _wc.EditWalkingSession(ws.SessionID, date, startTime, endTime);

            Assert.AreEqual(_context.WalkingSessions.Find(2), ws);
        }

        [Test]
        public void DeleteWalkingSession_WhenCalled_RemovesSessionFromDB()
        {
            int sessionID = 1;

            var task = _wc.DeleteWalkingSession(sessionID);
            task.Wait();

            Assert.IsNull(_context.WalkingSessions.Find(sessionID));
        }

        // Does not pass, maybe need to change AddDogRating method to allow changes. Speak to Pulvi. JC
        [Test]
        public void AddDogRating_OldRating_SavesChangesToDB()
        {
            int walkerID = 5;
            int dogID = 1;
            double rating = 4;

            var task = _wc.AddDogRating(walkerID, dogID, rating);
            task.Wait();

            Assert.IsNotEmpty(_context.DogRatings.Where(dr => dr.DogID == dogID)
                                                 .Where(dr => dr.WalkerID == walkerID)
                                                 .Where(dr => dr.Rating == rating));
        }

        [Test]
        public void AddDogRating_NewRating_SavesRatingToDB()
        {
            int walkerID = 5;
            int dogID = 2;
            double rating = 3;

            var task = _wc.AddDogRating(walkerID, dogID, rating);
            task.Wait();

            Assert.IsNotEmpty(_context.DogRatings.Where(dr => dr.DogID == dogID)
                                                 .Where(dr => dr.WalkerID == walkerID));
        }

        // This test is wrong, JC to speak to DP.
        [Test]
        public void GetDogRating_WhenCalled_ReturnsAverageRating()
        {

            int walkerID = 5;
            int dogID = 2;
            double rating = 3;

            var task = _wc.AddDogRating(walkerID, dogID, rating);
            task.Wait();

            Assert.IsNotEmpty(_context.DogRatings.Where(dr => dr.DogID == dogID)
                                                 .Where(dr => dr.WalkerID == walkerID));


            Task<double> dogratings = _wc.GetDogRating(2);

            Assert.AreEqual(dogratings, 2);
        }


    }
}
