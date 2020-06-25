﻿using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin.Forms.CommonCore
{
    public static class AnimationExtension
    {
        public static async Task<bool> Animate(this VisualElement visualElement, AnimationBase animation)
        {
            try
            {
                animation.Target = visualElement;

                await animation.Begin();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}