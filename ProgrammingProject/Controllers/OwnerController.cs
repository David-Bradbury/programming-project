using ProgrammingProject.Data;
using ProgrammingProject.Models;
using ProgrammingProject.Filters;
using Microsoft.AspNetCore.Mvc;
using ProgrammingProject.Helper;
using System.Text.RegularExpressions;
using System.Net;

namespace ProgrammingProject.Controllers
{
  //Mask URL
  [Route("/Owner")]


  public class OwnerController : Controller
  {
    private readonly EasyWalkContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public AddDogViewModel viewModel = new AddDogViewModel();
    private int OwnerID => HttpContext.Session.GetInt32(nameof(Owner.UserId)).Value;

    public OwnerController(EasyWalkContext context, IWebHostEnvironment webHostEnvironment)
    {
      _context = context;
      _webHostEnvironment = webHostEnvironment;
    }


    [AuthorizeUser]
    public async Task<IActionResult> Index()
    {
      var owner = await _context.Owners.FindAsync(OwnerID);
      return View(owner);
    }

    //Add a dog to the owner
    [Route("/Owner/AddDog",
 Name = "AddDog")]
    public async Task<IActionResult> AddDog()
    {
      viewModel.DogSizeList = DropDownLists.GetDogSize();
      viewModel.TemperamentList = DropDownLists.GetTemperament();
      viewModel.TrainingLevelList = DropDownLists.GetTrainingLevel();
      viewModel.StatesList = DropDownLists.GetStates();
      viewModel.IsVaccinatedList = DropDownLists.GetVaccinatedList();

      return View(viewModel);
    }

