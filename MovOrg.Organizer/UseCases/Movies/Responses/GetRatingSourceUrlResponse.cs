using Common.UseCases;

using MovOrg.Organizer.UseCases.DTOs;

namespace MovOrg.Organizer.UseCases.Responses
{
	public class GetRatingSourceUrlResponse : DataResponseBase<MovieRatingSourceUrlDto>
	{
		public MovieRatingSourceUrlDto MovieRatingSiteDto { get; set; }

		public GetRatingSourceUrlResponse()
		{
		}

		public GetRatingSourceUrlResponse(MovieRatingSourceUrlDto dto)
		{
			MovieRatingSiteDto = dto;
		}

		public GetRatingSourceUrlResponse(string error) : base(error)
		{
		}
	}
}