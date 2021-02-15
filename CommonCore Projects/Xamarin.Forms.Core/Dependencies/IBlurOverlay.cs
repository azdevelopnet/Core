using System;
using System.Threading.Tasks;

namespace Xamarin.Forms.Core
{
	public interface IBlurOverlay
	{
        Task BlurAsync();
        void Unblur();
    }
}
