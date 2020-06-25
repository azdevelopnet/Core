using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin.Forms.Core
{
    public interface IImageResource
    {
        Task<Size> GetSize(ImageSource source);
    }
}
