using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProgrammingProject.Models;
using System.Globalization;
using System.Text.Json;
using System.Linq;

namespace ProgrammingProject.Data
{
    public class SeedData
    {

        public static async void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new EasyWalkContext(
                serviceProvider.GetRequiredService<DbContextOptions<EasyWalkContext>>());


            //  Look for any Owners.
            if (context.Owners.Any())
                return; // DB has been seeded already.


            // Saving code as can be used to verify suburbs

            /* 
             * Breeds resourced from
             * 
             * Postcode API (no date) Australian Suburb and Postcode API | Home. 
             * Available at: https://postcodeapi.com.au/ (Accessed: April 22, 2023). 
             * 
             */

            //const string Url = "http://v0.postcodeapi.com.au/suburbs/6026.json";

            //    using var client = new HttpClient();
            //var json = client.GetStringAsync(Url).Result;

            //List<Suburb> items = JsonConvert.DeserializeObject<List<Suburb>>(json);

            //Suburb sub = new Suburb();

            // Seed Suburbs

            /* 
             * Suburbs resourced from
             * 
             * Schappim (2021) Schappim/Australian-Postcodes: List of australian post codes and suburbs, GitHub. 
             * Available at: https://github.com/schappim/australian-postcodes (Accessed: April 22, 2023). 
             * 
             */



            using (StreamReader r = new StreamReader("./Data/australian-postcodes.json"))
            {
                string json = r.ReadToEnd();

                List<Suburb> items = JsonConvert.DeserializeObject<List<Suburb>>(json);

                foreach (var s in items)
                {
                    context.Suburbs.Add(s);
                    context.SaveChanges();

                }
            }

            var suburb1 = context.Suburbs.Where(x => x.Postcode == "3153").FirstOrDefault();
            //var suburb1 = context.Suburbs.Find("3153", "Bayswater", "VIC");
            //var suburb1 = new Suburb();

            //suburb1.Postcode = "3153";
            //suburb1.SuburbName = "Bayswater";
            //suburb1.State = "Victoria";
            //suburb1.Lat = "-37.840";
            //suburb1.Lon = "145.270";

            //var suburb2 = new Suburb();
            var suburb2 = context.Suburbs.Where(x => x.Postcode == "3149").FirstOrDefault();
            //var suburb2 = context.Suburbs.Find("3149", "Mount Waverly", "VIC");

            //suburb2.Postcode = "3149";
            //suburb2.SuburbName = "Mount Waverly";
            //suburb2.State = "Victoria";

            var suburb3 = context.Suburbs.Where(x => x.Postcode == "3174").FirstOrDefault();
            //var suburb3 = context.Suburbs.Find("3174", "Noble Park", "VIC");
            //var suburb3 = new Suburb();

            //suburb3.Postcode = "3174";
            //suburb3.SuburbName = "Noble Park";
            //suburb3.State = "Victoria";

            var suburb4 = context.Suburbs.Where(x => x.Postcode == "3148").FirstOrDefault();
            //var suburb4 = context.Suburbs.Find("3148", "Chadstone", "VIC");
            //var suburb4 = new Suburb();

            //suburb4.Postcode = "3148";
            //suburb4.SuburbName = "Chadstone";
            //suburb4.State = "Victoria";
            //suburb1.Lat = "-37.890";
            //suburb1.Lon = "145.080";

            var suburb5 = context.Suburbs.Where(x => x.Postcode == "3122").FirstOrDefault();
            //var suburb5 = context.Suburbs.Find("3122", "Hawthorn", "VIC");
            //var suburb5 = new Suburb();

            //suburb5.Postcode = "3122";
            //suburb5.SuburbName = "Hawthorn";
            //suburb5.State = "Victoria";

            var suburb6 = context.Suburbs.Where(x => x.Postcode == "3145").FirstOrDefault();
            //var suburb6 = context.Suburbs.Find("3145", "Malvern East", "VIC");
            //var suburb6 = new Suburb();

