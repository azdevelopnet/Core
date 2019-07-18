using System;
using Xamarin.Forms;
using Xamarin.Forms.Core;

namespace Fonts.Views
{
    public class FontsCollectionView : CorePage<SomeViewModel>
    {
        public FontsCollectionView()
        {
            this.Title = VM.FontType.ToString();
            var list = new CoreListView()
            {
                ItemTemplate = new DataTemplate(typeof(FontsCollectionViewCell)),
                RowHeight = 75,
                SeparatorColor = Color.Transparent,
                SeparatorVisibility = SeparatorVisibility.None
            };
            list.SetBinding(CoreListView.ItemsSourceProperty, "Items");

            Content = list;
        }
    }
}
