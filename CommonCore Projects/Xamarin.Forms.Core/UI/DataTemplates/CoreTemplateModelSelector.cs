using System;
namespace Xamarin.Forms.Core
{
    /// <summary>
    /// Template selector that allows direct model property assignment for performance.  Should not be used on items that need to reflect updated values.
    /// see: https://codetraveler.io/2020/07/12/improving-collectionview-scrolling/
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CoreTemplateModelSelector<T> : DataTemplateSelector
    where T : class, new()
    {
        Func<T, DataTemplate> _obj;

        public CoreTemplateModelSelector(Func<T, DataTemplate> obj)
        {
            _obj = obj;

        }
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item != null)
                return _obj((T)item);
            else
                return _obj(new T());
        }
    }
}


/*
 
     public class ImageModel
    {
        public string ImageTitle { get; set; }
        public string ImageUrl { get; set; }
    }

    public class CollectionViewModel
    {
        public ObservableCollection<ImageModel> ImageList { get; set; }
        public CollectionViewModel()
        {
            var lst = new ObservableCollection<ImageModel>();
            lst.Add(new ImageModel()
            {
                ImageTitle = "Dog",
                ImageUrl = "https://i.ytimg.com/vi/MPV2METPeJU/maxresdefault.jpg"
            });
            ImageList = lst;
        }
    }
    public class CollectionViewPage : ContentPage
    {
        public CollectionViewPage()
        {
            this.BindingContext = new CollectionViewModel();
            Content = new CollectionView
            {
                ItemTemplate = new CoreTemplateModelSelector<ImageModel>((obj) => 
                    new DataTemplate(() =>
                    {
                        return new StackLayout()
                        {
                            Children =
                            {
                                new Image { Source = obj.ImageUrl },
                                new Label { Text = obj.ImageTitle }
                            }
                        };
                    })
                )
            }.Bind(CollectionView.ItemsSourceProperty, nameof(CollectionViewModel.ImageList));
        }
    }
 */