            //suburb6.Postcode = "3145";
            //suburb6.SuburbName = "Malvern East";
            //suburb6.State = "Victoria";

            // Seed Vets
            var vet1 = new Vet();

            vet1.BusinessName = "Bays Vets";
            vet1.PhNumber = "0402 201 201";
            vet1.StreetAddress = "3 Baywater Road";
            vet1.Suburb = suburb1;          
            vet1.Country = "Australia";
            vet1.Email = "baysvets@gmail.com";

            context.Vets.Add(vet1);

            var vet2 = new Vet();

            vet2.BusinessName = "Pets of the Mount";
            vet2.PhNumber = "0440 404 040";
            vet2.StreetAddress = "21 Mountain Street";
            vet2.Suburb = suburb2;
            vet2.Country = "Australia";
            vet2.Email = "petsofthemount@hotmail.com";

            context.Vets.Add(vet2);

            var vet3 = new Vet();

            vet3.BusinessName = "Dr Schmackos";
            vet3.PhNumber = "1300 000 000";
            vet3.StreetAddress = "31 Schmackos Place";
            vet3.Suburb = suburb3;
            vet3.Country = "Australia";
            vet3.Email = "drschmackos@optus.net";

            context.Vets.Add(vet3);

            using (StreamReader r = new StreamReader("./Data/vets.json"))
            {
                string json = r.ReadToEnd();

                List<Vet> items = JsonConvert.DeserializeObject<List<Vet>>(json);

                foreach (var v in items)
                {
                    context.Vets.Add(v);
                    context.SaveChanges();

                }
            }

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

            using (StreamReader r = new StreamReader("./Data/logins.json"))
            {
                string json = r.ReadToEnd();

                List<Login> items = JsonConvert.DeserializeObject<List<Login>>(json);

                foreach (var l in items)
                {
                    context.Logins.Add(l);
                    context.SaveChanges();

                }
            }


            // Seed Users
            var owner1 = new Owner();

            owner1.FirstName = "John";
            owner1.LastName = "Smith";
            owner1.Email = login1.Email;
            owner1.StreetAddress = "12 Pine Rd";
            owner1.Suburb = suburb1;
            owner1.Country = "Australia";
            owner1.PhNumber = "0424 225 877";
            owner1.ProfileImage = "defaultProfile.png";
            login1.User = owner1;

            context.Owners.Add(owner1);


            var owner2 = new Owner();

            owner2.FirstName = "Peter";
            owner2.LastName = "Carrigan";
            owner2.Email = login2.Email;
            owner2.StreetAddress = "26 Wills Ave";
            owner2.Suburb = suburb2;
            owner2.Country = "Australia";
            owner2.PhNumber = "0411 672 900";
            owner2.ProfileImage = "defaultProfile.png";
            login2.User = owner2;

            context.Owners.Add(owner2);


            var owner3 = new Owner();

            owner3.FirstName = "Judy";
            owner3.LastName = "Wong";
            owner3.Email = login3.Email;
            owner3.StreetAddress = "42 Buckley St";
            owner3.Suburb = suburb3;
            owner3.Country = "Australia";
            owner3.PhNumber = "0456 853 345";
            owner3.ProfileImage = "defaultProfile.png";
            login3.User = owner3;

            context.Owners.Add(owner3);


            var walker1 = new Walker();

            walker1.FirstName = "Karen";
            walker1.LastName = "Fisher";
            walker1.Email = login4.Email;
            walker1.StreetAddress = "2 Jacana St";
            walker1.Suburb = suburb4;
            walker1.Country = "Australia";
            walker1.PhNumber = "0488 044 222";
            walker1.IsInsured = true;
            walker1.ExperienceLevel = ExperienceLevel.Advanced;
            walker1.ProfileImage = "defaultProfile.png";
            login4.User = walker1;

            context.Walkers.Add(walker1);


            var walker2 = new Walker();

