using System;
using Xamarin.Forms;
using Xamarin.Forms.Core;

namespace MasterDetail
{
    public class SomePage : CorePage<SomeViewModel>
    {
        public SomePage()
        {
            this.Title = "Some Page";

            var header = new Label()
            {
                Text = "Enter Text:",
                Margin = new Thickness(5, 5, 5, 0)
            };
            var entry = new CoreMaskedEntry()
            {
                Margin = 5
            };
            entry.SetBinding(CoreMaskedEntry.TextProperty, "SomeText");

            var lbl = new Label()
            {
                Margin = 5
            };
            lbl.SetBinding(Label.TextProperty, new Binding(path: "SomeText", converter: CoreSettings.UpperText));

            var btn = new CoreButton()
            {
                Text = "Some Action",
                Style = CoreStyles.LightOrange,
                Margin = 5
            };
            btn.SetBinding(Button.CommandProperty, "SomeAction");

            var lstCount = new Label()
            {
                Margin = 5
            };
            lstCount.SetBinding(Label.TextProperty, new Binding(path: "TotalItems", stringFormat: "Total count is {0}"));

            Content = new StackLayout()
            {
                Padding = 20,
                Children = { header, entry, lbl, btn, lstCount }
            };
        }
    }
}