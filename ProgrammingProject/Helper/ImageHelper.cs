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

        private string UploadFile(EditDogProfileViewModel viewModel)
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
        private string UploadFile(object viewModel)
        {
            RegisterViewModel vm = new RegisterViewModel();
            if (viewModel.GetType() == typeof(RegisterViewModel))              
                vm = (RegisterViewModel)viewModel;

            string fileName = null;
            if (vm.ProfileImage != null)
            {
                string uploadDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "img");
                fileName = Guid.NewGuid().ToString() + "-" + vm.ProfileImage.FileName;
                string filePath = Path.Combine(uploadDirectory, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    vm.ProfileImage.CopyTo(fileStream);
                }
            }
            return fileName;
        }
    }
}
