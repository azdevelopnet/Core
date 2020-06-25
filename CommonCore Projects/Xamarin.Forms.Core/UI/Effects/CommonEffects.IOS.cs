#if __IOS__
using System;
using System.ComponentModel;
using CoreGraphics;
using Foundation;
using UIKit;
using WebKit;
using Xamarin.Forms;
using Xamarin.Forms.Core;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportEffect(typeof(ListRemoveEmptyRows), "ListRemoveEmptyRows")]
[assembly: ExportEffect(typeof(WKWebViewDisableScroll), "WKWebViewDisableScroll")]
[assembly: ExportEffect(typeof(HideTableSeparator), "HideTableSeparator")]
namespace Xamarin.Forms.Core
{

    public class ListRemoveEmptyRows : PlatformEffect
	{
		protected override void OnAttached()
		{
			if (Control != null)
			{
				var table = Control as UITableView;
				table.TableFooterView = new UIView();
			}
		}

		protected override void OnDetached()
		{

		}
	}

	public class WKWebViewDisableScroll : PlatformEffect
	{
		protected override void OnAttached()
		{
			if (Control != null)
			{
                var wv = Control as WKWebView;
                wv.ScrollView.ScrollEnabled = false;
				wv.ScrollView.Bounces = false;
			}
		}

		protected override void OnDetached()
		{

		}
	}

	public class HideTableSeparator : PlatformEffect
	{
		protected override void OnAttached()
		{
			if (Control != null)
			{
				var tableView = Control as UITableView;
				tableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
			}
		}

		protected override void OnDetached()
		{

		}
	}

    public class UnderlineColor : PlatformEffect
    {
        public Xamarin.Forms.Color LineColor { get; set; }

        protected override void OnAttached()
        {

        }

        protected override void OnDetached()
        {

        }
    }


}
#endif
