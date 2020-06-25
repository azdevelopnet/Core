using System;
using System.ComponentModel;

namespace Xamarin.Forms.Core
{
    public class NotifyModel:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
