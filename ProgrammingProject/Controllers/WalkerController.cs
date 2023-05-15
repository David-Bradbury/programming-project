using ProgrammingProject.Data;
using ProgrammingProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ProgrammingProject.Filters;
using GeoCoordinatePortable;

namespace ProgrammingProject.Controllers
{
    //Mask URL
    [Route("/Walker/[action]")]
    public class WalkerController : Controller
    {
        private readonly EasyWalkContext _context;
        private int WalkerID => HttpContext.Session.GetInt32(nameof(Walker.UserId)).Value;

        public WalkerController(EasyWalkContext context)
        {
            _context = context;
        }

        [AuthorizeUser]
        public async Task<IActionResult> Index()
        {
            //lazy loading
            var walker = await _context.Walkers.FindAsync(WalkerID);
            ViewBag.Walker = walker;
            ViewBag.Dogs = await MatchDogsToWalker(WalkerID);
            return View();
        }

        // Match suitable dogs to walkers
        [HttpPost]
        public async Task<List<Dog>> MatchDogsToWalker(int id)
        {

            // Get full list of dogs
            var dogs = await _context.Dogs.ToListAsync();
            //var dogs = _context.Dogs.AsEnumerable();

            // Get specific walker
            var walker = await _context.Walkers.Where(x => x.UserId == id).SingleOrDefaultAsync();
            //var walker = await _context.Walkers.FindAsync(id);

            // Get list of dogs suited for the walker
            var tempList = await GetListOfSuitableDogsToWalkers(walker, (IEnumerable<Dog>)dogs);

            // passes the list of dogs to be filtered down based on specific parameters.
            var filteredDogs = FilterDogs(tempList);

            // Return filteredDog to View. View to list suitable dogs
            // with contact details/button for the walker to select.
            // Might be worth returning an IPagedList .DP
            // ViewBag.Dog = tempList; --Uncomment after testing and remove return statement.
            // Or create another method which calls this for added abstraction.DP

            return filteredDogs;
        }

        // Filter list of dogs based off specific rules
        public List<Dog> FilterDogs(List<Dog> dogs)
        {
            var filteredDogs = new List<Dog>();

            foreach (var d in dogs)
            {
                if (d.IsVaccinated == true && d.Vet != null)
                    filteredDogs.Add(d);
            }

            var locationFilteredDogs = FilterLocationByRadius(filteredDogs);

            return locationFilteredDogs.Result;

            // Notes to discuss: AllowUnvaccinated as a question
            // when adding walker (saved to model). Allows user to set requirements and
            // makes filtering easy (as per below).

            // TODO: Could add logic here to filter list down based on user preferences.
            // E.g.Location or dates/times. Some filter work started below...
        }

        /*  Nigel Sampson and Tomas Kubes (2011)
         *  Calculating distance between two latitude and longitude geocoordinates,
         *  Stack Overflow. Available at:
         *  https://stackoverflow.com/questions/6366408/calculating-distance-between-two-latitude-and-longitude-geocoordinates
         *  (Accessed: April 30, 2023). 
         */

        // Filters dogs to within 10km of the walker's Suburb
        public async Task<List<Dog>> FilterLocationByRadius(List<Dog> dogs, int? range = 10000)
        {
            var walker = await _context.Walkers.FindAsync(WalkerID);
            var suburb = walker.Suburb;
            var filteredDogs = new List<Dog>();
            var userLocation = new GeoCoordinate(double.Parse(suburb.Lat), double.Parse(suburb.Lon));

            var location = new GeoCoordinate();
            var owner = new Owner();

            foreach (var dog in dogs)
            {
                owner = dog.Owner;
                if (owner.Suburb != null)
                {
                    location.Latitude = double.Parse(owner.Suburb.Lat);
                    location.Longitude = double.Parse(owner.Suburb.Lon);

                    if (userLocation.GetDistanceTo(location) < range)
                    {
                        filteredDogs.Add(dog);
                    }
                }
            }

            return filteredDogs;
        }

        // Filter dogs by training level
        public async Task<List<Dog>> FilterDogsByTrainingLevel(List<Dog> dogs, TrainingLevel level)
        {
            List<Dog> filteredDogs = new List<Dog>();

            if (level == TrainingLevel.None)
            {
                foreach (Dog dog in dogs)
                {
                    if (dog.TrainingLevel == TrainingLevel.None)
                    {
                        filteredDogs.Add(dog);
                    }
                }
            }
            else if (level == TrainingLevel.Basic)
            {
                foreach (Dog dog in dogs)
                {
                    if (dog.TrainingLevel == TrainingLevel.Basic)
                    {
                        filteredDogs.Add(dog);
                    }
                }
            }
            else
            {
                filteredDogs = FilterDogsByMinimumTrainingLevel(dogs, level).Result;
            }
            return filteredDogs;
        }

