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

            Suburb[] suburbs = new Suburb[10];

            suburbs[0] = new Suburb();
            suburbs[0].Postcode = "3153";
            suburbs[0].SuburbName = "Bayswater";

            suburbs[1] = new Suburb();
            suburbs[1].Postcode = "3149";
            suburbs[1].SuburbName = "Mount Waverly";

            suburbs[2] = new Suburb();
            suburbs[2].Postcode = "3174";
            suburbs[2].SuburbName = "Noble Park";

            suburbs[3] = new Suburb();
            suburbs[3].Postcode = "3148";
            suburbs[3].SuburbName = "Chadstone";

            suburbs[4] = new Suburb();
            suburbs[4].Postcode = "3122";
            suburbs[4].SuburbName = "Hawthorn";

            suburbs[5] = new Suburb();
            suburbs[5].Postcode = "3145";
            suburbs[5].SuburbName = "Malvern East";

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

            var owner1 = new Owner();


            owner1.FirstName = "John";
            owner1.LastName = "Smith";
            owner1.Email = "johnsmith@proton.me";
            owner1.StreetAddress = "12 Pine Rd";
            owner1.Suburb = suburbs[0];
            owner1.State = "Victoria";
            owner1.Country = "Australia";
            owner1.PhNumber = "0424 225 877";


            context.Owners.Add(owner1);

            var owner2 = new Owner();


            owner2.FirstName = "Peter";
            owner2.LastName = "Carrigan";
            owner2.Email = "p.carrigan@gogo.com";
            owner2.StreetAddress = "26 Wills Ave";
            owner2.Suburb = suburbs[1];
            owner2.State = "Victoria";
            owner2.Country = "Australia";
            owner2.PhNumber = "0411 672 900";


            context.Owners.Add(owner2);

            var owner3 = new Owner();

            owner3.FirstName = "Judy";
            owner3.LastName = "Wong";
            owner3.Email = "jwong45@me.com";
            owner3.StreetAddress = "42 Buckley St";
            owner3.Suburb = suburbs[2];
            owner3.State = "Victoria";
            owner3.Country = "Australia";
            owner3.PhNumber = "0456 853 345";


            context.Owners.Add(owner3);


            var walker1 = new Walker();

            walker1.FirstName = "Karen";
            walker1.LastName = "Fisher";
            walker1.Email = "kf8877@gmail.com";
            walker1.StreetAddress = "2 Jacana St";
            walker1.Suburb = suburbs[3];
            walker1.State = "Victoria";
            walker1.Country = "Australia";
            walker1.PhNumber = "0488 044 222";
            walker1.IsInsured = true;
            walker1.ExperienceLevel = ExperienceLevel.Advanced;



            context.Walkers.Add(walker1);


            var walker2 = new Walker();

            walker2.FirstName = "Mitchell";
            walker2.LastName = "Moses";
            walker2.Email = "mitchy555@gmail.com";
            walker2.StreetAddress = "10 Camden Rd";
            walker2.Suburb = suburbs[4];
            walker2.State = "Victoria";
            walker2.Country = "Australia";
            walker2.PhNumber = "0432 142 732";
            walker2.IsInsured = true;
            walker2.ExperienceLevel = ExperienceLevel.Intermediate;


            context.Walkers.Add(walker2);


            var walker3 = new Walker();

            walker3.FirstName = "Jane";
            walker3.LastName = "Edgerton";
            walker3.Email = "jane.e@hotmail.com";
            walker3.StreetAddress = "14 Nirvana Ave";
            walker3.Suburb = suburbs[5];
            walker3.State = "Victoria";
            walker3.Country = "Australia";
            walker3.PhNumber = "0455 332 897";
            walker3.IsInsured = false;
            walker3.ExperienceLevel = ExperienceLevel.Beginner;


            context.Walkers.Add(walker3);

            context.SaveChanges();


            var admin = new Administrator();

            admin.FirstName = "EasyWalk";
            admin.LastName = "Administrator";
            admin.Email = "admin@easywalk.com";
            context.Administrators.Add(admin);

            context.SaveChanges();

            foreach (var owner in context.Owners)
            {
                if (owner.UserId == 1)
                {
                    var login = new Login();
                    login.User = owner;
                    login.UserId = owner.UserId;
                    login.LoginID = "12345678";
                    login.PasswordHash = "Up6PDCVvwYAJB6r4eob7OBmxNi7X8qKnRp5uOf4bFRcgm/P4uo9lsGtaWbalPDNG";
                    login.Locked = Locked.unlocked;
                    owner.Login = login;
                    context.Logins.Add(login);

                }
                if (owner.UserId == 2)
                {
                    var login = new Login();
                    login.User = owner;
                    login.UserId = owner.UserId;
                    login.LoginID = "54632896";
                    login.PasswordHash = "OSChybFH9bpx45dkxtL/xIRgchujLLW/xrO5m09AYRHlrG8wQHwYM8aEG5I9N6kj";
                    login.Locked = Locked.unlocked;
                    owner.Login = login;
                    context.Logins.Add(login);

                }
                if (owner.UserId == 3)
                {
                    var login = new Login();
                    login.User = owner;
                    login.UserId = owner.UserId;
                    login.LoginID = "44665588";
                    login.PasswordHash = "+vNQb95E/n3R9+ocgKD7lh7x3FiJ9hrmeOs9RqQ0VK8W6wMvlX3SPqn6+Er3kUFN";
                    login.Locked = Locked.unlocked;
                    owner.Login = login;
                    context.Logins.Add(login);

                }

            }
            context.SaveChanges();

            foreach (var walker in context.Walkers)
            {
                if (walker.UserId == 4)
                {
                    var login = new Login();
                    login.User = walker;
                    login.UserId = walker.UserId;
                    login.LoginID = "62547813";
                    login.PasswordHash = "0VpvMkBh9U527v321Wro+dJC0nY8HCYwiPFWp24WMSOdLW15krvKisfs6Ku5HzHt";
                    login.Locked = Locked.unlocked;
                    walker.Login = login;
                    context.Logins.Add(login);

                }
                if (walker.UserId == 5)
                {
                    var login = new Login();
                    login.User = walker;
                    login.UserId = walker.UserId;
                    login.LoginID = "98765432";
                    login.PasswordHash = "ow/m3D+6bQTtuw8Pld486o9CzDDxFH1Gxy5XEmKXvswygE9ny0FPgNTnUokRxuI4";
                    login.Locked = Locked.unlocked;

                    context.Logins.Add(login);

                }

                if (walker.UserId == 6)
                {
                    var login = new Login();
                    login.User = walker;
                    login.UserId = walker.UserId;
                    login.LoginID = "55548692";
                    login.PasswordHash = "U3aiSza/zGtXt2AEuBwPSg4asyvXzErkOL4upGkbzo+RcoQ90c+E10n0Dy3HFviR";
                    login.Locked = Locked.unlocked;

                    context.Logins.Add(login);

                }

                context.SaveChanges();
            }

            foreach (var a in context.Administrators)
            {
                if (admin.UserId == 7)
                {
                    var login = new Login();
                    login.User = a;
                    login.UserId = a.UserId;
                    login.LoginID = "09876543";
                    login.PasswordHash = "IDjaWUHN76Ouz/3t9DKV/6t+YURpFHemb9JCo6B5Jfq/W1Iw5yo/fQd673znSE2K";
                    login.Locked = Locked.unlocked;
                    admin.Login = login;
                    context.Logins.Add(login);

                }
            }
            context.SaveChanges();

            Dog[] dogs = new Dog[10];

            foreach (var owner in context.Owners) {

                if (owner.UserId == 1)
                {
                    dogs[0] = new Dog();
                    dogs[0].Name = "Max";
                    dogs[0].Breed = "Golden Retriever";
                    dogs[0].MicrochipNumber = "123456";
                    dogs[0].IsVaccinated = true;
                    dogs[0].Temperament = Temperament.Friendly;
                    dogs[0].DogSize = DogSize.Large;
                    dogs[0].TrainingLevel = TrainingLevel.Basic;
                    dogs[0].Owner = owner;
                    dogs[0].Vet = vet1;
                }
                if (owner.UserId == 2)
                {
                    dogs[1] = new Dog();
                    dogs[1].Name = "Bella";
                    dogs[1].Breed = "Labrador";
                    dogs[1].MicrochipNumber = "152655";
                    dogs[1].IsVaccinated = true;
                    dogs[1].Temperament = Temperament.Calm;
                    dogs[1].DogSize = DogSize.Large;
                    dogs[1].TrainingLevel = TrainingLevel.Fully;
                    dogs[1].Owner = owner;
                    dogs[1].Vet = vet2;
                }
                if (owner.UserId == 3) 
                    {
                    dogs[2] = new Dog();
                    dogs[2].Name = "Teddy";
                    dogs[2].Breed = "Beagle";
                    dogs[2].MicrochipNumber = "111111";
                    dogs[2].IsVaccinated = true;
                    dogs[2].Temperament = Temperament.Friendly;
                    dogs[2].DogSize = DogSize.Small;
                    dogs[2].TrainingLevel = TrainingLevel.None;
                    dogs[2].Owner = owner;
                    dogs[2].Vet = vet3;

                    dogs[3] = new Dog();
                    dogs[3].Name = "Ruby";
                    dogs[3].Breed = "Beagle";
                    dogs[3].MicrochipNumber = "111112";
                    dogs[3].IsVaccinated = true;
                    dogs[3].Temperament = Temperament.Friendly;
                    dogs[3].DogSize = DogSize.Small;
                    dogs[3].TrainingLevel = TrainingLevel.Basic;
                    dogs[3].Owner = owner;
                    dogs[3].Vet = vet3;
                }
                }
            foreach (Dog d in dogs) {
                context.Dogs.Add(d);
                context.SaveChanges();
            }
            context.SaveChanges();
        }
    }
}
