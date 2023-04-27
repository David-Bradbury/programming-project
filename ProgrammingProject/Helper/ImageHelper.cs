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
        public string UploadFile(RegisterViewModel viewModel)
        {
            string fileName = null;
            if (viewModel.ProfileImage != null)
            {
                string uploadDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "img");
                fileName = Guid.NewGuid().ToString() + "-" + viewModel.ProfileImage.FileName;
                string filePath = Path.Combine(uploadDirectory, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    viewModel.ProfileImage.CopyTo(fileStream);
                }
            }
            return fileName;
        }
    }
}
