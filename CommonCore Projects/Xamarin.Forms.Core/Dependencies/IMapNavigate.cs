using System;
namespace Xamarin.Forms.Core
{
    public interface IMapNavigate
    {
        void NavigateWithAddress(string address);
        void NavigateLatLong(double latitude, double longtitude);
    }
}
