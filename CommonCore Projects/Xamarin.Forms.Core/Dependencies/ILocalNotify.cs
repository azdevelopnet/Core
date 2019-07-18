using System;
namespace Xamarin.Forms.Core
{
	public class LocalNotification
	{
		public int Id { get; set; } = 1;
		public string Title { get; set; }
		public string SubTitle { get; set; } = string.Empty;
		public string Message { get; set; }
		public string Icon { get; set; }
		public string Sound { get; set; }
		public int? Badge { get; set; }
		public double SecondsOffSet { get; set; } = 0.01;
		public string MetaData { get; set; }
	}
	public interface ILocalNotify
	{
		void RequestPermission(Action<bool> callBack);
		void Show(LocalNotification notification);
	}
}
