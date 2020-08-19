using MovOrg.Organizer.Domain;

using System.Collections.Generic;

namespace MovOrg.Organizer.UseCases.DTOs
{
	public class MovieWithDetailsDto
	{
		#region Basic Data

		public string Id { get; set; }

		public string Title { get; set; }
		public string Plot { get; set; }
		public string Description { get; set; }
		public float? IMDbRating { get; set; }

		public string CoverImageUrl { get; set; }
		public string Year { get; set; }

		public string Tagline { get; set; }
		public string Keywords { get; set; }
		public string RuntimeStr { get; set; }
		public string Awards { get; set; }
		public string Genres { get; set; }
		public string Countries { get; set; }
		public string Languages { get; set; }

		public string ReleaseDate { get; set; }

		#endregion Basic Data

		public Trailer Trailer { get; set; }
		public BoxOffice BoxOffice { get; set; }
		public List<ActorDto> ActorList { get; set; }
		public List<DirectorDto> DirectorList { get; set; }
		public List<WriterDto> WriterList { get; set; }
		public List<CompanyDto> CompanyList { get; set; }
		public List<SimilarDto> Similars { get; set; }

		public List<RatingDto> Ratings { get; set; }

		public List<MovieImageDto> Images { get; set; }
	}
}