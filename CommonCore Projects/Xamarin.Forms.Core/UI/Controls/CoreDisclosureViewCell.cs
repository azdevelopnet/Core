using System;
namespace Xamarin.Forms.Core.Controls
{
    public class CoreDisclosureViewCell : ViewCell
    {
        private ContentView content;

        public static readonly BindableProperty ActiveColorProperty =
            BindableProperty.Create(propertyName: "ActiveColor",
            returnType: typeof(Color),
            declaringType: typeof(CoreDisclosureViewCell),
            defaultValue: Color.Black);

        public static readonly BindableProperty InActiveColorProperty =
            BindableProperty.Create(propertyName: "InActiveColor",
            returnType: typeof(Color),
            declaringType: typeof(CoreDisclosureViewCell),
            defaultValue: Color.LightGray);

        public Color InActiveColor
        {
            get { return (Color)GetValue(InActiveColorProperty); }
            set { SetValue(InActiveColorProperty, value); }
        }
        public Color ActiveColor
        {
            get { return (Color)GetValue(ActiveColorProperty); }
            set { SetValue(ActiveColorProperty, value); }
        }

        public ContentView Content
        {
            get { return content; }
            set
            {

                content = value;
                SetContent();
            }
        }

        public CoreDisclosureViewCell()
        {
            this.StyleId = "disclosure";
        }

        private void SetContent()
        {

#if __ANDROID__
            Content.HorizontalOptions = LayoutOptions.StartAndExpand;
            this.View = new StackLayout()
            {
                Orientation= StackOrientation.Horizontal,
                Children=
                {
                    Content,
                    new Label()
                    {
                        
                    }
                }
            };
#endif

#if __IOS__
            this.View = content;
#endif

        }
    }
}
