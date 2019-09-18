using System;
using BackgroundingExample.Models;
using Xamarin.Forms;
using Xamarin.Forms.Core;

namespace BackgroundingExample
{
    public class SomePage : CorePage<SomeViewModel>
    {
        public SomePage()
        {
            this.Title = "Some Page";
            this.Visual = VisualMarker.Material;

            Content = new StackLayout()
            {
                Padding = 20,
                Children = {
                    new Button()
                    {
                        Text="Start Single Process",
                        TextColor = Color.Black,
                        BackgroundColor = Color.Yellow,
                        Margin = new Thickness(20,60,20,10),
                        Command = new Command(()=>{
                            DependencyService.Get<IBackgroundTask>().RegisterBackgroundProcess<MyJob>();
                        })
                    },
                    new StackLayout()
                    {
                        HeightRequest = 1,
                        Margin = 5,
                        BackgroundColor = Color.Gray
                    },
                    new Button()
					{
						Text="Start Timer Process",
						TextColor = Color.Black,
						BackgroundColor = Color.Yellow,
						Margin = new Thickness(20,20,20,10),
						Command = new Command(()=>{
                            DependencyService.Get<IBackgroundTask>().RegisterTimerBackgroundProcess<MyJob>(1);
						})
					},
                    new Button()
                    {
                        Text="Stop Timer Process",
                        TextColor = Color.Black,
                        BackgroundColor = Color.Yellow,
                        Margin = new Thickness(20,20,20,10),
                        Command = new Command(()=>{
                            DependencyService.Get<IBackgroundTask>().StopTimerBackgroundProcess<MyJob>();
                        })
                    },
                    new StackLayout()
                    {
                        HeightRequest = 1,
                        Margin = 5,
                        BackgroundColor = Color.Gray
                    },
                    new Button()
                    {
                        Text="Start Periodic Process",
                        TextColor = Color.Black,
                        BackgroundColor = Color.Yellow,
                        Margin = new Thickness(20,20,20,10),
                        Command = new Command(()=>{
                            DependencyService.Get<IBackgroundTask>().RegisterPeriodicBackgroundProcess<MyJob>(20);
						})
                    },
                    new Button()
                    {
                        Text="Stop Periodic Process",
                        TextColor = Color.Black,
                        BackgroundColor = Color.Yellow,
                        Margin = new Thickness(20,20,20,10),
                        Command = new Command(()=>{
                            DependencyService.Get<IBackgroundTask>().StopPeriodicBackgroundProcess<MyJob>();
                        })
                    }
                }
            };
        }

    }
}