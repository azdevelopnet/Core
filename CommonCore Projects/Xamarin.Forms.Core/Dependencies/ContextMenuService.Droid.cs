#if __ANDROID__
using System;
using System.Collections.Generic;
using Android.Content;
using Android.Widget;
using Plugin.CurrentActivity;
using Xamarin.Forms;
using Xamarin.Forms.Core;
using Views = Android.Views;

[assembly: Dependency(typeof(ContextMenuService))]
namespace Xamarin.Forms.Core
{
    public class ContextMenuService : IContextMenuService
    {
        private PopupMenu menu;
        private Dictionary<string, Action> menuItems;

        public Context Ctx
        {
            get => CrossCurrentActivity.Current.Activity;
        }

		public void ShowContextMenu(Xamarin.Forms.View viewRoot, Dictionary<string, Action> menuItems)
		{
            this.menuItems = menuItems;
			menu = new PopupMenu(Ctx, viewRoot.GetNativeView());
			menu.Gravity = Views.GravityFlags.Right;
			foreach (var item in menuItems)
			{
				menu.Menu.Add(new Java.Lang.String(item.Key));
			}

            menu.MenuItemClick += MenuClicked;
			menu.Show();
		}

        private void MenuClicked(object sender, PopupMenu.MenuItemClickEventArgs args)
        {
			var key = args.Item.TitleFormatted.ToString();
			if (menuItems.ContainsKey(key))
			{
                menu.MenuItemClick -= MenuClicked;
				menuItems[key].Invoke();
			}
        }
    }
}
#endif
