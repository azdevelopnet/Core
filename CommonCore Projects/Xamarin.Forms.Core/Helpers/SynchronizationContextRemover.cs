using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Xamarin.Forms.Core
{
	/// <summary>
	/// Synchronization context remover.
	/// From Github -> https://github.com/negativeeddy/blog-examples/blob/master/ConfigureAwaitBehavior/ExtremeConfigAwaitLibrary/SynchronizationContextRemover.cs
	/// Comments from StackOverFlow #14906092
	/// As a side note, in both cases SomeMethod() could still block your UI thread, 
	/// because await client.GetAsync(address) needs time to create a task to pass 
	/// to ConfigureAwait(false). And your time consuming operation might have already 
	/// started before task is returned.
	/// 
	/// </summary>
	public struct SynchronizationContextRemover : INotifyCompletion
	{
		public bool IsCompleted
		{
			get { return SynchronizationContext.Current == null; }
		}

		public void OnCompleted(Action continuation)
		{
			var prevContext = SynchronizationContext.Current;
			try
			{
				SynchronizationContext.SetSynchronizationContext(null);
				continuation();
			}
			finally
			{
				SynchronizationContext.SetSynchronizationContext(prevContext);
			}
		}

		public SynchronizationContextRemover GetAwaiter()
		{
			return this;
		}

		public void GetResult()
		{
		}
	}
}
