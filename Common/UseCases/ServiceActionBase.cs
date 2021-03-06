﻿using Common.UseCases;

using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Common.UseCases
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

		protected DataResponseBase<TData> ReturnErrorIfNull<TData>(TData data)
		{
			if (data == null)
				return new DataResponseBase<TData>(Errors.ToString());
			else
			{
				var response = new DataResponseBase<TData>();
				response.Data = data;
				return response;
			}
		}

		protected ResponseBase ReturnErrorIfFalse(bool success)
		{
			if (!success)
				return new ResponseBase(Errors.ToString());
			else
				return new ResponseBase();
		}
	}
}