﻿using Common.Adapters;
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
		public ICommand WikipediaCommand { get => new SyncCommand(GoToWikipedia); }
		public ICommand IMDbCommand { get => new SyncCommand(GoToIMDbPage); }
		public ICommand TrailerCommand { get => new SyncCommand(ShowTrailer); }
		public ICommand IsFavoriteCommand { get => new SyncCommand(UpdateFavoriteStatus); }
		public ICommand IsMustWatchCommand { get => new SyncCommand(UpdateMustWatch); }
		public ICommand IsWatchedCommand { get => new SyncCommand(UpdateWatched); }

		public ICommand ShowImagesCommand { get => new SyncCommand(ShowImages); }

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
			ImagePresenter.Images.Clear();
			ImagePresenter.Images.Add(SelectedMovie.CoverImage);
			var imageResponse = service.GetMovieImagesById(SelectedMovie.Id);
			if (!imageResponse.HasError)
			{
				var imagesVM = mapper.Map<IEnumerable<ImageViewModel>>(imageResponse.MovieImages);
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
	}
}