namespace Xamarin.Forms.Core.Charting.Helpers
{
    internal static class EaseHelper
    {
        public static float EaseOut(float t) => t * t * t;
        public static float EaseIn(float t) => (--t) * t * t + 1;
    }
}
