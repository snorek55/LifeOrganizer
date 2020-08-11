using Common.Adapters;
using Common.Config;
using Common.Extensions;

using MovOrg.Organizers.Adapters.Items;
using MovOrg.Organizers.Domain;
using MovOrg.Organizers.UseCases;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace MovOrg.Organizers.Adapters.Sections
{
	public class MoviesSectionViewModel : SectionViewModel
	{
		public ObservableCollection<MovieViewModel> Movies { get; } = new ObservableCollection<MovieViewModel>();

		public MovieViewModel SelectedMovie { get; set; }

		public MovieDetailsPanelViewModel MovieDetailsPanel { get; private set; }

		public bool AreDetailsShowing { get => SelectedMovie != null; }

		#region Commands

		public ICommand SearchCommand { get => new AsyncCommand(SearchMoviesWithChosenTitleAsync, NotificationsHandler); }

		public ICommand ClearSearchCommand { get => new AsyncCommand(ClearSearchAsync, NotificationsHandler); }

		public ICommand Top250Command { get => new AsyncCommand(SearchTop250MoviesAsync, NotificationsHandler); }

		public ICommand SortAlphabeticallyCommand { get => new SyncCommand(SortAlphabetically); }

		#endregion Commands

		#region Filtering Properties

		public bool OnlyFavorites { get; set; }

		public bool OnlyMustWatch { get; set; }

		public bool OnlyWatched { get; set; }

		public bool OnlyTop250 { get; set; }

		private string SuggestedTitleFilter { get; set; }

		#endregion Filtering Properties

		#region Data

		public string SuggestedTitle { get; set; }

		public DateTime? LastUpdatedTop250 { get; set; }

		#endregion Data

		#region Private Fields

		private ICollectionView moviesCollectionView;

		private IMoviesService moviesService;
		private IAutoMapper mapper;
		private IDispatcher dispatcher;

		#endregion Private Fields

		public MoviesSectionViewModel()
		{
			SectionName = "Movies";
			moviesCollectionView = CollectionViewSource.GetDefaultView(Movies);
			moviesCollectionView.Filter = new Predicate<object>(x => MovieFiltering(x as MovieViewModel));
			Movies.CollectionChanged += Movies_CollectionChanged;
		}

		public MoviesSectionViewModel(IMoviesService moviesService, IAutoMapper autoMapper, IDispatcher dispatcher) : this()
		{
			mapper = autoMapper;
			this.dispatcher = dispatcher;
			this.moviesService = moviesService;
			MovieDetailsPanel = new MovieDetailsPanelViewModel(moviesService, this);
			GetAllMoviesFromLocalOnStartup();
		}

		private void GetAllMoviesFromLocalOnStartup()
		{
			dispatcher.BeginInvoke(async () =>
			{
				var response = await ExecuteCommandTaskAsync(() => moviesService.GetAllMoviesFromLocal(), "Loaded movies from local");
				if (!response.HasError)
					ResetAndUpdateMovies(response.Movies);
			});
		}

		#region Command Methods

		private async Task ClearSearchAsync()
		{
			SuggestedTitleFilter = string.Empty;
			var moviesResponse = await ExecuteCommandTaskAsync(() => moviesService.GetAllMoviesFromLocal(), "Loaded movies from local");

			if (!moviesResponse.HasError)
				ResetAndUpdateMovies(moviesResponse.Movies);
		}

		private async Task SearchTop250MoviesAsync()
		{
			var updateResponse = await ExecuteCommandTaskAsync(() => moviesService.UpdateTopMovies(), "");
			if (updateResponse.HasError)
				return;

			var moviesResponse = await ExecuteCommandTaskAsync(() => moviesService.GetAllMoviesFromLocal(), "Actualizadas mejores 250 peliculas");
			if (!moviesResponse.HasError)
				ResetAndUpdateMovies(moviesResponse.Movies);
		}

		private async Task SearchMoviesWithChosenTitleAsync()
		{
			if (string.IsNullOrEmpty(SuggestedTitle))
			{
				await ClearSearchAsync();
				return;
			}

			var suggestedTitleResponse = await ExecuteCommandTaskAsync(() => moviesService.GetMoviesFromSuggestedTitleAsync(SuggestedTitle), "");

			if (suggestedTitleResponse.AlreadySearched)
			{
				SuggestedTitleFilter = SuggestedTitle;
				RefreshFilter();
				return;
			}

			if (!suggestedTitleResponse.HasError)
			{
				ResetAndUpdateMovies(suggestedTitleResponse.Movies);
				NotifyStatus("Found " + suggestedTitleResponse.Movies.Count() + " movies");
			}
			else
			{
				Movies.Clear();
			}
		}

		#endregion Command Methods

		#region Filtering and Sorting Methods

		private bool MovieFiltering(MovieViewModel movie)
		{
			bool conditions = true;

			if (OnlyFavorites)
				conditions &= movie.IsFavorite;
			if (OnlyMustWatch)
				conditions &= movie.IsMustWatch;
			if (OnlyWatched)
				conditions &= movie.IsWatched;
			if (OnlyTop250)
				conditions &= movie.Rank != null && movie.Rank > 0;
			if (!string.IsNullOrEmpty(SuggestedTitleFilter))
			{
				var titleContainsFilter = movie.Title.Contains(SuggestedTitleFilter);
				var descriptionContainsFilter = false;
				if (movie.Description != null)
					descriptionContainsFilter = movie.Description.Contains(SuggestedTitleFilter);
				conditions &= titleContainsFilter || descriptionContainsFilter;
			}

			return conditions;
		}

		private void SortAlphabetically()
		{
			if (moviesCollectionView.SortDescriptions.Count == 0)
				moviesCollectionView.SortDescriptions.Add(new SortDescription(nameof(SelectedMovie.Title), ListSortDirection.Ascending));
			else
				moviesCollectionView.SortDescriptions.Clear();
		}

		private void RefreshFilter()
		{
			moviesCollectionView.Refresh();
			NotifyStatus("Filtered movies: " + moviesCollectionView.Count());
		}

		#endregion Filtering and Sorting Methods

		#region Events

#pragma warning disable IDE0051 // Quitar miembros privados no utilizados

		private void Movies_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			NotifyItemTotal(Movies.Count);
		}

		private void OnOnlyTop250Changed()
		{
			RefreshFilter();
		}

		private void OnOnlyMustWatchChanged()
		{
			RefreshFilter();
		}

		private void OnOnlyWatchedChanged()
		{
			RefreshFilter();
		}

		private void OnOnlyFavoritesChanged()
		{
			RefreshFilter();
		}

		private void OnSelectedMovieChanged()
		{
			ShowSelectedMovieInfoAsync().FireAndForgetSafeAsync(NotificationsHandler);
		}

		public async Task ShowSelectedMovieInfoAsync(bool forceUpdate = false)
		{
			if (SelectedMovie == null) return;

			var response = await ExecuteCommandTaskAsync(() => moviesService.GetMovieWithId(SelectedMovie.Id, forceUpdate), "");
			if (!response.HasError)
			{
				MovieDetailsPanel.SelectedMovie = MapMovie(response.Movie);
				NotifyStatus("Loaded info for movie " + SelectedMovie.Title);
			}
		}

#pragma warning restore IDE0051 // Quitar miembros privados no utilizados

		#endregion Events

		#region Rendering

		private void ResetAndUpdateMovies(IEnumerable<Movie> movies)
		{
			Movies.Clear();
			if (movies == null) return;

			movies = movies.OrderByDescending(x => x.Rank != null).ThenBy(x => x.Rank);

			var moviesVm = mapper.Map<IEnumerable<MovieViewModel>>(movies);

			foreach (var item in moviesVm)
			{
				Movies.Add(item);
			}
		}

		public void HandleError(string errorMessage)
		{
			Debug.Write(errorMessage);
			NotifyError(errorMessage);
		}

		private MovieViewModel MapMovie(Movie movie)
		{
			var movieVm = mapper.Map<MovieViewModel>(movie);
			return movieVm;
		}

		#endregion Rendering
	}
}