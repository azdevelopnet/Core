using System;
using System.Threading.Tasks;

namespace Xamarin.Forms.Core
{
    public class Prompt
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string[] ButtonTitles { get; set; }
        public Action<int> Callback { get; set; }
    }

    public class PromptMetaData
    {
        public Xamarin.Forms.Rectangle Rect { get; set; }
        public Xamarin.Forms.View Control { get; set; }
    }

    public interface IDialogPrompt
    {
        void ShowMessage(Prompt prompt);
        void ShowActionSheet(string title, string subTitle, string[] list, Action<int> callBack, PromptMetaData metaData = null);
        void ShowToast(string message);
    }
}

