using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Xamarin.Forms.Core
{
    public interface IHttpService
    {
        void AddTokenHeader(string token);
        void AddTokenHeader(AuthenticationModel coreAuth);
        void AddNetworkCredentials(NetworkCredential cred);
        Task<(string Response, bool Success, Exception Error)> FormPost(string url, HttpContent content, CancellationToken? ct = null);
        Task<(T Response, bool Success, Exception Error)> Get<T>(string url, CancellationToken? ct = null) where T : class, new();
        Task<(T Response, bool Success, Exception Error)> Post<T>(string url, object obj, CancellationToken? ct = null) where T : class, new();
        Task<(T Response, bool Success, Exception Error)> Put<T>(string url, object obj, CancellationToken? ct = null) where T : class, new();
        Task<string> GetStringContent<T>(HttpResponseMessage response) where T : class, new();
        Task<(string Response, bool Success, Exception Error)> GetRaw(string url, CancellationToken? ct = null);
        Task<(string Response, bool Success, Exception Error)> PostRaw(string url, object obj, CancellationToken? ct = null);
        Task<(bool Success, Exception Error)> UploadFile(string url, byte[] obj, string fileName, CancellationToken? ct = null);
        Task<byte[]> DownloadFile(string url, Action<double> percentChanged, Action<Exception> error, string token = null, CancellationToken? ct = null);
        HttpClient Client { get; set; }
        void AddHeader(string name, string value);
        void RemoveHeader(string name);
        bool ContainsHeader(string name);
        bool IsConnected { get; set; }

    }
}
