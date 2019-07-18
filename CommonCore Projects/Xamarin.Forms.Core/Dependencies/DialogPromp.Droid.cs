#if __ANDROID__
using System;
using Android.App;
using Android.Widget;
using Xamarin.Forms.Core;
using Xamarin.Forms;
using Android.Content;
using Plugin.CurrentActivity;

[assembly: Xamarin.Forms.Dependency(typeof(DialogPrompt))]
namespace Xamarin.Forms.Core
{
    public class DialogPrompt : IDialogPrompt
    {
        public Context Ctx
        {
            get => CrossCurrentActivity.Current.Activity;
        }

        public void ShowMessage(Prompt prompt)
        {
            if (prompt.ButtonTitles == null || prompt.ButtonTitles.Length == 0)
                return;
            
            try
            {
                var d = new AlertDialog.Builder(Ctx).Create();
                d.SetTitle(prompt.Title);
                d.SetMessage(prompt.Message);
                if (prompt.ButtonTitles.Length > 2)
                {
                    d.SetButton(prompt.ButtonTitles[0], (e, a) =>
                    {
                        prompt.Callback?.Invoke(0);
                    });
                    d.SetButton2(prompt.ButtonTitles[1], (e, a) =>
                    {
                        prompt.Callback?.Invoke(1);
                    });
                    d.SetButton3(prompt.ButtonTitles[2], (e, a) =>
                    {
                        prompt.Callback?.Invoke(2);
                    });

                }
                else if(prompt.ButtonTitles.Length == 2)
                {
                    d.SetButton(prompt.ButtonTitles[0], (e, a) =>
                    {
                        prompt.Callback?.Invoke(0);
                    });
                    d.SetButton2(prompt.ButtonTitles[1], (e, a) =>
                    {
                        prompt.Callback?.Invoke(1);
                    });
                }
                else if (prompt.ButtonTitles.Length == 1){
                    d.SetButton(prompt.ButtonTitles[0], (e, a) =>
                    {
                        prompt.Callback?.Invoke(0);
                    });
                }
   
                d.Show();
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine(ex.Message);
#endif
            }
        }

        public void ShowActionSheet(string title, string subTitle, string[] list, Action<int> callBack, PromptMetaData metaData)
        {
            var dlg = new AlertDialog.Builder(Ctx);
            dlg.SetTitle(title);
            dlg.SetSingleChoiceItems(list, -1, (s, a) =>
            {

                var index = list.IndexOf(list[a.Which]);
                callBack?.Invoke(index);
                ((AlertDialog)s).Dismiss();

            });
            dlg.SetPositiveButton("Cancel", (s, a) =>
            {

            });

            var dialog = dlg.Show();

        }

        public void ShowToast(string message)
        {
            Toast.MakeText(Ctx, message, ToastLength.Long).Show();
        }
    }
}
#endif
