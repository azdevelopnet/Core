using System;
using Xamarin.Forms;
using Xamarin.Forms.Core;

namespace LitedbStorage
{
    public class SomeObject : CoreModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
    }
}
