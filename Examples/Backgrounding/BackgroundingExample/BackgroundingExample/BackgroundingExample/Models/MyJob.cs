using System;
using Xamarin.Forms.Core;
using Xamarin.Forms;

namespace BackgroundingExample.Models
{
	public class MyJob : ICoreJob
	{
		public void PerformWork()
		{
			var notify = DependencyService.Get<ILocalNotify>();
			notify.RequestPermission((result) =>
			{
				if (result)
				{
					notify.Show(new LocalNotification()
					{
						Title = "Background Process",
						SubTitle = "Time has expired",
						Message = $"The periodic background process has fired at {DateTime.Now.ToShortTimeString()}",
					});
				}
			});
		}
	}
}
