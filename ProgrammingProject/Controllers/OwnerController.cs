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
        public async Task AddDog(DogSize size, Temperament temperament, int id, 
                                 string name, string microchip, bool IsVaccinated, Owner owner, Vet vet)
        {
            var dog = await _context.Dogs.FindAsync(id);

            if (dog != null)
            {
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
            }

        }

        public async Task MatchDogToWalker(int id)
        {
            var dog = await _context.Dogs.FindAsync(id);

            if (dog != null)
            {
                

                await _context.SaveChangesAsync();
            }

        }


    }
}
}