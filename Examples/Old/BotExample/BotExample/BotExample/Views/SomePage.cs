using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Core;

namespace BotExample
{
    public class SomePage : CorePage<SomeViewModel>
    {
        private WebView browser;
        public SomePage()
        {
            this.Title = "Bot Framework";

			Content = new WebView()
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Source = new HtmlWebViewSource()
				{
					Html = DependencyService.Get<IBaseUrl>().ReadContent("bot.html"),
					BaseUrl = DependencyService.Get<IBaseUrl>().Get()
				}
			}.Assign(out browser);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            Task.Run(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await VM.DirectLineInitialized();
                    await Task.Delay(3000);
                    await browser.EvaluateJavaScriptAsync($"init('{VM.DirectLineToken}', '{VM.UserName}')");

                });
            });

        }
    }
}