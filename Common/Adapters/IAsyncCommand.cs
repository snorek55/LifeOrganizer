using System.Threading.Tasks;
using System.Windows.Input;

namespace Common.Adapters
{
	internal interface IAsyncCommand : ICommand
	{
		Task ExecuteAsync();

		bool CanExecute();
	}

	internal interface IAsyncCommand<T> : ICommand
	{
		Task ExecuteAsync(T parameter);

		bool CanExecute(T parameter);
	}
}