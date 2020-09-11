using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace MovOrg.Organizer.UseCases
{
	public interface IServiceAction<in TIn, TOut>
	{
		IImmutableList<ValidationResult> Errors { get; }
		bool HasErrors { get; }

		Task<TOut> Action(TIn dto);
	}
}