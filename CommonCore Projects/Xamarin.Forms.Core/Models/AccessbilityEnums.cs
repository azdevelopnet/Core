using System;
namespace Xamarin.Forms.Core
{
    public enum FieldAccessibilityType
    {
        None,
        Button,
        Link,
        Image,
        Selected,
        PlaysSound,
        KeyboardKey,
        StaticText,
        SummaryElement,
        NotEnabled,
        UpdatesFrequently,
        SearchField,
        StartsMediaSession,
        Adjustable,
        AllowsDirectInteraction,
        CausesPageTurn,
        Header
    }
    public enum AccessbillityNotificationType
    {
        Announcement,
        LayoutChanged,
        PageScrolled,
        ScreenChanged,
    }
}
