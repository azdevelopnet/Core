using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Xamarin.Forms.Core
{
   
    internal class FileDownloadManager : BindableObject, IDisposable
	{
        private string _bearerToken;
        public WebClient Client { get; set; }
        public Action<(bool response, Exception ex)> LocalDownloadcompleted { get; set; }
        public Action<byte[]> DownloadCompleted { get; set; }
        public Action<double> ProgressChanged { get; set; }
        public string DownloadUrl { get; set; }
        public string LocalPath { get; set; }
        public string BearerToken
        {
            get { return _bearerToken; }
            set
            {
                _bearerToken = value;
                if (Client == null)
                    Client = new WebClient();

                if (Client.Headers[HttpRequestHeader.Authorization] != null)
                    Client.Headers[HttpRequestHeader.Authorization] = _bearerToken;
                else
                    Client.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + _bearerToken);
            }
        }

        public FileDownloadManager()
        {
            if (Client == null)
                Client = new WebClient();
        }

        public async Task StartDownload()
		{
			await Task.Run(() =>
			{
				Client.DownloadProgressChanged += DownprogressChanged;
				Client.DownloadDataCompleted += DownloadComplete;
				Client.DownloadDataAsync(new Uri(DownloadUrl));
			});

		}

        public async Task StartLocalDownload()
        {
            try
            {
                await Task.Run(() =>
                {
                    Client.DownloadProgressChanged += DownprogressChanged;
                    Client.DownloadFileCompleted += LocalDownloadCompleted;
                    Client.DownloadFileAsync(new Uri(DownloadUrl), LocalPath);
                });
            }
            catch (Exception ex)
            {
                LocalDownloadcompleted?.Invoke((false,ex));
            }


        }

        public void Dispose()
        {
            if (Client != null)
            {
                Client.DownloadProgressChanged -= DownprogressChanged;
                Client.DownloadDataCompleted -= DownloadComplete;
            }
        }
        private void LocalDownloadCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            try
            {
                var success = e.Error == null ? true : false;
                LocalDownloadcompleted?.Invoke((success,e.Error));
            }
            catch (Exception ex)
            {
                var token = Client.Headers[HttpRequestHeader.Authorization];
                var im = ex.InnerException.Message;
            }

        }

        private void DownloadComplete(object sender, DownloadDataCompletedEventArgs args)
		{
            try
            {
                DownloadCompleted?.Invoke(args.Result);
            }
            catch (Exception ex)
            {
                var token = Client.Headers[HttpRequestHeader.Authorization];
                var im = ex.InnerException.Message;
            }
            
		}
		private void DownprogressChanged(object sender, DownloadProgressChangedEventArgs args)
		{
			var progress = (float)args.BytesReceived / (float)args.TotalBytesToReceive;
            ProgressChanged?.Invoke(progress);
		}

	}
}
