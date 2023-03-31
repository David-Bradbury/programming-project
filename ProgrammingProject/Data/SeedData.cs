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
            //if (context.Owners.Any())
            //    return; // DB has been seeded already.

            //Suburb[] suburbs = new Suburb[10];

            //suburbs[0] = new Suburb();
            //suburbs[0].Postcode = "3153";
            //suburbs[0].SuburbName = "Bayswater";

            //suburbs[1] = new Suburb();
            //suburbs[1].Postcode = "3149";
            //suburbs[1].SuburbName = "Mount Waverly";

            //suburbs[2] = new Suburb();
            //suburbs[2].Postcode = "3174";
            //suburbs[2].SuburbName = "Noble Park";

            //suburbs[3] = new Suburb();
            //suburbs[3].Postcode = "3148";
            //suburbs[3].SuburbName = "Chadstone";

            //suburbs[4] = new Suburb();
            //suburbs[4].Postcode = "3122";
            //suburbs[4].SuburbName = "Hawthorn";

            //suburbs[5] = new Suburb();
            //suburbs[5].Postcode = "3145";
            //suburbs[5].SuburbName = "Malvern East";

            

           

            //var owner1 = new Owner();
            ////           var login1 = new Login();

            //owner1.FirstName = "John";
            //owner1.LastName = "Smith";
            //owner1.Email = "johnsmith@proton.me";
            //owner1.StreetAddress = "12 Pine Rd";
            //owner1.Suburb = suburbs[0];
            //owner1.State = "Victoria";
            //owner1.Country = "Australia";
            //owner1.PhNumber = "0424 225 877";
            ////login1.User = owner;
            ////login1.UserID = userID;
            ////login1.LoginID = "12345678";
            ////login1.PasswordHash = "Up6PDCVvwYAJB6r4eob7OBmxNi7X8qKnRp5uOf4bFRcgm/P4uo9lsGtaWbalPDNG";
            ////login1.Locked = Locked.unlocked;
            ////owner1.Login = login1;

            //context.Owners.Add(owner1);

            //var owner2 = new Owner();
            ////         var login2 = new Login();

            //owner2.FirstName = "Peter";
            //owner2.LastName = "Carrigan";
            //owner2.Email = "p.carrigan@gogo.com";
            //owner2.StreetAddress = "26 Wills Ave";
            //owner2.Suburb = suburbs[1];
            //owner2.State = "Victoria";
            //owner2.Country = "Australia";
            //owner2.PhNumber = "0411 672 900";
            ////login2.User = owner;
            ////login2.UserID = userID;///remove once customer ID is sorted
            ////login2.LoginID = "54632896";
            ////login2.PasswordHash = "OSChybFH9bpx45dkxtL/xIRgchujLLW/xrO5m09AYRHlrG8wQHwYM8aEG5I9N6kj";
            ////login2.Locked = Locked.unlocked;
            ////owner2.Login = login2;

            //context.Owners.Add(owner2);

            //var owner3 = new Owner();

            //owner3.FirstName = "Judy";
            //owner3.LastName = "Wong";
            //owner3.Email = "jwong45@me.com";
            //owner3.StreetAddress = "42 Buckley St";
            //owner3.Suburb = suburbs[2];
            //owner3.State = "Victoria";
            //owner3.Country = "Australia";
            //owner3.PhNumber = "0456 853 345";
            ////login3.User = owner;
            ////login3.UserID = userID;///remove once customer ID is sorted
            ////login3.LoginID = "44665588";
            ////login3.PasswordHash = "+vNQb95E/n3R9+ocgKD7lh7x3FiJ9hrmeOs9RqQ0VK8W6wMvlX3SPqn6+Er3kUFN";
            ////login3.Locked = Locked.unlocked;

            //context.Owners.Add(owner3);


            //var walker1 = new Walker();

            //walker1.FirstName = "Karen";
            //walker1.LastName = "Fisher";
            //walker1.Email = "kf8877@gmail.com";
            //walker1.StreetAddress = "2 Jacana St";
            //walker1.Suburb = suburbs[3];
            //walker1.State = "Victoria";
            //walker1.Country = "Australia";
            //walker1.PhNumber = "0488 044 222";
            //walker1.IsInsured = true;
            //walker1.ExperienceLevel = ExperienceLevel.Advanced;
            ////login4.User = walker;
            ////login4.UserID = userID;///remove once customer ID is sorted
            ////login4.LoginID = "62547813";
            ////login4.PasswordHash = "0VpvMkBh9U527v321Wro+dJC0nY8HCYwiPFWp24WMSOdLW15krvKisfs6Ku5HzHt";
            ////login4.Locked = Locked.unlocked;

            //context.Walkers.Add(walker1);


            //var walker2 = new Walker();

            //walker2.FirstName = "Mitchell";
            //walker2.LastName = "Moses";
            //walker2.Email = "mitchy555@gmail.com";
            //walker2.StreetAddress = "10 Camden Rd";
            //walker2.Suburb = suburbs[4];
            //walker2.State = "Victoria";
            //walker2.Country = "Australia";
            //walker2.PhNumber = "0432 142 732";
            //walker2.IsInsured = true;
            //walker2.ExperienceLevel = ExperienceLevel.Intermediate;
            ////login5.User = walker;
            ////login5.UserID = userID;///remove once customer ID is sorted
            ////login5.LoginID = "98765432";
            ////login5.PasswordHash = "ow/m3D+6bQTtuw8Pld486o9CzDDxFH1Gxy5XEmKXvswygE9ny0FPgNTnUokRxuI4";
            ////login5.Locked = Locked.unlocked;

            //context.Walkers.Add(walker2);


            //var walker3 = new Walker();

            //walker3.FirstName = "Jane";
            //walker3.LastName = "Edgerton";
            //walker3.Email = "jane.e@hotmail.com";
            //walker3.StreetAddress = "14 Nirvana Ave";
            //walker3.Suburb = suburbs[5];
            //walker3.State = "Victoria";
            //walker3.Country = "Australia";
            //walker3.PhNumber = "0455 332 897";
            //walker3.IsInsured = false;
            //walker3.ExperienceLevel = ExperienceLevel.Beginner;
            ////login6.User = walker;
            ////login6.LoginID = "55548692";
            ////login6.PasswordHash = "U3aiSza/zGtXt2AEuBwPSg4asyvXzErkOL4upGkbzo+RcoQ90c+E10n0Dy3HFviR";
            ////login6.Locked = Locked.unlocked;

            //context.Walkers.Add(walker3);

            //context.SaveChanges();


            //var admin = new Administrator();

            //admin.FirstName = "EasyWalk";
            //admin.LastName = "Administrator";
            //admin.Email = "admin@easywalk.com";
            //context.Administrators.Add(admin);

            //context.SaveChanges();  
            
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
                    login.UserId= owner.UserId;
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
            



        }
    }
}
