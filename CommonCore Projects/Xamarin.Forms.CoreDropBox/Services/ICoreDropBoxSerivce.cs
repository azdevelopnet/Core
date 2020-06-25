using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dropbox.Api;
using Dropbox.Api.Files;

namespace Xamarin.Forms.Core
{
    public interface ICoreDropBoxService
    {
        Task<(string response, Exception error)> Authenticate();
        Task<Dropbox.Api.Users.FullAccount> GetAccount();
        Task<(IList<Metadata> Entries, Exception Error)> ListFiles();
        Task<(IList<Metadata> Entries, Exception Error)> ListFiles(string path, bool ensurePath = false);
        Task<bool> FolderExists(DropboxClient client, string path);
        Task<DropBoxImage> GetImageFileFromPath(string path);
        Task<IList<DropBoxImage>> GetImageListFiles(List<Metadata> files);
        Task<(bool Success, Exception Error)> MoveFolder(string oldPath, string newPath);
        Task<(byte[] content, Exception error)> ReadFile(string file);
        Task<(byte[] content, Exception error)> ReadFile(string file, DropboxClient client);
        Task<(T response, Exception error)> ReadObject<T>(string filePath, DropboxClient client) where T : class, new();
        Task<(T response, Exception error)> ReadObject<T>(string filePath) where T : class, new();
        Task<(FileMetadata file, Exception error)> WriteFile(byte[] fileContent, string filename);
        Task<DropboxClient> GetClient();
        Task CreateFolderAsync(DropboxClient client, string folder);
        Task CopyAsync(DropboxClient client, string originalFile, string newFile);
    }
}
