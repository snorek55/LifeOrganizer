using Common.Adapters;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace MovOrg.Organizer.Adapters.Items
{
	public class MovieViewModel : BaseViewModel
	{
		#region General Info

		public string Id { get; set; }

		public string Title { get; set; }

		public float? IMDbRating { get; set; }

		public ImageViewModel CoverImage { get; set; }

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

		public List<ImageViewModel> Images { get; set; } = new List<ImageViewModel>();

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
	}
}