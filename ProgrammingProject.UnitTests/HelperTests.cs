using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using NUnit.Framework;
using ProgrammingProject.Data;
using ProgrammingProject.Helper;
using ProgrammingProject.Models;
using ProgrammingProject.Utilities;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;


namespace ProgrammingProject.UnitTests
{
    [TestFixture]
    internal class HelperTests
    {
        private EasyWalkContext _context;
        private IWebHostEnvironment _webHostEnvironment;

        [SetUp]
        public void SetUp()
        {
            var mt = new MasterTest();
            _context = mt.CreateContext();

        }

        [Test]
        public void CreateVet_NewVet_ReturnsVet()
        {
            // Arrange 
            var CreateHelper = new Create(_context, _webHostEnvironment);

            string businessName = "Vet";
            string phNumber = "0400 000 000";
            string email = "Vet@proton.me";
            string streetAddress = "13 test avenue";
            string country = "Australia";

            var suburb = _context.Suburbs.Where(s => s.SuburbName == "Barangaroo")
                                          .Where(s => s.Postcode == "2000")
                                          .Where(s => s.State == "NSW").FirstOrDefault();

            int vetID = 0;

            // Act
            var vet = CreateHelper.CreateVet(businessName, phNumber, email, streetAddress, country, suburb, vetID);

            // Assert        
            Assert.IsInstanceOf<Vet>(vet);
        }

        [Test]
        public void CreateVet_EditVet_ReturnsVetAndChangesNameLocally()
        {
            // Arrange 
            var CreateHelper = new Create(_context, _webHostEnvironment);

            int vetID = 3;
            var vet = _context.Vets.Find(vetID);

            string businessName = "Vetsss";
            string phNumber = vet.PhNumber;
            string email = vet.Email;
            string streetAddress = vet.StreetAddress;
            string country = vet.Country;

            var suburb = _context.Suburbs.Where(s => s.SuburbName == "Barangaroo")
                                          .Where(s => s.Postcode == "2000")
                                          .Where(s => s.State == "NSW").FirstOrDefault();

            // Act
            vet = CreateHelper.CreateVet(businessName, phNumber, email, streetAddress, country, suburb, vetID);

            // Assert        
            Assert.IsInstanceOf<Vet>(vet);
            Assert.That(vet.BusinessName, Is.EqualTo(businessName));
        }

        [Test]
        public void CreateDog_NewDog_SavesDogToDB()
        {
            // Arrange 
            var CreateHelper = new Create(_context, _webHostEnvironment);
            var vet = _context.Vets.Find(1);
            var owner = _context.Owners.Find(2);

            string name = "Doggo";
            string breed = "Beagle";
            string microchipNumber = "121 121 121 121 121";
            string temperament = "NonReactive";
            string dogSize = "Small";
            string trainingLevel = "Basic";
            IFormFile profileImage = null;

            int dogID = 0;

            // Act
            CreateHelper.CreateDog(name, breed, microchipNumber, temperament, dogSize, trainingLevel, profileImage, vet, owner, dogID);

            // Assert        
            Assert.IsNotEmpty(_context.Dogs.Where(d => d.Name == name));
        }

        [Test]
        public void CreateDog_EditDog_ChangesLocalDogName()
        {
            // Arrange 
            var CreateHelper = new Create(_context, _webHostEnvironment);

            var dog = _context.Dogs.Find(1);
            var vet = _context.Vets.Find(dog.Vet);
            var owner = _context.Owners.Find(dog.Owner);

            string name = "Maxine";
            string breed = "GOLDEN RETRIEVER";
            string microchipNumber = dog.MicrochipNumber;
            string temperament = dog.Temperament.ToString();
            string dogSize = dog.DogSize.ToString();
            string trainingLevel = dog.TrainingLevel.ToString();
            IFormFile profileImage = null;

            int dogID = dog.Id;

            // Act
            CreateHelper.CreateDog(name, breed, microchipNumber, temperament, dogSize, trainingLevel, profileImage, vet, owner, dogID);

            // Assert        
            Assert.That(dog.Name == name);
        }

