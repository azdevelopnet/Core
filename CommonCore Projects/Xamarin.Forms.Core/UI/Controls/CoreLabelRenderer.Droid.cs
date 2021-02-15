#if __ANDROID__
using System;
using System.ComponentModel;
using System.Reflection;
using Android.Content;
using Android.Text;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Core;
using Xamarin.Forms.Platform.Android;
using Graphics = Android.Graphics;
using Java.Lang;
using Android.Text.Style;
using Android.Graphics;
using Android.Util;
using System.Collections.Generic;
using Xamarin.Forms.Internals;

[assembly: ExportRenderer(typeof(CoreLabel), typeof(CoreLabelRenderer))]
namespace Xamarin.Forms.Core
{
    public class CoreLabelRenderer : ViewRenderer<CoreLabel, TextView>
    {
        CoreLabel label;
        TextView textView;
        private Context _ctx;

        public CoreLabelRenderer(Context context) : base(context)
        {
            _ctx = context;
        }

        /// <summary>
        /// This is a work around for Google bug when Selectable text controls are in a scrollview 
        /// </summary>
        protected override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();
            textView.Enabled = false;
            textView.Enabled = true;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<CoreLabel> e)
        {
            base.OnElementChanged(e);

            label = (CoreLabel)Element;
            if (label == null)
                return;

            if (Control == null)
            {
                textView = new TextView(this.Context);
            }

            textView.Enabled = true;
            textView.Focusable = true;
            textView.LongClickable = true;
            textView.SetTextIsSelectable(true);


            SetNativeControl(textView);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == CoreLabel.TextProperty.PropertyName)
            {
                if (Control != null && Element != null && !string.IsNullOrWhiteSpace(Element.Text))
                {
                    textView.Text = Element.Text;
                }
            }
            if (e.PropertyName == CoreLabel.TextColorProperty.PropertyName)
            {
                if (Control != null && Element != null)
                {
                    textView.SetTextColor(Element.TextColor.ToAndroid());
                }
            }
            if (e.PropertyName == CoreLabel.FontAttributesProperty.PropertyName
                        || e.PropertyName == CoreLabel.FontSizeProperty.PropertyName)
            {
                if (Control != null && Element != null)
                {
                    SetTypeFace(Element.FontAttributes);
                    textView.TextSize = (float)Element.FontSize;
                }
            }

            if (e.PropertyName == "Renderer")
            {
                if (label.FormattedText == null)
                {
                    UpdateSimpleText();
                }
                else
                {
                    UpdateFormattedText();
                }
            }

        }

        private void UpdateSimpleText()
        {
            // Initial properties Set
            textView.Text = label.Text;
            textView.SetTextColor(label.TextColor.ToAndroid());
            SetTypeFace(label.FontAttributes);
            textView.TextSize = (float)label.FontSize;
        }

        private void UpdateFormattedText()
        {
            if (Element?.FormattedText == null)
                return;

            var spanList = new List<SpannableString>();
            foreach(var span in label.FormattedText.Spans)
            {
                var ss = new SpannableString(span.Text);

                ss.SetSpan(new ForegroundColorSpan(span.TextColor.ToAndroid()), 0, span.Text.Length, SpanTypes.ExclusiveExclusive);
                ss.SetSpan(new AbsoluteSizeSpan((int)span.FontSize,true), 0, span.Text.Length, SpanTypes.ExclusiveExclusive);
                if (!string.IsNullOrEmpty(span.FontFamily))
                {
                    var exportFonts = FontRegistrar.HasFont(span.FontFamily);
                    if (exportFonts.hasFont)
                    {
                        var tf = Typeface.CreateFromFile(exportFonts.fontPath);
                        ss.SetSpan(new TypefaceSpan(tf), 0, span.Text.Length, SpanTypes.ExclusiveExclusive);
                    }
                    else
                    {
                        var tf = GetTypeface();
                        ss.SetSpan(new TypefaceSpan(span.FontFamily), 0, span.Text.Length, SpanTypes.ExclusiveExclusive);
                    }
                }
                else
                {
                    var tf = GetTypeface();
                    ss.SetSpan(new TypefaceSpan(span.FontFamily), 0, span.Text.Length, SpanTypes.ExclusiveExclusive);
                }

                spanList.Add(ss);
            }

            var result = new SpannableString(TextUtils.ConcatFormatted(spanList.ToArray()));
            Control.TextFormatted = result;
        }

        private void SetTypeFace(FontAttributes attr)
        {
            switch (attr)
            {
                case FontAttributes.None:
                    textView.SetTypeface(null, Graphics.TypefaceStyle.Normal);
                    break;
                case FontAttributes.Bold:
                    textView.SetTypeface(null, Graphics.TypefaceStyle.Bold);
                    break;
                case FontAttributes.Italic:
                    textView.SetTypeface(null, Graphics.TypefaceStyle.Italic);
                    break;
                default:
                    textView.SetTypeface(null, Graphics.TypefaceStyle.Normal);
                    break;
            }
        }
        private Typeface GetTypeface()
        {
            var tf = Typeface.DefaultFromStyle(TypefaceStyle.Normal);
            switch (label.FontAttributes)
            {
                case FontAttributes.None:
                    tf = Typeface.DefaultFromStyle(TypefaceStyle.Normal);
                    break;
                case FontAttributes.Bold:
                    tf = Typeface.DefaultFromStyle(TypefaceStyle.Bold);
                    break;
                case FontAttributes.Italic:
                    tf = Typeface.DefaultFromStyle(TypefaceStyle.Italic);
                    break;
                default:
                    tf = Typeface.DefaultFromStyle(TypefaceStyle.Normal);
                    break;
            }
            return tf;
        }

    }

}
#endif
