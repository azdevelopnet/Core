#if __IOS__
using System;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Core.MaterialDesign;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CoreFloatingActionButton), typeof(CoreFloatingActionButtonRenderer))]
namespace Xamarin.Forms.Core.MaterialDesign
{

	public partial class CoreFloatingActionButtonRenderer : ViewRenderer<CoreFloatingActionButton, MNFloatingActionButton>
	{
		protected override void OnElementChanged(ElementChangedEventArgs<CoreFloatingActionButton> e)
		{
			base.OnElementChanged(e);

			if (this.Control == null)
			{
				var fab = new MNFloatingActionButton();
				fab.Frame = new CoreGraphics.CGRect(0, 0, 24, 24);

				this.SetNativeControl(fab);

				this.UpdateStyles();
			}

			if (e.NewElement != null)
			{
				this.Control.TouchUpInside += this.Fab_TouchUpInside;
			}

			if (e.OldElement != null)
			{
				this.Control.TouchUpInside -= this.Fab_TouchUpInside;
			}
		}

		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == CoreFloatingActionButton.SizeProperty.PropertyName)
			{
				this.SetSize();
			}
			else if (e.PropertyName == CoreFloatingActionButton.ColorNormalProperty.PropertyName ||
					 e.PropertyName == CoreFloatingActionButton.ColorPressedProperty.PropertyName)
			{
				this.SetBackgroundColors();
			}
			else if (e.PropertyName == CoreFloatingActionButton.HasShadowProperty.PropertyName)
			{
				this.SetHasShadow();
			}
			else if (e.PropertyName == CoreFloatingActionButton.SizeProperty.PropertyName ||
					 e.PropertyName == CoreFloatingActionButton.WidthProperty.PropertyName ||
					 e.PropertyName == CoreFloatingActionButton.HeightProperty.PropertyName)
			{
				this.SetImage();
			}
			else if (e.PropertyName == CoreFloatingActionButton.IsEnabledProperty.PropertyName)
			{
				this.UpdateEnabled();
			}
			else
			{
				base.OnElementPropertyChanged(sender, e);
			}
		}

		public override SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
		{
			var viewSize = this.Element.Size == FABControlSize.Normal ? 56 : 40;

			return new SizeRequest(new Size(viewSize, viewSize));
		}

		private void UpdateStyles()
		{
			this.SetSize();

			this.SetBackgroundColors();

			this.SetHasShadow();

			this.SetImage();

			this.UpdateEnabled();
		}

		private void SetSize()
		{

			switch (this.Element.Size)
			{
				case FABControlSize.Mini:
					this.Control.Size = FABControlSize.Mini;
					break;
				case FABControlSize.Normal:
					this.Control.Size = FABControlSize.Normal;
					break;
			}
		}

		private void SetBackgroundColors()
		{
			this.Control.BackgroundColor = this.Element.ColorNormal.ToUIColor();
			this.Control.PressedBackgroundColor = this.Element.ColorPressed.ToUIColor();
		}

		private void SetHasShadow()
		{
			this.Control.HasShadow = this.Element.HasShadow;
		}

		private void SetImage()
		{
			var source = ImageSource.FromFile(this.Element.ImageName);
			SetImageAsync(source, this.Control);
		}

		private void UpdateEnabled()
		{
			this.Control.Enabled = this.Element.IsEnabled;

			if (this.Control.Enabled == false)
			{
				this.Control.BackgroundColor = this.Element.ColorPressed.ToUIColor();
				this.Control.PressedBackgroundColor = this.Element.ColorPressed.ToUIColor();
			}
			else
			{
				this.SetBackgroundColors();
			}
		}

		private void Fab_TouchUpInside(object sender, EventArgs e)
		{
			this.Element?.Command?.Execute(null);
			this.Element?.Clicked?.Invoke(sender, e);
		}

		private async static void SetImageAsync(ImageSource source, MNFloatingActionButton targetButton)
		{
			var widthRequest = targetButton.Frame.Width;
			var heightRequest = targetButton.Frame.Height;

			var handler = source.GetHandler();
			using (UIImage image = await handler.LoadImageAsync(source))
			{
				UIGraphics.BeginImageContext(new CoreGraphics.CGSize(widthRequest, heightRequest));
				image.Draw(new CoreGraphics.CGRect(0, 0, widthRequest, heightRequest));
				using (var resultImage = UIGraphics.GetImageFromCurrentImageContext())
				{
					if (resultImage != null)
					{
						UIGraphics.EndImageContext();
						using (var resizableImage = resultImage.CreateResizableImage(new UIEdgeInsets(0f, 0f, widthRequest, heightRequest)))
						{
							targetButton.CenterImageView.Image = resizableImage.ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal);
						}
					}
				}
			}
		}
	}

	public class MNFloatingActionButton : UIControl
	{
		public enum ShadowState
		{
			ShadowStateShown,
			ShadowStateHidden
		}

		private readonly nfloat animationDuration;
		private readonly nfloat animationScale;
		private readonly nfloat shadowOpacity;
		private readonly nfloat shadowRadius;


		private FABControlSize size = FABControlSize.Normal;

		public FABControlSize Size
		{
			get { return size; }
			set
			{
				if (size == value)
					return;

				size = value;
				this.UpdateBackground();
			}
		}

		UIImageView _centerImageView;

		public UIImageView CenterImageView
		{
			get
			{
				if (_centerImageView == null)
				{
					_centerImageView = new UIImageView();
				}

				return _centerImageView;
			}
			private set
			{
				_centerImageView = value;
			}
		}

		UIColor _backgroundColor;

		public new UIColor BackgroundColor
		{
			get
			{
				return _backgroundColor;
			}
			set
			{
				_backgroundColor = value;

				this.UpdateBackground();
			}
		}

		UIColor _pressedBackgroundColor;

		public UIColor PressedBackgroundColor
		{
			get { return _pressedBackgroundColor; }
			set
			{
				_pressedBackgroundColor = value;
				this.UpdateBackground();
			}
		}

		UIColor _shadowColor;

		public UIColor ShadowColor
		{
			get { return _shadowColor; }
			set
			{
				_shadowColor = value;
				this.UpdateBackground();
			}
		}

		bool _hasShadow;

		public bool HasShadow
		{
			get { return _hasShadow; }
			set
			{
				_hasShadow = value;
				this.UpdateBackground();
			}
		}

		public nfloat ShadowOpacity { get; private set; }

		public nfloat ShadowRadius { get; private set; }

		public nfloat AnimationScale { get; private set; }

		public nfloat AnimationDuration { get; private set; }

		public bool IsAnimating { get; private set; }

		public UIView BackgroundCircle { get; private set; }

		public MNFloatingActionButton()
			: base()
		{
			this.animationDuration = 0.05f;
			this.animationScale = 0.85f;
			this.shadowOpacity = 0.6f;
			this.shadowRadius = 1.5f;

			this.CommonInit();
		}

		public MNFloatingActionButton(CGRect frame)
			: base(frame)
		{
			this.animationDuration = 0.05f;
			this.animationScale = 0.85f;
			this.shadowOpacity = 0.6f;
			this.shadowRadius = 1.5f;

			this.CommonInit();
		}

		void CommonInit()
		{
			this.BackgroundCircle = new UIView();

			this.BackgroundColor = UIColor.Red.ColorWithAlpha(0.4f);
			this.BackgroundColor = new UIColor(33.0f / 255.0f, 150.0f / 255.0f, 243.0f / 255.0f, 1.0f);
			this.BackgroundCircle.BackgroundColor = this.BackgroundColor;
			this.ShadowOpacity = shadowOpacity;
			this.ShadowRadius = shadowRadius;
			this.AnimationScale = animationScale;
			this.AnimationDuration = animationDuration;

			this.BackgroundCircle.AddSubview(this.CenterImageView);
			this.AddSubview(this.BackgroundCircle);

		}

		public override void TouchesBegan(NSSet touches, UIEvent evt)
		{
			base.TouchesBegan(touches, evt);

			this.AnimateToSelectedState();
			this.SendActionForControlEvents(UIControlEvent.TouchDown);
		}

		public override void TouchesEnded(NSSet touches, UIEvent evt)
		{
			this.AnimateToDeselectedState();
			this.SendActionForControlEvents(UIControlEvent.TouchUpInside);
		}

		public override void TouchesCancelled(NSSet touches, UIEvent evt)
		{
			this.AnimateToDeselectedState();
			this.SendActionForControlEvents(UIControlEvent.TouchCancel);
		}

		public void AnimateToSelectedState()
		{
			this.IsAnimating = true;
			this.ToggleShadowAnimationToState(ShadowState.ShadowStateHidden);
			UIView.Animate(animationDuration, () =>
				{
					this.BackgroundCircle.Transform = CGAffineTransform.MakeScale(this.AnimationScale, this.AnimationScale);
					this.BackgroundCircle.BackgroundColor = this.PressedBackgroundColor;
				}, () =>
				{
					this.IsAnimating = false;
				});
		}

		public void AnimateToDeselectedState()
		{
			this.IsAnimating = true;
			this.ToggleShadowAnimationToState(ShadowState.ShadowStateShown);
			UIView.Animate(animationDuration, () =>
				{
					this.BackgroundCircle.Transform = CGAffineTransform.MakeScale(1.0f, 1.0f);
					this.BackgroundCircle.BackgroundColor = this.BackgroundColor;
				}, () =>
				{
					this.IsAnimating = false;
				});
		}

		public void ToggleShadowAnimationToState(ShadowState state)
		{
			nfloat endOpacity = 0.0f;
			if (state == ShadowState.ShadowStateShown)
			{
				endOpacity = this.ShadowOpacity;
			}

			CABasicAnimation animation = CABasicAnimation.FromKeyPath("shadowOpacity");
			animation.From = NSNumber.FromFloat((float)this.ShadowOpacity);
			animation.To = NSNumber.FromFloat((float)endOpacity);
			animation.Duration = animationDuration;
			this.BackgroundCircle.Layer.AddAnimation(animation, "shadowOpacity");
			this.BackgroundCircle.Layer.ShadowOpacity = (float)endOpacity;
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			this.CenterImageView.Center = this.BackgroundCircle.Center;
			if (!this.IsAnimating)
			{
				this.UpdateBackground();
			}
		}

		private void UpdateBackground()
		{
			this.BackgroundCircle.Frame = this.Bounds;
			this.BackgroundCircle.Layer.CornerRadius = this.Bounds.Size.Height / 2;
			this.BackgroundCircle.Layer.ShadowColor = this.ShadowColor != null ? this.ShadowColor.CGColor : this.BackgroundColor.CGColor;
			this.BackgroundCircle.Layer.ShadowOpacity = (float)this.ShadowOpacity;
			this.BackgroundCircle.Layer.ShadowRadius = this.ShadowRadius;
			this.BackgroundCircle.Layer.ShadowOffset = new CGSize(1.0, 1.0);
			this.BackgroundCircle.BackgroundColor = this.BackgroundColor;

			var xPos = (this.BackgroundCircle.Bounds.Width / 2) - 12;
			var yPos = (this.BackgroundCircle.Bounds.Height / 2) - 12;

			this.CenterImageView.Frame = new CGRect(xPos, yPos, 24, 24);
		}
	}

	internal static class AccessExtensions
	{
		internal static object Call(this object o, string methodName, params object[] args)
		{
			var mi = o.GetType().GetMethod(methodName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
			if (mi != null)
			{
				return mi.Invoke(o, args);
			}
			return null;
		}
		internal static IImageSourceHandler GetHandler(this ImageSource source)
		{
			IImageSourceHandler returnValue = null;
			if (source is UriImageSource)
			{
				returnValue = new ImageLoaderSourceHandler();
			}
			else if (source is FileImageSource)
			{
				returnValue = new FileImageSourceHandler();
			}
			else if (source is StreamImageSource)
			{
				returnValue = new StreamImagesourceHandler();
			}
			return returnValue;
		}
	}
}
#endif

