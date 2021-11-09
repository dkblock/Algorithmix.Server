using Microsoft.AspNetCore.Http;
using System.IO;

namespace Algorithmix.Api.Core
{
    public interface IAlgorithmDataManager
    {
        void CreateAlgorithmDescription(string algorithmId, IFormFile description);
        void DeleteAlgorithmDescription(string algorithmId);
        void CreateAlgorithmConstructor(string algorithmId, IFormFile description);
        void DeleteAlgorithmConstructor(string algorithmId);
        string CreateAlgorithmImage(string algorithmId, IFormFile image);
        void DeleteAlgorithmImage(string imagePath);
        FileStream GetAlgorithmDataTemplate(string algorithmId);
        void DeleteAlgorithmDataFolder(string algorithmId);
        string DefaultAlgorithmImageUrl { get; }
    }

    public class AlgorithmDataManager : IAlgorithmDataManager
    {
        private readonly IFileManager _fileManager;

        private const string AlgorithmImagesDirectoryPath = "images\\algorithms";
        private const string DefaultAlgorithmImage = "__algorithm-default__.png";

        public AlgorithmDataManager(IFileManager fileManager)
        {
            _fileManager = fileManager;
        }

        public string DefaultAlgorithmImageUrl => _fileManager.CombinePaths(AlgorithmImagesDirectoryPath, DefaultAlgorithmImage);

        public void CreateAlgorithmDescription(string algorithmId, IFormFile description)
        {
            var path = _fileManager.CombinePaths("algorithms", algorithmId, "description");

            if (!_fileManager.DirectoryExists(path))
                _fileManager.CreateDirectory(path);

            _fileManager.ExtractZipFileToDirectory(description, path);
        }

        public void DeleteAlgorithmDescription(string algorithmId)
        {
            var path = _fileManager.CombinePaths("algorithms", algorithmId, "description");
            _fileManager.DeleteDirectory(path);
        }

        public void CreateAlgorithmConstructor(string algorithmId, IFormFile constructor)
        {
            var path = _fileManager.CombinePaths("algorithms", algorithmId, "constructor");

            if (!_fileManager.DirectoryExists(path))
                _fileManager.CreateDirectory(path);

            _fileManager.ExtractZipFileToDirectory(constructor, path);
        }

        public void DeleteAlgorithmConstructor(string algorithmId)
        {
            var path = _fileManager.CombinePaths("algorithms", algorithmId, "constructor");
            _fileManager.DeleteDirectory(path);
        }

        public string CreateAlgorithmImage(string algorithmId, IFormFile image)
        {
            var ext = _fileManager.GetFileExtension(image.FileName);
            var fileName = $"{algorithmId}.{ext}";
            var filePath = _fileManager.CombinePaths(AlgorithmImagesDirectoryPath, fileName);

            _fileManager.CreateFile(image, filePath);

            return filePath;
        }

        public void DeleteAlgorithmImage(string imagePath)
        {
            if (imagePath != DefaultAlgorithmImageUrl)
                _fileManager.DeleteFile(imagePath);
        }

        public FileStream GetAlgorithmDataTemplate(string algorithmId)
        {
            var filePath = _fileManager.CombinePaths("dev-tools", "temp", $"{algorithmId}.zip");

            if (!_fileManager.FileExists(filePath))
            {
                var tempDirectory = _fileManager.CombinePaths("dev-tools", "temp");

                if (!_fileManager.DirectoryExists(tempDirectory))
                    _fileManager.CreateDirectory(tempDirectory);

                var sourceFilePath = _fileManager.CombinePaths("dev-tools", "dev-tools.zip");
                var targetFilePath = filePath;

                _fileManager.CopyFile(sourceFilePath, targetFilePath);
                _fileManager.CopyFileWithReplaceInZipArchive(filePath, "index.html", $"{algorithmId}.html");
            }

            return _fileManager.GetFile(filePath);
        }

        public void DeleteAlgorithmDataFolder(string algorithmId)
        {
            var path = _fileManager.CombinePaths("algorithms", algorithmId);
            _fileManager.DeleteDirectory(path);
        }
    }
}
