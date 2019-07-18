using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using SqliteStorage.Models;
using Xamarin.Forms.Core;

namespace SqliteStorage
{
    public class SomeViewModel : CoreViewModel
    {

        private ObservableCollection<Person> _people = new ObservableCollection<Person>();

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ObservableCollection<Person> People { get; set; }
        public ICommand AddPerson { get; set; }

        public SomeViewModel()
        {
            People = new ObservableCollection<Person>();
            AddPerson = new CoreCommand(async (obj) =>
            {
                var p = new Person()
                {
                    FirstName = this.FirstName,
                    LastName = this.LastName,
                    DOB = DateTime.Now
                };
                var result = await SqliteDb.AddOrUpdate<Person>(p);
                if (result.Success) 
                {
                    People.Add(p);
                    FirstName = string.Empty;
                    LastName = string.Empty;
                    await CoreSettings.AppNav.PopAsync();
                }
               

               
            });
        }

        public override void OnViewMessageReceived(string key, object obj)
        {

        }

        public override void OnInit()
        {
            Task.Run(async () =>
            {

                var results = await SqliteDb.GetAll<Person>();
                if (results.Success)
                {
                    People = results.Response.ToObservable();
                }
                else
                {
                    DialogPrompt.ShowMessage(new Prompt()
                    {
                        Title = "Error",
                        Message = results.Error.Message,
                        ButtonTitles = new string[] { "Okay" }
                    });
                }
            });
        }
    }
}
