using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Algorithmix.Api.Core
{
    public interface IFileManager
    {
        void CreateFile(IFormFile file, string path);
        bool FileExists(string path);
        FileStream GetFile(string path);
        void DeleteFile(string path);
        void CopyFile(string sourceFile, string destinationFile);
        string GetFileExtension(string path);
        string GetFileName(string path);

        void CreateDirectory(string path);
        bool DirectoryExists(string path);
        void DeleteDirectory(string path);
        string[] GetDirectoryFiles(string path);

        string CombinePaths(params string[] paths);
    }

    public class FileManager : IFileManager
    {
        private readonly IWebHostEnvironment _env;

        public FileManager(IWebHostEnvironment env)
        {
            _env = env;
        }

        public void CreateFile(IFormFile file, string path)
        {
            var filePath = GetAbsolutePath(path);
            using var stream = new FileStream(filePath, FileMode.Create);
            file.CopyTo(stream);
        }

        public bool FileExists(string path)
        {
            var filePath = GetAbsolutePath(path);
            return File.Exists(filePath);
        }

        public FileStream GetFile(string path)
        {
            var filePath = GetAbsolutePath(path);
            return File.OpenRead(filePath);
        }

        public void CopyFile(string sourceFilePath, string destinationFilePath)
        {
            File.Copy(sourceFilePath, destinationFilePath, true);
        }

        public string GetFileExtension(string path)
        {
            return Path.GetExtension(path).Replace(".", "").ToLower();
        }

        public string GetFileName(string path)
        {
            return Path.GetFileName(path);
        }

        public void DeleteFile(string path)
        {
            var filePath = GetAbsolutePath(path);
            File.Delete(filePath);
        }

        public void CreateDirectory(string path)
        {
            var dirPath = GetAbsolutePath(path);
            Directory.CreateDirectory(dirPath);
        }

        public bool DirectoryExists(string path)
        {
            var dirPath = GetAbsolutePath(path);
            return Directory.Exists(dirPath);
        }

        public void DeleteDirectory(string path)
        {
            var dirPath = GetAbsolutePath(path);
            Directory.Delete(dirPath);
        }

        public string[] GetDirectoryFiles(string path)
        {
            var dirPath = GetAbsolutePath(path);
            return Directory.GetFiles(dirPath);
        }

        public string CombinePaths(params string[] paths)
        {
            return Path.Combine(paths);
        }

        private string GetAbsolutePath(string path)
        {
            return CombinePaths(_env.WebRootPath, path);
        }
    }
}
