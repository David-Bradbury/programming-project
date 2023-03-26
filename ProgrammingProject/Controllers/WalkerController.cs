using ProgrammingProject.Data;
using ProgrammingProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleHashing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingProject.Controllers
{
    //Mask URL
    [Route("/Walker")]
    public class OwnerController : Controller
    {
        private readonly EasyWalkContext _context;

        public LoginController(EasyWalkContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            //lazy loading
            var walker = await _context.Walker.FindAsync(WalkerID);
            return View(walker);
        }

        public async Task<IActionResult> Walker(int id)
        {
            var walker = await _context.Walker.FindAsync(id);
            return View(walker);
        }

        // Add a new walker
        public async Task<IActionResult> AddWalker(int id) => View(await _context.Walker.FindAsync(id));

        [HttpPost]
        public async Task<IActionResult> AddWalker(int walkerID, string firstName, string lastName, string address,
                                 string phNumber, string email, bool IsInsured, ExperienceLevel experience)
        {
            var walker = await _context.Walker.FindAsync(walkerID);

            if (firstName == null)
                ModelState.AddModelError(nameof(firstName), "We need your name.");
            if (lastName == null)
                ModelState.AddModelError(nameof(lastName), "We need your surname.");
            if (address == null)
                ModelState.AddModelError(nameof(address), "We need your address.");
            if (phNumber == null)
                ModelState.AddModelError(nameof(phNumber), "We need to know how to contact you.");
            if (email == null)
                ModelState.AddModelError(nameof(email), "we need your email.");
            if (IsInsured == null)
                ModelState.AddModelError(nameof(IsInsured), "We need to know your insurance status.");
            if (experience == null)
                ModelState.AddModelError(nameof(experience), "We need to know how experienced you are.");

            Walker.Add(
            new Walker
            {
                FirstName = firstName,
                LastName = lastName,
                Address = address,
                PhNumber = phNumber,
                Email = email,
                IsInsured = IsInsured,
                ExperienceLevel = experience
            });

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Match suitable dogs to walkers
        public async Task<IActionResult> MatchDogsToWalker(int id) => View(await _context.Dogs.FindAsync(id));

        [HttpPost]
        public async Task MatchDogsToWalker(int id)
        {
            var dogs = await _context.Dogs.FindAsync();

            var walker = await _context.Walker.FindAsync(id);

            var tempList = new List<Dog>();

            var score = 0;

            foreach (var dog in dogs)
            {
                // Sets a difficulty score to the dog
                if (dog != null)
                {
                    score += (int)dog.DogSize;
                    score += (int)dog.Temperament;
                }

                // Below matches dog to suitable Walkers
                if ((int)walker.ExperienceLevel == 4)
                {
                    tempList.Add(dog);
                }
                else if ((int)walker.ExperienceLevel == 3 && score < 7)
                {
                    tempList.Add(dog);
                }
                else if ((int)walker.ExperienceLevel == 2 && score < 5)
                {
                    tempList.Add(dog);
                }
                else if ((int)walker.ExperienceLevel == 1 && score < 3)
                {
                    tempList.Add(dog);
                }

            }
            // Return tempList to View. View to list suitable dogs
            // with contact details/button for the walker to select.DP
            ViewBag.Dog = tempList;

        }


    }
}
}