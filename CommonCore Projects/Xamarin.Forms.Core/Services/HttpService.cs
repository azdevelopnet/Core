using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Essentials;
using ModernHttpClient;

namespace Xamarin.Forms.Core
{

    public class HttpService : IHttpService, IDisposable
    {
        private HttpClient httpClient;
        private HttpMessageHandler handler;
        private JsonSerializer _serializer;
        public string json { get; set; }

        public bool IsConnected { get; set; }

        public HttpService()
        {
            if(Connectivity.NetworkAccess==Essentials.NetworkAccess.None || Connectivity.NetworkAccess == Essentials.NetworkAccess.Unknown)
            {
                IsConnected = false;
            }
            else
            {
                IsConnected = true;
            }
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }
        public HttpClient Client
        {
            get
            {
                if (httpClient == null)
                {
                    _serializer = new JsonSerializer();

#if __IOS__
                   
                    switch (CoreSettings.Config.HttpSettings.IOSHttpHandler)
                    {
                        case "ModernHttpClient":
                            handler = new NativeMessageHandler()
                            {
                                AllowAutoRedirect = CoreSettings.Config.HttpSettings.HttpAllowAutoRedirect,
                            };

                            break;
                        case "CFNetwork":
                            handler = new CFNetworkHandler()
                            {
                                AllowAutoRedirect = CoreSettings.Config.HttpSettings.HttpAllowAutoRedirect
                            };
                            break;
                        case "NSURLSession":
                            handler = new NSUrlSessionHandler()
                            {
                                AllowAutoRedirect = CoreSettings.Config.HttpSettings.HttpAllowAutoRedirect
                            };
                            break;
                        default:
                            handler = new HttpClientHandler()
                            {
                                AllowAutoRedirect = CoreSettings.Config.HttpSettings.HttpAllowAutoRedirect
                            };

                            break;
                    }

#elif __ANDROID__
                    switch (CoreSettings.Config.HttpSettings.AndroidHttpHandler)
                    {
                        case "ModernHttpClient":
                            handler = new NativeMessageHandler()
                            {
                                AllowAutoRedirect = CoreSettings.Config.HttpSettings.HttpAllowAutoRedirect,
                            };
                            break;
                        case "AndroidClientHandler":
                            handler = new Xamarin.Android.Net.AndroidClientHandler()
                            {
                                AllowAutoRedirect = CoreSettings.Config.HttpSettings.HttpAllowAutoRedirect
                            };
                            break;
                        default:
                            handler = new HttpClientHandler()
                            {
                                AllowAutoRedirect = CoreSettings.Config.HttpSettings.HttpAllowAutoRedirect,
                            };
                            break;
                    }
#else
            handler = new HttpClientHandler();
#endif


                    if (CoreSettings.Config.HttpSettings.HttpTimeOut > 0)
                    {
                        httpClient = new HttpClient(handler, true) { Timeout = new TimeSpan(0, 0, CoreSettings.Config.HttpSettings.HttpTimeOut) };
                    }
                    else
                    {
                        httpClient = new HttpClient(handler, true);
                        httpClient.Timeout = Timeout.InfiniteTimeSpan;
                    }
                }

                return httpClient;
            }
            set { httpClient = value; }
        }
        public bool ContainsHeader(string name) 
        {
            return Client.DefaultRequestHeaders.Contains(name);
        }
        public void AddHeader(string name, string value) 
        {
            if (Client.DefaultRequestHeaders.Contains(name)) 
            {
                Client.DefaultRequestHeaders.Remove(name);
            }
            Client.DefaultRequestHeaders.TryAddWithoutValidation(name, value);
        }
        public void RemoveHeader(string name) 
        {
            if (Client.DefaultRequestHeaders.Contains(name))
            {
                Client.DefaultRequestHeaders.Remove(name);
            }
        }
        public void AddTokenHeader(string token)
        {
            if (Client.DefaultRequestHeaders.Authorization != null)
                Client.DefaultRequestHeaders.Remove("Authorization");

            Client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + token);
        }

