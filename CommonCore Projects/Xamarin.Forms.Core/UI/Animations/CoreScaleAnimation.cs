using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin.Forms.Core
{
    public class CoreScaleToAnimation : AnimationBase
    {
        public static readonly BindableProperty ScaleProperty =
            BindableProperty.Create(nameof(Scale), typeof(double), typeof(CoreScaleToAnimation), default(double),  
                BindingMode.TwoWay, null);

        public double Scale
        {
            get { return (double)GetValue(ScaleProperty); }
            set { SetValue(ScaleProperty, value); }
        }

        public override void CancelAnimation()
        {
            ViewExtensions.CancelAnimations(Target);
        }

        protected override Task BeginAnimation()
        {
            if (Target == null)
            {
                throw new NullReferenceException("Null Target property.");
            }

            return Target.ScaleTo(Scale, Convert.ToUInt32(Duration), EasingHelper.GetEasing(Easing));
        }
    }

    public class CoreRelScaleToAnimation : AnimationBase
    {
        public static readonly BindableProperty ScaleProperty = 
            BindableProperty.Create(nameof(Scale), typeof(double), typeof(CoreRelScaleToAnimation), default(double),      
                BindingMode.TwoWay, null);

        public double Scale
        {
            get { return (double)GetValue(ScaleProperty); }
            set { SetValue(ScaleProperty, value); }
        }

        public override void CancelAnimation()
        {
            ViewExtensions.CancelAnimations(Target);
        }

        protected override Task BeginAnimation()
        {
            if (Target == null)
            {
                throw new NullReferenceException("Null Target property.");
            }

            return Target.RelScaleTo(Scale, Convert.ToUInt32(Duration), EasingHelper.GetEasing(Easing));
        }
    }
}