    [Route("/Owner/AddDog",
Name = "AddDog")]
    [HttpPost]
    public async Task<IActionResult> AddDog(AddDogViewModel viewModel)
    {
      //var viewModel = new AddDogViewModel();

      viewModel.DogSizeList = DropDownLists.GetDogSize();
      viewModel.TemperamentList = DropDownLists.GetTemperament();
      viewModel.TrainingLevelList = DropDownLists.GetTrainingLevel();
      viewModel.StatesList = DropDownLists.GetStates();
      viewModel.IsVaccinatedList = DropDownLists.GetVaccinatedList();

      var owner = new Owner();
      foreach (var o in _context.Owners)
        if (o.UserId == OwnerID)
          owner = o;

      if (viewModel.Name == null)
        ModelState.AddModelError(nameof(viewModel.Name), "Dogs Name is required.");
      if (viewModel.Breed == null)
        ModelState.AddModelError(nameof(viewModel.Breed), "Dogs Breed is required.");
      if (viewModel.MicrochipNumber == null)
        ModelState.AddModelError(nameof(viewModel.MicrochipNumber), "Dogs Microchip Number is required.");
      if (viewModel.IsVaccinated == null)
        ModelState.AddModelError(nameof(viewModel.IsVaccinated), "Dogs Vaccination Status is required.");
      if (viewModel.Temperament == null)
        ModelState.AddModelError(nameof(viewModel.Temperament), "Dogs Temperament is required.");
      if (viewModel.DogSize == null)
        ModelState.AddModelError(nameof(viewModel.DogSize), "Dogs Size is required.");
      if (viewModel.TrainingLevel == null)
        ModelState.AddModelError(nameof(viewModel.TrainingLevel), "Dogs Training Level is required.");
      //if (image == null)
      //   ModelState.AddModelError(nameof(image), "Dogs Image is required.");
      if (viewModel.BusinessName == null)
        ModelState.AddModelError(nameof(viewModel.BusinessName), "Vets Business Name is required.");
      if (viewModel.PhNumber == null)
        ModelState.AddModelError(nameof(viewModel.PhNumber), "Vets Phone Number is required.");
      if (viewModel.Email == null)
        ModelState.AddModelError(nameof(viewModel.Email), "Vets Email is required.");
      if (viewModel.StreetAddress == null)
        ModelState.AddModelError(nameof(viewModel.StreetAddress), "Vets Street Address is required.");
      if (viewModel.SuburbName == null)
        ModelState.AddModelError(nameof(viewModel.SuburbName), "Vets Suburb is required.");
      if (viewModel.Postcode == null)
        ModelState.AddModelError(nameof(viewModel.Postcode), "Vets Postcode is required.");
      if (viewModel.State == null)
        ModelState.AddModelError(nameof(viewModel.State), "Vets State is required.");
      if (viewModel.Country == null)
        ModelState.AddModelError(nameof(viewModel.Country), "Vets Country is required.");

      if (!Regex.IsMatch(viewModel.Postcode, @"(^0[289][0-9]{2}\s*$)|(^[1-9][0-9]{3}\s*$)"))
        ModelState.AddModelError(nameof(viewModel.Postcode), "This postcode does not match any Australian postcode. Please enter an Australian 4 digit postcode");
      // Not perfect and needs updates for proper Australian phone numbers.
      if (!Regex.IsMatch(viewModel.PhNumber, @"^(\+?\(61\)|\(\+?61\)|\+?61|(0[1-9])|0[1-9])?( ?-?[0-9]){7,9}$"))
        ModelState.AddModelError(nameof(viewModel.PhNumber), "This is not a valid Australian mobile phone number. Please enter a valid Australian mobile phone number");
      if (!Regex.IsMatch(viewModel.Email, @"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+\s?$"))
        ModelState.AddModelError(nameof(viewModel.Email), "This is not a valid email address. Please enter a valid email address");
      // Maybe This Error Message IS Not Enough, maybe a pop up or something needs to occur. Discuss at nextr meeting. JC
      if (viewModel.IsVaccinated.Equals("Unvaccinated"))
        ModelState.AddModelError(nameof(viewModel.IsVaccinated), "Sorry, All dogs must be vaccinated before being registered with EasyWalk");

      // Checks the extension of the file to ensure a certain file format. Bring up Thurs meeting to see if extra verification needed. JC.
      string filename = Path.GetFileName(viewModel.DogImage.FileName);
      string extension = Path.GetExtension(filename).ToLower();

      if (extension != ".jpg" && extension != ".jpeg" && extension != ".png")
        ModelState.AddModelError(nameof(viewModel.DogImage), "Image must be of the jpg/jpeg on png format");


      // Checking to see if the state of the model is valid before continuing.
      if (!ModelState.IsValid)
      {
        return View(viewModel);
      }

      string imageFileName = UploadFile(viewModel);

      // Creating suburb based on form details
      var suburb = new Suburb();
      suburb.SuburbName = viewModel.SuburbName;
      suburb.Postcode = viewModel.Postcode;

      // Check is Suburb is already known to Easy Walk DB, and rejects entry if known.
      bool match = false;
      foreach (var s in _context.Suburbs)
      {
        if (s.Postcode == viewModel.Postcode && s.SuburbName == viewModel.SuburbName)
        {
          match = true;
          suburb = s;
        }
      }
      if (!match)
        _context.Suburbs.Add(suburb);


      // Create a new Vet from form submission.
      var vet = new Vet();

      vet.BusinessName = viewModel.BusinessName;
      vet.PhNumber = viewModel.PhNumber;
      vet.Email = viewModel.Email;
      vet.StreetAddress = viewModel.StreetAddress;
      vet.Suburb = suburb;
      vet.State = viewModel.State;
      vet.Country = viewModel.Country;

      // Checking BusinessName for now but this is wrong as BusinessName is not key.
      match = false;
      foreach (var v in _context.Vets)
      {
        if (v.BusinessName == viewModel.BusinessName)
        {
          match = true;
          vet = v;
        }
      }
      if (!match)
        _context.Vets.Add(vet);


      // Create a new dog from form submission.
      var dog = new Dog();

      dog.Name = viewModel.Name;
      dog.Breed = viewModel.Breed;
      dog.MicrochipNumber = viewModel.MicrochipNumber;
      dog.DogImage = imageFileName;
      dog.Owner = owner;
      dog.Vet = vet;
      dog.IsVaccinated = true;

      if (viewModel.Temperament.Equals("NonReactive"))
        dog.Temperament = Temperament.NonReactive;
      if (viewModel.Temperament.Equals("Calm"))
        dog.Temperament = Temperament.Calm;
      if (viewModel.Temperament.Equals("Friendly"))
        dog.Temperament = Temperament.Friendly;
      if (viewModel.Temperament.Equals("Reactive"))
        dog.Temperament = Temperament.Reactive;
      if (viewModel.Temperament.Equals("Agressive"))
        dog.Temperament = Temperament.Aggressive;

      if (viewModel.DogSize.Equals("Small"))
        dog.DogSize = DogSize.Small;
      if (viewModel.DogSize.Equals("Medium"))
        dog.DogSize = DogSize.Medium;
      if (viewModel.DogSize.Equals("Large"))
        dog.DogSize = DogSize.Large;
      if (viewModel.DogSize.Equals("ExtraLarge"))
        dog.DogSize = DogSize.ExtraLarge;

      if (viewModel.TrainingLevel.Equals("None"))
        dog.TrainingLevel = TrainingLevel.None;
      if (viewModel.TrainingLevel.Equals("Basic"))
        dog.TrainingLevel = TrainingLevel.Basic;
      if (viewModel.TrainingLevel.Equals("Fully"))
        dog.TrainingLevel = TrainingLevel.Fully;

      _context.Add(dog);

      _context.SaveChanges();


      return RedirectToAction(nameof(Index));
    }

