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
    internal class WalkerTests : MasterTest
    {
        private EasyWalkContext _context;
        private WalkerController _wc;
        //private const string ConnectionString_ = ;

        [SetUp]
        public void SetUp()
        {
            //using var context = new EasyWalkContext(
                //serviceProvider.GetRequiredService<DbContextOptions<EasyWalkContext>>());
            //_context = new EasyWalkContext("Server=(localdb)\\MSSQLLocalDB;Database=TestLocalEasyWalk;Trusted_Connection=True;MultipleActiveResultSets=true");
            _wc = new WalkerController();

        }

        [Test]
        public async void MatchDogsToWalker_WhenCalled_ReturnsListOfSuitableDogs()
        {
            var walker = new Walker();
            walker.UserId = 4;
            
            await _wc.MatchDogsToWalker(walker.UserId);       

            // Add Assert to make sure correct implementation.

        }

        [Test]
        public void GetListOfSuitableDogsToWalkers_WhenProvidedWalkerID_ReturnsListOfDogs()
        {
            var walker = new Walker();
            walker.ExperienceLevel = ExperienceLevel.Beginner;


            // Get list of dogs

        }

        [Test]
        public async Task GetDogTraitScore_WhenCalled_ReturnsTheDogTraitScoreAsync()
        {
            var dog = new Dog();
            dog.DogSize = DogSize.Medium;
            dog.Temperament = Temperament.Calm;

            var result = await _wc.GetDogTraitScore(dog);

            Assert.That(result, Is.EqualTo(4));
            //Assert.That(result, Is.EqualTo(3));
        }
    }
}
