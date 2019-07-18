using System;
using Common.Core;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ResolutionGroupName("CommonEffects")]
[assembly: ExportEffect(typeof(ListRemoveEmptyRows), "ListRemoveEmptyRows")]
namespace Common.Core
{
    public class ListRemoveEmptyRows:PlatformEffect
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
}
