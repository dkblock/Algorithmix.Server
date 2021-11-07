using Microsoft.AspNetCore.Http;

namespace Algorithmix.Api.Core
{
    public interface IAlgorithmDataManager
    {
        string CreateAlgorithmImage(string algorithmId, IFormFile image);
        void DeleteAlgorithmImage(string imagePath);
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
    }
}
