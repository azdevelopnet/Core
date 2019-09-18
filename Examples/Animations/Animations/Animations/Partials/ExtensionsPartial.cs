using System;
namespace Xamarin.Forms.Core
{
    public static partial class CoreExtensions
    {
        public static CoreTriggerAction AssignTarget(this CoreTriggerAction action, VisualElement view)
        {
            action.Animation.Target = view;
            return action;
        }
    }
}
