using System;

namespace Xamarin.Forms.Core
{
    public static partial class CoreExtensions
    {
        public static void ShowLoadingDialog(this CoreViewModel model, string msg)
        {
            CoreDefaultDialog.ShowLoadingDialog(msg);
            //CoreMaterialDialog.ShowLoadingDialog(msg);
        }

        public static void CloseLoadingDialog(this CoreViewModel model)
        {
            CoreDefaultDialog.CloseLoadingDialog();
            //CoreMaterialDialog.CloseLoadingDialog();
        }

        public static void ShowLoadingPercentDialog(this CoreViewModel model, string message, double percent)
        {
            CoreDefaultDialog.ShowLoadingPercentDialog(message, percent);
            //CoreMaterialDialog.ShowLoadingPercentDialog(message, percent);
        }

        public static void CloseLoadingPercentDialog(this CoreViewModel model)
        {
            CoreDefaultDialog.CloseLoadingPercentDialog();
            //CoreMaterialDialog.CloseLoadingPercentDialog();
        }
    }

}
