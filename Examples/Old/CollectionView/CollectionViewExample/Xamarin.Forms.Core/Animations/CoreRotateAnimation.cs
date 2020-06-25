using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin.Forms.CommonCore
{
    public class CoreRotateToAnimation : AnimationBase
    {
        public static readonly BindableProperty RotationProperty = 
            BindableProperty.Create(nameof(Rotation), typeof(double), typeof(CoreRotateToAnimation), default(double), 
                BindingMode.TwoWay, null);

        public double Rotation
        {
            get { return (double)GetValue(RotationProperty); }
            set { SetValue(RotationProperty, value); }
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

            return Target.RotateTo(Rotation, Convert.ToUInt32(Duration), EasingHelper.GetEasing(Easing));
        }
    }

    public class CoreRelRotateToAnimation : AnimationBase
    {
        public static readonly BindableProperty RotationProperty =
            BindableProperty.Create(nameof(Rotation), typeof(double), typeof(CoreRelRotateToAnimation), default(double),
                BindingMode.TwoWay, null);

        public double Rotation
        {
            get { return (double)GetValue(RotationProperty); }
            set { SetValue(RotationProperty, value); }
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

            return Target.RelRotateTo(Rotation, Convert.ToUInt32(Duration), EasingHelper.GetEasing(Easing));
        }
    }

    public class CoreRotateXToAnimation : AnimationBase
    {
        public static readonly BindableProperty RotationProperty =
            BindableProperty.Create(nameof(Rotation), typeof(double), typeof(CoreRotateXToAnimation), default(double),
                BindingMode.TwoWay, null);

        public double Rotation
        {
            get { return (double)GetValue(RotationProperty); }
            set { SetValue(RotationProperty, value); }
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

            return Target.RotateXTo(Rotation, Convert.ToUInt32(Duration), EasingHelper.GetEasing(Easing));
        }
    }

    public class CoreRotateYToAnimation : AnimationBase
    {
        public static readonly BindableProperty RotationProperty =
            BindableProperty.Create(nameof(Rotation), typeof(double), typeof(CoreRotateYToAnimation), default(double),
                BindingMode.TwoWay, null);

        public double Rotation
        {
            get { return (double)GetValue(RotationProperty); }
            set { SetValue(RotationProperty, value); }
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

            return Target.RotateYTo(Rotation, Convert.ToUInt32(Duration), EasingHelper.GetEasing(Easing));
        }
    }
}