    private string UploadFile(AddDogViewModel viewModel)
    {
      string fileName = null;
      if (viewModel.DogImage != null)
      {
        string uploadDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "img");
        fileName = Guid.NewGuid().ToString() + "-" + viewModel.DogImage.FileName;
        string filePath = Path.Combine(uploadDirectory, fileName);
        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
          viewModel.DogImage.CopyTo(fileStream);
        }
      }
      return fileName;
    }




    //    owner.Dogs.Add(
    //    new Dog
    //    {
    //        DogSize = size,
    //        Temperament = temperament,
    //        ID = id,
    //        Name = name,
    //        MicrochipNumber = microchip,
    //        IsVaccinated = IsVaccinated,
    //        Owner = owner,
    //        Vet = vet
    //    });

    //    await _context.SaveChangesAsync();

    //    return RedirectToAction(nameof(Index));
    //}

    //// Match suitable walkers to the dog

    //public async Task<IActionResult> MatchWalkersToDog(int id) => View(await _context.Dogs.FindAsync(id));

    //[HttpPost]
    //public async Task MatchWalkersToDog(int id)
    //{
    //    var dog = await _context.Dogs.FindAsync(id);


    //    var walkers = await _context.Walker.FindAsync();


    //    var tempList = new List<Walker>();

    //    var score = 0;

    //    // Sets a difficulty score to the dog
    //    if (dog != null)
    //    {
    //        score += (int)dog.DogSize;
    //        score += (int)dog.Temperament;
    //    }
    //    foreach (var walker in walkers)
    //    {
    //        // Below matches dog to suitable Walkers
    //        if ((int)walker.ExperienceLevel == 4)
    //        {
    //            tempList.Add(walker);
    //        }
    //        else if ((int)walker.ExperienceLevel == 3 && score < 7)
    //        {
    //            tempList.Add(walker);
    //        }
    //        else if ((int)walker.ExperienceLevel == 2 && score < 5)
    //        {
    //            tempList.Add(walker);
    //        }
    //        else if ((int)walker.ExperienceLevel == 1 && score < 3)
    //        {
    //            tempList.Add(walker);
    //        }
    //    }

    //    // Could add logic here to filter list down based on user preferences.
    //    // E.g.Location or dates/times. Some filter work started below...

    //    // Notes to discuss: AllowUninsured + min experiencelevel as a question
    //    // when adding dog (saved to model). Allows user to set requirements and
    //    // makes filtering easy (as per below).

    //    foreach (var temp in tempList)
    //    {
    //        if (temp.IsInsured == false /*&& dog.Owner.AllowUninsured == false */)
    //            tempList.Remove(temp);
    //        //if (temp.ExperienceLevel > dog.MinimumAllowedExperienceLevel)
    //        //    tempList.Remove(temp);
    //    }


    //    // Return tempList to View. View to list suitable walkers
    //    // with contact details/button for the owner to select.
    //    // Might be worth returning an IPagedList .DP
    //    ViewBag.Walker = tempList;

    //}


  }

}


