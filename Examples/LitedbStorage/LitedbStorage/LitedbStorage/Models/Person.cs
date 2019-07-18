using System;
using Xamarin.Forms.Core;

namespace LitedbStorage.Models
{
    public class Person: LiteDbModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; }
    }
}
