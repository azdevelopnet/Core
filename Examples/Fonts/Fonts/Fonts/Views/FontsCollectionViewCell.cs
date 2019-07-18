using System;
using Xamarin.Forms;
using Xamarin.Forms.Core;

namespace Fonts.Views
{
    public class FontsCollectionViewCell : ViewCell
    {
        private FontView col1;
        private FontView col2;
        private FontView col3;

        public FontsCollectionViewCell()
        {
            Height = 75;
            var gd = new GridContainer(true);
            gd.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            gd.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            gd.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

            col1 = new FontView() { VerticalOptions = LayoutOptions.Center };
            col2 = new FontView() { VerticalOptions = LayoutOptions.Center };
            col3 = new FontView() { VerticalOptions = LayoutOptions.Center };

            gd.AddChild(col1, 0, 0);
            gd.AddChild(col2, 0, 1);
            gd.AddChild(col3, 0, 2);

            View = gd;
        }

        protected override void OnBindingContextChanged()
        {
            var binding = (FontItemRow)this.BindingContext;
            col1.BindingContext = binding.Item1;
            col2.BindingContext = binding.Item2;
            col3.BindingContext = binding.Item3;
            base.OnBindingContextChanged();
        }
    }
}
