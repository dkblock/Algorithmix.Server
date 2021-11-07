using Algorithmix.Common.Constants;
using Microsoft.AspNetCore.Http;
using System;

namespace Algorithmix.Api.Core
{
    public class TestDataManager
    {
        private readonly IFileManager _fileManager;
        private readonly Random _random;

        public TestDataManager(IFileManager fileManager)
        {
            _fileManager = fileManager;
            _random = new Random();
        }

        public void CreateTestQuestionImagesDirectory(int testId, bool isPublished = false)
        {
            var dirPath = GetQuestionImagesDirectory(testId, isPublished);

            if (!_fileManager.DirectoryExists(dirPath))
                _fileManager.CreateDirectory(dirPath);
        }

        public void DeleteTestQuestionImagesDirectory(int testId, bool isPublished = false)
        {
            var dirPath = GetQuestionImagesDirectory(testId, isPublished);

            if (_fileManager.DirectoryExists(dirPath))
                _fileManager.DeleteDirectory(dirPath);
        }

        public string CreateTestQuestionImage(int testId, int questionId, IFormFile image, bool isPublished = false)
        {
            var ext = _fileManager.GetFileExtension(image.FileName);
            var fileName = $"question{questionId}_{_random.Next(0, 10000)}.{ext}";
            var dirPath = GetQuestionImagesDirectory(testId, isPublished);
            var filePath = _fileManager.CombinePaths(dirPath, fileName);

            if (!_fileManager.DirectoryExists(dirPath))
                CreateTestQuestionImagesDirectory(testId, isPublished);

            if (_fileManager.FileExists(filePath))
                _fileManager.DeleteFile(filePath);

            _fileManager.CreateFile(image, filePath);

            return filePath;
        }

        public void DeleteTestQuestionImage(string imagePath)
        {
            if (_fileManager.FileExists(imagePath))
                _fileManager.DeleteFile(imagePath);
        }

        public void CopyTestQuestionImagesToPublishedTest(int testId)
        {
            var sourceDirectory = GetQuestionImagesDirectory(testId, false);
            var destinationDirectory = GetQuestionImagesDirectory(testId, true);

            if (!_fileManager.DirectoryExists(sourceDirectory))
                return;

            if (!_fileManager.DirectoryExists(destinationDirectory))
                CreateTestQuestionImagesDirectory(testId, true);

            var sourceFiles = _fileManager.GetDirectoryFiles(sourceDirectory);

            foreach (var file in sourceFiles)
            {
                var fileName = _fileManager.GetFileName(file);
                _fileManager.CopyFile(file, _fileManager.CombinePaths(destinationDirectory, fileName));
            }
        }

        private string GetQuestionImagesDirectory(int testId, bool isPublished)
        {
            var testsDirectory = isPublished
                ? TestQuestionImageDirectories.PublishedTestImagesDirectory
                : TestQuestionImageDirectories.TestImagesDirectory;

            return _fileManager.CombinePaths("images", testsDirectory, $"test_{testId}");
        }
    }
}
