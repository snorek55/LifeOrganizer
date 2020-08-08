using Organizers.Common.Adapters;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace Organizers.MovOrg.Adapters.Items
{
	public class MovieViewModel : BaseViewModel, IEquatable<MovieViewModel>
	{
		#region General Info

		public string Id { get; set; }

		public string Title { get; set; }

		public float? IMDbRating { get; set; }

		public string CoverImage { get; set; }

		public int? Rank { get; set; }

		#endregion General Info

		#region Details

		public string Plot { get; set; }

		public string Description { get; set; } = "";

		public string Year { get; set; }

		public string Tagline { get; set; }

		public string Keywords { get; set; }

		public string RuntimeStr { get; set; }

		public string Awards { get; set; }

		public string Genres { get; set; }

		public string Countries { get; set; }

		public string Languages { get; set; }

		public string WikipediaUrl { get; set; }

		//TODO: this should be datetime here and in the domain
		public string ReleaseDate { get; set; }

		public string LastUpdatedDetails { get; set; }

		public BoxOfficeViewModel BoxOffice { get; set; }

		public TrailerViewModel Trailer { get; set; }

		public List<DirectorViewModel> DirectorList { get; set; } = new List<DirectorViewModel>();

		public ObservableCollection<ActorViewModel> ActorList { get; set; } = new ObservableCollection<ActorViewModel>();
		private ICollectionView actorCollectionView;

		public List<CompanyViewModel> CompanyList { get; set; } = new List<CompanyViewModel>();

		public List<WriterViewModel> WriterList { get; set; } = new List<WriterViewModel>();

		public List<RatingViewModel> Ratings { get; set; } = new List<RatingViewModel>();

		public List<ImageDataViewModel> Images { get; set; } = new List<ImageDataViewModel>();

		public List<SimilarMovieViewModel> Similars { get; set; } = new List<SimilarMovieViewModel>();
		public SimilarMovieViewModel SelectedSimilar { get; set; }

		#endregion Details

		#region User Preferences

		public bool IsFavorite { get; set; }

		public bool IsMustWatch { get; set; }

		public bool IsWatched { get; set; }

		public bool ShowAllActors { get; set; }

		#endregion User Preferences

#pragma warning disable IDE0051 // Quitar miembros privados no utilizados

		private void OnActorListChanged()

		{
			actorCollectionView = CollectionViewSource.GetDefaultView(ActorList);
			actorCollectionView.Filter = new Predicate<object>(x => ActorFiltering(x as ActorViewModel));

			RefreshFilter();
		}

		private void OnShowAllActorsChanged()
#pragma warning restore IDE0051 // Quitar miembros privados no utilizados
		{
			RefreshFilter();
		}

		private bool ActorFiltering(ActorViewModel actorViewModel)
		{
			return actorViewModel.IsStar || ShowAllActors;
		}

		private void RefreshFilter()
		{
			actorCollectionView.Refresh();
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as MovieViewModel);
		}

		public bool Equals(MovieViewModel other)
		{
			return other != null &&
				   Id == other.Id &&
				   Title == other.Title &&
				   IMDbRating == other.IMDbRating &&
				   CoverImage == other.CoverImage &&
				   Rank == other.Rank &&
				   Plot == other.Plot &&
				   Description == other.Description &&
				   Year == other.Year &&
				   Tagline == other.Tagline &&
				   Keywords == other.Keywords &&
				   RuntimeStr == other.RuntimeStr &&
				   Awards == other.Awards &&
				   Genres == other.Genres &&
				   Countries == other.Countries &&
				   Languages == other.Languages &&
				   WikipediaUrl == other.WikipediaUrl &&
				   ReleaseDate == other.ReleaseDate &&
				   //EqualityComparer<BoxOfficeViewModel>.Default.Equals(BoxOffice, other.BoxOffice) &&
				   //EqualityComparer<TrailerViewModel>.Default.Equals(Trailer, other.Trailer) &&
				   //EqualityComparer<List<DirectorViewModel>>.Default.Equals(DirectorList, other.DirectorList) &&
				   //EqualityComparer<ObservableCollection<ActorViewModel>>.Default.Equals(ActorList, other.ActorList) &&
				   //EqualityComparer<List<CompanyViewModel>>.Default.Equals(CompanyList, other.CompanyList) &&
				   //EqualityComparer<List<WriterViewModel>>.Default.Equals(WriterList, other.WriterList) &&
				   //EqualityComparer<List<RatingViewModel>>.Default.Equals(Ratings, other.Ratings) &&
				   //EqualityComparer<List<ImageDataViewModel>>.Default.Equals(Images, other.Images) &&
				   //EqualityComparer<List<SimilarMovieViewModel>>.Default.Equals(Similars, other.Similars) &&
				   IsFavorite == other.IsFavorite &&
				   IsMustWatch == other.IsMustWatch &&
				   IsWatched == other.IsWatched;
		}

		public override int GetHashCode()
		{
			var hash = new HashCode();
			hash.Add(Id);
			hash.Add(Title);
			hash.Add(IMDbRating);
			hash.Add(CoverImage);
			hash.Add(Rank);
			hash.Add(Plot);
			hash.Add(Description);
			hash.Add(Year);
			hash.Add(Tagline);
			hash.Add(Keywords);
			hash.Add(RuntimeStr);
			hash.Add(Awards);
			hash.Add(Genres);
			hash.Add(Countries);
			hash.Add(Languages);
			hash.Add(WikipediaUrl);
			hash.Add(ReleaseDate);
			//hash.Add(BoxOffice);
			//hash.Add(Trailer);
			//hash.Add(DirectorList);
			//hash.Add(ActorList);
			//hash.Add(CompanyList);
			//hash.Add(WriterList);
			//hash.Add(Ratings);
			//hash.Add(Images);
			//hash.Add(Similars);
			hash.Add(IsFavorite);
			hash.Add(IsMustWatch);
			hash.Add(IsWatched);
			return hash.ToHashCode();
		}
	}
}