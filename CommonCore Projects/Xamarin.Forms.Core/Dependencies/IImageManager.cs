using System;
using System.Threading.Tasks;

namespace Xamarin.Forms.Core
{
    public interface IImageManager
    {
        Size GetSize(byte[] bytes);
        Task<Size> GetSize(ImageSource source);
    }
}
