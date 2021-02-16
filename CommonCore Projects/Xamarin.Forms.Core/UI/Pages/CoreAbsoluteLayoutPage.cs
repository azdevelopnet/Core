using System;
using System.Linq;

namespace Xamarin.Forms.Core
{
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

    public abstract class CoreAbsoluteLayoutPage : CorePage
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

    public enum DisplayPosition
    {
        Above,
        Below
    }

    public class AnchorPopup
    {
        public bool UseParentBindingContext { get; set; } = true;
        public double Width { get; set; }
        public double Height { get; set; }
        public View AnchorView { get; set; }
        public DisplayPosition DisplayPosition { get; set; } = DisplayPosition.Below;

        public Rectangle ToAbsoluteRectange(Page page, View popup)
        {
            if (AnchorView != null && page != null)
            {
                var coord = DependencyService.Get<IVisualElementLocation>().GetCoordinates(AnchorView);
                var xPosition = (double)coord.X;
                var yPosition = 0.0;
                if (DisplayPosition == DisplayPosition.Below)
                {
                    yPosition = (double)(coord.Y + AnchorView.Height);
                }
                else
                {
                    yPosition = (double)(coord.Y - popup.HeightRequest);
                }
                return new Rectangle(xPosition, yPosition, Width, Height);
            }
            else
            {
                return new Rectangle();
            }
        }
    }

    public class PagePopup
    {
        public bool UseParentBindingContext { get; set; } = true;
        public double PercentHorizontal { get; set; } = 0.5;
        public double PercentVertical { get; set; } = 0.5;
        public double PercentWidth { get; set; } = 0.85;
        public double PercentHeight { get; set; } = 0.5;
        public bool HasBackgroundOverlay { get; set; } = false;
        public double OverlayOpacity { get; set; } = 1;
        public Color OverlayColor { get; set; } = Color.FromHex("#80000000");

        public Rectangle ToPercentRectange()
        {
            return new Rectangle(PercentHorizontal, PercentVertical, PercentWidth, PercentWidth);
        }

    }

    public static class CoreAbsoluteLayoutPageExtensions
    {
        public static void ShowAnchorPopup(this Page page, View popup, AnchorPopup parameters)
        {
            if (popup == null || parameters == null || parameters.AnchorView == null)
                return;

            var layout = page.GetAbsoluteLayout();
            if (layout != null)
            {

                popup.AutomationId = "CorePopupAutomationId";
                if (parameters.UseParentBindingContext)
                    popup.BindingContext = layout.BindingContext;

                AbsoluteLayout.SetLayoutBounds(popup, parameters.ToAbsoluteRectange(page, popup));
                layout.Children.Add(popup);

                popup.ScaleTo(0.99, 200).ContinueWith(async (t) =>
                {
                    await popup.ScaleTo(1, 200);
                });
            }
        }
        public static void ShowPagePopup(this Page page, View popup, PagePopup parameters)
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
                        AutomationId = "CoreContainerBackdropId",
                        BackgroundColor = parameters.OverlayColor,
                        Opacity = parameters.OverlayOpacity
                    };

                    AbsoluteLayout.SetLayoutBounds(overlay, new Rectangle(1, 1, 1, 1));
                    AbsoluteLayout.SetLayoutFlags(overlay, AbsoluteLayoutFlags.All);
                    layout.Children.Add(overlay);
                }


                popup.AutomationId = "CorePopupAutomationId";
                if (parameters.UseParentBindingContext)
                    popup.BindingContext = layout.BindingContext;

                AbsoluteLayout.SetLayoutBounds(popup, parameters.ToPercentRectange());
                AbsoluteLayout.SetLayoutFlags(popup, AbsoluteLayoutFlags.All);
                layout.Children.Add(popup);

                popup.ScaleTo(0.99, 200).ContinueWith(async (t) =>
                {
                    await popup.ScaleTo(1, 200);
                });
            }
        }

        public static bool PopupIsOpen(this Page page)
        {
            var layout = page.GetAbsoluteLayout();
            var popup = layout.Children.FirstOrDefault(x => x.AutomationId == "CorePopupAutomationId");
            if (popup != null)
                return true;
            else
                return false;
        }

        public static void ClosePopup(this Page page)
        {
            var layout = page.GetAbsoluteLayout();
            if (layout != null)
            {
                var overlay = layout.Children.FirstOrDefault(x => x.AutomationId == "CoreContainerBackdropId");
                var popup = layout.Children.FirstOrDefault(x => x.AutomationId == "CorePopupAutomationId");

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

