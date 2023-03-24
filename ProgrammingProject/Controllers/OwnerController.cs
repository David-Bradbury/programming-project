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
    [Route("/Owner")]
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
            var owner = await _context.Owner.FindAsync(OwnerID);
            return View(customer);
        }

        public async Task<IActionResult> Dogs(int id)
        {
            var dog = await _context.Dogs.FindAsync(id);
            return View(dog);
        }

        // Add a dog to the owner
        public async Task<IActionResult> AddDog(int id) => View(await _context.Dogs.FindAsync(id));

        [HttpPost]
        public async Task<IActionResult> AddDog(int ownerID, DogSize size, Temperament temperament, int id,
                                 string name, string microchip, bool IsVaccinated, Vet vet)
        {
            //var dog = await _context.Dogs.FindAsync(id);
            var owner = await _context.Owner.FindAsync(ownerID);

            if (size == null)
                ModelState.AddModelError(nameof(dogSize), "invalid Size chosen.");
            if (temperament == null)
                ModelState.AddModelError(nameof(dogTemperament), "invalid Temperament chosen.");
            if (name == null)
                ModelState.AddModelError(nameof(dogName), "Your dog has to have a name.");
            if (microchip == null)
                ModelState.AddModelError(nameof(MicrochipNumber), "Please enter in your dogs Microchip number.");
            if (IsVaccinated == null)
                ModelState.AddModelError(nameof(VaccinationStatus), "You need to let us know your dogs vaccination status.");
            if (vet == null)
                ModelState.AddModelError(nameof(dogsVet), "We can't walk your dog if we don't know their vet.");

            owner.Dogs.Add(
            new Dog
            {
                DogSize = size,
                Temperament = temperament,
                ID = id,
                Name = name,
                MicrochipNumber = microchip,
                IsVaccinated = IsVaccinated,
                Owner = owner,
                Vet = vet
            });

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Match suitable walkers to the dog
        public async Task<IActionResult> MatchDogToWalker(int id) => View(await _context.Dogs.FindAsync(id));

        [HttpPost]
        public async Task MatchDogToWalker(int id)
        {
            var dog = await _context.Dogs.FindAsync(id);

            var walker = await _context.Walker.FindAsync();

            var tempList = new List<Walker>();

            var score = 0;

            // Sets a difficulty score to the dog
            if (dog != null)
            {
                score += (int)dog.DogSize;
                score += (int)dog.Temperament;
            }

            // Below matches dog to suitable Walkers
            if ((int)walker.ExperienceLevel == 4)
            {
                tempList.Add(walker);
            }
            else if ((int)walker.ExperienceLevel == 3 && score < 7)
            {
                tempList.Add(walker);
            }
            else if ((int)walker.ExperienceLevel == 2 && score < 5)
            {
                tempList.Add(walker);
            }
            else if ((int)walker.ExperienceLevel == 1 && score < 3)
            {
                tempList.Add(walker);
            }

            // Return tempList to View. View to list suitable walkers
            // with contact details/button for the owner to select.DP
            ViewBag.Walker = tempList;

        }


    }
}
}