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

            //// Look for any Owners.
            //if (context.Owners.Any())
            //    return; // DB has been seeded already.

            var owners = new List<Owner>();

            var owner1 = new Owner();
 //           var login1 = new Login();

            owner1.FirstName = "John";
            owner1.LastName = "Smith";
            owner1.Email = "johnsmith@proton.me";
            owner1.Address = "12 Pine Rd, Bayswater, VIC, 3153";
            owner1.PhNumber = "0424 225 877";
            //login1.User = owner;
            //login1.UserID = userID;
            //login1.LoginID = "12345678";
            //login1.PasswordHash = "Up6PDCVvwYAJB6r4eob7OBmxNi7X8qKnRp5uOf4bFRcgm/P4uo9lsGtaWbalPDNG";
            //login1.Locked = Locked.unlocked;
            //owner1.Login = login1;

            context.Owners.Add(owner1);

            var owner2 = new Owner();
   //         var login2 = new Login();

            owner2.FirstName = "Peter";
            owner2.LastName = "Carrigan";
            owner2.Email = "p.carrigan@gogo.com";
            owner2.Address = "26 Wills Ave, Mount Waverley, VIC, 3149";
            owner2.PhNumber = "0411 672 900";
            //login2.User = owner;
            //login2.UserID = userID;///remove once customer ID is sorted
            //login2.LoginID = "54632896";
            //login2.PasswordHash = "OSChybFH9bpx45dkxtL/xIRgchujLLW/xrO5m09AYRHlrG8wQHwYM8aEG5I9N6kj";
            //login2.Locked = Locked.unlocked;
            //owner2.Login = login2;

            context.Owners.Add(owner2);

            var owner3 = new Owner();

            owner3.FirstName = "Judy";
            owner3.LastName = "Wong";
            owner3.Email = "jwong45@me.com";
            owner3.Address = "42 Buckley St, Noble Park, VIC, 3174";
            owner3.PhNumber = "0456 853 345";
            //login.User = owner;
            //login.UserID = userID;///remove once customer ID is sorted
            //login.LoginID = "44665588";
            //login.PasswordHash = "+vNQb95E/n3R9+ocgKD7lh7x3FiJ9hrmeOs9RqQ0VK8W6wMvlX3SPqn6+Er3kUFN";
            //login.Locked = Locked.unlocked;

            context.Owners.Add(owner3);


            var walker1 = new Walker();

            walker1.FirstName = "Karen";
            walker1.LastName = "Fisher";
            walker1.Email = "kf8877@gmail.com";
            walker1.Address = "2 Jacana St, Chadstone, VIC, 3148";
            walker1.PhNumber = "0488 044 222";
            walker1.IsInsured = true;
            walker1.ExperienceLevel = ExperienceLevel.Advanced;
            //login.User = walker;
            //login.UserID = userID;///remove once customer ID is sorted
            //login.LoginID = "62547813";
            //login.PasswordHash = "0VpvMkBh9U527v321Wro+dJC0nY8HCYwiPFWp24WMSOdLW15krvKisfs6Ku5HzHt";
            //login.Locked = Locked.unlocked;

            context.Walkers.Add(walker1);


            var walker2 = new Walker();

            walker2.FirstName = "Mitchell";
            walker2.LastName = "Moses";
            walker2.Email = "mitchy555@gmail.com";
            walker2.Address = "10 Camden Rd, Hawthorn, VIC, 3122";
            walker2.PhNumber = "0432 142 732";
            walker2.IsInsured = true;
            walker2.ExperienceLevel = ExperienceLevel.Intermediate;
            //login.User = walker;
            //login.UserID = userID;///remove once customer ID is sorted
            //login.LoginID = "98765432";
            //login.PasswordHash = "ow/m3D+6bQTtuw8Pld486o9CzDDxFH1Gxy5XEmKXvswygE9ny0FPgNTnUokRxuI4";
            //login.Locked = Locked.unlocked;

            context.Walkers.Add(walker2);


            var walker3 = new Walker();

            walker3.FirstName = "Jane";
            walker3.LastName = "Edgerton";
            walker3.Email = "jane.e@hotmail.com";
            walker3.Address = "14 Nirvana Ave, Malvern East, VIC, 3145";
            walker3.PhNumber = "0455 332 897";
            walker3.IsInsured = false;
            walker3.ExperienceLevel = ExperienceLevel.Beginner;
            //login.User = walker;
            //login.LoginID = "55548692";
            //login.PasswordHash = "U3aiSza/zGtXt2AEuBwPSg4asyvXzErkOL4upGkbzo+RcoQ90c+E10n0Dy3HFviR";
            //login.Locked = Locked.unlocked;

            context.Walkers.Add(walker3);




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
