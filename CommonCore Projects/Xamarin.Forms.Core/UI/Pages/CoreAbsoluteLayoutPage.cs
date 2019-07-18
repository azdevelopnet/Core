using System;
using Xamarin.Forms.Core.MaterialDesign;

namespace Xamarin.Forms.Core
{
	public interface IPopup
    {
        AbsoluteLayout Parent { get; set; }
        Frame ParentObject { get; set; }
    }
    public class PopupView : ContentView, IPopup
    {
        public bool AnimateOpen { get; set; } = true;
        public float CornerRadius = 3;
        public bool HasShadow { get; set; } = true;
		public Color BorderColor { get; set; } = Color.Gray;
        public new bool IsClippedToBounds { get; set; } = false;
        public new AbsoluteLayout Parent { get; set; }
        public Frame ParentObject { get; set; }
        public virtual void Close()
        {
            this.Parent.Children.Remove(ParentObject);
        }
    }

    public abstract class CoreAbsoluteLayoutPage<T> : CorePage<T>
     where T : CoreViewModel, new()
    {
        private AbsoluteLayout layout;
        private View content;
        private Frame wrapper;

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


        public void ShowPopup(PopupView view, Rectangle bounds, int padding)
        {

            wrapper = new Frame()
            {
                Content = view,
                HasShadow = view.HasShadow,
				BorderColor = view.BorderColor,
                IsClippedToBounds = view.IsClippedToBounds,
                CornerRadius = view.CornerRadius,
                Padding = padding
            };
            ((IPopup)view).Parent = this.layout;
            ((IPopup)view).ParentObject = this.wrapper;

            AbsoluteLayout.SetLayoutBounds(wrapper, bounds);
            AbsoluteLayout.SetLayoutFlags(wrapper, AbsoluteLayoutFlags.All);
            this.layout.Children.Add(wrapper);
            view.BindingContext = this.BindingContext;
            if (view.AnimateOpen)
            {
                wrapper.ScaleTo(0.99, 200).ContinueWith(async (t) =>
                {
                    await wrapper.ScaleTo(1, 200);
                });
            }
        }

        public void ClosePopup()
        {
            if (wrapper != null)
            {
                ((IPopup)wrapper).Parent = null;
                this.layout.Children.Remove(wrapper);
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
        private Frame wrapper;

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

        public void ShowPopup(PopupView view, Rectangle bounds, int padding)
        {

            wrapper = new Frame()
            {
                Content = view,
                HasShadow = view.HasShadow,
				BorderColor = view.BorderColor,
                IsClippedToBounds = view.IsClippedToBounds,
                CornerRadius = view.CornerRadius,
                Padding = padding
            };
            ((IPopup)view).Parent = this.layout;
            ((IPopup)view).ParentObject = this.wrapper;

            AbsoluteLayout.SetLayoutBounds(wrapper, bounds);
            AbsoluteLayout.SetLayoutFlags(wrapper, AbsoluteLayoutFlags.All);
            this.layout.Children.Add(wrapper);
            view.BindingContext = this.BindingContext;
            if (view.AnimateOpen)
            {
                wrapper.ScaleTo(0.99, 200).ContinueWith(async (t) =>
                {
                    await wrapper.ScaleTo(1, 200);
                });
            }
        }

        public void ClosePopup()
        {
            if (wrapper != null)
            {
                ((IPopup)wrapper).Parent = null;
                this.layout.Children.Remove(wrapper);
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

