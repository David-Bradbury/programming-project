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
            if (context.Owners.Any())
                return; // DB has been seeded already.

            context.Owners.AddRange(
                new Owner
                {
                    FirstName = "Peter",
                    LastName = "Carrigan",
                    Password = "",
                    Email = "p.carrigan@gogo.com",
                    Address = "12 Pine Rd, Bayswater, VIC, 3153",
                    PhNumber = "0424 225 877" 
                },
                new Owner
                {
                    FirstName = "John",
                    LastName = "Smith",
                    Password = "",
                    Email = "johnsmith@proton.me",
                    Address = "26 Wills Ave, Mount Waverley, VIC, 3149",
                    PhNumber = "0411 672 900"
                },
                new Owner
                {
                    FirstName = "Judy",
                    LastName = "Wong",
                    Password = "",
                    Email = "jwong45@me.com",
                    Address = "42 Buckley St, Noble Park, VIC, 3174",
                    PhNumber = "0456 853 345"
                }
            );

            context.Walkers.AddRange(
                new Walker
                {
                    FirstName = "Karen",
                    LastName = "Fisher",
                    Password = "",
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
                    Password = "",
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
                    Password = "",
                    Email = "jane.e@hotmail.com",
                    Address = "14 Nirvana Ave, Malvern East, VIC, 3145",
                    PhNumber = "0455 332 897",
                    IsInsured = false,
                    ExperienceLevel = ExperienceLevel.Beginner
                }
                );

            context.SaveChanges();
        }
    }
}
