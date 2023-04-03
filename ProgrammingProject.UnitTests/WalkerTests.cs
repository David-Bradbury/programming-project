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
            var walker = new Walker();
            walker.UserId = 6;

            Task<List<Dog>> task = _wc.MatchDogsToWalker((int)walker.UserId);
            task.Wait();

            var result = _wc.FilterDogs(task.Result);

            // Can improve once additional filters are provided.
            Assert.That(result.Count, Is.LessThanOrEqualTo(task.Result.Count));
        }

        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        [Test]
        public void MatchDogsToWalker_WhenCalled_ReturnsListOfSuitableDogs(int a)
        {
            var walker = new Walker();
            walker.UserId = a;

            Task<List<Dog>> task = _wc.MatchDogsToWalker((int)walker.UserId);
            task.Wait();

            var result = task.Result;

            // Can improve on this ensure exact amount is produced.
            Assert.That(result.Count, Is.GreaterThanOrEqualTo(1));
            
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
        [TestCase(DogSize.Small,Temperament.Reactive, 3)]
        [TestCase(DogSize.Medium,Temperament.Calm, 2)]
        [TestCase(DogSize.Large,Temperament.NonReactive, 2)]
        public async Task GetDogTraitScore_WhenCalled_ReturnsTheDogTraitScoreAsync(DogSize a, Temperament b, int expectedResult)
        {
            var dog = new Dog();
            dog.DogSize = a;
            dog.Temperament = b;

            var result = await _wc.GetDogTraitScore(dog);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

    }
}
