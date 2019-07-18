using System;
namespace Xamarin.Forms.Core
{
    public interface IProgressIndicator
    {
        void ShowProgress(string message);
        void ShowProgress(string message, double percentage);
        void Dismiss();
    }
}

