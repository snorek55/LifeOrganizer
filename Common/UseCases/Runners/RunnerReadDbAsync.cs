using EntityFramework.DbContextScope.Interfaces;

using System.Threading.Tasks;

namespace Common.UseCases.Runners
{
	//Calls business logic and saves change
	public class RunnerReadDbAsync<TIn, TOut> : BaseRunner
	{
		private readonly IServiceActionAsync<TIn, TOut> _actionClass;

		public RunnerReadDbAsync(IServiceActionAsync<TIn, TOut> actionClass, IDbContextScopeFactory dbContextScopeFactory) : base(dbContextScopeFactory, actionClass)
		{
			_actionClass = actionClass;
		}

		public async Task<TOut> RunAction(TIn dataIn)
		{
			using var context = dbContextScopeFactory.CreateReadOnly();
			var result = await _actionClass.Action(dataIn);
			return result;
		}
	}
}