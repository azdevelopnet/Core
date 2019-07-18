#if __IOS__
using CoreGraphics;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms.Core.MaterialDesign;
using UIKit;
using CoreAnimation;
using System;

[assembly: ExportRenderer(typeof(CoreFloatingTextEntry), typeof(CoreFloatingTextEntryRenderer))]
namespace Xamarin.Forms.Core.MaterialDesign
{
	public class FloatTextEntry : UIView
	{
		public FloatingTextEntryValidator Validator { get; set; }
		public bool IsFloated { get; set; }
		private UILabel floatingLabel;
		private UILabel errorLabel;
		private UITextField entry;
		private CALayer underline;

		public IElementController ControllerReference { get; set; }

		public string Text
		{
			get
			{
				return entry.Text;
			}
			set
			{
				entry.Text = value;
			}
		}

		public UIColor TextColor
		{
			get
			{
				return entry.TextColor;
			}
			set
			{
				entry.TextColor = value;
			}
		}

		public bool IsPassword
		{
			get
			{
				return entry.SecureTextEntry;
			}
			set
			{
				entry.SecureTextEntry = value;
			}
		}

		public UIColor FloatingTextColor
		{
			get
			{
				return floatingLabel.TextColor;
			}
			set
			{
				floatingLabel.TextColor = value;
			}
		}
		public string FloatingText
		{
			get
			{
				return floatingLabel.Text;
			}
			set
			{
				floatingLabel.Text = value;
			}
		}

		public CGColor UnderlineColor
		{
			get
			{
				return underline.BackgroundColor;
			}
			set
			{
				underline.BackgroundColor = value;
			}
		}

		public string ErrorText
		{
			get
			{
				return errorLabel.Text;
			}
			set
			{
				errorLabel.Text = value;
			}
		}

		public bool HasError
		{
			get
			{
				return errorLabel.Hidden;
			}
			set
			{
				errorLabel.Hidden = value;
			}
		}

		public UIColor ErrorTextColor
		{
			get
			{
				return errorLabel.TextColor;
			}
			set
			{
				errorLabel.TextColor = value;
			}
		}

		public FloatTextEntry()
		{
			floatingLabel = new UILabel(new CGRect(0, 0, 290, 32));
			floatingLabel.Text = "User Name";
			floatingLabel.TextColor = UIColor.Gray;
			floatingLabel.Hidden = true;

			entry = new UITextField(new CGRect(0, 0, 320, 32));
			entry.EditingDidEnd += EntryLostFocus;
			entry.EditingChanged += EntryChangedValue;
			entry.TouchDown += EntryTouched;
			entry.Hidden = true;

			underline = new CALayer();
			underline.Frame = new CGRect(0, entry.Frame.Height - 1, entry.Frame.Width, 1);
			underline.BackgroundColor = UIColor.Gray.CGColor;
			entry.Layer.AddSublayer(underline);
			underline.Hidden = true;

			errorLabel = new UILabel(new CGRect(2, underline.Frame.Y - 5, 290, 32));
			errorLabel.Text = "ErrorMessage";
			errorLabel.Hidden = true;
			errorLabel.Font = UIFont.FromName(floatingLabel.Font.Name, floatingLabel.Font.PointSize - 5);
			errorLabel.TextColor = UIColor.Red;

			this.Add(entry);
			this.Add(floatingLabel);
			this.Add(errorLabel);

		}

		protected override void Dispose(bool disposing)
		{
			entry.EditingChanged -= EntryChangedValue;
			entry.TouchDown -= EntryTouched;
			entry.EditingDidEnd -= EntryLostFocus;
			base.Dispose(disposing);
		}

		private void EntryChangedValue(object sender, EventArgs args)
		{
			if (Validator != null)
			{
				var isValid = Validator(Text);
				this.HasError = isValid;
			}
			ControllerReference?.SetValueFromRenderer(CoreFloatingTextEntry.TextProperty, Text);

		}
		private void EntryLostFocus(object sender, EventArgs args)
		{
			if (entry.Text == string.Empty)
			{
				UIView.Animate(
					duration: 0.2,
					delay: 0,
					options: UIViewAnimationOptions.CurveLinear,
					animation: () =>
					{
						nfloat newY = floatingLabel.Center.Y + 25;
						floatingLabel.Center = new CGPoint(floatingLabel.Center.X + 0.5, newY);
						floatingLabel.Font = UIFont.FromName(floatingLabel.Font.Name, floatingLabel.Font.PointSize + 5);
						IsFloated = false;
					},
					completion: () => { });

			}
		}
		private void EntryTouched(object sender, EventArgs args)
		{
			if (!IsFloated)
			{
				UIView.Animate(
					duration: 0.2,
					delay: 0,
					options: UIViewAnimationOptions.CurveLinear,
					animation: () =>
					{
						nfloat newY = floatingLabel.Center.Y - 25;
						floatingLabel.Center = new CGPoint(floatingLabel.Center.X - 0.5, newY);
						floatingLabel.Font = UIFont.FromName(floatingLabel.Font.Name, floatingLabel.Font.PointSize - 5);
						IsFloated = true;
					},
					completion: () => { });
			}
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			var height = Bounds.Height > 0 ? Bounds.Height : 32;
			floatingLabel.Frame = new CGRect(Bounds.X + 2, Bounds.Y, Bounds.Width - 20, height);
			entry.Frame = new CGRect(Bounds.X, Bounds.Y, Bounds.Width, height);
			underline.Frame = new CGRect(Bounds.X, entry.Frame.Height - 1, entry.Frame.Width, 1);
			errorLabel.Frame = new CGRect(Bounds.X + 2, underline.Frame.Y - 5, Bounds.Width, height);
			floatingLabel.Hidden = false;
			entry.Hidden = false;
			underline.Hidden = false;
		}

	}
	public class CoreFloatingTextEntryRenderer : ViewRenderer<CoreFloatingTextEntry, FloatTextEntry>
	{
		private FloatTextEntry fte;

		protected override void OnElementChanged(ElementChangedEventArgs<CoreFloatingTextEntry> e)
		{
			base.OnElementChanged(e);

			if (this.Control == null)
			{
				fte = new FloatTextEntry()
				{
					ErrorText = Element.ErrorText,
					ErrorTextColor = Element.ErrorColor.ToUIColor(),
					FloatingText = Element.Placeholder,
					Frame = new CGRect(0, 0, 320, 32),
					IsPassword = Element.IsPassword,
					Validator = Element.Validator,
					ControllerReference = (base.Element as IElementController)
				};
				this.SetNativeControl(fte);
			}

		}

		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);


			if (e.PropertyName == "Width" || e.PropertyName == "Height")
			{
				if (Element != null && fte != null)
				{
					if (Element.Width != -1 && Element.Height != -1)
					{
						if (Element.Width != fte.Frame.Width)
							fte.Frame = new CGRect(Element.X, Element.Y, Element.Width, Element.Height);
					}
				}
			}
			if (e.PropertyName == CoreFloatingTextEntry.ValidatorProperty.PropertyName)
			{
				fte.Validator = Element.Validator;
			}
		}

	}
}
#endif

