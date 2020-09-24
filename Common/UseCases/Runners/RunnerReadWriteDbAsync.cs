using EntityFramework.DbContextScope.Interfaces;

using System.Threading.Tasks;

namespace Common.UseCases.Runners
{
	//Calls business logic and saves change
	public class RunnerReadWriteDbAsync<TIn, TOut> : BaseRunner
	{
		private readonly IServiceActionAsync<TIn, TOut> _actionClass;

		public RunnerReadWriteDbAsync(IServiceActionAsync<TIn, TOut> actionClass, IDbContextScopeFactory dbContextScopeFactory) : base(dbContextScopeFactory, actionClass)
		{
			_actionClass = actionClass;
		}

		public async Task<TOut> RunAction(TIn dataIn)
		{
			using var context = dbContextScopeFactory.Create();
			var result = await _actionClass.Action(dataIn);
			if (!HasErrors)
				context.SaveChanges();
			return result;
		}
	}
}