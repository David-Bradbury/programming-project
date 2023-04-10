using Microsoft.EntityFrameworkCore;
using ProgrammingProject.Models;

namespace ProgrammingProject.Data
{
    public class SeedData
    {

        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new EasyWalkContext(
                serviceProvider.GetRequiredService<DbContextOptions<EasyWalkContext>>());


            //  Look for any Owners.
            if (context.Owners.Any())
                return; // DB has been seeded already.


            // Seed Suburbs
            var suburb1 = new Suburb();

            suburb1.Postcode = "3153";
            suburb1.SuburbName = "Bayswater";

            context.Suburbs.Add(suburb1);


            var suburb2 = new Suburb();

            suburb2.Postcode = "3149";
            suburb2.SuburbName = "Mount Waverly";

            context.Suburbs.Add(suburb2);


            var suburb3 = new Suburb();

            suburb3.Postcode = "3174";
            suburb3.SuburbName = "Noble Park";

            context.Suburbs.Add(suburb3);


            var suburb4 = new Suburb();

            suburb4.Postcode = "3148";
            suburb4.SuburbName = "Chadstone";

            context.Suburbs.Add(suburb4);


            var suburb5 = new Suburb();

            suburb5.Postcode = "3122";
            suburb5.SuburbName = "Hawthorn";

            context.Suburbs.Add(suburb5);


            var suburb6 = new Suburb();

            suburb6.Postcode = "3145";
            suburb6.SuburbName = "Malvern East";

            context.Suburbs.Add(suburb6);


            // Seed Vets
            var vet1 = new Vet();

            vet1.BusinessName = "Bays Vets";
            vet1.PhNumber = "0402 201 201";
            vet1.Address = "3 Baywater Road";
            vet1.Email = "baysvets@gmail.com";

            context.Vets.Add(vet1);

            var vet2 = new Vet();

            vet2.BusinessName = "Pets of the Mount";
            vet2.PhNumber = "0440 404 040";
            vet2.Address = "21 Mountain Street";
            vet2.Email = "petsofthemount@hotmail.com";

            context.Vets.Add(vet2);

            var vet3 = new Vet();

            vet3.BusinessName = "Dr Schmackos";
            vet3.PhNumber = "1300 000 000";
            vet3.Address = "31 Schmackos Place";
            vet3.Email = "drschmackos@optus.net";

            context.Vets.Add(vet3);


            // Seed Logins
            var login1 = new Login();

            login1.Email = "johnsmith@proton.me";
            login1.PasswordHash = "Up6PDCVvwYAJB6r4eob7OBmxNi7X8qKnRp5uOf4bFRcgm/P4uo9lsGtaWbalPDNG";
            login1.Locked = Locked.unlocked;

            context.Logins.Add(login1);


            var login2 = new Login();

            login2.Email = "p.carrigan@gogo.com";
            login2.PasswordHash = "OSChybFH9bpx45dkxtL/xIRgchujLLW/xrO5m09AYRHlrG8wQHwYM8aEG5I9N6kj";
            login2.Locked = Locked.unlocked;

            context.Logins.Add(login2);


            var login3 = new Login();

            login3.Email = "jwong45@me.com";
            login3.PasswordHash = "+vNQb95E/n3R9+ocgKD7lh7x3FiJ9hrmeOs9RqQ0VK8W6wMvlX3SPqn6+Er3kUFN";
            login3.Locked = Locked.unlocked;

            context.Logins.Add(login3);


            var login4 = new Login();

            login4.Email = "kf8877@gmail.com";
            login4.PasswordHash = "0VpvMkBh9U527v321Wro+dJC0nY8HCYwiPFWp24WMSOdLW15krvKisfs6Ku5HzHt";
            login4.Locked = Locked.unlocked;

            context.Logins.Add(login4);


            var login5 = new Login();

            login5.Email = "mitchy555@gmail.com";
            login5.PasswordHash = "ow/m3D+6bQTtuw8Pld486o9CzDDxFH1Gxy5XEmKXvswygE9ny0FPgNTnUokRxuI4";
            login5.Locked = Locked.unlocked;

            context.Logins.Add(login5);


            var login6 = new Login();

            login6.Email = "jane.e@hotmail.com";
            login6.PasswordHash = "U3aiSza/zGtXt2AEuBwPSg4asyvXzErkOL4upGkbzo+RcoQ90c+E10n0Dy3HFviR";
            login6.Locked = Locked.unlocked;

            context.Logins.Add(login6);


            var login7 = new Login();

            login7.Email = "admin@easywalk.com";
            login7.PasswordHash = "IDjaWUHN76Ouz/3t9DKV/6t+YURpFHemb9JCo6B5Jfq/W1Iw5yo/fQd673znSE2K";
            login7.Locked = Locked.unlocked;

            context.Logins.Add(login7);


            // Seed Users
            var owner1 = new Owner();

            owner1.FirstName = "John";
            owner1.LastName = "Smith";
            owner1.Email = login1.Email;
            owner1.StreetAddress = "12 Pine Rd";
            owner1.Suburb = suburb1;
            owner1.State = "Victoria";
            owner1.Country = "Australia";
            owner1.PhNumber = "0424 225 877";
            login1.User = owner1;

            context.Owners.Add(owner1);


            var owner2 = new Owner();

