using System;
using Xamarin.Forms;

namespace Xamarin.Forms.Core
{
    public class PhoneMaskBehavior : Behavior<Entry>
    {

        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += OnEntryTextChanged;
            base.OnAttachedTo(entry);
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= OnEntryTextChanged;
            base.OnDetachingFrom(entry);
        }

        void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            var ctrl = (Entry)sender;

            var txt = args.NewTextValue.Replace("(", string.Empty).Replace(")", string.Empty).Replace("-", string.Empty).Replace(" ", string.Empty);
            txt = txt.Length > 10 ? txt.Substring(0, 10) : txt;
            long result;
            bool isValid = long.TryParse(txt, out result);
            if (isValid)
            {
                ctrl.TextColor = Color.Default;
                if (txt.Length <= 3)
                {
                    ctrl.Text = result.ToString("(###");
                }
                else if (txt.Length > 3 && txt.Length <= 6)
                {
                    var areaCode = txt.Substring(0, 3);
                    var prefix = txt.Substring(3);
                    ctrl.Text = int.Parse(areaCode).ToString("(###") + ") " + int.Parse(prefix).ToString("###");
                }
                else {
                    var areaCode = txt.Substring(0, 3);
                    var prefix = txt.Substring(3, 3);
                    var suffix = txt.Substring(6);
                    ctrl.Text = int.Parse(areaCode).ToString("(###)") + " " + int.Parse(prefix).ToString("###") + "-" + int.Parse(suffix).ToString("####");
                }

            }
            else {
                ctrl.TextColor = Color.Red;
            }

        }
    }
}

