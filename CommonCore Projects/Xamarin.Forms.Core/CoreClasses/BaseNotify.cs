using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Xamarin.Forms.Core
{
    public class BaseNotify : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        //protected bool SetProperty<T>(
        //    ref T backingStore, T value,
        //    [CallerMemberName]string propertyName = "",
        //    Action onChanged = null)
        //{
        //    if (EqualityComparer<T>.Default.Equals(backingStore, value))
        //        return false;

        //    backingStore = value;

        //    if (onChanged != null)
        //        onChanged();

        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        //    return true;
        //}

        /// <summary>
        /// Handle the PropertyChanged event
        /// TODO: refactor this using the newer CallerMemberName.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="property"></param>
        //public void OnPropertyChanged<T>(Expression<Func<T>> property)
        //{
        //    if (property == null)
        //        throw new ArgumentNullException();

        //    MemberExpression body = property.Body as MemberExpression;

        //    if (body == null)
        //        throw new ArgumentException("The body must be a member expression");

        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(body.Member.Name));
        //}
        //public void OnPropertyChanged<T>([CallerMemberName]string propertyName = " ")
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}
    }
}
