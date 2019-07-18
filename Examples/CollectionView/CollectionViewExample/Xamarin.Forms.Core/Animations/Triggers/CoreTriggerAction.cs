using Xamarin.Forms;

namespace Xamarin.Forms.CommonCore
{
    public class CoreTriggerAction : TriggerAction<VisualElement>
    {
        public AnimationBase Animation { get; set; }

        protected override async void Invoke(VisualElement sender)
        {
            if (Animation == null)
                return;

            await Animation.Begin();
        }
    }
}