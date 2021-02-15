#if __IOS__
using System;
using System.ComponentModel;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Core;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CoreLabel), typeof(CoreLabelRenderer))]
namespace Xamarin.Forms.Core
{
    public class CoreLabelRenderer : ViewRenderer<CoreLabel, UITextView>
    {
        UITextView uiTextView;
        CoreLabel label;

        protected override void OnElementChanged(ElementChangedEventArgs<CoreLabel> e)
        {
            base.OnElementChanged(e);

            label = (CoreLabel)Element;

            if (label == null)
                return;

            if (Control == null)
            {
                uiTextView = new UITextView();
            }

            uiTextView.Selectable = true;
            uiTextView.Editable = false;
            uiTextView.ScrollEnabled = false;
            uiTextView.TextContainerInset = UIEdgeInsets.Zero;
            uiTextView.TextContainer.LineFragmentPadding = 0;
            uiTextView.BackgroundColor = UIColor.Clear;

            //uiTextView.TextContainerInset = new UIEdgeInsets(15, 15, 15, 15);
            //uiTextView.Layer.CornerRadius = 5;
            //uiTextView.Layer.BorderWidth = 1;
            //uiTextView.Layer.BorderColor = Xamarin.Forms.Color.LightGray.ToCGColor();
            //uiTextView.ClipsToBounds = true;

            SetNativeControl(uiTextView);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if(e.PropertyName== "Renderer")
            {
                if (label.FormattedText == null)
                {
                    SetDefaultText();
                }
                else
                {
                    SetFormattedText();
                }
            }

        }

        private void SetDefaultText()
        {
            uiTextView.Text = label.Text;
            uiTextView.TextColor = label.TextColor.ToUIColor();
            if (!string.IsNullOrEmpty(label.FontFamily))
            {
                uiTextView.Font = UIFont.FromName(label.FontFamily, (float)label.FontSize);
            }
            else
            {
                switch (label.FontAttributes)
                {
                    case FontAttributes.None:
                        uiTextView.Font = UIFont.SystemFontOfSize(new nfloat(label.FontSize));
                        break;
                    case FontAttributes.Bold:
                        uiTextView.Font = UIFont.BoldSystemFontOfSize(new nfloat(label.FontSize));
                        break;
                    case FontAttributes.Italic:
                        uiTextView.Font = UIFont.ItalicSystemFontOfSize(new nfloat(label.FontSize));
                        break;
                    default:
                        uiTextView.Font = UIFont.BoldSystemFontOfSize(new nfloat(label.FontSize));
                        break;
                }
            }
        }

        private void SetFormattedText()
        {
            if (Control != null)
            {
                var text = new NSMutableAttributedString(string.Empty);
                foreach (var span in Element.FormattedText.Spans)
                {
                    var range = new NSRange(0, span.Text.Length);
                    var subText = new NSMutableAttributedString(span.Text, foregroundColor: span.TextColor.ToUIColor());

                    var font = UIFont.SystemFontOfSize(new nfloat(span.FontSize));
                    if (string.IsNullOrEmpty(span.FontFamily))
                    {
                        switch (label.FontAttributes)
                        {
                            case FontAttributes.None:
                                font = UIFont.SystemFontOfSize(new nfloat(span.FontSize));
                                break;
                            case FontAttributes.Bold:
                                font = UIFont.BoldSystemFontOfSize(new nfloat(span.FontSize));
                                break;
                            case FontAttributes.Italic:
                                font = UIFont.ItalicSystemFontOfSize(new nfloat(span.FontSize));
                                break;
                            default:
                                font = UIFont.BoldSystemFontOfSize(new nfloat(span.FontSize));
                                break;
                        }
                    }
                    else
                    {
                        var exportFonts = FontRegistrar.HasFont(span.FontFamily);
                        if (exportFonts.hasFont)
                        {
                            var pathName = exportFonts.fontPath;
                            pathName = pathName.Replace(".ttf", string.Empty).Replace(".otf", string.Empty);
                            font = UIFont.FromName(pathName, new nfloat(span.FontSize));
                        }
                        else
                        {
                            var fn = GetFontName(span.FontFamily, span.FontAttributes);
                            font = UIFont.FromName(fn, new nfloat(span.FontSize));
                        }

                    }
                    subText.AddAttribute(UIStringAttributeKey.Font, font, range);

                    if (span.BackgroundColor != Color.Default)
                    {
                        subText.AddAttribute(UIStringAttributeKey.BackgroundColor, span.BackgroundColor.ToUIColor(), range);
                    }

                    text.Append(subText);
                }

                Control.AttributedText = text;
            }
        }

        private static string GetFontName(string fontFamily, FontAttributes fontAttributes)
        {
            var postfix = "";
            var bold = fontAttributes.HasFlag(FontAttributes.Bold);
            var italic = fontAttributes.HasFlag(FontAttributes.Italic);
            if (bold && italic) { postfix = "-BoldItalic"; }
            else if (bold) { postfix = "-Bold"; }
            else if (italic) { postfix = "-Italic"; }

            return fontFamily + postfix;
        }
    }
}
#endif
