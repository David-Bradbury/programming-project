using ProgrammingProject.Data;
using ProgrammingProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace ProgrammingProject.Controllers
{
    //Mask URL
    [Route("/Walker")]
    public class WalkerController : Controller
    {
        private readonly EasyWalkContext _context;
        private int WalkerID => HttpContext.Session.GetInt32(nameof(Walker.UserId)).Value;

        //private int WalkerID => HttpContext.Session.GetInt32(nameof(Walker.WalkerID)).Value;
        //private int WalkerID = 1; // This needs to be swapped after working out how to retrieve User.id 


        public WalkerController(EasyWalkContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            //lazy loading
            var walker = await _context.Walkers.FindAsync(WalkerID);
            return View(walker);
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

        //[HttpPost]
        //public async Task MatchDogsToWalker(int id)
        //{

        //    //var dogs = await _context.Dogs.FindAsync();
        //    var dogs = _context.Dogs.AsQueryable<Dog>;

        //    var walker = await _context.Walkers.FindAsync(id);


        //    var tempList = new List<Dog>();

        //    var score = 0;

        //    foreach (var dog in dogs)
        //    {
        //        // Sets a difficulty score to the dog
        //        if (dog != null)
        //        {
        //            score += (int)dog.DogSize;
        //            score += (int)dog.Temperament;
        //        }

        //        // Below matches dog to suitable Walkers
        //        if ((int)walker.ExperienceLevel == 4)
        //        {
        //            tempList.Add(dog);
        //        }
        //        else if ((int)walker.ExperienceLevel == 3 && score < 7)
        //        {
        //            tempList.Add(dog);
        //        }
        //        else if ((int)walker.ExperienceLevel == 2 && score < 5)
        //        {
        //            tempList.Add(dog);
        //        }
        //        else if ((int)walker.ExperienceLevel == 1 && score < 3)
        //        {
        //            tempList.Add(dog);
        //        }

        //    }

        //    // TODO: Could add logic here to filter list down based on user preferences.
        //    // E.g.Location or dates/times. Some filter work started below...

        //    // Notes to discuss: AllowUnvaccinated as a question
        //    // when adding walker (saved to model). Allows user to set requirements and
        //    // makes filtering easy (as per below).

        //    foreach (var temp in tempList)
        //    {
        //        if (temp.IsVaccinated == false /*&& walker.AllowUnvaccinated == false */)
        //            tempList.Remove(temp);
        //        if (temp.Vet == null)
        //            tempList.Remove(temp);
        //    }

        //    // Return tempList to View. View to list suitable dogs
        //    // with contact details/button for the walker to select.
        //    // Might be worth returning an IPagedList .DP
        //    ViewBag.Dog = tempList;
        //}


        // Add dog to walking session

        //public async Task<IActionResult> AddDogToWalkingSession(int DogID, int sessionID) => View(await _context.Dogs.FindAsync(DogID));

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