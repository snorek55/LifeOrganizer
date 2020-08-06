using Organizers.Common.Domain;

using System;
using System.Collections.Generic;

namespace Organizers.MovOrg.Domain
{
	public class Movie : Entity, IEquatable<Movie>
	{
		#region Basic Data

		public string Title { get; set; }
		public string Plot { get; set; }
		public string Description { get; set; }

		public string IMDbRating { get; set; }
		public string CoverImage { get; set; }
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

		public List<MovieActor> ActorList { get; set; } = new List<MovieActor>();
		public List<MovieDirector> DirectorList { get; set; } = new List<MovieDirector>();

		public List<MovieWriter> WriterList { get; set; } = new List<MovieWriter>();

		public List<MovieCompany> CompanyList { get; set; } = new List<MovieCompany>();

		public List<Rating> Ratings { get; set; } = new List<Rating>();

		public List<MovieImageData> Images { get; set; } = new List<MovieImageData>();

		#endregion Related data

		#region User Data

		public bool IsFavorite { get; set; }
		public bool IsWatched { get; set; }
		public bool IsMustWatch { get; set; }

		#endregion User Data

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
				   Description == other.Description &&
				   CoverImage == other.CoverImage &&
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
				   AreDetailsAvailable == other.AreDetailsAvailable &&
				   ReleaseDate == other.ReleaseDate &&
				   Rank == other.Rank &&
				   EqualityComparer<Trailer>.Default.Equals(Trailer, other.Trailer) &&
				   EqualityComparer<BoxOffice>.Default.Equals(BoxOffice, other.BoxOffice) &&
				   WikipediaUrl == other.WikipediaUrl &&
				   //TODO: create equality comparers for these
				   //EqualityComparer<List<MovieActor>>.Default.Equals(ActorList, other.ActorList) &&
				   //EqualityComparer<List<MovieDirector>>.Default.Equals(DirectorList, other.DirectorList) &&
				   //EqualityComparer<List<MovieWriter>>.Default.Equals(WriterList, other.WriterList) &&
				   //EqualityComparer<List<MovieCompany>>.Default.Equals(CompanyList, other.CompanyList) &&
				   //EqualityComparer<List<Rating>>.Default.Equals(Ratings, other.Ratings) &&
				   //EqualityComparer<List<MovieImageData>>.Default.Equals(Images, other.Images) &&
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
			hash.Add(Description);
			hash.Add(CoverImage);
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
			hash.Add(AreDetailsAvailable);
			hash.Add(ReleaseDate);
			hash.Add(Rank);
			hash.Add(Trailer);
			hash.Add(BoxOffice);
			hash.Add(WikipediaUrl);
			hash.Add(ActorList);
			hash.Add(DirectorList);
			hash.Add(WriterList);
			hash.Add(CompanyList);
			hash.Add(Ratings);
			hash.Add(Images);
			hash.Add(IsFavorite);
			hash.Add(IsWatched);
			hash.Add(IsMustWatch);
			return hash.ToHashCode();
		}

		//public List<Movie> Similars { get; set; }
	}
}