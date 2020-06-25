//using System;

//#if __IOS__
//using BigTed;
//#else
//    using Plugin.CurrentActivity;
//    using AndroidHUD;
//#endif

//namespace Xamarin.Forms.Core
//{
//    public partial class CoreViewModel
//    {
//        public void ShowLoadingDialog(string msg)
//        {

//#if __IOS__

//            BTProgressHUD.Show(msg, -1, ProgressHUD.MaskType.Black);
//#else
//            AndHUD.Shared.Show(CrossCurrentActivity.Current.Activity, msg, (int)MaskType.Clear);
//#endif


//        }

//        public void CloseLoadingDialog(string message)
//        {

//#if __IOS__
//            BTProgressHUD.Dismiss();
//#else
//            AndHUD.Shared.Dismiss(CrossCurrentActivity.Current.Activity);
//#endif


//        }

//        public void ShowLoadingPercentDialog(string message, double percent)
//        {

//        }
//        public void CloseLoadingPercentDialog()
//        {

//        }
//    }
//}
