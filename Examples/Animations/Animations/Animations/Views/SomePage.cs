using System;
using Xamarin.Forms;
using Xamarin.Forms.Core;

namespace Animations
{
    public class SomePage : CorePage<SomeViewModel>
    {
        public SomePage()
        {
            Title = "Animations";

            var bounceButton = CreateAnimation(
                "Bounce In",
                Color.Red,
                new CoreTriggerAction()
                {
                    Animation = new CoreBounceInAnimation()
                    {
                        Duration = "500"
                    }
                }
            );

            var flipButton = CreateAnimation(
                "Flip",
                Color.Olive,
                new CoreTriggerAction()
                {
                    Animation = new CoreFlipAnimation()
                    {
                        Duration = "500"
                    }
                }
            );

            var roateButton = CreateAnimation(
                "Rotate",
                Color.Gray,
                new CoreTriggerAction()
                {
                    Animation = new CoreRotateToAnimation()
                    {
                        Duration = "500",
                        Rotation = 360
                    }
                }
            );


            var fadeButton = CreateAnimation(
                "Fade",
                Color.ForestGreen,
                new CoreTriggerAction()
                {
                    Animation = new CoreFadeToAnimation()
                    {
                        Duration = "500",
                        Opacity = 0
                    }
                }
            );

            var scaleButton = CreateAnimation(
                "Scale",
                Color.DarkTurquoise,
                new CoreTriggerAction()
                {
                    Animation = new CoreScaleToAnimation()
                    {
                        Duration = "500",
                        Scale = 0.5
                    }
                }
            );

            var translateButton = CreateAnimation(
                "Translate",
                Color.DarkOrange,
                new CoreTriggerAction()
                {
                    Animation = new CoreTranslateToAnimation()
                    {
                        Duration = "250",
                        TranslateX = -5,
                        TranslateY = -32
                    }
                }
            );

            var turnsTileButton = CreateAnimation(
                "Turnstile Out",
                Color.Cyan,
                new CoreTriggerAction()
                {
                    Animation = new CoreTurnstileOutAnimation()
                    {
                        Duration = "150",
                    }
                }
            );

            var shakeButton = CreateAnimation(
                "Shake",
                Color.BlanchedAlmond,
                new CoreTriggerAction()
                {
                    Animation = new CoreShakeAnimation()
                }
            );

            var heartBeatButton = CreateAnimation(
                "Heartbeat",
                Color.DarkRed,
                new CoreTriggerAction()
                {
                    Animation = new CorePulseAnimation()
                    {
                        Duration = "250"
                    }
                }
            );

            var jumpButton = CreateAnimation(
                "Jump",
                Color.Aquamarine,
                new CoreTriggerAction()
                {
                    Animation = new CorePulseAnimation()
                    {
                        Duration = "500"
                    }
                }
            );

            var storyboardButton = CreateAnimation(
                "StoryBoard",
                Color.Brown,
                new CoreTriggerAction()
                {
                    Animation = new CoreStoryBoard()
                    {
                        Animations ={
                            new CoreShakeAnimation(),
                            new CoreScaleToAnimation()
                            {
                                Duration = "500",
                                Scale=0.8
                            }
                        }
                    }
                }
            );

            var dataTrigger = CreateDataTrigger();


            var container = new StackContainer(true)
            {

                Padding = 20,
                Spacing = 10,
                Children = {
                    bounceButton,
                    flipButton,
                    roateButton,
                    fadeButton,
                    scaleButton,
                    translateButton,
                    turnsTileButton,
                    shakeButton,
                    heartBeatButton,
                    jumpButton,
                    storyboardButton,
                    dataTrigger
                }
            };

            Content = new ScrollView()
            {
                Content = container
            };
        }

        public GridContainer CreateAnimation(string buttonText, Color boxColor, CoreTriggerAction animation)
        {
            var box = new BoxView()
            {
                HeightRequest = 28,
                WidthRequest = 28,
                BackgroundColor = boxColor
            };

            animation.Animation.Target = box;

            var trigger = new EventTrigger()
            {
                Event = "Clicked",
                Actions = { animation }
            };

            var btn = new CoreButton()
            {
                Text = buttonText,
                Style = CoreStyles.LightOrange,
                Triggers = { trigger }
            };

            var grid = new GridContainer(true);

            grid.AddChild(box, 0, 0);
            grid.AddChild(btn, 0, 1);
            return grid;
        }

        public GridContainer CreateDataTrigger()
        {
            var box = new BoxView()
            {
                HeightRequest = 28,
                WidthRequest = 28,
                BackgroundColor = Color.Crimson
            };

            var fadeAnimation = new CoreTriggerAction()
            {
                Animation = new CoreFadeToAnimation()
                {
                    Target = box,
                    Duration = "300",
                    Opacity = 0
                }
            };

            var trigger = new DataTrigger(typeof(Button))
            {
                Binding = new Binding(path: "ClickCount", mode: BindingMode.TwoWay),
                Value = 3,
                EnterActions = { fadeAnimation }
            };


            var btn = new CoreButton()
            {
                Text = "Data Trigger (3)",
                Style = CoreStyles.LightOrange,
                Triggers = { trigger }
            };
            btn.SetBinding(CoreButton.CommandProperty, "ClickEvent");

            var grid = new GridContainer(true);

            grid.AddChild(box, 0, 0);
            grid.AddChild(btn, 0, 1);
            return grid;
        }
    }
}