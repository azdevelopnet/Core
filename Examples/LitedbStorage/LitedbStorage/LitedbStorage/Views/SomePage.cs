using System;
using LitedbStorage.Models;
using Xamarin.Forms;
using Xamarin.Forms.Core;

namespace LitedbStorage
{
    public class PersonCell : ViewCell
    {
        Label lbl;
        public PersonCell()
        {
            lbl = new Label()
            {
                Margin = 5
            };
            View = new StackLayout()
            {
                Children = { lbl }
            };
        }
        protected override void OnBindingContextChanged()
        {
            var p = (Person)this.BindingContext;
            lbl.Text = $"{p.FirstName} {p.LastName}";
            base.OnBindingContextChanged();
        }
    }
    public class SomePage : CorePage<SomeViewModel>
    {
        public SomePage()
        {
            this.Title = "People";

            this.ToolbarItems.Add(new ToolbarItem()
            {
                Text = "Add Person",
                Command = new Command(async () => { await CoreSettings.AppNav.PushAsync(new AddPerson()); })
            });

            var lst = new CoreListView(ListViewCachingStrategy.RecycleElement)
            {
                ItemTemplate = new DataTemplate(typeof(PersonCell))
            };
            lst.SetBinding(CoreListView.ItemsSourceProperty, "People");


            Content = new StackLayout()
            {
                Padding = 10,
                Children = { lst }
            };
        }
    }
}