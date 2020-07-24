using System;
namespace Xamarin.Forms.Core
{
    public static partial class CoreExtensions
    {
        public static void ShowLoadingDialog(this CoreViewModel model, string msg)
        {
            CoreMaterialDialog.ShowLoadingDialog(msg);
        }

        public static void CloseLoadingDialog(this CoreViewModel model)
        {
            CoreMaterialDialog.CloseLoadingDialog();
        }

        public static void ShowLoadingPercentDialog(this CoreViewModel model, string message, double percent)
        {
            CoreMaterialDialog.ShowLoadingPercentDialog(message,percent);
        }

        public static void CloseLoadingPercentDialog(this CoreViewModel model)
        {
            CoreMaterialDialog.CloseLoadingPercentDialog();
        }
    }
}
