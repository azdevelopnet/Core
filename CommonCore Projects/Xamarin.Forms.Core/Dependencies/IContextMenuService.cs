using System;
using System.Collections.Generic;

namespace Xamarin.Forms.Core
{
    public interface IContextMenuService
    {
        void ShowContextMenu(Xamarin.Forms.View viewRoot, Dictionary<string, Action> menuItems);
    }
}
