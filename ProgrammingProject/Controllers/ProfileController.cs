﻿using Microsoft.AspNetCore.Mvc;
using ProgrammingProject.Data;
using ProgrammingProject.Models;
using ProgrammingProject.Filters;


namespace ProgrammingProject.Controllers
{
    public class ProfileController : Controller
    {
        private readonly EasyWalkContext _context;
        private bool isOwner;
        private int UserID => HttpContext.Session.GetInt32(nameof(Owner.UserId)).Value;


        [AuthorizeUser]
        public ProfileController(EasyWalkContext context)
        {
            _context = context;

        }

        [AuthorizeUser]
        public async Task<IActionResult> Index()
        {
            var o = new Owner();
            o = await _context.Owners.FindAsync(UserID);

            var w = new Walker();
            w = await _context.Walkers.FindAsync(UserID);

            var viewModel = new EditProfileViewModel();

            //Check usertype and create viewModel
            if (o == null)
            {
                //User is Walker
                viewModel.UserType = typeof(Walker).Name;
                viewModel.FirstName = w.FirstName;
                viewModel.LastName = w.LastName;
                viewModel.Email = w.Email;
                viewModel.StreetAddress = w.StreetAddress;
                viewModel.SuburbName = w.Suburb.SuburbName;
                viewModel.Postcode = w.Suburb.SuburbName;
                viewModel.State = w.State;
                viewModel.Country = w.Country;
                viewModel.PhNumber = w.PhNumber;
                viewModel.IsInsured = w.IsInsured;
                viewModel.ExperienceLevel = (int)w.ExperienceLevel;

            }
            else
            {
                //User is Owner
                viewModel.UserType = typeof(Owner).Name;
                viewModel.FirstName = o.FirstName;
                viewModel.LastName = o.LastName;
                viewModel.Email = o.Email;
                viewModel.StreetAddress = o.StreetAddress;
                viewModel.SuburbName = o.Suburb.SuburbName;
                viewModel.Postcode = o.Suburb.SuburbName;
                viewModel.State = o.State;
                viewModel.Country = o.Country;
                viewModel.PhNumber = o.PhNumber;
                //default values for ExperienceLevel an IsInsured as are not used for owner.

            }

            return View(viewModel);

        }


        public async Task<IActionResult> EditProfile(int id)
        {
            //create view model and assign the selected field from the profile page.
            var viewModel = new EditProfileViewModel();


            var o = new Owner();
            o = await _context.Owners.FindAsync(UserID);

            var w = new Walker();
            w = await _context.Walkers.FindAsync(UserID);



            // Check usertype and create viewModel
            if (o == null)
            {
                //User is Walker
                viewModel.UserType = typeof(Walker).Name;
                viewModel.FirstName = w.FirstName;
                viewModel.LastName = w.LastName;
                viewModel.Email = w.Email;
                viewModel.StreetAddress = w.StreetAddress;
                viewModel.SuburbName = w.Suburb.SuburbName;
                viewModel.Postcode = w.Suburb.SuburbName;
                viewModel.State = w.State;
                viewModel.Country = w.Country;
                viewModel.PhNumber = w.PhNumber;
                viewModel.IsInsured = w.IsInsured;
                viewModel.ExperienceLevel = (int)w.ExperienceLevel;

            }
            else
            {
                //User is Owner
                viewModel.UserType = typeof(Owner).Name;
                viewModel.FirstName = o.FirstName;
                viewModel.LastName = o.LastName;
                viewModel.Email = o.Email;
                viewModel.StreetAddress = o.StreetAddress;
                viewModel.SuburbName = o.Suburb.SuburbName;
                viewModel.Postcode = o.Suburb.SuburbName;
                viewModel.State = o.State;
                viewModel.Country = o.Country;
                viewModel.PhNumber = o.PhNumber;
                //default values for ExperienceLevel an IsInsured as are not used for owner.

            }
            if (id == 1)
                viewModel.SelectedField = nameof(viewModel.FirstName);
            if (id == 2)
                viewModel.SelectedField = nameof(viewModel.LastName);
            if (id == 3)
                viewModel.SelectedField = nameof(viewModel.StreetAddress);
            if (id == 4)
                viewModel.SelectedField = nameof(viewModel.SuburbName);
            if (id == 5)
                viewModel.SelectedField = nameof(viewModel.State);
            if (id == 6)
                viewModel.SelectedField = nameof(viewModel.PhNumber);
            if (id == 7)
                viewModel.SelectedField = nameof(viewModel.IsInsured);
            if (id == 8)
                viewModel.SelectedField = nameof(viewModel.ExperienceLevel);

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(string selectedField, string userType, string firstName, string lastName,
         string streetAddress, string state, string phNumber, bool isInsured, int experienceLevel)
        {
            var viewModel = new EditProfileViewModel
            {
                SelectedField = selectedField,
                UserType = userType,
                FirstName = firstName,
                LastName = lastName,
                StreetAddress = streetAddress,
                State = state,
                PhNumber = phNumber,
                IsInsured = isInsured,
                ExperienceLevel = experienceLevel
            };
            var o = new Owner();
            o = await _context.Owners.FindAsync(UserID);

            var w = new Walker();
            w = await _context.Walkers.FindAsync(UserID);

            //Check if walker or Owner and update fields accordingly
            if (w == null)
            {
                if (selectedField.Equals(nameof(viewModel.FirstName)))
                {
                    o.FirstName = viewModel.FirstName;
                    //set session data
                    HttpContext.Session.SetString(nameof(o.FirstName), o.FirstName);
                }


                if (selectedField.Equals(nameof(viewModel.LastName)))
                    o.LastName = viewModel.LastName;

                if (selectedField.Equals(nameof(viewModel.StreetAddress)))
                    o.StreetAddress = viewModel.StreetAddress;

                if (selectedField.Equals(nameof(viewModel.State)))
                    o.State = viewModel.State;

                if (selectedField.Equals(nameof(viewModel.PhNumber)))
                    o.PhNumber = viewModel.PhNumber;

            }
            else
            {
                if (selectedField.Equals(nameof(viewModel.FirstName)))
                {
                    w.FirstName = viewModel.FirstName;
                    //set session data
                    HttpContext.Session.SetString(nameof(o.FirstName), w.FirstName);
                }


                if (selectedField.Equals(nameof(viewModel.LastName)))
                    w.LastName = viewModel.LastName;

                if (selectedField.Equals(nameof(viewModel.StreetAddress)))
                    w.StreetAddress = viewModel.StreetAddress;

                if (selectedField.Equals(nameof(viewModel.State)))
                    w.State = viewModel.State;

                if (selectedField.Equals(nameof(viewModel.PhNumber)))
                    w.PhNumber = viewModel.PhNumber;

                if (selectedField.Equals(nameof(viewModel.IsInsured)))
                    w.IsInsured = viewModel.IsInsured;

                if (selectedField.Equals(nameof(viewModel.PhNumber)))
                    w.IsInsured = viewModel.IsInsured;

            }

            await _context.SaveChangesAsync();

            return View("Index", viewModel);
        }

    }
}

