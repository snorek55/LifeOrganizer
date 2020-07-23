﻿using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Organizers.Common.Adapters
{
	//https://johnthiriet.com/removing-async-void/
	internal class AsyncCommand : IAsyncCommand
	{
		public event EventHandler CanExecuteChanged
		{
			add => CommandManager.RequerySuggested += value;

			remove => CommandManager.RequerySuggested -= value;
		}

		private bool _isExecuting;
		private readonly Func<Task> _execute;
		private readonly Func<bool> _canExecute;
		private readonly IErrorHandler _errorHandler;

		internal AsyncCommand(
			Func<Task> execute,
			IErrorHandler errorHandler = null,
				Func<bool> canExecute = null)
		{
			_execute = execute;
			_canExecute = canExecute;
			_errorHandler = errorHandler ?? new BaseErrorHandler();
		}

		public bool CanExecute()
		{
			return !_isExecuting && (_canExecute?.Invoke() ?? true);
		}

		public async Task ExecuteAsync()
		{
			if (CanExecute())
			{
				try
				{
					_isExecuting = true;
					await _execute();
				}
				finally
				{
					_isExecuting = false;
				}
			}
		}

		#region Explicit implementations

		bool ICommand.CanExecute(object parameter)
		{
			return CanExecute();
		}

		void ICommand.Execute(object parameter)
		{
			ExecuteAsync().FireAndForgetSafeAsync(_errorHandler);
		}

		#endregion Explicit implementations
	}

	internal class AsyncCommand<T> : IAsyncCommand<T>
	{
		public event EventHandler CanExecuteChanged
		{
			add => CommandManager.RequerySuggested += value;

			remove => CommandManager.RequerySuggested -= value;
		}

		private bool _isExecuting;
		private readonly Func<T, Task> _execute;
		private readonly Func<T, bool> _canExecute;
		private readonly IErrorHandler _errorHandler;

		internal AsyncCommand(Func<T, Task> execute, IErrorHandler errorHandler = null, Func<T, bool> canExecute = null)
		{
			_execute = execute;
			_canExecute = canExecute;
			_errorHandler = errorHandler;
		}

		public bool CanExecute(T parameter)
		{
			return !_isExecuting && (_canExecute?.Invoke(parameter) ?? true);
		}

		public async Task ExecuteAsync(T parameter)
		{
			if (CanExecute(parameter))
			{
				try
				{
					_isExecuting = true;
					await _execute(parameter);
				}
				finally
				{
					_isExecuting = false;
				}
			}
		}

		#region Explicit implementations

		bool ICommand.CanExecute(object parameter)
		{
			return CanExecute((T)parameter);
		}

		void ICommand.Execute(object parameter)
		{
			ExecuteAsync((T)parameter).FireAndForgetSafeAsync(_errorHandler);
		}

		#endregion Explicit implementations
	}
}