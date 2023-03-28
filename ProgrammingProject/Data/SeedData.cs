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

            // Look for any Owners.
            // if (context.Owners.Any())
            //   return; // DB has been seeded already.

            
            Suburb[] suburb = new Suburb[100];
           
            suburb[0] = new Suburb();
            suburb[0].Postcode = "33333";
            suburb[0].SuburbName = "XXXXX";

            suburb[1] = new Suburb();
            suburb[1].Postcode = "44444";
            suburb[1].SuburbName = "UUUUUUUU";

           foreach (Suburb s in suburb)
            {
                context.Suburbs.Add(s);
                context.SaveChanges();
            }
           // context.SaveChanges();

            /*
           suburb.Postcode = "3153";
           suburb.SuburbName = "Bayswater";

           context.Suburbs.Add(suburb);
           context.SaveChanges();
           */

            // User ID is just set here as need to work out whether we database generate this or not. 
            //Need to look into how to do this while seeding users i.e. retrieve the user ID to set in the Login.UserID.
            // Dave B
            /*
            var owner = new Owner();
            var login = new Login();

            owner.FirstName = "John";
            owner.LastName = "Smith";
            owner.Email = "johnsmith@proton.me";
            owner.StreetAddress = "12 Pine Rd";
            owner.State = "Victoria";
            owner.Country = "Australia";
            owner.PhNumber = "0424 225 877";
            owner.Suburb = suburb;
            login.User = owner;
            //login.UserID = userID;///remove once customer ID is sorted
            login.LoginID = "12345678";
            login.PasswordHash = "Up6PDCVvwYAJB6r4eob7OBmxNi7X8qKnRp5uOf4bFRcgm/P4uo9lsGtaWbalPDNG";
            login.Locked = Locked.unlocked;

            context.Owners.Add(owner);
            context.Logins.Add(login);
                         ///remove once customer ID is sorted
/*
            owner.FirstName = "Peter";
            owner.LastName = "Carrigan";
            owner.Email = "p.carrigan@gogo.com";
            owner.Address = "26 Wills Ave, Mount Waverley, VIC, 3149";
            owner.PhNumber = "0411 672 900";
            login.User = owner;
            login.UserID = userID;///remove once customer ID is sorted
            login.LoginID = "54632896";
            login.PasswordHash = "OSChybFH9bpx45dkxtL/xIRgchujLLW/xrO5m09AYRHlrG8wQHwYM8aEG5I9N6kj";
            login.Locked = Locked.unlocked;

            context.Owners.Add(owner);
                       ///remove once customer ID is sorted

            owner.FirstName = "Judy";
            owner.LastName = "Wong";
            owner.Email = "jwong45@me.com";
            owner.Address = "42 Buckley St, Noble Park, VIC, 3174";
            owner.PhNumber = "0456 853 345";
            login.User = owner;
            login.UserID = userID;///remove once customer ID is sorted
            login.LoginID = "44665588";
            login.PasswordHash = "+vNQb95E/n3R9+ocgKD7lh7x3FiJ9hrmeOs9RqQ0VK8W6wMvlX3SPqn6+Er3kUFN";
            login.Locked = Locked.unlocked;

            context.Owners.Add(owner);
                     ///remove once customer ID is sorted

            var walker = new Walker();

            walker.FirstName = "Karen";
            walker.LastName = "Fisher";
            walker.Email = "kf8877@gmail.com";
            walker.Address = "2 Jacana St, Chadstone, VIC, 3148";
            walker.PhNumber = "0488 044 222";
            login.User = walker;
            login.UserID = userID;///remove once customer ID is sorted
            login.LoginID = "62547813";
            login.PasswordHash = "+vNQb95E/n3R9+ocgKD7lh7x3FiJ9hrmeOs9RqQ0VK8W6wMvlX3SPqn6+Er3kUFN";
            login.Locked = Locked.unlocked;

            context.Walkers.Add(walker);
                     ///remove once customer ID is sorted

            walker.FirstName = "Karen";
            walker.LastName = "Fisher";
            walker.Email = "kf8877@gmail.com";
            walker.Address = "2 Jacana St, Chadstone, VIC, 3148";
            walker.PhNumber = "0488 044 222";
            walker.IsInsured = true;
            walker.ExperienceLevel = ExperienceLevel.Advanced;
            login.User = walker;
            login.UserID = userID;///remove once customer ID is sorted
            login.LoginID = "62547813";
            login.PasswordHash = "0VpvMkBh9U527v321Wro+dJC0nY8HCYwiPFWp24WMSOdLW15krvKisfs6Ku5HzHt";
            login.Locked = Locked.unlocked;

            context.Walkers.Add(walker);
                      ///remove once customer ID is sorted

            walker.FirstName = "Mitchell";
            walker.LastName = "Moses";
            walker.Email = "mitchy555@gmail.com";
            walker.Address = "10 Camden Rd, Hawthorn, VIC, 3122";
            walker.PhNumber = "0432 142 732";
            walker.IsInsured = true;
            walker.ExperienceLevel = ExperienceLevel.Intermediate;
            login.User = walker;
            login.UserID = userID;///remove once customer ID is sorted
            login.LoginID = "98765432";
            login.PasswordHash = "ow/m3D+6bQTtuw8Pld486o9CzDDxFH1Gxy5XEmKXvswygE9ny0FPgNTnUokRxuI4";
            login.Locked = Locked.unlocked;

            context.Walkers.Add(walker);
                     ///remove once customer ID is sorted

            walker.FirstName = "Jane";
            walker.LastName = "Edgerton";
            walker.Email = "jane.e@hotmail.com";
            walker.Address = "14 Nirvana Ave, Malvern East, VIC, 3145";
            walker.PhNumber = "0455 332 897";
            walker.IsInsured = false;
            walker.ExperienceLevel = ExperienceLevel.Beginner;
            login.User = walker;
            login.UserID = userID;///remove once customer ID is sorted
            login.LoginID = "55548692";
            login.PasswordHash = "U3aiSza/zGtXt2AEuBwPSg4asyvXzErkOL4upGkbzo+RcoQ90c+E10n0Dy3HFviR";
            login.Locked = Locked.unlocked;

            context.Walkers.Add(walker);
                   ///remove once customer ID is sorted



            /*   context.Walkers.AddRange(
                   new Walker
                   {
                       FirstName = "Karen",
                       LastName = "Fisher",
                       HashedPassword = "0VpvMkBh9U527v321Wro+dJC0nY8HCYwiPFWp24WMSOdLW15krvKisfs6Ku5HzHt",
                       Email = "kf8877@gmail.com",
                       Address = "2 Jacana St, Chadstone, VIC, 3148",
                       PhNumber = "0488 044 222",
                       IsInsured = true,
                       ExperienceLevel = ExperienceLevel.Advanced
                   },
                   new Walker
                   {
                       FirstName = "Mitchell",
                       LastName = "Moses",
                       HashedPassword = "ow/m3D+6bQTtuw8Pld486o9CzDDxFH1Gxy5XEmKXvswygE9ny0FPgNTnUokRxuI4",
                       Email = "mitchy555@gmail.com",
                       Address = "10 Camden Rd, Hawthorn, VIC, 3122",
                       PhNumber = "0432 142 732",
                       IsInsured = true,
                       ExperienceLevel = ExperienceLevel.Intermediate
                   },
                   new Walker
                   {
                       FirstName = "Jane",
                       LastName = "Edgerton",
                       HashedPassword = "U3aiSza/zGtXt2AEuBwPSg4asyvXzErkOL4upGkbzo+RcoQ90c+E10n0Dy3HFviR",
                       Email = "jane.e@hotmail.com",
                       Address = "14 Nirvana Ave, Malvern East, VIC, 3145",
                       PhNumber = "0455 332 897",
                       IsInsured = false,
                       ExperienceLevel = ExperienceLevel.Beginner
                   }
                   );


                           context.Owners.AddRange(
                   new Owner
                   {
                       FirstName = "John",
                       LastName = "Smith",
                       HashedPassword = "Up6PDCVvwYAJB6r4eob7OBmxNi7X8qKnRp5uOf4bFRcgm/P4uo9lsGtaWbalPDNG",
                       Email = "p.carrigan@gogo.com",
                       Address = "12 Pine Rd, Bayswater, VIC, 3153",
                       PhNumber = "0424 225 877",

                   },
                   new Owner
                   {
                       FirstName = "Peter",
                       LastName = "Carrigan",
                       HashedPassword = "OSChybFH9bpx45dkxtL/xIRgchujLLW/xrO5m09AYRHlrG8wQHwYM8aEG5I9N6kj",
                       Email = "johnsmith@proton.me",
                       Address = "26 Wills Ave, Mount Waverley, VIC, 3149",
                       PhNumber = "0411 672 900"
                   },
                   new Owner
                   {
                       FirstName = "Judy",
                       LastName = "Wong",
                       HashedPassword = "+vNQb95E/n3R9+ocgKD7lh7x3FiJ9hrmeOs9RqQ0VK8W6wMvlX3SPqn6+Er3kUFN",
                       Email = "jwong45@me.com",
                       Address = "42 Buckley St, Noble Park, VIC, 3174",
                       PhNumber = "0456 853 345"
                   }
               );
            */
            context.SaveChanges();
        }
    }
}
