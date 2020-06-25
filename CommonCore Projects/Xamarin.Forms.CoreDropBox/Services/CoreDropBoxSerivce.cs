using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Dropbox.Api;
using Dropbox.Api.Files;
using ModernHttpClient;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;
using Dropbox.Api.Common;
using Xamarin.Essentials;

namespace Xamarin.Forms.Core
{
    public class CoreDropBoxSerivce: ICoreDropBoxService
    {
        private static string AppKeyDropboxtoken = CoreSettings.Config.Dropbox.DropBoxAppKey;
        private static string ClientId = CoreSettings.Config.Dropbox.DropBoxSecret;
        private static string RedirectUri = CoreSettings.Config.Dropbox.RedirectUri;
        private string AccessToken { get; set; }

        protected IFileStore Store
        {
            get
            {
                return CoreDependencyService.GetService<IFileStore, FileStore>(true);
            }
        }

        public async Task<(string response, Exception error)> Authenticate()
        {
            try
            {
                var storedToken = await this.GetAccessTokenFromSettings();
                if (storedToken)
                {
                    return (this.AccessToken, null);
                }
                else
                {
                    var state = Guid.NewGuid().ToString("N");
                    var url = $"https://www.dropbox.com/oauth2/authorize?client_id={ClientId}&state={state}&response_type=token&redirect_uri={RedirectUri}";
                    var authResult = await WebAuthenticator.AuthenticateAsync(
                        new Uri(url),
                        new Uri(RedirectUri));

                    this.AccessToken = authResult?.AccessToken;
                    await SaveDropboxToken(AccessToken);
                    return (this.AccessToken, null);
                }

            }
            catch (Exception ex)
            {
                return (null, ex);
            }

        }

        public async Task<Dropbox.Api.Users.FullAccount> GetAccount()
        {
            using (var client = await this.GetClient())
            {
                return await client.Users.GetCurrentAccountAsync();
            }
        }

        public async Task<(IList<Metadata> Entries,Exception Error)> ListFiles()
        {
            try
            {
                using (var client = await this.GetClient())
                {
                    var list = await client.Files.ListFolderAsync(string.Empty);
                    return (list?.Entries, null);
                }
            }
            catch (Exception ex)
            {
                return (new List<Metadata>(), ex);
            }
        }

        public async Task<(IList<Metadata> Entries, Exception Error)> ListFiles(string path, bool ensurePath = false)
        {
            try
            {
                using (var client = await this.GetClient())
                {
                    if (ensurePath)
                    {
                        var exists = await FolderExists(client, path);
                        if (!exists)
                        {
                            await client.Files.CreateFolderAsync(new CreateFolderArg(path));
                        }
                    }

                    var list = await client.Files.ListFolderAsync(new ListFolderArg(path));
                    return (list?.Entries, null);
                }
            }
            catch (Exception ex)
            {
                return (new List<Metadata>(), ex);
            }
        }

        public async Task<bool> FolderExists(DropboxClient client, string path)
        {
            try
            {
                var results = await client.Files.GetMetadataAsync(new GetMetadataArg(path));
                if (results != null)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }

        }

        public async Task<DropBoxImage> GetImageFileFromPath(string path)
        {
            using (var client = await this.GetClient())
            {
                var list = new List<DropBoxImage>();
                var argList = new List<ThumbnailArg>();
                argList.Add(new ThumbnailArg(path, size: ThumbnailSize.W640h480.Instance));

                var result = await client.Files.GetThumbnailBatchAsync(new GetThumbnailBatchArg(argList));
                if (result != null && result.Entries != null)
                {
                    foreach (var entry in result.Entries)
                    {
                        if (entry.IsSuccess)
                        {
                            list.Add(new DropBoxImage()
                            {
                                Id = entry.AsSuccess.Value.Metadata.Id,
                                Name = entry.AsSuccess.Value.Metadata.Name,
                                Path = entry.AsSuccess.Value.Metadata.PathLower,
                                Base64Image = entry.AsSuccess.Value.Thumbnail
                            });

                        }
                    }
                }

                return list.First();

            }
        }

        public async Task<IList<DropBoxImage>> GetImageListFiles(List<Metadata> files)
        {
            using (var client = await this.GetClient())
            {
                var list = new List<DropBoxImage>();
                var argList = new List<ThumbnailArg>();
                files.ForEach(x => argList.Add(new ThumbnailArg(x.PathLower, size: ThumbnailSize.W256h256.Instance)));

                var result = await client.Files.GetThumbnailBatchAsync(new GetThumbnailBatchArg(argList));
                if (result != null && result.Entries != null)
                {
                    foreach (var entry in result.Entries)
                    {
                        if (entry.IsSuccess)
                        {
                            list.Add(new DropBoxImage()
                            {
                                Id = entry.AsSuccess.Value.Metadata.Id,
                                Name = entry.AsSuccess.Value.Metadata.Name,
                                Path = entry.AsSuccess.Value.Metadata.PathLower,
                                Base64Image = entry.AsSuccess.Value.Thumbnail
                            });

                        }
                    }
                }

                return list;

            }
        }

