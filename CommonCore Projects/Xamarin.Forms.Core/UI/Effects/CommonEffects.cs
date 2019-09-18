using Xamarin.Forms;

[assembly: ResolutionGroupName("CoreEffects")]
namespace Xamarin.Forms.Core
{

    public class RemoveEmptyRowsEffect : RoutingEffect
    {
        public RemoveEmptyRowsEffect() : base($"CoreEffects.{typeof(ListRemoveEmptyRows).Name}") { }
    }

#if __IOS__

	public class DisableWebViewScrollEffect : RoutingEffect
	{
		public DisableWebViewScrollEffect() : base($"CoreEffects.{typeof(WKWebViewDisableScroll).Name}") { }
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
