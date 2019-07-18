using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms.Core;

namespace LitedbStorage
{
    public class SomeBusinessLogic : CoreBusiness
    {
        public (List<SomeObject> data, Exception error) GetSomeData()
        {
            try
            {
                var lst = new List<SomeObject>();
                lst.Add(new SomeObject() { Id = 1, Description = "One" });
                lst.Add(new SomeObject() { Id = 2, Description = "Two" });
                lst.Add(new SomeObject() { Id = 4, Description = "Four" });
                return (lst, null);
            }
            catch (Exception ex)
            {
                return (null, ex);
            }

        }
    }
}