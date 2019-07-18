using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;

namespace Xamarin.Forms.Core
{
	/// <summary>
	/// ObservableCollection implementation which supports 
	/// turning off notifications for mass updates through 
	/// the <see cref="BeginMassUpdate"/> method.
	/// </summary>
	/// <example>
	/// <code>
	/// var coll = new OptimizedObservableCollection&lt;string&gt;();
	/// ...
	/// using (BeginMassUpdate ()) {
	///    foreach (var value in names)
	///       coll.Add (value);
	/// }
	/// </code>
	/// </example>
	[DebuggerDisplay("Count={Count}")]
	public class OptimizedObservableCollection<T> : ObservableCollection<T>
	{
		bool shouldRaiseNotifications = true;

		/// <summary> 
		/// Init a new instance of the collection.
		/// </summary> 
		public OptimizedObservableCollection()
		{
		}

		/// <summary>
		/// Initialize a new instance of the collection from an existing data set.
		/// </summary>
		/// <param name="collection">Collection.</param>
		public OptimizedObservableCollection(IEnumerable<T> collection)
			: base(collection)
		{
		}

		/// <summary>
		/// This method turns off notifications until the returned object
		/// is Disposed. At that point, the entire collection is invalidated.
		/// </summary>
		/// <returns>IDisposable</returns>
		public IDisposable BeginMassUpdate()
		{
			return new MassUpdater(this);
		}

		/// <summary>
		/// Turn off the collection changed notification
		/// </summary>
		/// <param name="e">E.</param>
		protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			if (shouldRaiseNotifications)
				base.OnCollectionChanged(e);
		}

		/// <summary>
		/// Turn off the property changed notification
		/// </summary>
		/// <param name="e">E.</param>
		protected override void OnPropertyChanged(PropertyChangedEventArgs e)
		{
			if (shouldRaiseNotifications)
				base.OnPropertyChanged(e);
		}

		/// <summary>
		/// IDisposable class which turns off updating
		/// </summary>
		class MassUpdater : IDisposable
		{
			readonly OptimizedObservableCollection<T> parent;
			public MassUpdater(OptimizedObservableCollection<T> parent)
			{
				this.parent = parent;
				parent.shouldRaiseNotifications = false;
			}

#if DEBUG
			~MassUpdater()
			{
				Debug.Assert(true, "Did not dispose returned object from OptimizedObservableCollection.BeginMassUpdate!");
			}
#endif


			public void Dispose()
			{
				parent.shouldRaiseNotifications = true;
				parent.OnPropertyChanged(new PropertyChangedEventArgs("Count"));
				parent.OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
				parent.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			}
		}
	}
}
