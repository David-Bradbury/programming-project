﻿using Microsoft.AspNetCore.Hosting;
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

        // Uploads file to img folder in wwwroot and returns string to be saved to database.
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
    }
}
