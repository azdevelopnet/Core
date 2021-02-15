using System;
#if __ANDROID__
using Android.Views.InputMethods;
#else
using UIKit;
#endif


namespace Xamarin.Forms.Core
{
	public enum ReturnKeyTypes : int
	{
		Default,
		Go,
		Google,
		Join,
		Next,
		Route,
		Search,
		Send,
		Yahoo,
		Done,
		EmergencyCall,
		Continue
	}

	public static class ReturnKeyExt
    {

#if __ANDROID__
		public static ImeAction ToImeAction(this ReturnKeyTypes type)
        {
			var imeType = ImeAction.Unspecified;

			switch (type)
            {
				case ReturnKeyTypes.Done:
					imeType = ImeAction.Done;
					break;
				case ReturnKeyTypes.Send:
					imeType = ImeAction.Send;
					break;
				case ReturnKeyTypes.Next:
					imeType = ImeAction.Next;
					break;
				case ReturnKeyTypes.Go:
					imeType = ImeAction.Go;
					break;
				case ReturnKeyTypes.Search:
					imeType = ImeAction.Search;
					break;
				default:
					imeType = ImeAction.Unspecified;
					break;
			}

			return imeType;
        }
#else
		public static UIReturnKeyType ToUIReturnKey(this ReturnKeyTypes type)
		{
			var returnType = UIReturnKeyType.Default;

			switch (type)
			{
				case ReturnKeyTypes.Done:
					returnType = UIReturnKeyType.Done;
					break;
				case ReturnKeyTypes.Send:
					returnType = UIReturnKeyType.Send;
					break;
				case ReturnKeyTypes.Next:
					returnType = UIReturnKeyType.Next;
					break;
				case ReturnKeyTypes.Go:
					returnType = UIReturnKeyType.Go;
					break;
				case ReturnKeyTypes.Search:
					returnType = UIReturnKeyType.Search;
					break;
				case ReturnKeyTypes.Google:
					returnType = UIReturnKeyType.Google;
					break;
				case ReturnKeyTypes.Join:
					returnType = UIReturnKeyType.Join;
					break;
				case ReturnKeyTypes.Route:
					returnType = UIReturnKeyType.Route;
					break;
				case ReturnKeyTypes.Continue:
					returnType = UIReturnKeyType.Continue;
					break;
				case ReturnKeyTypes.EmergencyCall:
					returnType = UIReturnKeyType.EmergencyCall;
					break;
				case ReturnKeyTypes.Yahoo:
					returnType = UIReturnKeyType.Yahoo;
					break;
			}

			return returnType;
		}
#endif

	}
}
