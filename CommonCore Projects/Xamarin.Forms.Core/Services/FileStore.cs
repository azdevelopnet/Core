using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Xamarin.Forms.Core
{

    public class FileStore : IFileStore
    {
        private JsonSerializer _serializer;

        private static SemaphoreSlim fileStoreLock = new SemaphoreSlim(1);

        public FileStore()
        {
            _serializer = new JsonSerializer();
        }

        public async Task<bool> FileExists(string contentName)
        {
            await fileStoreLock.WaitAsync();
            return await Task.Run(() => {
                using (var isoStorage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    return isoStorage.FileExists(contentName);
                }
            });
        }

        public async Task<(T Response, bool Success, Exception Error)> GetAsync<T>(string contentName) where T : class, new()
        {
            await fileStoreLock.WaitAsync();
            return await Task.Run(() =>
            {
                (T Response, bool Success, Exception Error) response = (null, false, null);
                try
                {
                    using (var isoStorage = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        if (isoStorage.FileExists(contentName))
                        {
                            try
                            {
                                using (var s = isoStorage.OpenFile(contentName, FileMode.OpenOrCreate))
                                {
									using (var reader = new StreamReader(s))
									{
										using (var json = new JsonTextReader(reader))
										{
											response.Response = _serializer.Deserialize<T>(json);
                                            response.Success = true;
										}
									}
                                }
                            }
                            catch (Exception ex)
                            {
                                response.Error = ex;
                            }
                        }
                        else
                        {
                            response.Error = new ApplicationException("File does not exist");
                        }
                    }

                }
                catch (Exception ex)
                {
                    ex.ConsoleWrite();
					response.Error = ex;
                }
                finally
                {
                    fileStoreLock.Release();
                }
                return response;
            });

        }

        public async Task<(bool Success, Exception Error)> DeleteAsync(string contentName)
        {
            await fileStoreLock.WaitAsync();
            (bool Success, Exception Error) response = (false, null);
            try
            {
                await Task.Run(() =>
                {
                    using (var isoStorage = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        if (isoStorage.FileExists(contentName))
                        {
                            isoStorage.DeleteFile(contentName);
                        }
                        response.Success = true;
                    }
                });
            }
            catch (Exception ex)
            {
                response.Error = ex;
                ex.ConsoleWrite();
            }
            finally
            {
                fileStoreLock.Release();
            }
            return response;
        }

        public async Task<(bool Success, Exception Error)> SaveAsync<T>(string contentName, object obj)
        {
            await fileStoreLock.WaitAsync();
            return await Task.Run(() =>
            {
				(bool Success, Exception Error) response = (false, null);
                try
                {
                    using (var isoStorage = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        using (var s = isoStorage.OpenFile(contentName, FileMode.Create))
                        {
                            using (var sw = new StreamWriter(s))
                            {
                                _serializer.Serialize(new JsonTextWriter(sw), obj);
                                sw.Flush();
                                sw.Close();
                                response.Success = true;
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    response.Error = ex;
                }
                finally
                {
                    fileStoreLock.Release();
                }
                return response;
            });

        }

        public async Task<(string Response, bool Success, Exception Error)> GetStringAsync(string contentName)
        {
            await fileStoreLock.WaitAsync();
            (string Response, bool Success, Exception Error) response = (null, false, null);
            return await Task.Run(() =>
            {
                try
                {
                    using (var isoStorage = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        if (isoStorage.FileExists(contentName))
                        {
                            try
                            {
                                using (var s = isoStorage.OpenFile(contentName, FileMode.OpenOrCreate))
                                {
                                    using (var sr = new StreamReader(s))
                                    {
                                        var content = sr.ReadToEnd();
                                        sr.Close();
                                        response.Response = content;
                                        response.Success = true;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                response.Error = ex;
                            }
                        }
						else
						{
							response.Error = new ApplicationException("File does not exist");
						}
                    }
                }
                catch (Exception ex)
                {
                    response.Error = ex;
                }
                finally
                {
                    fileStoreLock.Release();
                }
                return response;
            });
        }

        public async Task<(bool Success, Exception Error)> SaveStringAsync(string contentName, string obj)
        {
            await fileStoreLock.WaitAsync();

            (bool Success, Exception Error) response = (false, null);
            return await Task.Run(() =>
            {
                try
                {
                    using (var isoStorage = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        using (var s = isoStorage.OpenFile(contentName, FileMode.Create))
                        {
                            using (var sw = new StreamWriter(s))
                            {
                                sw.Write(obj);
                                sw.Flush();
                                sw.Close();
                                response.Success = true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    response.Error = ex;
                }
                finally
                {

                    fileStoreLock.Release();
                }
                return response;
            });
        }

    }
}

