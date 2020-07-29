using Organizers.Common.Domain;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Organizers.MovOrg.Domain
{
	public class Movie : Entity, IEquatable<Movie>
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

		public override bool Equals(object obj)
		{
			return Equals(obj as Movie);
		}

		public bool Equals(Movie other)
		{
			return other != null &&
				   Id == other.Id &&
				   Title == other.Title &&
				   Plot == other.Plot &&
				   IMDbRating == other.IMDbRating &&
				   Description == other.Description &&
				   Image == other.Image &&
				   Year == other.Year &&
				   Tagline == other.Tagline &&
				   Keywords == other.Keywords &&
				   RuntimeStr == other.RuntimeStr &&
				   Awards == other.Awards &&
				   Genres == other.Genres &&
				   Countries == other.Countries &&
				   Languages == other.Languages &&
				   LastUpdatedTop250 == other.LastUpdatedTop250 &&
				   IsTop250 == other.IsTop250 &&
				   //TODO: time is not the same and makes more difficult to compare on testing. What to do? Fake Datetime
				   //  LastUpdatedDetails == other.LastUpdatedDetails &&
				   AreDetailsAvailable == other.AreDetailsAvailable &&
				   ReleaseDate == other.ReleaseDate &&
				   Rank == other.Rank &&
				   EqualityComparer<Trailer>.Default.Equals(Trailer, other.Trailer) &&
				   EqualityComparer<BoxOffice>.Default.Equals(BoxOffice, other.BoxOffice) &&
				   WikipediaUrl == other.WikipediaUrl &&
				   DirectorList.SequenceEqual(other.DirectorList) &&
				   CompanyList.SequenceEqual(other.CompanyList) &&
				   WriterList.SequenceEqual(other.WriterList) &&
				   IsFavorite == other.IsFavorite &&
				   IsWatched == other.IsWatched &&
				   IsMustWatch == other.IsMustWatch;
		}

		public override int GetHashCode()
		{
			var hash = new HashCode();
			hash.Add(Id);
			hash.Add(Title);
			hash.Add(Plot);
			hash.Add(IMDbRating);
			hash.Add(Description);
			hash.Add(Image);
			hash.Add(Year);
			hash.Add(Tagline);
			hash.Add(Keywords);
			hash.Add(RuntimeStr);
			hash.Add(Awards);
			hash.Add(Genres);
			hash.Add(Countries);
			hash.Add(Languages);
			hash.Add(LastUpdatedTop250);
			hash.Add(IsTop250);
			hash.Add(LastUpdatedDetails);
			hash.Add(AreDetailsAvailable);
			hash.Add(ReleaseDate);
			hash.Add(Rank);
			hash.Add(Trailer);
			hash.Add(BoxOffice);
			hash.Add(WikipediaUrl);
			hash.Add(DirectorList);
			hash.Add(ActorList);
			hash.Add(CompanyList);
			hash.Add(WriterList);
			hash.Add(Ratings);
			hash.Add(IsFavorite);
			hash.Add(IsWatched);
			hash.Add(IsMustWatch);
			return hash.ToHashCode();
		}
	}
}