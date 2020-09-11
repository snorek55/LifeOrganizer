﻿using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MovOrg.Organizer.UseCases
{
	public abstract class ServiceActionBase
	{
		private readonly List<ValidationResult> _errors = new List<ValidationResult>();

		public IImmutableList<ValidationResult>
		Errors => _errors.ToImmutableList();

		public bool HasErrors => _errors.Any();

		protected void AddError(string errorMessage, params string[] propertyNames)
		{
			_errors.Add(new ValidationResult
			(errorMessage, propertyNames));
		}
	}
}