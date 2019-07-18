using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using LitedbStorage.Models;
using Xamarin.Forms.Core;

namespace LitedbStorage
{
    public class SomeViewModel : CoreViewModel
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ObservableCollection<Person> People { get; set; } = new ObservableCollection<Person>();
        public ICommand AddPerson { get; set; }

        public SomeViewModel()
        {
            AddPerson = new CoreCommand(async (obj) =>
            {
                var p = new Person()
                {
                    FirstName = this.FirstName,
                    LastName = this.LastName,
                    DOB = DateTime.Now
                };
  
                var result = await LiteDb.Insert<Person>(p);
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

                var results = await LiteDb.GetAll<Person>();
                if (results.Error==null)
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
