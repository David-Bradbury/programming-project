using Microsoft.AspNetCore.Hosting;
using ProgrammingProject.Data;
using ProgrammingProject.Models;

namespace ProgrammingProject.Helper
{
    public class ImageHelper
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ImageHelper(IWebHostEnvironment webHostEnvironment)
        {    
            _webHostEnvironment = webHostEnvironment;
        }
     
        public string UploadFile(IFormFile image)
        {         
            string fileName = null;
            if (image != null)
            {
                string uploadDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "img");
                fileName = Guid.NewGuid().ToString() + "-" + image.FileName;
                string filePath = Path.Combine(uploadDirectory, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(fileStream);
                }
            }
            return fileName;
        }
        // Kept for time being to return to if problems arise with new method.
        //private string UploadFile(AddDogViewModel viewModel)
        //{
        //    string fileName = null;
        //    if (viewModel.ProfileImage != null)
        //    {
        //        string uploadDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "img");
        //        fileName = Guid.NewGuid().ToString() + "-" + viewModel.ProfileImage.FileName;
        //        string filePath = Path.Combine(uploadDirectory, fileName);
        //        using (var fileStream = new FileStream(filePath, FileMode.Create))
        //        {
        //            viewModel.ProfileImage.CopyTo(fileStream);
        //        }
        //    }
        //    return fileName;
        //}
    }
}