        // Filter dogs by minimum training level
        public async Task<List<Dog>> FilterDogsByMinimumTrainingLevel(List<Dog> dogs, TrainingLevel minimumLevel)
        {
            List<Dog> filteredDogs = new List<Dog>();

            if (minimumLevel == TrainingLevel.None)
            {
                return dogs;
            }
            else if (minimumLevel == TrainingLevel.Basic)
            {
                foreach (Dog dog in dogs)
                {
                    if (dog.TrainingLevel == TrainingLevel.Basic || dog.TrainingLevel == TrainingLevel.Fully)
                    {
                        filteredDogs.Add(dog);
                    }
                }
            }
            else
            {
                foreach (Dog dog in dogs)
                {
                    if (dog.TrainingLevel == TrainingLevel.Fully)
                    {
                        filteredDogs.Add(dog);
                    }
                }
            }

            return filteredDogs;
        }

        // Returns a list of dogs which match the experience level of the passed walker.
        public async Task<List<Dog>> GetListOfSuitableDogsToWalkers(Walker walker, IEnumerable<Dog> dogs)
        {

            var tempList = new List<Dog>();
            var score = 0;

            foreach (var dog in dogs)
            {
                if (dog != null)
                {
                    score = await GetDogTraitScore(dog);
                }
                if (walker != null)
                {
                    if ((int)walker.ExperienceLevel == 4)
                    {
                        tempList.Add(dog);
                    }
                    else if ((int)walker.ExperienceLevel == 3 && score <= 6)
                    {
                        tempList.Add(dog);
                    }
                    else if ((int)walker.ExperienceLevel == 2 && score <= 4)
                    {
                        tempList.Add(dog);
                    }
                    else if ((int)walker.ExperienceLevel == 1 && score <= 3)
                    {
                        tempList.Add(dog);
                    }
                }
            }

            return tempList;
        }

        // Sets a difficulty score to the dog
        public async Task<int> GetDogTraitScore(Dog dog)
        {
            var score = 0;

            if (dog != null)
            {
                score += (int)dog.DogSize;
                score += (int)dog.Temperament;
            }

            return await Task.FromResult(score);
        }

