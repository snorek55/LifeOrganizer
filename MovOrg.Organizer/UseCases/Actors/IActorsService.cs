using Common.UseCases;

using MovOrg.Organizer.UseCases.Actors.DTO;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovOrg.Organizer.UseCases
{
	public interface IActorsService
	{
		Task<DataResponseBase<bool>> UpdateFavoriteStatus(string id, bool isFavorite);

		Task<DataResponseBase<IEnumerable<ActorListItemDto>>> GetAllActorsFromLocal();

		Task<DataResponseBase<ActorWithDetailsDto>> GetActorWithId(string id, bool forceUpdate);

		Task<DataResponseBase<IEnumerable<ActorListItemDto>>> GetActorsFromSuggestedNameAsync(string suggestedName);
	}
}