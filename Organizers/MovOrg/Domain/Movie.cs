using Organizers.Common.Domain;

using System;
using System.Collections.Generic;

namespace Domain
{
	public class Movie : Entity
	{
		#region Basic Data

		public string Title { get; set; }
		public string Plot { get; set; }
		public float IMDbRating { get; set; }
		public string Description { get; set; }
		public string Image { get; set; }
		public string Year { get; set; }
		public string Tagline { get; set; }
		public string Keywords { get; set; }
		public string RuntimeStr { get; set; }
		public string Awards { get; set; }
		public string Genres { get; set; }
		public string Countries { get; set; }
		public string Languages { get; set; }

		public DateTime? LastUpdatedTop250 { get; set; }
		public bool IsTop250 { get => LastUpdatedTop250 != null; }
		public DateTime? LastUpdatedDetails { get; set; }
		public bool AreDetailsAvailable { get => LastUpdatedDetails != null; }
		public string ReleaseDate { get; set; }

		public int? Rank { get; set; }

		#endregion Basic Data

		#region Related data

		public Trailer Trailer { get; set; }

		public BoxOffice BoxOffice { get; set; }

		public string WikipediaUrl { get; set; }

		public List<MovieDirector> DirectorList { get; set; } = new List<MovieDirector>();

		public List<MovieActor> ActorList { get; set; } = new List<MovieActor>();

		public List<MovieCompany> CompanyList { get; set; } = new List<MovieCompany>();

		public List<MovieWriter> WriterList { get; set; } = new List<MovieWriter>();
		public List<Rating> Ratings { get; set; } = new List<Rating>();

		#endregion Related data

		#region User Data

		public bool IsFavorite { get; set; }
		public bool IsWatched { get; set; }
		public bool IsMustWatch { get; set; }

		#endregion User Data

		//public List<ImageData> Images { get; set; }
		//public List<Movie> Similars { get; set; }

		//public List<Actor> Stars { get; set; }
	}
}