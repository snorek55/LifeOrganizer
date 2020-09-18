using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace MovOrg.Organizer.UseCases
{
	//TODO: two versions async and sync
	public interface IServiceAction<in TIn, TOut>
	{
		IImmutableList<ValidationResult> Errors { get; }
		bool HasErrors { get; }

		Task<TOut> Action(TIn dto);
	}

	public interface IServiceAction<in TIn>
	{
		IImmutableList<ValidationResult> Errors { get; }
		bool HasErrors { get; }

		Task<bool> Action(TIn dto);
	}
}