            walker2.FirstName = "Mitchell";
            walker2.LastName = "Moses";
            walker2.Email = login5.Email;
            walker2.StreetAddress = "10 Camden Rd";
            walker2.Suburb = suburb5;
            walker2.Country = "Australia";
            walker2.PhNumber = "0432 142 732";
            walker2.IsInsured = true;
            walker2.ExperienceLevel = ExperienceLevel.Intermediate;
            walker2.ProfileImage = "defaultProfile.png";
            login5.User = walker2;

            context.Walkers.Add(walker2);


            var walker3 = new Walker();

            walker3.FirstName = "Jane";
            walker3.LastName = "Edgerton";
            walker3.Email = login6.Email;
            walker3.StreetAddress = "14 Nirvana Ave";
            walker3.Suburb = suburb6;
            walker3.Country = "Australia";
            walker3.PhNumber = "0455 332 897";
            walker3.IsInsured = false;
            walker3.ExperienceLevel = ExperienceLevel.Beginner;
            walker3.ProfileImage = "defaultProfile.png";
            login6.User = walker3;

            context.Walkers.Add(walker3);


            var admin = new Administrator();

            admin.FirstName = "EasyWalk";
            admin.LastName = "Administrator";
            admin.Email = login7.Email;
            login7.User = admin;

            context.Administrators.Add(admin);

            /* Breeds resourced from
             * 
             * Paiv (no date) Paiv/FCI-Breeds: List of Dog Breeds recognized by the FCI, GitHub. 
             * Available at: https://github.com/paiv/fci-breeds (Accessed: April 24, 2023). 
             * 
             */

            using (StreamReader r = new StreamReader("./Data/breeds.json"))
            {
                string json = r.ReadToEnd();
                List<Breed> items = JsonConvert.DeserializeObject<List<Breed>>(json);

                foreach (var b in items)
                {
                    context.Breeds.Add(b);
                    context.SaveChanges();
                }

            }

            var breed1 = new Breed();
            breed1 = context.Breeds.FirstOrDefault();
            var breed2 = new Breed();
            breed2 = context.Breeds.Find(5);
            var breed3 = new Breed();
            breed3 = context.Breeds.Find(30);
            var breed4 = new Breed();
            breed4 = context.Breeds.Find(90);

            //// Seed Dogs

            var dog1 = new Dog();

            dog1.Name = "Max";
            dog1.Breed = breed1;
            dog1.MicrochipNumber = "123456";
            dog1.IsVaccinated = true;
            dog1.Temperament = Temperament.Friendly;
            dog1.ProfileImage = "dog-avatar.jpg";
            dog1.DogSize = DogSize.Large;
            dog1.TrainingLevel = TrainingLevel.Basic;
            dog1.Owner = owner1;
            dog1.Vet = vet1;

            context.Dogs.Add(dog1);

            var dog2 = new Dog();

            dog2.Name = "Bella";
            dog2.Breed = breed2;
            dog2.MicrochipNumber = "152655";
            dog2.IsVaccinated = true;
            dog2.Temperament = Temperament.Calm;
            dog2.ProfileImage = "dog-avatar.jpg";
            dog2.DogSize = DogSize.Large;
            dog2.TrainingLevel = TrainingLevel.Fully;
            dog2.Owner = owner2;
            dog2.Vet = vet2;

            context.Dogs.Add(dog2);

            var dog3 = new Dog();

            dog3.Name = "Teddy";
            dog3.Breed = breed3;
            dog3.MicrochipNumber = "111111";
            dog3.IsVaccinated = true;
            dog3.Temperament = Temperament.Friendly;
            dog3.ProfileImage = "dog-avatar.jpg";
            dog3.DogSize = DogSize.Small;
            dog3.TrainingLevel = TrainingLevel.None;
            dog3.Owner = owner3;
            dog3.Vet = vet3;

            context.Dogs.Add(dog3);


            var dog4 = new Dog();