        [Test]
        public void CreateDog_EditDog_ChangesLocalDogTemperament()
        {
            // Arrange 
            var CreateHelper = new Create(_context, _webHostEnvironment);
            var dog = _context.Dogs.Find(1);
            var vet = _context.Vets.Find(dog.Vet);
            var owner = _context.Owners.Find(dog.Owner);

            string name = dog.Name;
            string breed = "GOLDEN RETRIEVER";
            string microchipNumber = dog.MicrochipNumber;
            string temperament = "Reactive";
            string dogSize = dog.DogSize.ToString();
            string trainingLevel = dog.TrainingLevel.ToString();
            IFormFile profileImage = null;

            int dogID = dog.Id;

            // Act
            CreateHelper.CreateDog(name, breed, microchipNumber, temperament, dogSize, trainingLevel, profileImage, vet, owner, dogID);

            // Assert        
            Assert.AreEqual(dog.Temperament.ToString(), temperament);
        }

        [Test]
        public void CreateDog_EditDog_ChangesLocalDogSize()
        {
            // Arrange 
            var CreateHelper = new Create(_context, _webHostEnvironment);
            var dog = _context.Dogs.Find(1);
            var vet = _context.Vets.Find(dog.Vet);
            var owner = _context.Owners.Find(dog.Owner);

            string name = dog.Name;
            string breed = "GOLDEN RETRIEVER";
            string microchipNumber = dog.MicrochipNumber;
            string temperament = dog.Temperament.ToString();
            string dogSize = "Small";
            string trainingLevel = dog.TrainingLevel.ToString();
            IFormFile profileImage = null;

            int dogID = dog.Id;

            // Act
            CreateHelper.CreateDog(name, breed, microchipNumber, temperament, dogSize, trainingLevel, profileImage, vet, owner, dogID);

            // Assert        
            Assert.AreEqual(dog.DogSize.ToString(), dogSize);
        }

        [Test]
        public void CreateDog_EditDog_ChangesLocalTrainingLevel()
        {
            // Arrange 
            var CreateHelper = new Create(_context, _webHostEnvironment);
            var dog = _context.Dogs.Find(1);
            var vet = _context.Vets.Find(dog.Vet);
            var owner = _context.Owners.Find(dog.Owner);

            string name = dog.Name;
            string breed = "GOLDEN RETRIEVER";
            string microchipNumber = dog.MicrochipNumber;
            string temperament = dog.Temperament.ToString();
            string dogSize = dog.DogSize.ToString();
            string trainingLevel = "Fully";
            IFormFile profileImage = null;

            int dogID = dog.Id;

            // Act
            CreateHelper.CreateDog(name, breed, microchipNumber, temperament, dogSize, trainingLevel, profileImage, vet, owner, dogID);

            // Assert        
            Assert.AreEqual(dog.TrainingLevel.ToString(), trainingLevel);
        }

        [Test]
        public void CreateOwner_NewOwner_SavesOwnerToDB()
        {
            // Arrange 
            var CreateHelper = new Create(_context, _webHostEnvironment);

            int ownerID = 0;

            string firstName = "Jane";
            string lastName = "Doe";
            string email = "janedoefakeemail@email.com.au";
            string streetAddress = "1 fake road";
            string country = "Australia";
            string phNumber = "0400 000 000";
            IFormFile profileImage = null;

            var suburb = _context.Suburbs.Where(s => s.SuburbName == "Barangaroo")
                                          .Where(s => s.Postcode == "2000")
                                          .Where(s => s.State == "NSW").FirstOrDefault();
            string password = "aaa111";

            var login = new Login();

            login.Email = email;
            login.PasswordHash = ControllerHelper.HashPassword(password);
            login.Locked = Locked.locked;

            _context.Logins.Add(login);
            _context.SaveChanges();

            // Act
            CreateHelper.CreateOwner(firstName, lastName, email, streetAddress, country, phNumber, profileImage, suburb, ownerID);

            // Assert        
            Assert.IsNotEmpty(_context.Owners.Where(o => o.FirstName == firstName));
        }

