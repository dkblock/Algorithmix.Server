using Algorithmix.Common.Constants;
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

        public FileStream GetImage(string src)
        {
            var path = Path.Combine(_env.WebRootPath, src);
            return File.OpenRead(path);
        }

        public bool Exists(string src)
        {
            var path = Path.Combine(_env.WebRootPath, src);
            return File.Exists(path);
        }

        public void CreateTestQuestionImagesDirectory(int testId, bool isPublished = false)
        {
            var path = GetTestQuestionImagesDirectory(testId, isPublished);
            Directory.CreateDirectory(path);
        }

        public void DeleteTestQuestionImagesDirectory(int testId, bool isPublished = false)
        {
            var path = GetTestQuestionImagesDirectory(testId, isPublished);

            if (Directory.Exists(path))
                Directory.Delete(path, true);
        }

        public async Task<string> CreateTestQuestionImage(int testId, int questionId, IFormFile image, bool isPublished = false)
        {
            var ext = Path.GetExtension(image.FileName).ToLower();
            var fileName = $"question{questionId}_{_random.Next(0, 10000)}{ext}";
            var imagesDirectory = GetTestQuestionImagesDirectory(testId, isPublished);

            if (!Directory.Exists(imagesDirectory))
                CreateTestQuestionImagesDirectory(testId);

            var absolutePath = Path.Combine(imagesDirectory, fileName);
            var path = absolutePath.Replace($"{_env.WebRootPath}", "").Remove(0, 1);

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

        public void CopyTestQuestionImagesToPublishedTest(int testId)
        {
            var sourceDirectory = GetTestQuestionImagesDirectory(testId, false);
            var targetDirectory = GetTestQuestionImagesDirectory(testId, true);

            if (!Directory.Exists(sourceDirectory))
                return;

            if (!Directory.Exists(targetDirectory))
                CreateTestQuestionImagesDirectory(testId, true);

            foreach (var file in Directory.GetFiles(sourceDirectory))
                File.Copy(file, Path.Combine(targetDirectory, Path.GetFileName(file)));
        }

        private string GetTestQuestionImagesDirectory(int testId, bool isPublished)
        {
            var testsDirectory = isPublished ? TestQuestionImageDirectories.PublishedTestImagesDirectory : TestQuestionImageDirectories.TestImagesDirectory;
            return Path.Combine(_env.WebRootPath, "images", testsDirectory, $"test_{testId}");
        }
    }
}
