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

            context.SaveChanges();

            foreach (var login in context.Logins)
            {

                if (login.Email == "johnsmith@proton.me")
                {
                    var owner = new Owner();


                    owner.FirstName = "John";
                    owner.LastName = "Smith";
                    owner.Email = login.Email;
                    owner.StreetAddress = "12 Pine Rd";
                    owner.Suburb = suburbs[0];
                    owner.State = "Victoria";
                    owner.Country = "Australia";
                    owner.PhNumber = "0424 225 877";
                    login.User = owner;


                    context.Owners.Add(owner);
                    context.SaveChanges();
                }
                if (login.Email == "p.carrigan@gogo.com")
                {
                    var owner = new Owner();


                    owner.FirstName = "Peter";
                    owner.LastName = "Carrigan";
                    owner.Email = login.Email;
                    owner.StreetAddress = "26 Wills Ave";
                    owner.Suburb = suburbs[1];
                    owner.State = "Victoria";
                    owner.Country = "Australia";
                    owner.PhNumber = "0411 672 900";
                    login.User = owner;


                    context.Owners.Add(owner);
                    context.SaveChanges();
                }
                if (login.Email == "jwong45@me.com")
                {
                    var owner = new Owner();


                    owner.FirstName = "Judy";
                    owner.LastName = "Wong";
                    owner.Email = login.Email;
                    owner.StreetAddress = "42 Buckley St";
                    owner.Suburb = suburbs[2];
                    owner.State = "Victoria";
                    owner.Country = "Australia";
                    owner.PhNumber = "0456 853 345";
                    login.User = owner;


                    context.Owners.Add(owner);
                    context.SaveChanges();
                }
                if (login.Email == "kf8877@gmail.com")
                {
                    var walker = new Walker();

                    walker.FirstName = "Karen";
                    walker.LastName = "Fisher";
                    walker.Email = login.Email;
                    walker.StreetAddress = "2 Jacana St";
                    walker.Suburb = suburbs[3];
                    walker.State = "Victoria";
                    walker.Country = "Australia";
                    walker.PhNumber = "0488 044 222";
                    walker.IsInsured = true;
                    walker.ExperienceLevel = ExperienceLevel.Advanced;
                    login.User = walker;


                    context.Walkers.Add(walker);
                    context.SaveChanges();
                }
                if (login.Email == "mitchy555@gmail.com")
                {
                    var walker = new Walker();

                    walker.FirstName = "Mitchell";
                    walker.LastName = "Moses";
                    walker.Email = login.Email;
                    walker.StreetAddress = "10 Camden Rd";
                    walker.Suburb = suburbs[4];
                    walker.State = "Victoria";
                    walker.Country = "Australia";
                    walker.PhNumber = "0432 142 732";
                    walker.IsInsured = true;
                    walker.ExperienceLevel = ExperienceLevel.Intermediate;
                    login.User = walker;


                    context.Walkers.Add(walker);
                    context.SaveChanges();
                }
                if (login.Email == "jane.e@hotmail.com")
                {
                    var walker = new Walker();

                    walker.FirstName = "Jane";
                    walker.LastName = "Edgerton";
                    walker.Email = login.Email;
                    walker.StreetAddress = "14 Nirvana Ave";
                    walker.Suburb = suburbs[5];
                    walker.State = "Victoria";
                    walker.Country = "Australia";
                    walker.PhNumber = "0455 332 897";
                    walker.IsInsured = false;
                    walker.ExperienceLevel = ExperienceLevel.Beginner;
                    login.User = walker;


                    context.Walkers.Add(walker);
                    context.SaveChanges();
                }
                if (login.Email == "admin@easywalk.com")
                {
                    var admin = new Administrator();

                    admin.FirstName = "EasyWalk";
                    admin.LastName = "Administrator";
                    admin.Email = "admin@easywalk.com";
                    login.User = admin;

                    context.Administrators.Add(admin);
                    context.SaveChanges();
                }
                context.SaveChanges();
            }
            context.SaveChanges();
            Dog[] dogs = new Dog[10];

            foreach (var owner in context.Owners)
            {

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
            foreach (Dog d in dogs)
            {
                context.Dogs.Add(d);
                context.SaveChanges();
            }
            context.SaveChanges();
        }
    }
}