        [Test]
        public void CreateOwner_EditOwner_SavesChangesLocally()
        {
            // Arrange 
            var CreateHelper = new Create(_context, _webHostEnvironment);
            int ownerID = 2;

            var owner = _context.Owners.Find(ownerID);

            string firstName = "Johnny";
            string lastName = owner.LastName;
            string email = owner.Email;
            string streetAddress = owner.StreetAddress;
            string country = owner.Country;
            string phNumber = owner.PhNumber;
            IFormFile profileImage = null;

            var suburb = _context.Suburbs.Where(s => s.SuburbName == "Barangaroo")
                                          .Where(s => s.Postcode == "2000")
                                          .Where(s => s.State == "NSW").FirstOrDefault();

            // Act
            CreateHelper.CreateOwner(firstName, lastName, email, streetAddress, country, phNumber, profileImage, suburb, ownerID);

            // Assert        
            Assert.That(owner.FirstName, Is.EqualTo(firstName));
        }

        [Test]
        public void CreateWalker_NewWalker_SaveWalkerToDB()
        {
            // Arrange 
            var CreateHelper = new Create(_context, _webHostEnvironment);

            int walkerID = 0;

            string firstName = "JohnnyBoy";
            string lastName = "Doe";
            string email = "johndoefakeemail@email.com.au";
            string streetAddress = "1 fake road";
            string country = "Australia";
            string phNumber = "0400 000 000";
            string insured = "false";
            string experienceLevel = "Beginner";
            IFormFile profileImage = null;

            var suburb = _context.Suburbs.Where(s => s.SuburbName == "Barangaroo")
                                          .Where(s => s.Postcode == "2000")
                                          .Where(s => s.State == "NSW").FirstOrDefault();

            string password = "aaa111";

            var login = new Login();

            login.Email = email;
            login.PasswordHash = ControllerHelper.HashPassword(password);
            login.Locked = Locked.locked;

            _context.Logins.Add(login);
            _context.SaveChanges();

            // Act
            CreateHelper.CreateWalker(firstName, lastName, email, streetAddress, country, phNumber, insured,
                experienceLevel, profileImage, suburb, walkerID);

            // Assert        
            Assert.IsNotEmpty(_context.Walkers.Where(w => w.FirstName == firstName));
        }

        [Test]
        public void CreateWalker_EditWalker_SavesChangesLocally()
        {
            // Arrange 
            var CreateHelper = new Create(_context, _webHostEnvironment);
            int walkerID = 7;

            var walker = _context.Walkers.Find(walkerID);

            string firstName = walker.FirstName;
            string lastName = "Mac";
            string email = walker.Email;
            string streetAddress = walker.StreetAddress;
            string country = walker.Country;
            string phNumber = walker.PhNumber;
            string insured = walker.IsInsured.ToString();
            string experienceLevel = walker.ExperienceLevel.ToString();
            IFormFile profileImage = null;

            var suburb = _context.Suburbs.Where(s => s.SuburbName == "Ascot")
                                          .Where(s => s.Postcode == "4007")
                                          .Where(s => s.State == "QLD").FirstOrDefault();

            // Act
            CreateHelper.CreateWalker(firstName, lastName, email, streetAddress, country, phNumber, insured,
               experienceLevel, profileImage, suburb, walkerID);

            // Assert        
            Assert.That(walker.LastName, Is.EqualTo(lastName));
        }

        [Test]
        public void CreateWalker_EditWalker_ChangesLocalInsuranceStatus()
        {
            // Arrange 
            var CreateHelper = new Create(_context, _webHostEnvironment);
            int walkerID = 7;

            var walker = _context.Walkers.Find(walkerID);

            string firstName = walker.FirstName;
            string lastName = walker.LastName;
            string email = walker.Email;
            string streetAddress = walker.StreetAddress;
            string country = walker.Country;
            string phNumber = walker.PhNumber;
            string insured = "Insured";
            string experienceLevel = walker.ExperienceLevel.ToString();
            IFormFile profileImage = null;

            var suburb = _context.Suburbs.Where(s => s.SuburbName == "Ascot")
                                          .Where(s => s.Postcode == "4007")
                                          .Where(s => s.State == "QLD").FirstOrDefault();

            // Act
            CreateHelper.CreateWalker(firstName, lastName, email, streetAddress, country, phNumber, insured,
               experienceLevel, profileImage, suburb, walkerID);

            // Assert        
            Assert.IsTrue(walker.IsInsured);
        }