        public void AddTokenHeader(AuthenticationModel coreAuth)
        {
            if (Client.DefaultRequestHeaders.Authorization != null)
                Client.DefaultRequestHeaders.Remove("Authorization");

            Client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + coreAuth.Token);
        }

        public void AddNetworkCredentials(NetworkCredential cred)
        {
            if (handler != null)
            {
                var prop = handler.GetType().GetProperty("Credentials");
                if (prop != null)
                    prop.SetValue(handler, cred, null);
            }
        }

        public async Task<(string Response, bool Success, Exception Error)> FormPost(string url, HttpContent content, CancellationToken? ct = null)
        {

            if (!IsConnected)
            {
                return (null, false, new ApplicationException("Network Connection Error"));
            }

            try
            {
                var token = ct ?? CancellationToken.None;

                await new SynchronizationContextRemover();

                var postResponse = await Client.PostAsync(url, content, token).ConfigureAwait(false);
                postResponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                postResponse.EnsureSuccessStatusCode();

                var raw = await postResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (raw != null)
                {
                    return (raw, true, null);
                }
                else
                {
                    return (null, false, new ApplicationException("Return value is empty"));
                }


            }
            catch (Exception ex)
            {
                ex.ConsoleWrite();
                return (null, false, ex);
            }
        }

        public async Task<(string Response, bool Success, Exception Error)> GetRaw(string url, CancellationToken? ct = null)
        {
            if (!IsConnected)
            {
                return (null, false, new ApplicationException("Network Connection Error"));
            }

            try
            {
                var token = ct ?? CancellationToken.None;

                await new SynchronizationContextRemover();

                using (var srvResponse = await Client.GetAsync(url, token).ConfigureAwait(false))
                {
                    var jsonResult = await srvResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (srvResponse.StatusCode == HttpStatusCode.OK)
                    {
                        return (jsonResult, true, null);
                    }
                    else
                    {
                        return (null, false, new ApplicationException(jsonResult));
                    }

                }

            }
            catch (Exception ex)
            {
                ex.ConsoleWrite();
                return (null, false, ex);
            }

        }

        public async Task<(string Response, bool Success, Exception Error)> PostRaw(string url, CancellationToken? ct = null)
        {
            if (!IsConnected)
            {
                return (null, false, new ApplicationException("Network Connection Error"));
            }

            try
            {
                var token = ct ?? CancellationToken.None;

                await new SynchronizationContextRemover();

                using (var srvResponse = await Client.GetAsync(url, token).ConfigureAwait(false))
                {
                    var jsonResult = await srvResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (srvResponse.StatusCode == HttpStatusCode.OK)
                    {
                        return (jsonResult, true, null);
                    }
                    else
                    {
                        return (null, false, new ApplicationException(jsonResult));
                    }

                }

            }
            catch (Exception ex)
            {
                ex.ConsoleWrite();
                return (null, false, ex);
            }

        }

        public async Task<(T Response, bool Success, Exception Error)> Get<T>(string url, CancellationToken? ct = null) where T : class, new()
        {


            if (!IsConnected)
            {
                return (null, false, new ApplicationException("Network Connection Error"));
            }

            try
            {
                var token = ct ?? CancellationToken.None;

                await new SynchronizationContextRemover();

                using (var srvResponse = await Client.GetAsync(url, token).ConfigureAwait(false))
                {
                    if (CoreSettings.Config.HttpSettings.DisplayRawJson)
                    {
                        json = await GetStringContent<T>(srvResponse).ConfigureAwait(false);
                        var response = await DeserializeObject<T>(json).ConfigureAwait(false);
                        json = string.Empty;
                        return (response, true, null);
                    }
                    else
                    {
                        srvResponse.EnsureSuccessStatusCode();
                        var response = await DeserializeStream<T>(srvResponse);
                        return (response, true, null);
                    }
                }

            }
            catch (Exception ex)
            {
                ex.ConsoleWrite();
                return (null, false, ex);
            }
        }

        public async Task<(bool Success, Exception Error)> UploadFile(string url, byte[] obj, string fileName, CancellationToken? ct = null)
        {
            try
            {
                var token = ct ?? CancellationToken.None;
                var fileContent = new ByteArrayContent(obj);
                fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = "file",
                    FileName = fileName
                };
                var id = CoreSettings.InstallationId.Replace("-", string.Empty);
                string boundary = $"---{id}";
                var multipartContent = new MultipartFormDataContent(boundary);
                multipartContent.Add(fileContent);
                var response = await Client.PostAsync(url, multipartContent, token);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    return (true, null);
                }
                else
                {
                    return (false, new ApplicationException(response.ReasonPhrase));
                }
            }
            catch (Exception ex)
            {
                return (false, ex);
            }

        }


        public async Task<byte[]> DownloadFile(string url, Action<double> percentChanged, Action<Exception> error, string token = null, CancellationToken? ct = null)
        {
            try
            {
                var ctoken = ct ?? CancellationToken.None;
                return await Task.Run(async () =>
                {
                    using (var dwn = new FileDownloadManager())
                    {
                        var taskCompletionSource = new TaskCompletionSource<byte[]>();
                        dwn.BearerToken = token;
                        dwn.DownloadUrl = url;
                        dwn.DownloadCompleted = (obj) => taskCompletionSource.SetResult(obj);
                        dwn.ProgressChanged = percentChanged;
                        await dwn.StartDownload();
                        return await taskCompletionSource.Task;
                    }
                });
            }
            catch (Exception ex)
            {
                error?.Invoke(ex);
                return null;
            }

        }

        public async Task<(string Response, bool Success, Exception Error)> PostRaw(string url, object obj, CancellationToken? ct = null)
        {
            if (!IsConnected)
            {
                return (null, false, new ApplicationException("Network Connection Error"));
            }

            try
            {
                var token = ct ?? CancellationToken.None;

                await new SynchronizationContextRemover();
                var data = JsonConvert.SerializeObject(obj);
                using (var srvResponse = await Client.PostAsync(url, new StringContent(data, Encoding.UTF8, "application/json"), token).ConfigureAwait(false))
                {
                    var jsonResult = await srvResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (srvResponse.StatusCode == HttpStatusCode.OK)
                    {
                        return (jsonResult, true, null);
                    }
                    else
                    {
                        return (null, false, new ApplicationException(jsonResult));
                    }

                }

            }
            catch (Exception ex)
            {
                ex.ConsoleWrite();
                return (null, false, ex);
            }

        }
        public async Task<(T Response, bool Success, Exception Error)> Post<T>(string url, object obj, CancellationToken? ct = null) where T : class, new()
        {
            if (!IsConnected)
            {
                return (null, false, new ApplicationException("Network Connection Error"));
            }

            try
            {
                var token = ct ?? CancellationToken.None;

                await new SynchronizationContextRemover();

                var data = JsonConvert.SerializeObject(obj);
                using (var srvResponse = await Client.PostAsync(url, new StringContent(data, Encoding.UTF8, "application/json")).ConfigureAwait(false))
                {
                    if (CoreSettings.Config.HttpSettings.DisplayRawJson)
                    {
                        json = await GetStringContent<T>(srvResponse).ConfigureAwait(false);
                        var response = await DeserializeObject<T>(json).ConfigureAwait(false);
                        json = string.Empty;
                        return (response, true, null);
                    }
                    else
                    {
                        srvResponse.EnsureSuccessStatusCode();
                        var response = await DeserializeStream<T>(srvResponse);
                        return (response, true, null);
                    }
                }

            }
            catch (Exception ex)
            {
                ex.ConsoleWrite(true);
                return (null, false, ex);
            }

        }
        public async Task<(T Response, bool Success, Exception Error)> Put<T>(string url, object obj, CancellationToken? ct = null) where T : class, new()
        {
            if (!IsConnected)
            {
                return (null, false, new ApplicationException("Network Connection Error"));
            }

            try
            {
                var token = ct ?? CancellationToken.None;

                await new SynchronizationContextRemover();

                var data = JsonConvert.SerializeObject(obj);
                using (var srvResponse = await Client.PutAsync(url, new StringContent(data, Encoding.UTF8, "application/json"), token))
                {
                    if (CoreSettings.Config.HttpSettings.DisplayRawJson)
                    {
                        json = await GetStringContent<T>(srvResponse);
                        var response = await DeserializeObject<T>(json);
                        json = string.Empty;
                        return (response, true, null);
                    }
                    else
                    {
                        srvResponse.EnsureSuccessStatusCode();
                        var response = await DeserializeStream<T>(srvResponse);
                        return (response, true, null);
                    }
                }

            }
            catch (Exception ex)
            {
                ex.ConsoleWrite();
                return (null, false, ex);
            }

        }

        public async Task<string> GetStringContent<T>(HttpResponseMessage response) where T : class, new()
        {
            var jsonResult = await response.Content.ReadAsStringAsync();
            if (CoreSettings.Config.HttpSettings.DisplayRawJson)
            {
                Console.WriteLine();
                Console.WriteLine();
                var name = typeof(T).Name;
                if (name == "List`1")
                {
                    var types = typeof(T).GetGenericArguments();
                    if (types != null && types.Length > 0)
                    {
                        var obj = types[0];
                        name = "Collection of " + obj.Name;
                    }

                }
                Console.WriteLine($"*-*-*-*-*-*-*-*-*-*-*-*- {name} - HTTP STRING RESULT *-*-*-*-*-*-*-*-*-*-*-*-*-");
                var formatted = await FormattedJson(jsonResult);
                Console.WriteLine(formatted);
                Console.WriteLine("*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-");
                Console.WriteLine();
                Console.WriteLine();
            }
            return jsonResult;
        }

        private Task<string> FormattedJson(string jsonResult)
        {
            return Task.Run(() =>
            {
                var obj = JsonConvert.DeserializeObject(jsonResult);
                return JsonConvert.SerializeObject(obj, Formatting.Indented);
            });
        }

        private Task<T> DeserializeObject<T>(string content) where T : class, new()
        {
            return Task.Run(() =>
            {
                return JsonConvert.DeserializeObject<T>(content);
            });
        }

        private Task<T> DeserializeStream<T>(HttpResponseMessage response)
        {
            return Task.Run(async () =>
            {
                using (var stream = await response.Content.ReadAsStreamAsync())
                {
                    using (var reader = new StreamReader(stream))
                    {
                        using (var json = new JsonTextReader(reader))
                        {
                            return _serializer.Deserialize<T>(json);
                        }
                    }
                }
            });
        }

        public void Dispose()
        {
            Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;

            if (!string.IsNullOrEmpty(json))
            {
                json = null;
            }

            if (httpClient != null)
            {
                handler.Dispose();
                httpClient.Dispose();
                _serializer = null;
            }

        }

        void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess == Essentials.NetworkAccess.None || e.NetworkAccess == Essentials.NetworkAccess.Unknown)
            {
                IsConnected = false;
            }
            else
            {
                IsConnected = true;
            }
           // var access = e.NetworkAccess;
           // var profiles = e.Profiles;
        }

    }

}