        [HttpPost]
        public async Task<IActionResult> CreateWalkingSessions(DateTime Date, DateTime StartTime, DateTime EndTime)
        {

            var walker = await _context.Walkers.FindAsync(WalkerID);

            var walkingSessions = await _context.WalkingSessions.ToListAsync();

            if (Date < DateTime.UtcNow)
                ModelState.AddModelError(nameof(Date), "Valid date needs to be selected");
            //if (StartTime == null)
            //    ModelState.AddModelError(nameof(StartTime), "Valid Start Time needs to be selected");
            if (EndTime < StartTime)
                ModelState.AddModelError(nameof(EndTime), "Valid End Time needs to be selected");

            DateTime start = new DateTime(Date.Year, Date.Month, Date.Day, StartTime.Hour,
                                          StartTime.Minute, StartTime.Second);

            DateTime end = new DateTime(Date.Year, Date.Month, Date.Day, EndTime.Hour,
                                          EndTime.Minute, EndTime.Second);

            walkingSessions.Add(
            new WalkingSession
            {
                Date = Date,
                ScheduledStartTime = start,
                ScheduledEndTime = end,
                WalkerID = walker.UserId,
                Walker = walker,
            });

            walker.WalkingSessions.Add(walkingSessions.Last());

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> WalkingSessions(int DogID)
        {

            var walkerSessions = _context.WalkingSessions.AsEnumerable();

            var walkingSession = new List<WalkingSession>();

            foreach (var walker in walkerSessions)
            {
                if (walker != null && DateTime.UtcNow <= walker.ScheduledStartTime)
                {
                    walkingSession.Add(walker);
                }
            }
            List<WalkingSession> SortedList = walkerSessions.OrderBy(o => o.Date).ToList();

            ViewBag.WalkingSession = SortedList;
            ViewBag.DogID = DogID;

            return View();
        }

        // Add dog to walking session
        [HttpPost]
        public async Task<IActionResult> AddDogToWalkingSession(int DogID, int SessionID, DateTime StartTime, DateTime EndTime)
        {
            var dog = await _context.Dogs.FindAsync(DogID);

            var walkerSession = await _context.WalkingSessions.FindAsync(SessionID);

            if (walkerSession.DogList.Count >= 6)
            {
                ModelState.AddModelError(nameof(StartTime), "Too many dogs on this walk.");
            }
            else if (walkerSession.DogList.Contains(dog))
            {
                ModelState.AddModelError(nameof(StartTime), "Dog is already on this walk.");
            }
            else
            {
                walkerSession.DogList.Add(dog);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Remove Dog from a walking session
        [HttpPost]
        public async Task<IActionResult> RemoveDogFromWalk(int DogID, int SessionID)
        {

            var dog = await _context.Dogs.FindAsync(DogID);

            var walkerSession = await _context.WalkingSessions.FindAsync(SessionID);

            if (DogID == null || SessionID == null)
            {
                ModelState.AddModelError(nameof(SessionID), "Can't find Dog or Session");
            }
            else if (!walkerSession.DogList.Contains(dog))
            {
                ModelState.AddModelError(nameof(SessionID), "Dog is not on this walk");
            }
            else if (walkerSession == null)
            {
                ModelState.AddModelError(nameof(SessionID), "Can't find walking Session");
            }
            else
            {
                walkerSession.DogList.Remove(dog);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));

        }

        // Start walking session

        public async Task<IActionResult> StartWalkingSession(int sessionID)
        {

            var walkerSession = await _context.WalkingSessions.FindAsync(sessionID);

            var walker = await _context.Walkers.FindAsync(walkerSession.WalkerID);

            foreach (var session in walker.WalkingSessions)
            {
                if (session.SessionID == sessionID)
                {
                    session.ActualStartTime = DateTime.Now;
                }
            }

            walkerSession.ActualStartTime = DateTime.Now;

            await _context.SaveChangesAsync();

            ViewBag.WalkingSession = walkerSession;

            return View();
        }

        // End walking session
        [HttpPost]
        public async Task<IActionResult> EndWalkingSession(int sessionID)
        {
            var walkerSession = await _context.WalkingSessions.FindAsync(sessionID);

            var walker = await _context.Walkers.FindAsync(walkerSession.WalkerID);

            foreach (var session in walker.WalkingSessions)
            {
                if (session.SessionID == sessionID)
                {
                    session.ActualEndTime = DateTime.Now;
                }
            }

            walkerSession.ActualEndTime = DateTime.Now;

            return RedirectToAction("Index");
        }

        // Edit walking session

        public async Task<IActionResult> EditWalkingSession(int sessionID)
        {

            var walkerSession = await _context.WalkingSessions.FindAsync(sessionID);

            // ViewBag.WalkingSession = walkerSession;

            return View(walkerSession);
        }

        public async Task<IActionResult> CancelChanges(int sessionID) => RedirectToAction("Index");


        [HttpPost]
        public async Task<IActionResult> UpdateWalkingSession(
            int sessionID, DateTime Date, DateTime StartTime, DateTime EndTime)
        {
            var walkerSession = await _context.WalkingSessions.FindAsync(sessionID);

            // var walker = await _context.Walkers.FindAsync(walkerSession.WalkerID);

            //var walk = walker.WalkingSessions.Find(walkerSession);

            if (Date < DateTime.UtcNow || Date == null)
                ModelState.AddModelError(nameof(Date), "Valid date needs to be selected");
            //if (StartTime == null)
            //    ModelState.AddModelError(nameof(StartTime), "Valid Start Time needs to be selected");
            if (EndTime < StartTime || StartTime == null || EndTime == null)
                ModelState.AddModelError(nameof(EndTime), "Valid End Time needs to be selected");

            if (!ModelState.IsValid)
            {
                return View(walkerSession);
            }

            DateTime start = new DateTime(Date.Year, Date.Month, Date.Day, StartTime.Hour,
                                          StartTime.Minute, StartTime.Second);

            DateTime end = new DateTime(Date.Year, Date.Month, Date.Day, EndTime.Hour,
                                          EndTime.Minute, EndTime.Second);
            bool changesMade = false;
            if (walkerSession.Date != Date)
            {
                walkerSession.Date = Date;
                changesMade = true;
            }

            if (walkerSession.ScheduledStartTime != start)
            {
                walkerSession.ScheduledStartTime = start;
                changesMade = true;
            }

            if (walkerSession.ScheduledEndTime != end)
            {
                walkerSession.ScheduledEndTime = end;
                changesMade = true;
            }


            //foreach (var session in walker.WalkingSessions)
            //{
            //    if (session.SessionID == sessionID)
            //    {
            //        session.Date = walkerSession.Date;
            //        session.ScheduledStartTime = walkerSession.ScheduledStartTime;
            //        session.ScheduledEndTime = walkerSession.ScheduledEndTime;
            //    }
            //}
            if (!changesMade)
            {
                await _context.SaveChangesAsync();
            }
            //ViewBag.Walker = walker;
            //ViewBag.Dogs = await MatchDogsToWalker(WalkerID);

            return RedirectToAction(nameof(Index));
        }

        // Delete walking session

        public async Task<IActionResult> DeleteWalkingSession(int sessionID)
        {

            var walkerSession = await _context.WalkingSessions.FindAsync(sessionID);

            var walker = await _context.Walkers.FindAsync(walkerSession.WalkerID);

            walker.WalkingSessions.Remove(walkerSession);
            _context.WalkingSessions.Remove(walkerSession);


            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        //Displays previous walking sessions
        //Not working when marked as Post???.DP
        //[HttpPost]
        public async Task<IActionResult> PreviousWalkingSessions()
        {
            if (HttpContext.Session.GetInt32(nameof(Walker.UserId)).Value == 0)
            {
                return RedirectToAction(nameof(Index), new { area = "Home" });
            }
            var walker = await _context.Walkers.FindAsync(WalkerID);
            var walkerSessions = walker.WalkingSessions;

            List<WalkingSession> SortedList = walkerSessions.OrderBy(o => o.Date).ToList();

            ViewBag.WalkingSession = SortedList;

            return View();
        }
    }
}