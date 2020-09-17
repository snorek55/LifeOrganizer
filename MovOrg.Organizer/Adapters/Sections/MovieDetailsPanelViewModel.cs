using Common.Adapters;
using Common.Extensions;
using Common.Setup;

using MovOrg.Organizer.Adapters.Items;
using MovOrg.Organizer.UseCases;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MovOrg.Organizer.Adapters.Sections
{
	public class MovieDetailsPanelViewModel : BaseViewModel
	{
		public MovieViewModel SelectedMovie { get; set; }

		public ImagePresenterViewModel ImagePresenter { get; } = new ImagePresenterViewModel();

		public bool AreImagesShowing { get; set; }

		public ICommand UpdateMovieCommand { get => new AsyncCommand(UpdateCurrentMovieAsync, parent.NotificationsHandler); }

		public ICommand ShowImagesCommand { get => new AsyncCommand(ShowImages, parent.NotificationsHandler); }
		public ICommand WikipediaCommand { get => new SyncCommand(GoToWikipedia); }
		public ICommand IMDbCommand { get => new SyncCommand(GoToIMDbPage); }
		public ICommand TrailerCommand { get => new SyncCommand(ShowTrailer); }
		public ICommand IsFavoriteCommand { get => new AsyncCommand(UpdateFavoriteStatus); }
		public ICommand IsMustWatchCommand { get => new AsyncCommand(UpdateMustWatch); }
		public ICommand IsWatchedCommand { get => new AsyncCommand(UpdateWatched); }

		public ICommand GoToMoviesWebPage { get => new AsyncCommand(GotToMoviesWebPage); }

		private MoviesSectionViewModel parent;

		private IMoviesService service;
		private IAutoMapper mapper;

		public MovieDetailsPanelViewModel(IMoviesService service, MoviesSectionViewModel parent, IAutoMapper mapper)
		{
			this.parent = parent;
			this.service = service;
			this.mapper = mapper;
			ImagePresenter.RequestedExit += HideImages;
		}

		private async Task UpdateWatched()
		{
			//TODO: must add interceptor
			var response = await service.UpdateWatched(SelectedMovie.Id, SelectedMovie.IsWatched);
			if (response.HasError)
				parent.HandleError(response.Error);
			parent.SelectedMovie.IsWatched = SelectedMovie.IsWatched;
		}

		private async Task UpdateMustWatch()
		{
			var response = await service.UpdateMustWatch(SelectedMovie.Id, SelectedMovie.IsMustWatch);
			if (response.HasError)
				parent.HandleError(response.Error);

			parent.SelectedMovie.IsMustWatch = SelectedMovie.IsMustWatch;
		}

		private async Task UpdateFavoriteStatus()
		{
			var response = await service.UpdateFavoriteStatus(SelectedMovie.Id, SelectedMovie.IsFavorite);
			if (response.HasError)
				parent.HandleError(response.Error);

			parent.SelectedMovie.IsFavorite = SelectedMovie.IsFavorite;
		}

		private async Task ShowImages()
		{
			ImagePresenter.Images.Clear();
			ImagePresenter.Images.Add(SelectedMovie.CoverImage);
			var imageResponse = await service.GetMovieImagesById(SelectedMovie.Id);
			if (!imageResponse.HasError)
			{
				var imagesVM = mapper.Map<IEnumerable<ImageViewModel>>(imageResponse.Data);
				ImagePresenter.Images.AddRange(imagesVM);
				ImagePresenter.CurrentImage = ImagePresenter.Images.First();
				AreImagesShowing = true;
			}
			else
			{
				parent.NotificationsHandler.NotifyError(imageResponse.Error);
			}
		}

		private void HideImages(object sender, EventArgs e)
		{
			AreImagesShowing = false;
		}

		private async Task UpdateCurrentMovieAsync()
		{
			await parent.ShowSelectedMovieInfoAsync(true);
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

		private async Task GotToMoviesWebPage()
		{
			if (SelectedMovie.SelectedRating == null)
				return;

			var response = await service.GetRatingSourceUrl(SelectedMovie.Id, SelectedMovie.SelectedRating.SourceName);
			if (response.Data.SourceUrl == null)
			{
				parent.NotificationsHandler.NotifyStatus("Url no existe");
				return;
			}

			Process.Start("explorer.exe", response.Data.SourceUrl);
		}
	}
}