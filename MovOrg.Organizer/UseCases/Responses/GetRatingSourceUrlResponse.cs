using Common.UseCases;

using MovOrg.Organizer.UseCases.DTOs;

namespace MovOrg.Organizer.UseCases.Responses
{
	public class GetRatingSourceUrlResponse : ResponseBase
	{
		//TODO: improve naming
		public MovieRatingSourceDto MovieRatingSiteDto { get; set; }

		public GetRatingSourceUrlResponse()
		{
		}

		public GetRatingSourceUrlResponse(MovieRatingSourceDto dto)
		{
			MovieRatingSiteDto = dto;
		}

		public GetRatingSourceUrlResponse(string error) : base(error)
		{
		}
	}
}