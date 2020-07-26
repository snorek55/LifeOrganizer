using Organizers.Common;
using Organizers.Common.Adapters;
using Organizers.Common.Config;
using Organizers.Main.Adapters.Sections;
using Organizers.MovOrg.Adapters.Items;
using Organizers.MovOrg.Domain;
using Organizers.MovOrg.UseCases;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace Organizers.MovOrg.Adapters.Sections
{
	public class MoviesSectionViewModel : SectionViewModel
	{
		public ObservableCollection<MovieViewModel> Movies { get; set; } = new ObservableCollection<MovieViewModel>();

		public MovieViewModel SelectedMovie { get; set; }

		public MovieDetailsPanelViewModel MovieDetailsPanel { get; private set; }

		#region Commands

		public ICommand SearchCommand { get; }
		public ICommand ClearSearchCommand { get; }
		public ICommand Top250Command { get; }

		public ICommand SortAlphabeticallyCommand { get; }

		#endregion Commands

		#region Filtering Properties

		public bool OnlyFavorites { get; set; }

		public bool OnlyMustWatch { get; set; }

		public bool OnlyWatched { get; set; }

		public bool OnlyTop250 { get; set; }

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

		public MoviesSectionViewModel(IMoviesService moviesService, IAutoMapper autoMapper, IDispatcher dispatcher)
		{
			this.mapper = autoMapper;
			this.dispatcher = dispatcher;
			SectionName = "Movies";
			this.moviesService = moviesService;

			MovieDetailsPanel = new MovieDetailsPanelViewModel(moviesService, this);

			SearchCommand = new AsyncCommand(SearchMoviesWithChosenTitleAsync, NotificationsHandler);
			ClearSearchCommand = new AsyncCommand(ClearSearchAsync, NotificationsHandler);
			Top250Command = new AsyncCommand(SearchTop250MoviesAsync, NotificationsHandler);
			SortAlphabeticallyCommand = new SyncCommand(SortAlphabetically);
			moviesCollectionView = CollectionViewSource.GetDefaultView(Movies);
			moviesCollectionView.Filter = new Predicate<object>(x => MovieFiltering(x as MovieViewModel));

			this.dispatcher.BeginInvoke(() => GetAllMoviesFromLocal().FireAndForgetSafeAsync(NotificationsHandler));
		}

		private async Task GetAllMoviesFromLocal()
		{
			NotificationsHandler.NotifyWaitMessage();
			var response = await moviesService.GetAllMoviesFromLocal();
			if (response.HasError)
				HandleError(response.Error);
			else
			{
				ResetAndUpdateMovies(response.Movies);
			}

			NotificationsHandler.NotifyStatusMessage("Loaded movies from local");
		}

		#region Command Methods

		private async Task ClearSearchAsync()
		{
			NotificationsHandler.NotifyWaitMessage();
			var localMoviesResponse = await moviesService.GetAllMoviesFromLocal();
			if (localMoviesResponse.HasError)
				HandleError(localMoviesResponse.Error);
			else
				ResetAndUpdateMovies(localMoviesResponse.Movies);

			NotificationsHandler.NotifyStatusMessage("Loaded movies from local");
		}

		private async Task SearchTop250MoviesAsync()
		{
			NotificationsHandler.NotifyWaitMessage();
			var response = await moviesService.UpdateTopMovies();
			//TODO: pasar a sectionviewmodel
			if (response.HasError)
			{
				HandleError(response.Error);
				return;
			}

			var responseAllMovies = await moviesService.GetAllMoviesFromLocal();

			if (responseAllMovies.HasError)
				HandleError(responseAllMovies.Error);
			else
			{
				ResetAndUpdateMovies(responseAllMovies.Movies);
				NotificationsHandler.NotifyStatusMessage("Actualizadas mejores 250 peliculas");
			}
		}

		//TODO: does not work
		private async Task SearchMoviesWithChosenTitleAsync()
		{
			NotificationsHandler.NotifyWaitMessage();
			if (string.IsNullOrEmpty(SuggestedTitle))
			{
				await ClearSearchAsync();
				return;
			}
			var suggestedTitleResponse = await moviesService.GetMoviesFromSuggestedTitleAsync(SuggestedTitle);

			if (suggestedTitleResponse.HasError)
				HandleError(suggestedTitleResponse.Error);
			else
			{
				//TODO: get the movies and then filter out
				ResetAndUpdateMovies(suggestedTitleResponse.Movies);
				NotificationsHandler.NotifyStatusMessage("Obtenidas " + suggestedTitleResponse.Movies.Count() + " peliculas");
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
			NotificationsHandler.NotifyStatusMessage("Peliculas filtradas. Viendo: " + moviesCollectionView.Cast<object>().Count());
		}

		#endregion Filtering and Sorting Methods

		#region Events

#pragma warning disable IDE0051 // Quitar miembros privados no utilizados

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
			NotificationsHandler.NotifyWaitMessage();
			if (SelectedMovie == null) return;

			var response = await moviesService.GetMovieWithId(SelectedMovie.Id, forceUpdate);
			if (response.HasError)
				HandleError(response.Error);
			else
			{
				MovieDetailsPanel.SelectedMovie = mapper.Map<MovieViewModel>(response.Movie);
				NotificationsHandler.NotifyStatusMessage("Obtenida info de película");
			}
		}

#pragma warning restore IDE0051 // Quitar miembros privados no utilizados

		#endregion Events

		#region Rendering

		private void ResetAndUpdateMovies(IEnumerable<Movie> movies)
		{
			movies = movies.OrderByDescending(x => x.Rank != null).ThenBy(x => x.Rank);

			var moviesVm = mapper.Map<IEnumerable<MovieViewModel>>(movies);
			Movies.Clear();
			foreach (var item in moviesVm)
			{
				Movies.Add(item);
			}
		}

		public void HandleError(string errorMessage)
		{
			Debug.Write(errorMessage);
			NotificationsHandler.NotifyError(errorMessage);
		}

		#endregion Rendering
	}
}