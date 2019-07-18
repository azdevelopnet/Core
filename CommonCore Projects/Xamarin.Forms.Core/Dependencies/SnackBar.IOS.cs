#if __IOS__
using System;
using TTGSnackBar;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(Xamarin.Forms.Core.SnackBar))]
namespace Xamarin.Forms.Core
{
    public class SnackBar : ISnackBar
    {
        public static TTGSnackbar Bar { get; set; }
        public void Show(Snack snack)
        {
            var paddedText = $"   {snack.Text}";
            SnackBar.Bar = new TTGSnackbar(paddedText);
            SnackBar.Bar.MessageTextColor = snack.TextColor.ToUIColor();

            SnackBar.Bar.BackgroundColor = snack.Background.ToUIColor();

            if(snack.Duration==-1)
            {
                SnackBar.Bar.Duration = TimeSpan.FromMinutes(5);//mimic indefinite
			}
            else{
                SnackBar.Bar.Duration = TimeSpan.FromMilliseconds(snack.Duration);
            }

			SnackBar.Bar.AnimationType = TTGSnackbarAnimationType.FadeInFadeOut;
            SnackBar.Bar.LocationType = snack.Orientation == SnackOrientation.Top ? TTGSnackbarLocation.Top : TTGSnackbarLocation.Bottom;

            if (!string.IsNullOrEmpty(snack.Icon)){
				SnackBar.Bar.Icon = UIImage.FromBundle(snack.Icon);
            }

			if (!string.IsNullOrEmpty(snack.ActionText))
			{
				SnackBar.Bar.SecondActionText = snack.ActionText;
                SnackBar.Bar.SecondActionTextColor = snack.ActionTextColor.ToUIColor();
				SnackBar.Bar.SecondActionBlock = snack.Action;
			}

            SnackBar.Bar.Show();

        }

        public void Close()
        {
			if (SnackBar.Bar != null)
			{
				SnackBar.Bar.Dismiss();
			}
        }

    }
}
#endif
