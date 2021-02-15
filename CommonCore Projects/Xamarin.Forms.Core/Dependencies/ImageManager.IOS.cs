#if __IOS__
using System;
using System.Threading.Tasks;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Core;

[assembly: Dependency(typeof(ImageManager))]
namespace Xamarin.Forms.Core
{
    public class ImageManager: IImageManager
    {
        public Size GetSize(byte[] bytes)
        {
            var data = NSData.FromArray(bytes);
            var originalImage = new UIImage(data);
            return new Size(originalImage.Size.Width, originalImage.Size.Height);
        }

        public async Task<Size> GetSize(ImageSource source)
        {
            var image = await source.ToUIImage();
            return new Size((double)image.Size.Width, (double)image.Size.Height);
        }
    }
}
#endif
