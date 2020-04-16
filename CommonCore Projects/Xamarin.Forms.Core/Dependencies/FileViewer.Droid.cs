#if __ANDROID__
using System;
using Android;
using Android.Content;
using Android.Webkit;
using Android.Widget;
using Java.IO;
using Plugin.CurrentActivity;
using Xamarin.Forms.Core;
using DroidUri = Android.Net.Uri;
using DroidEnvironment = Android.OS.Environment;
using DroidContent = Android.Content;
using DroidMimeTypeMap = Android.Webkit.MimeTypeMap;
using Dir = System.IO.Directory;
using Android.OS;
//using Android.Support.V4.Content;
using MimeTypeMap = Android.Webkit.MimeTypeMap;
using AndroidX.Core.Content;

[assembly: Xamarin.Forms.Dependency(typeof(FileViewer))]
namespace Xamarin.Forms.Core
{

    public class FileViewer: IFileViewer
    {
   
        public Context Ctx
        {
            get => CrossCurrentActivity.Current.Activity;
        }

        public void OpenFile(string filePath)
        {
            try
            {
                
                string mimeType = MimeTypeMap.Singleton.GetMimeTypeFromExtension(MimeTypeMap.GetFileExtensionFromUrl(filePath.ToLower()));
                if (mimeType == null)
                    mimeType = "*/*";

                var javaFile = new Java.IO.File(filePath);
                var pdfPath = FileProvider.GetUriForFile(Ctx, Ctx.PackageName + ".fileprovider", javaFile);
                Intent intent = new Intent(Intent.ActionView);
                intent.SetDataAndType(pdfPath, mimeType);
                intent.SetFlags(ActivityFlags.ClearWhenTaskReset | ActivityFlags.NewTask);
                intent.SetFlags(ActivityFlags.GrantReadUriPermission);
                Ctx.StartActivity(intent);
            }
            catch (Exception ex)
            {
                Toast.MakeText(Ctx,  "No Application Available to View PDF", ToastLength.Short).Show();
            }
        }
    }
}
#endif
