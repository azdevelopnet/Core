using System;
using CollectionViewExample.Models;
using CollectionViewExample.ViewModels;
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
        public CollectionViewItem()
        {

            Style = CoreStyles.RandomUserCollection;

            Content = new StackLayout()
            {
                Children = { new StackLayout() {
                    Orientation = StackOrientation.Horizontal,
                    Children=
                    {
                        new CachedImage()
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
                        }.Bind(CachedImage.SourceProperty, nameof(RandomUser.Photo)),
                        new Label()
                        {
                            Margin = new Thickness(0, 25, 0, 20)
                        }.Bind(Label.FormattedTextProperty, nameof(RandomUser.FriendlyText))
                    }
                } }
            };
        }

    }
    public class ListPage : CorePage<ListsViewModel>
    {

        public ListPage()
        {
            this.Title = "Lists";

            Content = new StackLayout()
            {
                Children =
                {
                    new CollectionView()
                    {
                        ItemTemplate = new DataTemplate(typeof(CollectionViewItem)),
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        EmptyView = new StackLayout()
                        {
                            Children = {
                                new StackContainer()
                                {
                                    Orientation = StackOrientation.Horizontal,
                                    Children =
                                    {
                                        new StackContainer(){HorizontalOptions = LayoutOptions.StartAndExpand},
                                        new Label()
                                        {
                                            Margin = 60,
                                            Text = "List is empty",
                                            TextColor = Color.DarkGray,
                                            FontSize = 24,
                                            HorizontalTextAlignment = TextAlignment.Center
                                        },
                                        new StackContainer(){HorizontalOptions = LayoutOptions.EndAndExpand},
                                    }
                                }

                            }
                        },
                        SelectionMode = SelectionMode.Single,
                    }.Bind(CollectionView.ItemsSourceProperty, nameof(ListsViewModel.Users))
                }
            };
        }
    }
}
