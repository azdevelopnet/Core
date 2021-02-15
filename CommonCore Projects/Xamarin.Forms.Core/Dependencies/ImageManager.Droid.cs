#if __ANDROID__
using System;
using System.Threading.Tasks;
using Android.Graphics;
using Xamarin.Forms;
using Xamarin.Forms.Core;

[assembly: Xamarin.Forms.Dependency(typeof(MapNavigate))]
namespace Xamarin.Forms.Core
{ 
    public class ImageManager: IImageManager
    {
        public Size GetSize(byte[] bytes)
        {
            var originalImage = BitmapFactory.DecodeByteArray(bytes, 0, bytes.Length);
            return new Size(originalImage.Width, originalImage.Height);
        }

        public async Task<Size> GetSize(ImageSource source)
        {
            var image = await source.ToBitmap();
            return new Size((double)image.Width, (double)image.Height);
        }
    }
}
#endif
