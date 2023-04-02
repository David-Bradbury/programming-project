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
            //_context = MasterTest.CreateContext();
            

        }

        [Test]
        public void MatchDogsToWalker_WhenCalled_ReturnsListOfSuitableDogs()
        {
            var walker = new Walker();
            walker.UserId = 4;

            Task<List<Dog>> task = _wc.MatchDogsToWalker((int)walker.UserId);
            //var result = await _wc.MatchDogsToWalker(walker.UserId);

            task.Wait();

            //List<Dog> result = task.Result;
            var result = task.Result;

            //Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.Count, Is.EqualTo(2));
            
        }

        [Test]
        public void GetListOfSuitableDogsToWalkers_WhenProvidedWalkerID_ReturnsListOfDogs()
        {
            var walker = new Walker();
            walker.ExperienceLevel = ExperienceLevel.Beginner;

            IEnumerable<Dog> dogs = _context.Dogs.AsEnumerable();
            // Get list of dogs

            //IEnumerable<Dog> dogs = GetDogs();

            Assert.That(dogs.Count, Is.EqualTo(2));

        }

        [Test]
        public async Task GetDogTraitScore_WhenCalled_ReturnsTheDogTraitScoreAsync()
        {
            var dog = new Dog();
            dog.DogSize = DogSize.Medium;
            dog.Temperament = Temperament.Calm;

            var result = await _wc.GetDogTraitScore(dog);

            Assert.That(result, Is.EqualTo(4));
        }
    }
}
