using System;
using System.Windows.Input;

namespace Organizers.Common.Adapters
{
	internal class SyncCommand : ICommand
	{
		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		private bool _isExecuting;
		private readonly Action _execute;
		private readonly Func<bool> _canExecute;

		public SyncCommand(Action execute, Func<bool> canExecute = null)
		{
			_execute = execute ?? throw new ArgumentNullException(nameof(execute));
			_canExecute = canExecute;
		}

		public bool CanExecute(object parameter)
		{
			return !_isExecuting && (_canExecute?.Invoke() ?? true);
		}

		public void Execute(object parameter)
		{
			if (CanExecute(parameter))
			{
				try
				{
					_isExecuting = true;
					_execute();
				}
				finally
				{
					_isExecuting = false;
				}
			}
		}
	}
}