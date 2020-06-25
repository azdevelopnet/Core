using System;
namespace Xamarin.Forms.Core
{
	public interface ITextMeter
	{
		double MeasureTextSize(string text, double width, double fontSize, string fontName = null);
	}
}
