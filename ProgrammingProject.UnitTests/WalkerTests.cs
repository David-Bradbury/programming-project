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
        //private EasyWalkContext _context;

        [SetUp]
        public void SetUp()
        {
            //_context = new EasyWalkContext();


        }

        [Test]
        public void MatchDogsToWalker_WhenCalled_ReturnsListOfSuitableDogs()
        {
            var walker = new Walker();
            walker.ExperienceLevel = ExperienceLevel.Beginner;

            // Get list of dogs

            // Call method with Walker and List of Dogs

            // Add Assert to make sure correct implementation.

        }

        [Test]
        public async Task GetDogTraitScore_WhenCalled_ReturnsTheDogTraitScoreAsync()
        {
            var dog = new Dog();
            dog.DogSize = DogSize.Medium;
            dog.Temperament = Temperament.Calm;

            var wc = new WalkerController();

            var result = await wc.GetDogTraitScore(dog);

            Assert.That(result, Is.EqualTo(4));
        }
    }
}
