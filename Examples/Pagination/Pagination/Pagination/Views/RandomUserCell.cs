using System;
using FFImageLoading.Forms;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using Xamarin.Forms;
using Xamarin.Forms.Core;

namespace Pagination.Views
{
    public class DatatumCell : ViewCell
    {
    
        private readonly Label lblFullName;
  

        public DatatumCell()
        {
            this.Height = 45;

            lblFullName = new Label()
            {
                Margin = new Thickness(10,10,10,10)
            };
            lblFullName.SetBinding(Label.TextProperty,
                                   new Binding(path: "name"));


            ContextActions.Add(new MenuItem()
            {
                Text = "More Info",
                IsDestructive = true,
                Command = new Command((obj) =>
                {
                    Device.BeginInvokeOnMainThread(() => {
                        var n = ((Datum)BindingContext).name;
                        DependencyService.Get<IDialogPrompt>().ShowMessage(new Prompt()
                        {
                            Title = "Row Selected",
                            Message = $"You chose {((Datum)BindingContext).name}",
                            ButtonTitles = new string[]{"OK"}

                        });
                    });
                })
            });

            View = new StackContainer(true)
            {
                Orientation = StackOrientation.Horizontal,
                Children = { lblFullName }
            };

        }

        //On a listview that uses RecycleElement binding can be costly
        //protected override void OnBindingContextChanged()
        //{

        //    var item = ((Datum)BindingContext);
        //    img.Source = item.ImageUrl;
        //    //lblFullName.Text = item.FullName;
        //    lblFullAddress.FormattedText = item.name;
        //    base.OnBindingContextChanged();
        //}
    }
}
