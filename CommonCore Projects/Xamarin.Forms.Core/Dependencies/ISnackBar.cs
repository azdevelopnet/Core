using System;
namespace Xamarin.Forms.Core
{
    public enum SnackOrientation
    {
        Top,
        Bottom
    }
    public class Snack
    {
        public string Icon { get; set; }
        public string Text { get; set; }
        public Color TextColor { get; set; } = Color.White;
        public Color Background { get; set; } = Color.Black;

        public Action<object> Action { get; set; }
        public string ActionText { get; set; }
        public Color ActionTextColor { get; set; } = Color.White;

        /// <summary>
        /// Display milliseconds -1 being indefinite
        /// </summary>
        /// <value>The duration.</value>
        public int Duration { get; set; } = 3000;

        public SnackOrientation Orientation { get; set; } = SnackOrientation.Bottom;
    }
    public interface ISnackBar
    {
        void Show(Snack snack);
        void Close();
    }
}
