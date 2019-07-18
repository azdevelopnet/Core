using System;
using System.Windows.Input;

namespace Xamarin.Forms.Core
{
	public interface ISearchProvider
	{
		ICommand SearchCommand { get; }
		bool SearchIsDefaultAction { get; }
		string QueryHint { get; }
	}
}
