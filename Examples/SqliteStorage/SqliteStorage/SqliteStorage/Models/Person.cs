using System;
using Xamarin.Forms.Core;

namespace SqliteStorage.Models
{
    public class Person: CoreSqlModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; }
    }
}