            dog4.Name = "Ruby";
            dog4.Breed = breed4;
            dog4.MicrochipNumber = "111112";
            dog4.IsVaccinated = true;
            dog4.Temperament = Temperament.Friendly;
            dog4.ProfileImage = "dog-avatar.jpg";
            dog4.DogSize = DogSize.Small;
            dog4.TrainingLevel = TrainingLevel.Basic;
            dog4.Owner = owner3;
            dog4.Vet = vet3;

            context.Dogs.Add(dog4);

            context.SaveChanges();

            using (StreamReader r = new StreamReader("./Data/NSW_Owners.json"))
            {
                string json = r.ReadToEnd();
                List<Owner> items = JsonConvert.DeserializeObject<List<Owner>>(json);

                var suburb = new Suburb();

                foreach (var w in items)
                {
                    Random random = new Random();
                    int post = random.Next(2000, 2914);
                    var pc = "" + post;
                    var b = context.Suburbs.Where(x => x.Postcode == pc).FirstOrDefault();
                    w.Suburb = b;
                    w.ProfileImage = "defaultProfile.png";
                    context.Owners.Add(w);
                    context.SaveChanges();
                }

            }

            using (StreamReader r = new StreamReader("./Data/VIC_Owners.json"))
            {
                string json = r.ReadToEnd();
                List<Owner> items = JsonConvert.DeserializeObject<List<Owner>>(json);
                var suburb = new Suburb();

                foreach (var w in items)
                {
                    Random random = new Random();
                    int post = random.Next(3000, 3996);
                    var pc = "" + post;
                    var b = context.Suburbs.Where(x => x.Postcode == pc).FirstOrDefault();
                    w.Suburb = b;

                    w.ProfileImage = "defaultProfile.png";
                    context.Owners.Add(w);
                    context.SaveChanges();
                }

            }

            using (StreamReader r = new StreamReader("./Data/Qld_Owners.json"))
            {
                string json = r.ReadToEnd();
                List<Owner> items = JsonConvert.DeserializeObject<List<Owner>>(json);

                var suburb = new Suburb();

                foreach (var w in items)
                {
                    Random random = new Random();
                    int post = random.Next(4000, 4895);
                    var pc = "" + post;
                    var b = context.Suburbs.Where(x => x.Postcode == pc).FirstOrDefault();
                    w.Suburb = b;
                    w.ProfileImage = "defaultProfile.png";
                    context.Owners.Add(w);
                    context.SaveChanges();
                }

            }

            using (StreamReader r = new StreamReader("./Data/Dogs.json"))
            {
                string json = r.ReadToEnd();

                List<Dog> items = JsonConvert.DeserializeObject<List<Dog>>(json);

                foreach (var d in items)
                {
                    Random random = new Random();
                    int id = random.Next(1, 300);
                    var b = context.Breeds.Find(id);
                    d.Breed = b;

                    int vetID = random.Next(1, 1000);

                    var v = context.Vets.Find(vetID);
                    d.Vet = v;

                    int ownerID = random.Next(8, 607);

                    var o = context.Owners.Find(ownerID);
                    d.Owner = o;
                    d.ProfileImage = "dog-avatar.jpg";
                    context.Dogs.Add(d);
                    context.SaveChanges();

                }
            }

            // seed walkingSessions


            var dogs = new List<Dog>();
            dogs.Add(dog4);
            dogs.Add(dog3);

            var walk = new WalkingSession();
            walk.ScheduledStartTime = DateTime.Now;
            walk.ScheduledEndTime = DateTime.Now.AddHours(1);
            walk.WalkerID = 4;
            walk.Walker = walker1;
            walk.DogList = dogs;

            var walk1 = new WalkingSession();
            walk1.ScheduledStartTime = DateTime.Now.AddHours(2);
            walk1.ScheduledEndTime = DateTime.Now.AddHours(3);
            walk1.WalkerID = 4;
            walk1.Walker = walker1;
            walk1.DogList = dogs;

            context.WalkingSessions.Add(walk1);

