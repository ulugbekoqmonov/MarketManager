using MarketManager.Application.Common.Interfaces;
using Microsoft.AspNetCore.Hosting;

namespace MarketManager.Application.Common.Services
{
    public class DeleteImg : IDeleteImg
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DeleteImg(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public void Delete_Img(string img)
        {
            string uplodFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
            string filePath = Path.Combine(uplodFolder, img);
            FileInfo fileInfo = new FileInfo(filePath);
            if (fileInfo.Exists)
            {
                fileInfo.Delete();
            }
            Console.WriteLine();
        }
    }
}
