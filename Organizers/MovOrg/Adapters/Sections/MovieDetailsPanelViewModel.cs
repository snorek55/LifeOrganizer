using Organizers.Common.Adapters;
using Organizers.MovOrg.Adapters.Items;
using Organizers.MovOrg.UseCases;

using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Organizers.MovOrg.Adapters.Sections
{
	public class MovieDetailsPanelViewModel : BaseViewModel
	{
		//TODO: style. All commands must be create on get method to make sure notifications handler is updated
		public MovieViewModel SelectedMovie { get; set; }

		public ICommand UpdateMovieCommand { get => new AsyncCommand(UpdateCurrentMovieAsync, parent.NotificationsHandler); }
		public ICommand WikipediaCommand { get => new SyncCommand(GoToWikipedia); }
		public ICommand IMDbCommand { get => new SyncCommand(GoToIMDbPage); }
		public ICommand TrailerCommand { get => new SyncCommand(ShowTrailer); }
		public ICommand IsFavoriteCommand { get => new SyncCommand(UpdateFavoriteStatus); }
		public ICommand IsMustWatchCommand { get => new SyncCommand(UpdateMustWatch); }
		public ICommand IsWatchedCommand { get => new SyncCommand(UpdateWatched); }

		private MoviesSectionViewModel parent;

		private IMoviesService service;

		public MovieDetailsPanelViewModel(IMoviesService service, MoviesSectionViewModel parent)
		{
			this.parent = parent;
			this.service = service;
		}

		private void UpdateWatched()
		{
			//TODO: must add interceptor
			var response = service.UpdateWatched(SelectedMovie.Id, SelectedMovie.IsWatched);
			if (response.HasError)
				parent.HandleError(response.Error);
			parent.SelectedMovie.IsWatched = SelectedMovie.IsWatched;
		}

		private void UpdateMustWatch()
		{
			var response = service.UpdateMustWatch(SelectedMovie.Id, SelectedMovie.IsMustWatch);
			if (response.HasError)
				parent.HandleError(response.Error);

			parent.SelectedMovie.IsMustWatch = SelectedMovie.IsMustWatch;
		}

		private void UpdateFavoriteStatus()
		{
			var response = service.UpdateFavoriteStatus(SelectedMovie.Id, SelectedMovie.IsFavorite);
			if (response.HasError)
				parent.HandleError(response.Error);

			parent.SelectedMovie.IsFavorite = SelectedMovie.IsFavorite;
		}

		private void GoToWikipedia()
		{
			Process.Start("explorer.exe", SelectedMovie.WikipediaUrl);
		}

		private void ShowTrailer()
		{
			Process.Start("explorer.exe", SelectedMovie.Trailer.LinkEmbed);
		}

		private void GoToIMDbPage()
		{
			Process.Start("explorer.exe", "https://www.imdb.com/title/" + SelectedMovie.Id + "/");
		}

		private async Task UpdateCurrentMovieAsync()
		{
			await parent.ShowSelectedMovieInfoAsync(true);
		}
	}
}