            context.SaveChanges();

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null,
                MissingFieldFound = null,
            };

            /*
             * Raziak (2023) How to read CSV file from our computer using .NET Core web api, Stack Overflow. 
             * Available at: https://stackoverflow.com/questions/75015482/how-to-read-csv-file-from-our-computer-using-net-core-web-api 
             * (Accessed: April 22, 2023). 
             * 
             */

            using (var reader = new StreamReader("./Data/Walking_Sessions.csv"))
            using (var csv = new CsvReader(reader, config))
            {
                var w = new WalkingSession();

                csv.Context.RegisterClassMap<WalkingSessionMap>();
                var records = csv.GetRecords<WalkingSession>();

                foreach (var record in records)
                {
                    w.Date = record.Date;
                    w.ScheduledStartTime = record.ScheduledStartTime;
                    w.ScheduledEndTime = record.ScheduledEndTime;
                    w.WalkerID = record.WalkerID;
                    w.SessionID = 0;
                    context.WalkingSessions.Add(w);
                    context.SaveChanges();

                }
            }

            context.SaveChanges();

            using (StreamReader r = new StreamReader("./Data/NSW_Walkers.json"))
            {
                string json = r.ReadToEnd();
                List<Walker> items = JsonConvert.DeserializeObject<List<Walker>>(json);
                var suburb = new Suburb();

                foreach (var w in items)
                {
                    Random random = new Random();
                    int post = random.Next(2000, 2914);
                    var pc = "" + post;
                    var b = context.Suburbs.Where(x => x.Postcode == pc).FirstOrDefault();
                    w.Suburb = b;
                    w.ProfileImage = "defaultProfile.png";
                    context.Walkers.Add(w);
                    context.SaveChanges();
                }

            }

            using (StreamReader r = new StreamReader("./Data/Vic_Walkers.json"))
            {
                string json = r.ReadToEnd();
                List<Walker> items = JsonConvert.DeserializeObject<List<Walker>>(json);
                var suburb = new Suburb();

                foreach (var w in items)
                {
                    // Try adding suburb after it is added to context.
                    Random random = new Random();
                    int post = random.Next(3000, 3996);
                    var pc = "" + post;
                    var b = context.Suburbs.Where(x => x.Postcode == pc).FirstOrDefault();
                    w.Suburb = b;
                    w.ProfileImage = "defaultProfile.png";
                    context.Walkers.Add(w);
                    context.SaveChanges();
                }

            }

        }

    }

    public class WalkingSessionMap : ClassMap<WalkingSession>
    {
        public WalkingSessionMap()
        {
            Map(m => m.Date).TypeConverter<CustomDateTimeConverter>().TypeConverterOption.Format("dd/MM/yyyy h:mm:ss tt");
            Map(m => m.ScheduledStartTime).TypeConverter<CustomDateTimeConverter>().TypeConverterOption.Format("dd/MM/yyyy h:mm:ss tt");
            Map(m => m.ScheduledEndTime).TypeConverter<CustomDateTimeConverter>().TypeConverterOption.Format("dd/MM/yyyy h:mm:ss tt");
            Map(m => m.ActualStartTime).Optional().TypeConverter<CustomDateTimeConverter>().TypeConverterOption.Format("dd/MM/yyyy h:mm:ss tt");
            Map(m => m.ActualEndTime).Optional().TypeConverter<CustomDateTimeConverter>().TypeConverterOption.Format("dd/MM/yyyy h:mm:ss tt");
            Map(m => m.IsRecurring).Optional();
            Map(m => m.DogList).Optional();
            Map(m => m.WalkerID);
            Map(m => m.Walker).Optional();
        }
    }

    public class CustomDateTimeConverter : DateTimeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if (text == "00:00.0")
            {
                return default(DateTime);
            }

            try
            {
                return base.ConvertFromString(text, row, memberMapData);
            }
            catch (TypeConverterException)
            {
                return default(DateTime);
            }
            catch
            {
                throw;
            }
        }
    }
}
