using EntityFramework.DbContextScope.Interfaces;

using System;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace MovOrg.Organizer.UseCases.Runners
{
	//Calls business logic and saves change
	public class RunnerReadWriteDb<TIn, TOut>
	{
		private readonly IServiceAction<TIn, TOut> _actionClass;

		private IDbContextScopeFactory dbContextScopeFactory;

		//TODO: make base error handling for runners
		public IImmutableList<ValidationResult> Errors => _actionClass.Errors;

		public bool HasErrors => _actionClass.HasErrors;

		public RunnerReadWriteDb(IServiceAction<TIn, TOut> actionClass, IDbContextScopeFactory dbContextScopeFactory)
		{
			this.dbContextScopeFactory = dbContextScopeFactory ?? throw new ArgumentNullException(nameof(dbContextScopeFactory));
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