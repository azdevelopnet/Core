using System;
using Xamarin.Essentials;
using XF.Material.Forms.UI.Dialogs;
using XF.Material.Forms.UI.Dialogs.Configurations;
namespace Xamarin.Forms.Core
{
    public class CoreMaterialDialog
    {
        private static IMaterialModalPage loadingModalPage;
        private static string _loadingIndicatorText;

        public static string LoadingIndicatorText
        {
            get
            {
                return _loadingIndicatorText;
            }
            set
            {
                _loadingIndicatorText = value;
                if (loadingModalPage != null)
                {
                    loadingModalPage.MessageText = _loadingIndicatorText;
                }
            }
        }
        public static void ShowLoadingDialog(string msg)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                _loadingIndicatorText = msg;

                loadingModalPage = await MaterialDialog.Instance.LoadingDialogAsync(msg, new MaterialLoadingDialogConfiguration()
                {
                    TintColor = Color.Black,
                    MessageTextColor = Color.Black,
                    CornerRadius = 5,
                });

            });


        }

        public static void CloseLoadingDialog()
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                if (loadingModalPage != null)
                {
                    await loadingModalPage.DismissAsync();
                }

            });


        }

        public static void ShowLoadingPercentDialog(string message, double percent)
        {

        }
        public static void CloseLoadingPercentDialog()
        {

        }
    }
}
