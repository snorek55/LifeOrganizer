using Organizers.Common.Adapters;
using Organizers.MovOrg.Adapters.Items;
using Organizers.MovOrg.UseCases;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Organizers.MovOrg.Adapters.Sections
{
	public class MovieDetailsPanelViewModel : BaseViewModel
	{
		public MovieViewModel SelectedMovie { get; set; }

		public ImagePresenterViewModel ImagePresenter { get; } = new ImagePresenterViewModel();

		public bool AreImagesShowing { get; set; }

		public ICommand UpdateMovieCommand { get => new AsyncCommand(UpdateCurrentMovieAsync, parent.NotificationsHandler); }
		public ICommand WikipediaCommand { get => new SyncCommand(GoToWikipedia); }
		public ICommand IMDbCommand { get => new SyncCommand(GoToIMDbPage); }
		public ICommand TrailerCommand { get => new SyncCommand(ShowTrailer); }
		public ICommand IsFavoriteCommand { get => new SyncCommand(UpdateFavoriteStatus); }
		public ICommand IsMustWatchCommand { get => new SyncCommand(UpdateMustWatch); }
		public ICommand IsWatchedCommand { get => new SyncCommand(UpdateWatched); }

		public ICommand ShowImagesCommand { get => new SyncCommand(ShowImages); }

		private MoviesSectionViewModel parent;

		private IMoviesService service;

		public MovieDetailsPanelViewModel(IMoviesService service, MoviesSectionViewModel parent)
		{
			this.parent = parent;
			this.service = service;
			ImagePresenter.RequestedExit += HideImages;
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

		private void ShowImages()
		{
			//TODO: refactor this
			var images = new List<ImageViewModel>();
			foreach (var imageData in SelectedMovie.Images)
			{
				var image = new ImageViewModel
				{
					Image = imageData.Image,
					Title = imageData.Title
				};
				images.Add(image);
			}

			var coverImg = new ImageViewModel
			{
				Image = SelectedMovie.CoverImage,
				Title = "Cover"
			};

			ImagePresenter.Images.Clear();
			ImagePresenter.Images.Add(coverImg);

			foreach (var img in images)
				ImagePresenter.Images.Add(img);

			ImagePresenter.CurrentImage = ImagePresenter.Images[0];
			AreImagesShowing = true;
		}

		private void HideImages(object sender, EventArgs e)
		{
			AreImagesShowing = false;
		}

		private async Task UpdateCurrentMovieAsync()
		{
			await parent.ShowSelectedMovieInfoAsync(true);
		}
	}
}