#if __IOS__
using System;
using Xamarin.Forms.Core;
using CoreGraphics;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Foundation;
using System.ComponentModel;

[assembly: ExportEffect(typeof(ViewShadow), "ViewShadow")]
[assembly: ExportEffect(typeof(ListRemoveEmptyRows), "ListRemoveEmptyRows")]
[assembly: ExportEffect(typeof(WebViewDisableScroll), "WebViewDisableScroll")]
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

	public class WebViewDisableScroll : PlatformEffect
	{
		protected override void OnAttached()
		{
			if (Control != null)
			{
				var wv = Control as UIKit.UIWebView;
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
	public class ViewShadow : PlatformEffect
	{
		protected override void OnAttached()
		{
			CreateShadow(Container);
		}

		protected override void OnDetached()
		{
			Container.Layer.ShadowOpacity = 0;
		}

		private void CreateShadow(UIView view)
		{
			// corner radius
			view.Layer.CornerRadius = 3;

			// border
			//view.Layer.BorderWidth = 1.0f;
			//view.Layer.BorderColor = UIColor.Black.CGColor;

			// shadow
			view.Layer.ShadowColor = UIColor.Gray.CGColor;
			view.Layer.ShadowOffset = new CGSize(width: 3, height: 3);
			view.Layer.ShadowOpacity = 0.7f;
			view.Layer.ShadowRadius = 4.0f;
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
