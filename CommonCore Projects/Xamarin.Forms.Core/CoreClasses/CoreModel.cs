using System;
using PropertyChanged;

namespace Xamarin.Forms.Core
{

    [AddINotifyPropertyChangedInterface]
    public abstract class CoreModel : ICloneable
    {
        public object Clone()
        {
            var clone = Activator.CreateInstance(this.GetType());
            foreach (var prop in this.GetType().GetProperties())
            {
                prop.SetValue(clone, prop.GetValue(this));
            }
            return clone;
        }
    }
}