        [Test]
        public void CreateWalker_EditWalker_ChangesLocalExperienceLevel()
        {
            // Arrange 
            var CreateHelper = new Create(_context, _webHostEnvironment);
            int walkerID = 7;

            var walker = _context.Walkers.Find(walkerID);

            string firstName = walker.FirstName;
            string lastName = walker.LastName;
            string email = walker.Email;
            string streetAddress = walker.StreetAddress;
            string country = walker.Country;
            string phNumber = walker.PhNumber;
            string insured = walker.IsInsured.ToString();
            string experienceLevel = "Expert";
            IFormFile profileImage = null;

            var suburb = _context.Suburbs.Where(s => s.SuburbName == "Ascot")
                                          .Where(s => s.Postcode == "4007")
                                          .Where(s => s.State == "QLD").FirstOrDefault();

            // Act
            CreateHelper.CreateWalker(firstName, lastName, email, streetAddress, country, phNumber, insured,
               experienceLevel, profileImage, suburb, walkerID);

            // Assert        
            Assert.That(walker.ExperienceLevel.ToString(), Is.EqualTo(experienceLevel));
        }

        [Test]
        public void GetStates_WhenCalled_ReturnsExpectedValues()
        {

            List<SelectListItem> testStates = new List<SelectListItem>();

            testStates.Add(new SelectListItem { Text = "South Australia", Value = "SA" });
            testStates.Add(new SelectListItem { Text = "Victoria", Value = "VIC" });
            testStates.Add(new SelectListItem { Text = "Western Australia", Value = "WA" });
            testStates.Add(new SelectListItem { Text = "Northern Territory", Value = "NT" });
            testStates.Add(new SelectListItem { Text = "New South Wales", Value = "NSW" });
            testStates.Add(new SelectListItem { Text = "Australian Capital Territory", Value = "ACT" });
            testStates.Add(new SelectListItem { Text = "Queensland", Value = "QLD" });
            testStates.Add(new SelectListItem { Text = "Tasmania", Value = "TAS" });

            var states = DropDownLists.GetStates();

            Assert.AreEqual(testStates.Count, states.Count);

            for (int i = 0; i < states.Count; i++)
                Assert.True(testStates[i].Value == states[i].Value && testStates[i].Text == states[i].Text);
        }

        [Test]
        public void GetInsuranceList_WhenCalled_ReturnsExpectedValues()
        {

            List<SelectListItem> testInsuranceList = new List<SelectListItem>();

            testInsuranceList.Add(new SelectListItem { Text = "Yes", Value = "Insured" });
            testInsuranceList.Add(new SelectListItem { Text = "No", Value = "Uninsured" });

            var InsuranceList = DropDownLists.GetInsuranceList();

            Assert.AreEqual(testInsuranceList.Count, InsuranceList.Count);

            for (int i = 0; i < InsuranceList.Count; i++)
                Assert.True(testInsuranceList[i].Value == InsuranceList[i].Value);
        }

        [Test]
        public void GetExperienceLevel_WhenCalled_ReturnsExpectedValues()
        {

            List<SelectListItem> testExperienceList = new List<SelectListItem>();

            testExperienceList.Add(new SelectListItem { Text = "B", Value = "Beginner" });
            testExperienceList.Add(new SelectListItem { Text = "I", Value = "Intermediate" });
            testExperienceList.Add(new SelectListItem { Text = "A", Value = "Advanced" });
            testExperienceList.Add(new SelectListItem { Text = "E", Value = "Expert" });

            var ExperienceList = DropDownLists.GetExperienceLevel();

            Assert.AreEqual(testExperienceList.Count, ExperienceList.Count);

            for (int i = 0; i < ExperienceList.Count; i++)
                Assert.True(testExperienceList[i].Value == ExperienceList[i].Value);
        }

