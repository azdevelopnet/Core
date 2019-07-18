using System;
using Xamarin.Forms;
using System.Linq;

[assembly: ResolutionGroupName("CoreEffects")]
namespace Xamarin.Forms.Core
{
    
#if __IOS__

    public class RemoveEmptyRowsEffect : RoutingEffect
	{
        public RemoveEmptyRowsEffect() : base($"CoreEffects.{typeof(ListRemoveEmptyRows).Name}") { }
	}
	public class DisableWebViewScrollEffect : RoutingEffect
	{
		public DisableWebViewScrollEffect() : base($"CoreEffects.{typeof(WebViewDisableScroll).Name}") { }
	}

#endif

    public class HideListSeparatorEffect : RoutingEffect
	{
		public HideListSeparatorEffect() : base($"CoreEffects.{typeof(HideTableSeparator).Name}") { }
	}
	public class ViewShadowEffect : RoutingEffect
	{
		public ViewShadowEffect() : base($"CoreEffects.{typeof(ViewShadow).Name}") { }
	}
    public class UnderlineColorEffect : RoutingEffect
    {
        public UnderlineColorEffect() : base($"CoreEffects.{typeof(UnderlineColor).Name}") { }
    }


}