        public async Task<(bool Success, Exception Error)> MoveFolder(string oldPath, string newPath)
        {
            try
            {
                using (var client = await this.GetClient())
                {
                    await client.Files.MoveAsync(oldPath, newPath);
                    return (true, null);

                }

            }
            catch (Exception ex)
            {
                return (false, ex);
            }
        }

        public async Task<(byte[] content, Exception error)> ReadFile(string file)
        {
            try
            {
                using (var client = await this.GetClient())
                {
                    var response = await client.Files.DownloadAsync(file);
                    var bytes = await response?.GetContentAsByteArrayAsync();
                    var xxx = bytes;

                    return (bytes, null);
                }
            }
            catch (Exception ex)
            {
                return (null, ex);
            }
        }

        public async Task<(byte[] content, Exception error)> ReadFile(string file, DropboxClient client)
        {
            try
            {

                var response = await client.Files.DownloadAsync(file);
                var bytes = response?.GetContentAsByteArrayAsync();
                return (bytes?.Result, null);

            }
            catch (Exception ex)
            {
                return (null, ex);
            }
        }

        public async Task<(T response, Exception error)> ReadObject<T>(string filePath, DropboxClient client) where T : class, new()
        {
            try
            {
                var folder = filePath.Substring(0, filePath.LastIndexOf("/"));
                var folderExists = await FolderExists(client, folder);
                if (!folderExists)
                {
                    await client.Files.CreateFolderAsync(new CreateFolderArg(folder));
                }

                var response = await client.Files.DownloadAsync(filePath);
                var str = await response?.GetContentAsStringAsync();
                return (JsonConvert.DeserializeObject<T>(str), null);

            }
            catch (Exception ex)
            {
                return (null, ex);
            }
        }

        public async Task<(T response, Exception error)> ReadObject<T>(string filePath) where T : class, new()
        {
            try
            {

                using (var client = await this.GetClient())
                {
                    var folder = filePath.Substring(0, filePath.LastIndexOf("/"));
                    var folderExists = await FolderExists(client, folder);
                    if (!folderExists)
                    {
                        await client.Files.CreateFolderAsync(new CreateFolderArg(folder));
                    }

                    var response = await client.Files.DownloadAsync(filePath);
                    var str = await response?.GetContentAsStringAsync();
                    return (JsonConvert.DeserializeObject<T>(str), null);
                }
            }
            catch (Exception ex)
            {
                return (null, ex);
            }
        }

        public async Task<(FileMetadata file, Exception error)> WriteFile(byte[] fileContent, string filename)
        {
            try
            {
                var commitInfo = new CommitInfo(filename, WriteMode.Overwrite.Instance, false, DateTime.Now);
                using (var client = await this.GetClient())
                {
                    var metadata = await client.Files.UploadAsync(commitInfo, new MemoryStream(fileContent));
                    return (metadata, null);
                }
            }
            catch (Exception ex)
            {
                return (null, ex);
            }
        }

        public async Task<DropboxClient> GetClient()
        {

            var handler = new NativeMessageHandler()
            {
                AllowAutoRedirect = CoreSettings.Config.HttpSettings.HttpAllowAutoRedirect,
            };

            var config = new DropboxClientConfig() { HttpClient = new HttpClient(handler, true) };

            var client = new DropboxClient(this.AccessToken, config);
            var account = await client.Users.GetCurrentAccountAsync();
            return client.WithPathRoot(new PathRoot.Root(account.RootInfo.RootNamespaceId));
        }

        public async Task CreateFolderAsync(DropboxClient client, string folder)
        {
            await client.Files.CreateFolderAsync(new CreateFolderArg(folder));
        }

        public async Task CopyAsync(DropboxClient client, string originalFile, string newFile)
        {
            await client.Files.CopyAsync(originalFile, newFile);
        }

        private async Task SaveDropboxToken(string token)
        {
            if (token == null)
            {
                return;
            }

            //if (Application.Current.Properties.ContainsKey(AppKeyDropboxtoken))
            //{
            //    Application.Current.Properties["AppKeyDropboxtoken"] = token;
            //}
            //else
            //{
            //    Application.Current.Properties.Add(AppKeyDropboxtoken, token);
            //}
            //await Application.Current.SavePropertiesAsync();

            await this.Store.SaveStringAsync("AppKeyDropboxtoken", token);
        


        }

        private async Task<bool> GetAccessTokenFromSettings()
        {
            try
            {
                //if (!Application.Current.Properties.ContainsKey(AppKeyDropboxtoken))
                //{
                //    return (false, new ApplicationException("Does not exist"));
                //}

                //this.AccessToken = Application.Current.Properties[AppKeyDropboxtoken]?.ToString();
                //if (this.AccessToken != null)
                //{
                //    return (true,null);
                //}
                //else
                //{
                //    return (false, new ApplicationException("Does not exist"));
                //}

                var result = await this.Store.GetStringAsync("AppKeyDropboxtoken");
                if (result.Success)
                {
                    this.AccessToken = result.Response;
                    return true;
                }
                else
                {
                    return false;
                }


            }
            catch (Exception ex)
            {
                return (false);
            }
        }

    }
}
