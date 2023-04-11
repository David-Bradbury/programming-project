using ProgrammingProject.Data;
using ProgrammingProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IActionResult> Index()
        {
            //lazy loading
            var walker = await _context.Walkers.FindAsync(WalkerID);
            //return View(walker);
            ViewBag.Walker = walker;
            ViewBag.Dogs = await MatchDogsToWalker(WalkerID);
            return View();
        }

        //public async Task<IActionResult> Walker(int id)
        //{
        //    var walker = await _context.Walker.FindAsync(id);
        //    return View(walker);
        //}

        //// Add a new walker
        //public async Task<IActionResult> AddWalker(int id) => View(await _context.Walker.FindAsync(id));

        //[HttpPost]
        //public async Task<IActionResult> AddWalker(int walkerID, string firstName, string lastName, string address,
        //                         string phNumber, string email, bool IsInsured, ExperienceLevel experience)
        //{
        //    var walker = await _context.Walker.FindAsync(walkerID);

        //    if (firstName == null)
        //        ModelState.AddModelError(nameof(firstName), "We need your name.");
        //    if (lastName == null)
        //        ModelState.AddModelError(nameof(lastName), "We need your surname.");
        //    if (address == null)
        //        ModelState.AddModelError(nameof(address), "We need your address.");
        //    if (phNumber == null)
        //        ModelState.AddModelError(nameof(phNumber), "We need to know how to contact you.");
        //    if (email == null)
        //        ModelState.AddModelError(nameof(email), "we need your email.");
        //    if (IsInsured == null)
        //        ModelState.AddModelError(nameof(IsInsured), "We need to know your insurance status.");
        //    if (experience == null)
        //        ModelState.AddModelError(nameof(experience), "We need to know how experienced you are.");

        //    Walker.Add(
        //    new Walker
        //    {
        //        FirstName = firstName,
        //        LastName = lastName,
        //        Address = address,
        //        PhNumber = phNumber,
        //        Email = email,
        //        IsInsured = IsInsured,
        //        ExperienceLevel = experience
        //    });

        //    await _context.SaveChangesAsync();

        //    return RedirectToAction(nameof(Index));
        //}

        //// Match suitable dogs to walkers

        //public async Task<IActionResult> MatchDogsToWalker(int id) => View(await _context.Dogs.FindAsync(id));

        [HttpPost]
        public async Task<List<Dog>> MatchDogsToWalker(int id)
        //public async Task<IActionResult> MatchDogsToWalker(int id)
        {

            // Get full list of dogs
            var dogs = await _context.Dogs.ToListAsync();
            //var dogs = _context.Dogs.AsEnumerable();

            // Get specific walker
            var walker = await _context.Walkers.FindAsync(id);

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

            return filteredDogs;

            // Notes to discuss: AllowUnvaccinated as a question
            // when adding walker (saved to model). Allows user to set requirements and
            // makes filtering easy (as per below).

            // TODO: Could add logic here to filter list down based on user preferences.
            // E.g.Location or dates/times. Some filter work started below...
        }

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

        public static implicit operator WalkerController(string v)
        {
            throw new NotImplementedException();
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

            Date.Add(StartTime.TimeOfDay);
            DateTime end = Date;
            end.Add(EndTime.TimeOfDay);

            walkingSessions.Add(
            new WalkingSession
            {
                Date = Date,
                ScheduledStartTime = Date,
                ScheduledEndTime = end,
                WalkerID = walker.UserId,
                Walker = walker,
            });

            //walker.WalkingSessions = walkingSessions;
            walker.WalkingSessions.Add(walkingSessions.Last());

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

            //return View();
        }

        //public async Task<IActionResult> SelectWalkingSession(int DogID) => View(await _context.Dogs.FindAsync(DogID));

        [HttpPost]
        public async Task<IActionResult> WalkingSessions(int DogID)
        {

            var walkerSessions = _context.WalkingSessions.AsEnumerable();

            //var walkerSession = await _context.WalkingSession.FindAsync(sessionID);

            var walkingSession = new List<WalkingSession>();

            foreach (var walker in walkerSessions)
            {
                if (walker != null && DateTime.UtcNow <= walker.ScheduledStartTime)
                {
                    walkingSession.Add(walker);
                }
            }

            ViewBag.WalkingSession = walkingSession;
            ViewBag.DogID = DogID;

            return View();
        }

        // Add dog to walking session

        //public async Task<IActionResult> AddDogToWalkingSession(int DogID, int sessionID) => View(await _context.Dogs.FindAsync(DogID));

        [HttpPost]
        public async Task<IActionResult> AddDogToWalkingSession(int DogID, int SessionID, DateTime StartTime, DateTime EndTime)
        {
            // logic to add dog to walking session.
            var dog = await _context.Dogs.FindAsync(DogID);

            //var walkerSession = await _context.Walker.WalkingSessions.FindAsync(sessionID);

            var walkerSession = await _context.WalkingSessions.FindAsync(SessionID);

            if (walkerSession.DogList.Count >= 6)
            {
                ModelState.AddModelError(nameof(StartTime), "Too many dogs on this walk.");
            }
            else
            {
                walkerSession.DogList.Add(dog);
                //walkerSession.Add(
                //    new WalkingSession
                //    {
                //        StartTime = StartTime,
                //        EndTime = EndTime,
                //        WalkerID = walker.UserId,
                //        Walker = walker,
                //    });

                //walker.WalkingSessions = walkingSessions;

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        //[HttpPost]
        //public async Task<IActionResult> AddDogToWalkingSession(int DogID, int sessionID)
        //{
        //    // logic to add dog to walking session.
        //    var dog = await _context.Dogs.FindAsync(DogID);

        //    //var walkerSession = await _context.Walker.WalkingSessions.FindAsync(sessionID);

        //    var walkerSession = await _context.WalkingSession.FindAsync(sessionID);

        //    if (walkerSession.DogList.count >= 6)
        //    {
        //        ModelState.AddModelError(nameof(walkerSession), "Too many dogs on this walk.");
        //    }

        //    return View();
        //}

        // Start walking session

        //public async Task<IActionResult> StartWalkingSession(DateTime? time, int sessionID) => View(await _context.WalkingSession.FindAsync(sessionID));

        //[HttpPost]
        //public async Task<IActionResult> StartWalkingSession(DateTime? time, int sessionID)
        //{
        //    if (time == null)
        //        time = DateTime.UtcNow;

        //    //var walkerSession = await _context.Walker.WalkingSessions.FindAsync(sessionID);

        //    var walkerSession = await _context.WalkingSession.FindAsync(sessionID);

        //    walkerSession.StartTime = time;
        //
        //    return View();
        // }
        //// End walking session
        //public async Task<IActionResult> EndWalkingSession(DateTime? time, int sessionID) => View(await _context.WalkingSession.FindAsync(sessionID));

        //[HttpPost]
        //public async Task<IActionResult> EndWalkingSession(DateTime? time, int sessionID)
        //{
        //    if (time == null)
        //        time = DateTime.UtcNow;

        //    //var walkerSession = await _context.Walker.WalkingSessions.FindAsync(sessionID);

        //    var walkerSession = await _context.WalkingSessions.FindAsync(sessionID);

        //    walkerSession.EndTime = time;

        //    return RedirectToAction("Index");
        //}


    }
}