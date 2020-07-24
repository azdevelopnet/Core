﻿using System;
using FFImageLoading.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Core;
using Xamarin.Forms.PancakeView;
using Xamarin.Forms.Markup;

namespace CoreReferenceExample
{
    #region DashboardButton
    public class DashboardButton : ContentView
    {
        public DashboardButton(string icon, string title, Action action)
        {
            Content = new StackLayout()
            {
                Margin = new Thickness(15, 0, 15, 0),
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    new StackLayout()
                    {
                        Children =
                        {
                            new PancakeView()
                            {
                                HeightRequest = 80,
                                WidthRequest = 80,
                                CornerRadius = 40,
                                Shadow =new DropShadow(){
                                     Opacity=0.3f,
                                     Color = Color.Black,
                                     Offset = new Point(2,2),
                                     BlurRadius=1
                                },
                                BackgroundColor = Color.FromHex(CoreStyles.BackgroundColor),
                                Content=new StackLayout()
                                {
                                    VerticalOptions  = LayoutOptions.Center,
                                    HorizontalOptions = LayoutOptions.Center,
                                    Children =
                                    {
                                        new Label()
                                        {
                                            FontFamily = CoreFontFamily.Fontawesome,
                                            Text = icon,
                                            FontSize = 32,
                                            TextColor = Color.White,
                                            HorizontalTextAlignment = TextAlignment.Center,
                                            VerticalTextAlignment = TextAlignment.Center,
                                        }
                                    }
                                },

                            }.BindTap(async () => {
                                await this.ScaleTo(.9, 75);
                                await this.ScaleTo(1, 75);
                                action?.Invoke();
                            }),

                            new Label()
                            {
                                Text=title,
                                HorizontalTextAlignment = TextAlignment.Center,
                                VerticalTextAlignment = TextAlignment.Center,
                            }
                        }
                    }.IsHeadless()
                }
            }.IsHeadless();
        }
    }
    #endregion

    #region PageBanner
    public class PageBanner : ContentView
    {
        public PageBanner(string title)
        {
            Content = new Grid()
            {
                BackgroundColor = Color.FromHex(CoreStyles.BackgroundColor),
                Children =
                {
                    new StackLayout()
                    {
                        Orientation = StackOrientation.Horizontal,
                        Children =
                        {
                            new Label()
                            {
                                FontFamily = CoreFontFamily.Fontawesome,
                                Text = Fontawesome.Chevron_left,
                                TextColor = Color.WhiteSmoke,
                                VerticalTextAlignment = TextAlignment.Center
                            },
                            new Label()
                            {
                                Text = "BACK",
                                TextColor = Color.WhiteSmoke,
                                VerticalTextAlignment = TextAlignment.Center
                            }.BindTap(async () => {
                                 await Navigation.PopAsync();
                            })
                        }
                    }.IsHeadless().Row(0).Column(0),
                    new StackLayout()
                    {
                        Orientation = StackOrientation.Horizontal,
                        Children =
                        {
                            new CachedImage()
                            {
                                Source = ImageSource.FromFile("monkeyHead.png"),
                                HeightRequest = 25,
                                Aspect = Aspect.AspectFit
                            },
                            new Label()
                            {
                                Text = title,
                                TextColor = Color.WhiteSmoke,
                                VerticalTextAlignment = TextAlignment.Center
                            }
                        }
                    }.IsHeadless().Row(0).Column(1),
                    new StackLayout()
                    {
                        Orientation = StackOrientation.Horizontal,
                        Children =
                        {
                            new StackLayout(){ HorizontalOptions=LayoutOptions.StartAndExpand}.IsHeadless(),
                            new Label()
                            {
                                FontFamily = CoreFontFamily.Fontawesome,
                                Text = Fontawesome.Home,
                                TextColor = Color.WhiteSmoke,
                                VerticalTextAlignment = TextAlignment.Center,
                                FontSize = 22
                            },
                        }
                    }.IsHeadless().Row(0).Column(2),

                }
            };
        }
    }
    #endregion
}
