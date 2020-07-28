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
		public MovieViewModel SelectedMovie { get; set; }

		public ICommand UpdateMovieCommand { get; private set; }
		public ICommand WikipediaCommand { get; private set; }
		public ICommand IMDbCommand { get; private set; }
		public ICommand TrailerCommand { get; private set; }
		public ICommand IsFavoriteCommand { get; private set; }
		public ICommand IsMustWatchCommand { get; private set; }
		public ICommand IsWatchedCommand { get; private set; }

		private MoviesSectionViewModel parent;

		private IMoviesService service;

		public MovieDetailsPanelViewModel(IMoviesService service, MoviesSectionViewModel parent)
		{
			this.parent = parent;
			this.service = service;
			UpdateMovieCommand = new AsyncCommand(UpdateCurrentMovieAsync, parent.NotificationsHandler);
			WikipediaCommand = new SyncCommand(GoToWikipedia);
			IMDbCommand = new SyncCommand(GoToIMDbPage);
			TrailerCommand = new SyncCommand(ShowTrailer);
			IsFavoriteCommand = new SyncCommand(UpdateFavoriteStatus);
			IsMustWatchCommand = new SyncCommand(UpdateMustWatch);
			IsWatchedCommand = new SyncCommand(UpdateWatched);
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
			Process.Start("explorer.exe", SelectedMovie.TrailerLinkEmbed);
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