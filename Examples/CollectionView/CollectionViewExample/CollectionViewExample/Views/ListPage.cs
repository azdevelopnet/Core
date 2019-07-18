using System;
using CollectionViewExample.Models;
using CollectionViewExample.ViewModels;
using Humanizer;
using Xamarin.Forms;
using Xamarin.Forms.Core;
using FFImageLoading.Forms;
using FFImageLoading.Transformations;
using System.Collections.Generic;
using FFImageLoading.Work;

namespace CollectionViewExample.Views
{
    public class CollectionViewItem : ContentView
    {
        private Label personalInfo;
        private CachedImage image;
        public CollectionViewItem()
        {
            Style = CoreStyles.RandomUserCollection;
            image = new CachedImage()
            {
                Margin=new Thickness(10,10,5,10),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                WidthRequest = 100,
                HeightRequest = 100,
                CacheDuration = TimeSpan.FromDays(30),
                DownsampleToViewSize = true,
                RetryCount = 0,
                RetryDelay = 250,
                BitmapOptimizations = false,
                Transformations = new List<ITransformation>() { new CircleTransformation(10, "#d3d3d3") }
            };
            
            image.SetBinding(CachedImage.SourceProperty, nameof(RandomUser.Photo));

            personalInfo = new Label()
            {
                Margin = new Thickness(0,25,0,20)
            };

            Content = new StackLayout()
            {
                Children = { new StackLayout() {
                    Orientation = StackOrientation.Horizontal,
                    Children={ image, personalInfo }
                } }
            };
        }
        protected override void OnBindingContextChanged()
        {
            var usrs = (RandomUser)this.BindingContext;

            var fs = new FormattedString();
            var title = $"{usrs.Title} {usrs.FirstName} {usrs.LastName}".Humanize(LetterCasing.Title) + "\n";
            fs.AddTextSpan(title, CoreStyles.CardTitle);
            fs.AddTextSpan($"{usrs.Address}".Humanize(LetterCasing.Title) + "\n");
            fs.AddTextSpan($"{usrs.City}, {usrs.State}   {usrs.ZipCode}".Humanize(LetterCasing.Title) + "\n");
            fs.AddTextSpan($"{usrs.CellPhone} \n");
            personalInfo.FormattedText = fs;

            base.OnBindingContextChanged();
        }
    }
    public class ListPage : CorePage<ListsViewModel>
    {

        public ListPage()
        {
            this.Title = "Lists";
            var clv = new CollectionView()
            {
                ItemTemplate = new DataTemplate(typeof(CollectionViewItem)),
                VerticalOptions = LayoutOptions.CenterAndExpand,
                EmptyView = new StackLayout()
                {
                    Children = { new Label() { Text = "Empty View" } }
                },
                SelectionMode = SelectionMode.Single,
                //BackgroundColor = Color.White
            };
            clv.SetBinding(CollectionView.ItemsSourceProperty, nameof(ListsViewModel.Users));

            Content = new StackLayout()
            {
                Children = { clv }
            };
        }
    }
}
