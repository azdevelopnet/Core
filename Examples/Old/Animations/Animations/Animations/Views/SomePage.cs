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

            Content = new ScrollView()
            {
                Content = new StackContainer()
                {
                    Padding = 20,
                    Spacing = 10,
                    Children =
                    {
                        CreateAnimation(
                            "Bounce In",
                            Color.Red,
                            new CoreBounceInAnimation()
                            {
                                Duration = "500"
                            }
                        ),
                        CreateAnimation(
                            "Flip",
                            Color.Olive,
                            new CoreFlipAnimation()
                            {
                                Duration = "500"
                            }
                        ),
                        CreateAnimation(
                            "Rotate",
                            Color.Gray,
                            new CoreRotateToAnimation()
                            {
                                Duration = "500",
                                Rotation = 360
                            }
                        ),
                        CreateAnimation(
                            "Fade",
                            Color.ForestGreen,
                            new CoreFadeToAnimation()
                            {
                                Duration = "500",
                                Opacity = 0
                            }
                        ),
                        CreateAnimation(
                            "Scale",
                            Color.DarkTurquoise,
                            new CoreScaleToAnimation()
                            {
                                Duration = "500",
                                Scale = 0.5
                            }
                        ),
                        CreateAnimation(
                            "Translate",
                            Color.DarkOrange,
                            new CoreTranslateToAnimation()
                            {
                                Duration = "250",
                                TranslateX = -5,
                                TranslateY = -32
                            }
                        ),
                        CreateAnimation(
                            "Turnstile Out",
                            Color.Cyan,
                            new CoreTurnstileOutAnimation()
                            {
                                Duration = "150",
                            }
                        ),
                        CreateAnimation(
                            "Shake",
                            Color.BlanchedAlmond,
                            new CoreShakeAnimation()
                        ),
                        CreateAnimation(
                            "Heartbeat",
                            Color.DarkRed,
                            new CorePulseAnimation()
                            {
                                Duration = "250"
                            }
                        ),
                        CreateAnimation(
                            "Jump",
                            Color.Aquamarine,
                            new CorePulseAnimation()
                            {
                                Duration = "500"
                            }
                        ),
                        CreateAnimation(
                            "StoryBoard",
                            Color.Brown,
                            new CoreStoryBoard()
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
                        ),
                        CreateDataTrigger()
                    }
                }
            };
        }

        public GridContainer CreateAnimation(string buttonText, Color boxColor, AnimationBase animationBase)
        {

            BoxView box = null;
            
            var container = new GridContainer()
            {
                Children =
                {
                    new BoxView()
                    {
                        HeightRequest = 28,
                        WidthRequest = 28,
                        BackgroundColor = boxColor
                    }.Assign(out box).Row(0).Col(0),
                    new CoreButton()
                    {
                        Text = buttonText,
                        Style = CoreStyles.LightOrange,
                        Triggers =
                        {
                            new EventTrigger()
                            {
                                Event = "Clicked",
                                Actions =
                                {
                                    new CoreTriggerAction()
                                    {
                                        Animation = animationBase,
                                    }.AssignTarget(box)
                                }
                            }
                        }
                    }.Row(0).Col(1)

                }
            };

            return container;
        }

        public GridContainer CreateDataTrigger()
        {
            BoxView box;

            return new GridContainer()
            {
                Children =
                {
                    new BoxView()
                    {
                        HeightRequest = 28,
                        WidthRequest = 28,
                        BackgroundColor = Color.Crimson
                    }.Assign(out box).Row(0).Col(0),
                    new CoreButton()
                    {
                        Text = "Data Trigger (3)",
                        Style = CoreStyles.LightOrange,
                        Triggers = {
                            new DataTrigger(typeof(Button))
                            {
                                Binding = new Binding(path: "ClickCount", mode: BindingMode.TwoWay),
                                Value = 3,
                                EnterActions =
                                {
                                    new CoreTriggerAction()
                                    {
                                        Animation = new CoreFadeToAnimation()
                                        {
                                            Target = box,
                                            Duration = "300",
                                            Opacity = 0
                                        }
                                    }
                                }
                            }
                        }
                    }.Bind(CoreButton.CommandProperty,"ClickEvent")
                    .Row(0).Col(1)
                }
            };

        }
    }
}