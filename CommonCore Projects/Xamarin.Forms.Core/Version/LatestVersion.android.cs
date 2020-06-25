#if __ANDROID__
using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using DroidApp = Android.App.Application;
using Net = Android.Net;

namespace Xamarin.Forms.Core
{
    /// <summary>
    /// <see cref="ILatestVersion"/> implementation for Android.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class LatestVersionImplementation : ILatestVersion
    {
        string _packageName => DroidApp.Context.PackageName;
        string _versionName => DroidApp.Context.PackageManager.GetPackageInfo(DroidApp.Context.PackageName, 0).VersionName;

        /// <inheritdoc />
        public string InstalledVersionNumber
        {
            get => _versionName;
        }

        /// <inheritdoc />
        public async Task<bool> IsUsingLatestVersion()
        {
            var latestVersion = string.Empty;

            try
            {
                latestVersion = await GetLatestVersionNumber();
                var latest = long.Parse(latestVersion);
                var current = long.Parse(_versionName);
                if (current < latest)
                    return false;
                else
                    return true;


            }
            catch (Exception e)
            {
                throw new LatestVersionException($"Error comparing current app version number with latest. Version name={_versionName} and lastest version={latestVersion} .", e);
            }
        }

        /// <inheritdoc />
        public async Task<string> GetLatestVersionNumber()
        {
            var version = string.Empty;
            var url = $"https://play.google.com/store/apps/details?id={_packageName}&hl=en";

            using (var request = new HttpRequestMessage(HttpMethod.Get, url))
            {
                using (var handler = new HttpClientHandler())
                {
                    using (var client = new HttpClient(handler))
                    {
                        using (var responseMsg = await client.SendAsync(request, HttpCompletionOption.ResponseContentRead))
                        {
                            if (!responseMsg.IsSuccessStatusCode)
                            {
                                throw new LatestVersionException($"Error connecting to the Play Store. Url={url}.");
                            }

                            try
                            {
                                var content = responseMsg.Content == null ? null : await responseMsg.Content.ReadAsStringAsync();

                                var versionMatch = Regex.Match(content, "<div[^>]*>Current Version</div><span[^>]*><div[^>]*><span[^>]*>(.*?)<").Groups[1];

                                if (versionMatch.Success)
                                {
                                    version = versionMatch.Value.Trim();
                                }
                            }
                            catch (Exception e)
                            {
                                throw new LatestVersionException($"Error parsing content from the Play Store. Url={url}.", e);
                            }
                        }
                    }
                }
            }

            return version;
        }

        /// <inheritdoc />
        public Task OpenAppInStore()
        {
            try
            {
                var intent = new Intent(Intent.ActionView, Net.Uri.Parse($"market://details?id={_packageName}"));
                intent.SetPackage("com.android.vending");
                intent.SetFlags(ActivityFlags.NewTask);
                DroidApp.Context.StartActivity(intent);
            }
            catch (ActivityNotFoundException)
            {
                var intent = new Intent(Intent.ActionView, Net.Uri.Parse($"https://play.google.com/store/apps/details?id={_packageName}"));
                DroidApp.Context.StartActivity(intent);
            }

            return Task.FromResult(true);
        }
    }
}
#endif
