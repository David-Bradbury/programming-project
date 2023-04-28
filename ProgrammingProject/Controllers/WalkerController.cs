﻿using ProgrammingProject.Data;
using ProgrammingProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ProgrammingProject.Filters;


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
            //return View(walker);
            ViewBag.Walker = walker;
            ViewBag.Dogs = await MatchDogsToWalker(WalkerID);
            return View();
        }

        // Match suitable dogs to walkers
        [HttpPost]
        public async Task<List<Dog>> MatchDogsToWalker(int id)
        //public async Task<IActionResult> MatchDogsToWalker(int id)
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

            return filteredDogs;

            // Notes to discuss: AllowUnvaccinated as a question
            // when adding walker (saved to model). Allows user to set requirements and
            // makes filtering easy (as per below).

            // TODO: Could add logic here to filter list down based on user preferences.
            // E.g.Location or dates/times. Some filter work started below...
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

            //var walkerSession = await _context.WalkingSession.FindAsync(sessionID);

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
        [HttpPost]
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
        [HttpPost]
        public async Task<IActionResult> EditWalkingSession(int sessionID)
        {

            var walkerSession = await _context.WalkingSessions.FindAsync(sessionID);

            ViewBag.WalkingSession = walkerSession;

            return View();
        }

        public async Task<IActionResult> CancelChanges(int sessionID) => RedirectToAction("Index");


        [HttpPost]
        public async Task<IActionResult> UpdateWalkingSession(
            int sessionID, DateTime Date, DateTime StartTime, DateTime EndTime)
        {
            var walkerSession = await _context.WalkingSessions.FindAsync(sessionID);

            var walker = await _context.Walkers.FindAsync(walkerSession.WalkerID);

            //var walk = walker.WalkingSessions.Find(walkerSession);

            if (Date < DateTime.UtcNow || Date == null)
                ModelState.AddModelError(nameof(Date), "Valid date needs to be selected");
            //if (StartTime == null)
            //    ModelState.AddModelError(nameof(StartTime), "Valid Start Time needs to be selected");
            if (EndTime < StartTime || StartTime == null || EndTime == null)
                ModelState.AddModelError(nameof(EndTime), "Valid End Time needs to be selected");

            DateTime start = new DateTime(Date.Year, Date.Month, Date.Day, StartTime.Hour,
                                          StartTime.Minute, StartTime.Second);

            DateTime end = new DateTime(Date.Year, Date.Month, Date.Day, EndTime.Hour,
                                          EndTime.Minute, EndTime.Second);

            if (walkerSession.Date.Year != Date.Year 
                || walkerSession.Date.Month != Date.Month 
                || walkerSession.Date.Day != Date.Day)
            {
                walkerSession.Date = Date;
                
            }

            if (walkerSession.ScheduledStartTime != start)
            {
                walkerSession.ScheduledStartTime = start;
            }

            if (walkerSession.ScheduledEndTime != end)
            {
                walkerSession.ScheduledEndTime = end;
            }


            foreach (var session in walker.WalkingSessions)
            {
                if (session.SessionID == sessionID)
                {
                    session.Date = walkerSession.Date;
                    session.ScheduledStartTime = walkerSession.ScheduledStartTime;
                    session.ScheduledEndTime = walkerSession.ScheduledEndTime;
                }
            }

            await _context.SaveChangesAsync();

            //ViewBag.Walker = walker;
            //ViewBag.Dogs = await MatchDogsToWalker(WalkerID);

            return RedirectToAction(nameof(Index));
        }

        // Delete walking session
        [HttpPost]
        public async Task<IActionResult> DeleteWalkingSession(int sessionID)
        {

            var walkerSession = await _context.WalkingSessions.FindAsync(sessionID);

            var walker = await _context.Walkers.FindAsync(walkerSession.WalkerID);

            walker.WalkingSessions.Remove(walkerSession);
            _context.WalkingSessions.Remove(walkerSession);


            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}