            owner2.FirstName = "Peter";
            owner2.LastName = "Carrigan";
            owner2.Email = login2.Email;
            owner2.StreetAddress = "26 Wills Ave";
            owner2.Suburb = suburb2;
            owner2.State = "Victoria";
            owner2.Country = "Australia";
            owner2.PhNumber = "0411 672 900";
            login2.User = owner2;

            context.Owners.Add(owner2);


            var owner3 = new Owner();

            owner3.FirstName = "Judy";
            owner3.LastName = "Wong";
            owner3.Email = login3.Email;
            owner3.StreetAddress = "42 Buckley St";
            owner3.Suburb = suburb3;
            owner3.State = "Victoria";
            owner3.Country = "Australia";
            owner3.PhNumber = "0456 853 345";
            login3.User = owner3;

            context.Owners.Add(owner3);


            var walker1 = new Walker();

            walker1.FirstName = "Karen";
            walker1.LastName = "Fisher";
            walker1.Email = login4.Email;
            walker1.StreetAddress = "2 Jacana St";
            walker1.Suburb = suburb4;
            walker1.State = "Victoria";
            walker1.Country = "Australia";
            walker1.PhNumber = "0488 044 222";
            walker1.IsInsured = true;
            walker1.ExperienceLevel = ExperienceLevel.Advanced;
            login4.User = walker1;

            context.Walkers.Add(walker1);


            var walker2 = new Walker();

            walker2.FirstName = "Mitchell";
            walker2.LastName = "Moses";
            walker2.Email = login5.Email;
            walker2.StreetAddress = "10 Camden Rd";
            walker2.Suburb = suburb5;
            walker2.State = "Victoria";
            walker2.Country = "Australia";
            walker2.PhNumber = "0432 142 732";
            walker2.IsInsured = true;
            walker2.ExperienceLevel = ExperienceLevel.Intermediate;
            login5.User = walker2;

            context.Walkers.Add(walker2);


            var walker3 = new Walker();

            walker3.FirstName = "Jane";
            walker3.LastName = "Edgerton";
            walker3.Email = login6.Email;
            walker3.StreetAddress = "14 Nirvana Ave";
            walker3.Suburb = suburb6;
            walker3.State = "Victoria";
            walker3.Country = "Australia";
            walker3.PhNumber = "0455 332 897";
            walker3.IsInsured = false;
            walker3.ExperienceLevel = ExperienceLevel.Beginner;
            login6.User = walker3;

            context.Walkers.Add(walker3);


            var admin = new Administrator();

            admin.FirstName = "EasyWalk";
            admin.LastName = "Administrator";
            admin.Email = login7.Email;
            login7.User = admin;

            context.Administrators.Add(admin);


            // Seed Dogs
            var dog1 = new Dog();

            dog1.Name = "Max";
            dog1.Breed = "Golden Retriever";
            dog1.MicrochipNumber = "123456";
            dog1.IsVaccinated = true;
            dog1.Temperament = Temperament.Friendly;
            dog1.DogSize = DogSize.Large;
            dog1.TrainingLevel = TrainingLevel.Basic;
            dog1.Owner = owner1;
            dog1.Vet = vet1;

            context.Dogs.Add(dog1);


            var dog2 = new Dog();
   
            dog2.Name = "Bella";
            dog2.Breed = "Labrador";
            dog2.MicrochipNumber = "152655";
            dog2.IsVaccinated = true;
            dog2.Temperament = Temperament.Calm;
            dog2.DogSize = DogSize.Large;
            dog2.TrainingLevel = TrainingLevel.Fully;
            dog2.Owner = owner2;
            dog2.Vet = vet2;

            context.Dogs.Add(dog2);


            var dog3 = new Dog();
         
            dog3.Name = "Teddy";
            dog3.Breed = "Beagle";
            dog3.MicrochipNumber = "111111";
            dog3.IsVaccinated = true;
            dog3.Temperament = Temperament.Friendly;
            dog3.DogSize = DogSize.Small;
            dog3.TrainingLevel = TrainingLevel.None;
            dog3.Owner = owner3;
            dog3.Vet = vet3;

            context.Dogs.Add(dog3);


            var dog4 = new Dog();

            dog4.Name = "Ruby";
            dog4.Breed = "Beagle";
            dog4.MicrochipNumber = "111112";
            dog4.IsVaccinated = true;
            dog4.Temperament = Temperament.Friendly;
            dog4.DogSize = DogSize.Small;
            dog4.TrainingLevel = TrainingLevel.Basic;
            dog4.Owner = owner3;
            dog4.Vet = vet3;

            context.Dogs.Add(dog4);

            // seed walkingSessions

            var dogs = new List<Dog>();
            dogs.Add(dog4);
            dogs.Add(dog3);

            var walk = new WalkingSession();
            walk.StartTime = DateTime.Now;
            walk.EndTime = DateTime.Now.AddHours(1);
            walk.WalkerID = 4;
            walk.Walker = walker1;
            walk.DogList = dogs;

            var walk1 = new WalkingSession();
            walk1.StartTime = DateTime.Now.AddHours(2);
            walk1.EndTime = DateTime.Now.AddHours(3);
            walk1.WalkerID = 4;
            walk1.Walker = walker1;
            walk1.DogList = dogs;

            context.WalkingSessions.Add(walk1);

            context.SaveChanges();
        }
    }
}
