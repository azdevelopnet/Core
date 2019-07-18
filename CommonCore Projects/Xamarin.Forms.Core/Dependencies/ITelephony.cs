using System;
namespace Xamarin.Forms.Core
{
	public class TelephonyCompleteStatus
	{
		public DateTime Completed { get; set; }
		public bool Success { get; set; }
		public Exception Error { get; set; }
	}

    public interface ITelephony
    {
		void PlaceCallWithCallBack(string phoneNumber, string key);
    }
}
