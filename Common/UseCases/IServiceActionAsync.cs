using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Common.UseCases
{
	public interface IServiceBase
	{
		IImmutableList<ValidationResult> Errors { get; }
		bool HasErrors { get; }
	}

	public interface IServiceActionAsync<in TIn, TOut> : IServiceBase
	{
		Task<TOut> Action(TIn dto);
	}

	public interface IServiceActionAsync<in TIn> : IServiceBase
	{
		Task<bool> Action(TIn dto);
	}

	public interface IServiceAction<in TIn, TOut> : IServiceBase
	{
		TOut Action(TIn dto);
	}

	public interface IServiceAction<in TIn> : IServiceBase
	{
		bool Action(TIn dto);
	}
}