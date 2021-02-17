using System;
namespace Xamarin.Forms.Core.UI.Pages
{
    public class CoreAbsoluteContentView<T> : CoreContentView<T>
         where T : CoreViewModel
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
    }
}
