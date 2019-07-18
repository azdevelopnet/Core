using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Xamarin.Forms.Core
{
 
    public abstract class CoreModel : BaseNotify, ICloneable
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
