using EntityFramework.DbContextScope.Interfaces;

using System;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace MovOrg.Organizer.UseCases.Runners
{
	//Calls business logic and saves change
	public class RunnerWriteDb<TIn>
	{
		private readonly IServiceAction<TIn> _actionClass;

		private IDbContextScopeFactory dbContextScopeFactory;

		//TODO: make base error handling for runners
		public IImmutableList<ValidationResult> Errors => _actionClass.Errors;

		public bool HasErrors => _actionClass.HasErrors;

		public RunnerWriteDb(IServiceAction<TIn> actionClass, IDbContextScopeFactory dbContextScopeFactory)
		{
			this.dbContextScopeFactory = dbContextScopeFactory ?? throw new ArgumentNullException(nameof(dbContextScopeFactory));
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