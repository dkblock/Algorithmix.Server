using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Algorithmix.Api.Core
{
    public class TestDataManager
    {
        private readonly IWebHostEnvironment _env;
        private readonly Random _random;

        public TestDataManager(IWebHostEnvironment env)
        {
            _env = env;
            _random = new Random();
        }

        public void CreateTestQuestionImagesDirectory(int testId)
        {
            var path = GetTestQuestionImagesDirectory(testId);
            Directory.CreateDirectory(path);
        }

        public void DeleteTestQuestionImagesDirectory(int testId)
        {
            var path = GetTestQuestionImagesDirectory(testId);

            if (Directory.Exists(path))
                Directory.Delete(path, true);
        }

        public async Task<string> CreateTestQuestionImage(int testId, int questionId, IFormFile image)
        {
            var ext = Path.GetExtension(image.FileName);
            var fileName = $"question{questionId}_{_random.Next(0, 10000)}{ext}";
            var imagesDirectory = GetTestQuestionImagesDirectory(testId);

            if (!Directory.Exists(imagesDirectory))
                CreateTestQuestionImagesDirectory(testId);

            var path = Path.Combine(GetTestQuestionImagesDirectory(testId), fileName);
            var absolutePath = Path.Combine(_env.WebRootPath, path);

            using var stream = new FileStream(absolutePath, FileMode.Create);
            await image.CopyToAsync(stream);

            return path;
        }

        public void DeleteTestQuestionImage(string imagePath)
        {
            var path = Path.Combine(_env.WebRootPath, imagePath);

            if (File.Exists(path))
                File.Delete(path);
        }

        private string GetTestQuestionImagesDirectory(int testId)
        {
            return Path.Combine(_env.WebRootPath, "images", "test-questions", $"test_{testId}");
        }
    }
}
