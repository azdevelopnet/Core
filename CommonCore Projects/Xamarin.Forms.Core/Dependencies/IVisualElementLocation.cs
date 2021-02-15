using System;
namespace Xamarin.Forms.Core
{
    public interface IVisualElementLocation
    {
        System.Drawing.PointF GetCoordinates(global::Xamarin.Forms.VisualElement view);
    }
}
