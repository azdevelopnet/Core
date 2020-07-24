using System;
using PView = Xamarin.Forms.PancakeView.PancakeView;

namespace Xamarin.Forms.Core
{
    public class PopupBounds
    {
        public PopupBounds(double percentHorizontal, double percentVertical, double percentWidth, double percentheight)
        {
            PercentHorizontal = percentHorizontal;
            PercentVertical = percentVertical;
            PercentWidth = percentWidth;
            Percentheight = percentheight;
        }
        public PopupBounds()
        {

        }
        public double PercentHorizontal { get; set; } = 0.5;
        public double PercentVertical { get; set; } = 0.5;
        public double PercentWidth { get; set; } = 0.85;
        public double Percentheight { get; set; } = 0.5;

        public Rectangle ToRectangle()
        {
            return new Rectangle(PercentHorizontal, PercentVertical, PercentWidth, Percentheight);
        }
    }
    public class PopupView : ContentView
    {
        public bool AnimateOpen { get; set; } = true;
        public float CornerRadius = 3;
        public bool HasShadow { get; set; } = true;
		public Color BorderColor { get; set; } = Color.Gray;
        public new bool IsClippedToBounds { get; set; } = false;
    }

    public abstract class CoreAbsoluteLayoutPage<T> : CorePage<T>
     where T : CoreViewModel, new()
    {
        private AbsoluteLayout layout;
        private View content;
        private PView pView;
        private StackLayout backDrop;

        public new View Content
        {
            get { return this.content; }
            set
            {
                if (this.content != null)
                    this.layout.Children.Remove(this.content);

                this.content = value;
                AbsoluteLayout.SetLayoutBounds(content, new Rectangle(1, 1, 1, 1));
                AbsoluteLayout.SetLayoutFlags(content, AbsoluteLayoutFlags.All);
                this.layout.Children.Add(this.content);
            }
        }

        public AbsoluteLayout AbsoluteLayer
        {
            get { return layout; }
            set { layout = value; }
        }


        public void ShowPopup(PopupView view, PopupBounds bounds, int padding)
        {
            pView = new PView()
            {
                Content = view,
                Border = new PancakeView.Border()
                {
                    Color = view.BorderColor
                },
                CornerRadius = view.CornerRadius,
                IsClippedToBounds = view.IsClippedToBounds,
                BackgroundColor = Color.White,
                Padding = padding,
                Shadow = new PancakeView.DropShadow()
                {
                    Color = Color.White,
                    Offset = new Point(1, 1),
                    BlurRadius = 1,
                    Opacity = 0.7f
                }
            };
            backDrop = new StackLayout()
            {
                BackgroundColor = Color.Black,
                Opacity= 0.5
            };


            AbsoluteLayout.SetLayoutBounds(backDrop, new Rectangle(1,1,1,1));
            AbsoluteLayout.SetLayoutFlags(backDrop, AbsoluteLayoutFlags.All);
            this.layout.Children.Add(backDrop);

            AbsoluteLayout.SetLayoutBounds(pView, bounds.ToRectangle());
            AbsoluteLayout.SetLayoutFlags(pView, AbsoluteLayoutFlags.All);
            this.layout.Children.Add(pView);

            view.BindingContext = this.BindingContext;
            if (view.AnimateOpen)
            {
                pView.ScaleTo(0.99, 200).ContinueWith(async (t) =>
                {
                    await pView.ScaleTo(1, 200);
                });
            }
        }

        public void ClosePopup()
        {
            if (pView != null)
            {
                this.layout.Children.Remove(pView);
                pView = null;
            }
            if (backDrop != null)
            {
                this.layout.Children.Remove(backDrop);
                backDrop = null;
            }
        }

        public CoreAbsoluteLayoutPage()
        {
            base.Content = this.layout = new AbsoluteLayout() { };
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
        private AbsoluteLayout layout;
        private View content;
        private PView pView;

        public new View Content
        {
            get { return this.content; }
            set
            {
                if (this.content != null)
                    this.layout.Children.Remove(this.content);

                this.content = value;
                AbsoluteLayout.SetLayoutBounds(content, new Rectangle(1, 1, 1, 1));
                AbsoluteLayout.SetLayoutFlags(content, AbsoluteLayoutFlags.All);
                this.layout.Children.Add(this.content);
            }
        }

        public AbsoluteLayout AbsoluteLayer
        {
            get { return layout; }
            set { layout = value; }
        }

        public void ShowPopup(PopupView view, PopupBounds bounds, int padding)
        {

            pView = new PView()
            {
                Content = view,
                Border = new PancakeView.Border()
                {
                    Color = view.BorderColor
                },
                IsClippedToBounds = view.IsClippedToBounds,
                CornerRadius = view.CornerRadius,
                Padding = padding
            };

            if (view.HasShadow)
            {
                pView.Shadow = new PancakeView.DropShadow()
                {
                    Color = Color.White,
                    Offset = new Point(1, 1),
                    BlurRadius = 1,
                    Opacity = 0.7f
                };
            }

            AbsoluteLayout.SetLayoutBounds(pView, bounds.ToRectangle());
            AbsoluteLayout.SetLayoutFlags(pView, AbsoluteLayoutFlags.All);
            this.layout.Children.Add(pView);
            view.BindingContext = this.BindingContext;
            if (view.AnimateOpen)
            {
                pView.ScaleTo(0.99, 200).ContinueWith(async (t) =>
                {
                    await pView.ScaleTo(1, 200);
                });
            }
        }

        public void ClosePopup()
        {
            if (pView != null)
            {
                this.layout.Children.Remove(pView);
                pView = null;
            }
        }

        public CoreAbsoluteLayoutPage()
        {
            base.Content = this.layout = new AbsoluteLayout() { };
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
}

