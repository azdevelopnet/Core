﻿using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin.Forms.CommonCore
{
    public class CorePulseAnimation : AnimationBase
    {
        public override void CancelAnimation()
        {
            AnimationExtensions.AbortAnimation(Target, "Hearth");
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
                    Target.Animate("Hearth", Hearth(), 16, Convert.ToUInt32(Duration));
                });
            });
        }

        internal Animation Hearth()
        {
            var animation = new Animation();

            animation.WithConcurrent(
               (f) => Target.Scale = f,
               Target.Scale, Target.Scale,
               Xamarin.Forms.Easing.Linear, 0, 0.1);

            animation.WithConcurrent(
               (f) => Target.Scale = f,
               Target.Scale, Target.Scale * 1.1,
               Xamarin.Forms.Easing.Linear, 0.1, 0.4);

            animation.WithConcurrent(
               (f) => Target.Scale = f,
               Target.Scale * 1.1, Target.Scale,
               Xamarin.Forms.Easing.Linear, 0.4, 0.5);

            animation.WithConcurrent(
              (f) => Target.Scale = f,
              Target.Scale, Target.Scale * 1.1,
              Xamarin.Forms.Easing.Linear, 0.5, 0.8);

            animation.WithConcurrent(
               (f) => Target.Scale = f,
               Target.Scale * 1.1, Target.Scale,
               Xamarin.Forms.Easing.Linear, 0.8, 1);

            return animation;
        }
    }
}
