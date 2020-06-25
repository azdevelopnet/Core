using System;
#if __IOS__
using System.Threading.Tasks;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Core;
using Xamarin.Forms.Platform.iOS;

[assembly: Dependency(typeof(ImageResource))]
namespace Xamarin.Forms.Core
{
    public class ImageResource : IImageResource
    {
        public async Task<Size> GetSize(ImageSource source)
        {
            UIImage image = await source.ToUIImage();
            return new Size((double)image.Size.Width, (double)image.Size.Height);
        }

    }
}
#endif
