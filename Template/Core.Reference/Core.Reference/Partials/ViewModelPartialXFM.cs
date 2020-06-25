//using System;
//using Xamarin.Essentials;
//using XF.Material.Forms.UI.Dialogs;
//using XF.Material.Forms.UI.Dialogs.Configurations;
//namespace Xamarin.Forms.Core
//{
//    public partial class CoreViewModel
//    {
//        private IMaterialModalPage loadingModalPage;
//        private string _loadingIndicatorText;

//        public string LoadingIndicatorText
//        {
//            get
//            {
//                return _loadingIndicatorText;
//            }
//            set
//            {
//                _loadingIndicatorText = value;
//                if (loadingModalPage != null)
//                {
//                    loadingModalPage.MessageText = _loadingIndicatorText;
//                }
//            }
//        }
//        public void ShowLoadingDialog(string msg)
//        {
//            MainThread.BeginInvokeOnMainThread(async () =>
//            {
//                _loadingIndicatorText = msg;

//                loadingModalPage = await MaterialDialog.Instance.LoadingDialogAsync(msg, new MaterialLoadingDialogConfiguration()
//                {
//                    TintColor = Color.Black,
//                    MessageTextColor = Color.Black,
//                    CornerRadius = 5,
//                });

//            });


//        }

//        public void CloseLoadingDialog()
//        {
//            MainThread.BeginInvokeOnMainThread(async () =>
//            {
//                if (loadingModalPage != null)
//                {
//                    await loadingModalPage.DismissAsync();
//                }

//            });


//        }

//        public void ShowLoadingPercentDialog(string message, double percent)
//        {

//        }
//        public void CloseLoadingPercentDialog()
//        {

//        }
//    }
//}
