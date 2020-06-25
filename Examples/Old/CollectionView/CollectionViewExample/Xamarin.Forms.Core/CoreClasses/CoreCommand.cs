using System;
using System.ComponentModel;
using System.Windows.Input;

namespace Xamarin.Forms.Core
{
	public class CoreCommand : ICommand, IDisposable
	{
		private Action<object> _execute;
		private Func<bool> _validator;
		private INotifyPropertyChanged _npc;
		public event EventHandler CanExecuteChanged;

        /// <summary>
        /// RelayCommand's INotifyPropertyChanged object.  Must be reset of the original object is reinstantiated.
        /// </summary>
        /// <value>The notify binder.</value>
        public INotifyPropertyChanged NotifyBinder{
            get
            {
                return _npc;
            }
            set
            {
                if (_npc != null)
                    _npc.PropertyChanged -= PropertyChangedEvent;

                _npc = value;

                if (_npc != null)
                    _npc.PropertyChanged += PropertyChangedEvent;
                
            }
        }
		public bool CanExecute(object parameter)
		{
			return _validator != null ? _validator.Invoke() : true;
		}

		public CoreCommand(Action<object> execute, Func<bool> validator = null, INotifyPropertyChanged npc = null)
		{
			_execute = execute;
			_validator = validator;
			_npc = npc;

			if (_npc != null)
			{
				_npc.PropertyChanged += PropertyChangedEvent;
			}
		}
		private void PropertyChangedEvent(object sender, PropertyChangedEventArgs args)
		{
			CanExecuteChanged?.Invoke(this, null);
		}

		public void Execute(object parameter)
		{
			_execute(parameter);
		}

		~CoreCommand()
		{
			if (_npc != null)
				_npc.PropertyChanged -= PropertyChangedEvent;
		}
		public void Dispose()
		{
			if (_npc != null)
				_npc.PropertyChanged -= PropertyChangedEvent;
		}
	}
}
