using System;
using System.Linq;

namespace Xamarin.Forms.Core
{
    public enum DisplayPosition
    {
        Above,
        Below
    }
    public class PopupParameter
    {
        public bool UseParentBindingContext { get; set; } = true;
        public bool IsAnimatedOpen { get; set; } = true;
        public bool IsAnimatedClose { get; set; } = true;
        public string ContainerAutomationId { get; set; } = "CorePopupAutomationId";
        public double PercentHorizontal { get; set; } = 0.5;
        public double PercentVertical { get; set; } = 0.5;
        public double PercentWidth { get; set; } = 0.85;
        public double PercentHeight { get; set; } = 0.5;

        public bool HasBackgroundOverlay { get; set; } = false;
        public string OverlayAutomationId { get; set; } = "CoreContainerBackdropId";
        public double OverlayOpacity { get; set; } = 1;
        public Color OverlayColor { get; set; } = Color.FromHex("#80000000");
        public View AnchorView { get; set; }
        public DisplayPosition DisplayPosition { get; set; } = DisplayPosition.Below;

        public Rectangle ToPercentRectange()
        {
            return new Rectangle(PercentHorizontal, PercentVertical, PercentWidth, PercentWidth);
        }
        public Rectangle ToAbsoluteRectange(Page page, View popup)
        {

            if (AnchorView != null && page != null)
            {
                var coord = DependencyService.Get<IVisualElementLocation>().GetCoordinates(AnchorView);
                PercentHorizontal = coord.X;
                if (DisplayPosition == DisplayPosition.Below)
                {
                    PercentVertical = coord.Y + AnchorView.Height;
                }
                else
                {
                    PercentVertical = coord.Y - popup.HeightRequest;
                }
            
                PercentWidth = page.Width * PercentWidth;
                PercentHeight = page.Height * PercentHeight;
            }
            return new Rectangle(PercentHorizontal, PercentVertical, PercentWidth, PercentWidth);
        }
    }

    public abstract class CoreAbsoluteLayoutPage<T> : CorePage<T>
     where T : CoreViewModel, new()
    {
        private AbsoluteLayout _layout;
        private View _content;

        public new View Content
        {
            get { return _content; }
            set
            {
                if (value != null)
                {
                    _content = value;
                    if (_layout == null)
                        _layout = new AbsoluteLayout();

                    AbsoluteLayout.SetLayoutBounds(_content, new Rectangle(1, 1, 1, 1));
                    AbsoluteLayout.SetLayoutFlags(_content, AbsoluteLayoutFlags.All);
                    _layout.Children.Add(this._content);

                    if (base.Content == null)
                        base.Content = _layout;
                }
            }
        }

        protected override void OnAppearing()
        {
            this.SizeChanged += PageSizeChanged;
            base.OnAppearing();
            PageSizeChanged(this, null);
        }
        protected override void OnDisappearing()
        {
            this.SizeChanged -= PageSizeChanged;
            base.OnDisappearing();
        }

        public virtual void PageSizeChanged(object sender, EventArgs args) { }

        public void Dispose()
        {
            this.SizeChanged -= PageSizeChanged;
        }

    }


    public abstract class CoreAbsoluteLayoutPage: CorePage
    {
        private AbsoluteLayout _layout;
        private View _content;

        public new View Content
        {
            get { return _content; }
            set
            {
                if (value != null)
                {
                    _content = value;
                    if (_layout == null)
                        _layout = new AbsoluteLayout();

                    AbsoluteLayout.SetLayoutBounds(_content, new Rectangle(1, 1, 1, 1));
                    AbsoluteLayout.SetLayoutFlags(_content, AbsoluteLayoutFlags.All);
                    _layout.Children.Add(this._content);

                    if (base.Content == null)
                        base.Content = _layout;
                }
            }
        }
        protected override void OnAppearing()
        {
            this.SizeChanged += PageSizeChanged;
            base.OnAppearing();
            PageSizeChanged(this, null);
        }
        protected override void OnDisappearing()
        {
            this.SizeChanged -= PageSizeChanged;
            base.OnDisappearing();
        }

        public virtual void PageSizeChanged(object sender, EventArgs args) { }

        public void Dispose()
        {
            this.SizeChanged -= PageSizeChanged;
        }
    }

    public static class CoreAbsoluteLayoutPageExtensions
    {
        public static void ShowPopup(this Page page, View popup, PopupParameter parameters)
        {
            if (popup == null || parameters == null)
                return;

            var layout = page.GetAbsoluteLayout();
            if (layout != null)
            {

                if (parameters.HasBackgroundOverlay)
                {
                    var overlay = new StackLayout()
                    {
                        AutomationId = parameters.OverlayAutomationId,
                        BackgroundColor = parameters.OverlayColor,
                        Opacity = parameters.OverlayOpacity
                    };

                    AbsoluteLayout.SetLayoutBounds(overlay, new Rectangle(1, 1, 1, 1));
                    AbsoluteLayout.SetLayoutFlags(overlay, AbsoluteLayoutFlags.All);
                    layout.Children.Add(overlay);
                }


                popup.AutomationId = parameters.ContainerAutomationId;
                if (parameters.UseParentBindingContext)
                    popup.BindingContext = layout.BindingContext;

                if (parameters.AnchorView != null)
                {
                    AbsoluteLayout.SetLayoutBounds(popup, parameters.ToAbsoluteRectange(page, popup));
                }
                else
                {
                    AbsoluteLayout.SetLayoutBounds(popup, parameters.ToPercentRectange());
                    AbsoluteLayout.SetLayoutFlags(popup, AbsoluteLayoutFlags.All);
                }
                layout.Children.Add(popup);

                if (parameters.IsAnimatedOpen)
                {
                    popup.ScaleTo(0.99, 200).ContinueWith(async (t) =>
                    {
                        await popup.ScaleTo(1, 200);
                    });
                }
            }
        }

        public static void ClosePopup(this Page page, PopupParameter parameters)
        {
            if (parameters == null)
                return;

            var layout = page.GetAbsoluteLayout();
            if (layout != null)
            {
                var overlay = layout.Children.FirstOrDefault(x => x.AutomationId == parameters.OverlayAutomationId);
                var popup = layout.Children.FirstOrDefault(x => x.AutomationId == parameters.ContainerAutomationId);

                if (popup != null)
                {
                    layout.Children.Remove(popup);
                }
                if (overlay != null)
                {
                    layout.Children.Remove(overlay);
                    overlay = null;
                }
            }
        }

        private static AbsoluteLayout GetAbsoluteLayout(this Page page)
        {
            if (page is ContentPage)
            {
                var contentPage = (ContentPage)page;
                if (contentPage.Content is AbsoluteLayout)
                    return (AbsoluteLayout)contentPage.Content;
                else
                    return null;
            }
            if (page is NavigationPage)
            {
                var nav = (NavigationPage)page;
                if (nav.CurrentPage is ContentPage)
                {
                    var contentPage = (ContentPage)nav.CurrentPage;
                    return contentPage.GetAbsoluteLayout();
                }
            }
            if (page is CarouselPage)
            {
                var carouselPage = (CarouselPage)page;
                return carouselPage.CurrentPage.GetAbsoluteLayout();
            }
            if (page is TabbedPage)
            {
                var tabbedPage = (TabbedPage)page;
                return tabbedPage.CurrentPage.GetAbsoluteLayout();
            }
            if (page is FlyoutPage)
            {
                var flyoutPage = (FlyoutPage)page;
                return flyoutPage.Detail.GetAbsoluteLayout();
            }

            return null;
        }
    }
}

