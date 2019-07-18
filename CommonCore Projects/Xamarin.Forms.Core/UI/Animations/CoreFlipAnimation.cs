using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin.Forms.Core
{
    public class CoreFlipAnimation : AnimationBase
    {
        public override void CancelAnimation()
        {
            AnimationExtensions.AbortAnimation(Target, "Flip");
        }

        public enum FlipDirection
        {
            Left,
            Right
        }

        public static readonly BindableProperty DirectionProperty = 
            BindableProperty.Create(nameof(Direction), typeof(FlipDirection), typeof(CoreFlipAnimation), FlipDirection.Right, 
                BindingMode.TwoWay, null);

        public FlipDirection Direction
        {
            get { return (FlipDirection)GetValue(DirectionProperty); }
            set { SetValue(DirectionProperty, value); }
        }

        protected override Task BeginAnimation()
        {
            if (Target == null)
            {
                throw new NullReferenceException("Null Target property.");
            }

            return Task.Run(() =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Target.Animate("Flip", Flip(), 16, Convert.ToUInt32(Duration));
                });
            });
        }

        internal Animation Flip()
        {
            var animation = new Animation();

            animation.WithConcurrent((f) => Target.Opacity = f, 0.5, 1);
            animation.WithConcurrent((f) => Target.RotationY = f, (Direction == FlipDirection.Left) ? 90 : -90, 0, Xamarin.Forms.Easing.Linear);

            return animation;
        }
    }
}
