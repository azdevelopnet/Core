#if __ANDROID__
using System;
using Android.App;
using Android.Widget;
using Xamarin.Forms.Core;
using Xamarin.Forms;
using Android.Content;
using Plugin.CurrentActivity;
using Android.Views;

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

        public void ShowInputMessage(string title, string message, string placeholder, string defaultValue, Action<string> callBack)
        {
            var dlg = new AlertDialog.Builder(Ctx);
            dlg.SetTitle(title);

            var edit = new EditText(Ctx);
            if (!string.IsNullOrEmpty(placeholder))
                edit.Hint = placeholder;
            if (!string.IsNullOrEmpty(defaultValue))
                edit.Text = defaultValue;

            var layout = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent);
            layout.SetMargins(30, 20, 30, 0);
            edit.LayoutParameters = layout;
           
            dlg.SetView(edit);

            dlg.SetNegativeButton("CANCEL", (s, a) => {
                callBack?.Invoke(null);
                ((AlertDialog)s).Dismiss();
            });

            dlg.SetPositiveButton("OK", (s, a) =>
            {
                callBack?.Invoke(edit.Text);
                ((AlertDialog)s).Dismiss();
            });

            var dialog = dlg.Show();

        }

        public void ShowActionSheet(string title, string subTitle, string[] list, Action<int> callBack, PromptMetaData metaData)
        {
            int idx = -1;
            var dlg = new AlertDialog.Builder(Ctx);
            dlg.SetTitle(title);
            dlg.SetSingleChoiceItems(list, -1, (s, a) =>
            {
                idx = list.IndexOf(list[a.Which]);
            });
            dlg.SetNegativeButton("CANCEL", (s, a) => {
                callBack?.Invoke(-1);
                ((AlertDialog)s).Dismiss();
            });
            dlg.SetPositiveButton("OK", (s, a) =>
            {
                callBack?.Invoke(idx);
                ((AlertDialog)s).Dismiss();
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
