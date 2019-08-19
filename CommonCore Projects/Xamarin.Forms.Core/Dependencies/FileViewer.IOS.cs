#if __IOS__
using System;
using System.IO;
using Foundation;
using QuickLook;
using UIKit;
using Xamarin.Forms.Core;

[assembly: Xamarin.Forms.Dependency(typeof(FileViewer))]
namespace Xamarin.Forms.Core
{
    public class FileViewer : IFileViewer
    {
        public void OpenFile(string filePath)
        {

            Device.BeginInvokeOnMainThread(() =>
            {
                FileInfo fi = new FileInfo(filePath);

                QLPreviewController previewController = new QLPreviewController();
                previewController.DataSource = new FilePreviewControllerDataSource(fi.FullName, fi.Name);

                UINavigationController controller = FindNavigationController();
                if (controller != null)
                    controller.PresentViewController(previewController, true, null);
            });

        }

        private UINavigationController FindNavigationController()
        {
            foreach (var window in UIApplication.SharedApplication.Windows)
            {
                if (window.RootViewController.NavigationController != null)
                    return window.RootViewController.NavigationController;
                else
                {
                    UINavigationController val = CheckSubs(window.RootViewController.ChildViewControllers);
                    if (val != null)
                        return val;
                }
            }

            return null;
        }

        private UINavigationController CheckSubs(UIViewController[] controllers)
        {
            foreach (var controller in controllers)
            {
                if (controller.NavigationController != null)
                    return controller.NavigationController;
                else
                {
                    UINavigationController val = CheckSubs(controller.ChildViewControllers);
                    if (val != null)
                        return val;
                }
            }
            return null;
        }
    }

    public class FileItem : QLPreviewItem
    {
        string title;
        string uri;

        public FileItem(string title, string uri)
        {
            this.title = title;
            this.uri = uri;
        }

        public override string ItemTitle
        {
            get { return title; }
        }

        public override NSUrl ItemUrl
        {
            get { return NSUrl.FromFilename(uri); }
        }
    }

    public class FilePreviewControllerDataSource : QLPreviewControllerDataSource
    {
        string url = "";
        string filename = "";

        public FilePreviewControllerDataSource(string url, string filename)
        {
            this.url = url;
            this.filename = filename;
        }


        public override IQLPreviewItem GetPreviewItem(QLPreviewController controller, nint index)
        {
            return new FileItem(filename, url);
        }

        public override nint PreviewItemCount(QLPreviewController controller)
        {
            return 1;
        }

    }
}
#endif
