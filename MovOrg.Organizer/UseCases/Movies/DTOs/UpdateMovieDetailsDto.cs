using System.Collections.Generic;

namespace MovOrg.Organizer.UseCases.DTOs
{
	public class UpdateMovieDetailsDto : MovieWithDetailsDto
	{
		public List<MovieImageDto> Images { get; set; }
	}
}