        [Test]
        public void GetDogSize_WhenCalled_ReturnsExpectedValues()
        {

            List<SelectListItem> testDogSizeList = new List<SelectListItem>();

            testDogSizeList.Add(new SelectListItem { Text = "S", Value = "Small" });
            testDogSizeList.Add(new SelectListItem { Text = "M", Value = "Medium" });
            testDogSizeList.Add(new SelectListItem { Text = "L", Value = "Large" });
            testDogSizeList.Add(new SelectListItem { Text = "XL", Value = "ExtraLarge" });

            var DogSizeList = DropDownLists.GetDogSize();

            Assert.AreEqual(testDogSizeList.Count, DogSizeList.Count);

            for (int i = 0; i < DogSizeList.Count; i++)
                Assert.True(testDogSizeList[i].Value == DogSizeList[i].Value);
        }

        [Test]
        public void GetTemperament_WhenCalled_ReturnsExpectedValues()
        {

            List<SelectListItem> testTemperamentList = new List<SelectListItem>();

            testTemperamentList.Add(new SelectListItem { Text = "NR", Value = "NonReactive" });
            testTemperamentList.Add(new SelectListItem { Text = "C", Value = "Calm" });
            testTemperamentList.Add(new SelectListItem { Text = "F", Value = "Friendly" });
            testTemperamentList.Add(new SelectListItem { Text = "R", Value = "Reactive" });
            testTemperamentList.Add(new SelectListItem { Text = "A", Value = "Agressive" });

            var TemperamentList = DropDownLists.GetTemperament();

            Assert.AreEqual(testTemperamentList.Count, TemperamentList.Count);

            for (int i = 0; i < TemperamentList.Count; i++)
                Assert.True(testTemperamentList[i].Value == TemperamentList[i].Value);
        }

        [Test]
        public void GetTrainingLevel_WhenCalled_ReturnsExpectedValues()
        {

            List<SelectListItem> testTrainingLevel = new List<SelectListItem>();

            testTrainingLevel.Add(new SelectListItem { Text = "N", Value = "None" });
            testTrainingLevel.Add(new SelectListItem { Text = "B", Value = "Basic" });
            testTrainingLevel.Add(new SelectListItem { Text = "F", Value = "Fully" });

            var TrainingLevel = DropDownLists.GetTrainingLevel();

            Assert.AreEqual(testTrainingLevel.Count, TrainingLevel.Count);

            for (int i = 0; i < TrainingLevel.Count; i++)
                Assert.True(testTrainingLevel[i].Value == TrainingLevel[i].Value);
        }

        [Test]
        public void GetVaccinatedList_WhenCalled_ReturnsExpectedValues()
        {

            List<SelectListItem> testVaccinatedList = new List<SelectListItem>();

            testVaccinatedList.Add(new SelectListItem { Text = "Y", Value = "Vaccinated" });
            testVaccinatedList.Add(new SelectListItem { Text = "N", Value = "Unvaccinated" });

            var VaccinatedList = DropDownLists.GetVaccinatedList();

            Assert.AreEqual(testVaccinatedList.Count, VaccinatedList.Count);

            for (int i = 0; i < VaccinatedList.Count; i++)
                Assert.True(testVaccinatedList[i].Value == VaccinatedList[i].Value);
        }

        // Can't properly mock wwwroot at this time. Manually Tested This Method.
        [Test]
        public void UploadFile_WhenCalled_ReturnsFileNameString()
        {
            var imageHelper = new ImageHelper(_webHostEnvironment);

            var content = "Hello World from a Fake File";
            var fileName = "test.png";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;

            //create FormFile with desired data
            IFormFile file = new FormFile(stream, 0, stream.Length, "id_from_form", fileName);

            string filestring = imageHelper.UploadFile(file);

            Assert.That(!filestring.IsNullOrEmpty());

        }
    }
}
