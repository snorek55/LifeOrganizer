using EntityFramework.DbContextScope.Interfaces;

using System;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Common.UseCases.Runners
{
	public abstract class BaseRunner
	{
		public IImmutableList<ValidationResult> Errors => actionClass.Errors;

		public string ErrorString => string.Join(Environment.NewLine, actionClass.Errors.Select(x => x.ErrorMessage));

		public bool HasErrors => actionClass.HasErrors;

		protected IDbContextScopeFactory dbContextScopeFactory;

		private readonly IServiceBase actionClass;

		protected BaseRunner(IDbContextScopeFactory dbContextScopeFactory, IServiceBase actionClass)
		{
			this.dbContextScopeFactory = dbContextScopeFactory;
			this.actionClass = actionClass;
		}
	}
}