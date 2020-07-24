using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Markup;

namespace Xamarin.Forms.Core
{
    public static class SlideLayoutExtension
    {
        public static RightSlideLayout AddScrollableContent(this RightSlideLayout content, View view)
        {
            content.MainContent.Children.Add(new ScrollView() { Content = view });
            return content;
        }
        public static RightSlideLayout AddContent(this RightSlideLayout content, View view)
        {
            content.MainContent.Children.Add(view);
            return content;
        }
    }
    public class RightSlideLayoutViewModel : CoreViewModel
    {
        public IRightSlideLayout SlideView { get; set; }
        public override void OnViewMessageReceived(string key, object obj) { }

        public async Task OpenPanel()
        {
            if (SlideView != null)
            {
                await SlideView.OpenPanel();
            }
        }
        public async Task ClosePanel()
        {
            if (SlideView != null)
            {
                await SlideView.ClosePanel();
            }
        }
    }

    public interface IRightSlideLayout
    {
        Task OpenPanel();
        Task ClosePanel();
        View Content { get; set; }
    }

    public class RightSlideLayout: CoreContentView<RightSlideLayoutViewModel>, IRightSlideLayout
    {
        public StackLayout MainContent;
        public StackLayout SlideContent;
        private StackLayout SlideContainer;
        private StackLayout tintPanel;
        private Grid ContentGrid;

        public static double PanelWidth { get; set; }


        public RightSlideLayout()
        {
            VM.SlideView = this;

            tintPanel = new StackLayout()
            {
                Opacity = 0.2,
                BackgroundColor = Color.Black
            };

            Content = new Grid()
            {
                Children =
                {
                    new StackLayout().Assign(out MainContent).Row(0).Column(0),
                    new StackLayout()
                    {
                        Orientation = StackOrientation.Horizontal,
                        Children =
                        {
                            new StackLayout(){
                                HorizontalOptions = LayoutOptions.StartAndExpand,
                            },
                            new StackLayout()
                            {
                               WidthRequest = RightSlideLayout.PanelWidth,
                            }.Assign(out SlideContent)
                        }
                    }.Assign(out SlideContainer).Row(0).Column(0)
                    //.BindViewTap(async()=>{
                    //    await ClosePanel();
                    //})
                }
            }.Assign(out ContentGrid);

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                SlideContainer.InputTransparent = true;
                await SlideContent.TranslateTo(RightSlideLayout.PanelWidth, 0, 100);
            });
        }

        public async Task OpenPanel()
        {
            if (SlideContent.TranslationX != 0)
            {
                ContentGrid.Children.Insert(1, tintPanel);
                await SlideContent.TranslateTo(0, 0, 100);
                SlideContainer.InputTransparent = false;
            }
            else
            {
                ContentGrid.Children.Remove(tintPanel);
                SlideContainer.InputTransparent = true;
                await SlideContent.TranslateTo(RightSlideLayout.PanelWidth, 0, 0);
            }
        }

        public async Task ClosePanel()
        {
            if (SlideContent.TranslationX != RightSlideLayout.PanelWidth)
            {
                ContentGrid.Children.Remove(tintPanel);
                SlideContainer.InputTransparent = true;
                await SlideContent.TranslateTo(RightSlideLayout.PanelWidth, 0, 100);
            }
            else
            {
                ContentGrid.Children.Insert(1, tintPanel);
                await SlideContent.TranslateTo(0, 0, 0);
                SlideContainer.InputTransparent = false;
            }
        }
    }
}
