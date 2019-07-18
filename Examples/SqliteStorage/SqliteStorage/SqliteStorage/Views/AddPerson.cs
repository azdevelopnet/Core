using System;
using Xamarin.Forms;
using Xamarin.Forms.Core;

namespace SqliteStorage.Views
{
    public class AddPerson : CorePage<SomeViewModel>
    {
        public AddPerson()
        {
            this.Title = "Add Person";

            var lblFirstName = new Label()
            {
                Text = "First Name:",
                Margin = new Thickness(5, 5, 5, 0)
            };
            var txtFirstName = new CoreMaskedEntry()
            {
                Margin = 5
            };
            txtFirstName.SetBinding(CoreMaskedEntry.TextProperty, "FirstName");

            var lblLastName = new Label()
            {
                Text = "Last Name",
                Margin = new Thickness(5, 5, 5, 0)
            };
            var txtLastName = new CoreMaskedEntry()
            {
                Margin = 5
            };
            txtLastName.SetBinding(CoreMaskedEntry.TextProperty, "LastName");

            var btnAddPerson = new CoreButton()
            {
                Text = "Add Person",
                Style = CoreStyles.LightOrange,
                Margin = 5
            };
            btnAddPerson.SetBinding(Button.CommandProperty, "AddPerson");

            Content = new StackLayout()
            {
                Padding = 20,
                Children = { lblFirstName, txtFirstName, lblLastName, txtLastName, btnAddPerson }
            };
        }
    }
}
