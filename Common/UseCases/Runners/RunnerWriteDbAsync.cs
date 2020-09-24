using EntityFramework.DbContextScope.Interfaces;

using System;
using System.Threading.Tasks;

namespace Common.UseCases.Runners
{
	//Calls business logic and saves change
	public class RunnerWriteDbAsync<TIn> : BaseRunner
	{
		private readonly IServiceActionAsync<TIn> _actionClass;

		public RunnerWriteDbAsync(IServiceActionAsync<TIn> actionClass, IDbContextScopeFactory dbContextScopeFactory) : base(dbContextScopeFactory, actionClass)
		{
			_actionClass = actionClass;
		}

		public async Task<bool> RunAction(TIn dataIn)
		{
			using var context = dbContextScopeFactory.Create();
			var result = await _actionClass.Action(dataIn);
			if (!HasErrors)
				context.SaveChanges();
			return result;
		}
	}
}