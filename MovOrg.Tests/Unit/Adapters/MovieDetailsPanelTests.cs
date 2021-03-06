﻿using AutoFixture;

using Common.Adapters;
using Common.UseCases;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using MovOrg.Organizer.Adapters.Items;
using MovOrg.Organizer.Adapters.Sections;
using MovOrg.Organizer.UseCases;
using MovOrg.Organizer.UseCases.DTOs;
using MovOrg.Organizer.UseCases.Responses;
using MovOrg.Tests.Setup;

using System.Collections.Generic;
using System.Linq;

namespace MovOrg.Tests.Unit.Adapters
{
	[TestClass]
	public class MovieDetailsPanelTests
	{
		private MovieDetailsPanelViewModel movieDetailsPanel;
		private Mock<IMoviesService> mockMoviesService = new Mock<IMoviesService>();
		private MoviesSectionViewModel moviesSection = new MoviesSectionViewModel();
		private Fixture fixture = new Fixture();

		[TestInitialize]
		public void Initialize()
		{
			fixture = new Fixture();
			fixture.Behaviors.Add(new OmitOnRecursionBehavior());
			movieDetailsPanel = new MovieDetailsPanelViewModel(mockMoviesService.Object, moviesSection, TestData.Mapper);
			movieDetailsPanel.SelectedMovie = fixture.Create<MovieViewModel>();
		}

		[TestMethod]
		public void WhenMarkedMustWatch_MovieIsUpdated()
		{
			movieDetailsPanel.SelectedMovie.IsMustWatch = false;
			moviesSection.SelectedMovie = movieDetailsPanel.SelectedMovie;
			mockMoviesService.Setup(x => x.UpdateMustWatch(movieDetailsPanel.SelectedMovie.Id, false)).Callback(() => movieDetailsPanel.SelectedMovie.IsMustWatch = true).ReturnsAsync(new UpdateMustWatchResponse());

			movieDetailsPanel.IsMustWatchCommand.Execute("");

			movieDetailsPanel.SelectedMovie.IsMustWatch.Should().BeTrue();
			moviesSection.SelectedMovie.IsMustWatch.Should().BeTrue();
		}

		[TestMethod]
		public void WhenMarkedFavorites_MovieIsUpdated()
		{
			movieDetailsPanel.SelectedMovie.IsFavorite = false;
			moviesSection.SelectedMovie = movieDetailsPanel.SelectedMovie;
			mockMoviesService.Setup(x => x.UpdateFavoriteStatus(movieDetailsPanel.SelectedMovie.Id, false)).Callback(() => movieDetailsPanel.SelectedMovie.IsFavorite = true).ReturnsAsync(new UpdateFavoriteResponse());

			movieDetailsPanel.IsFavoriteCommand.Execute("");

			movieDetailsPanel.SelectedMovie.IsFavorite.Should().BeTrue();
			moviesSection.SelectedMovie.IsFavorite.Should().BeTrue();
		}

		[TestMethod]
		public void WhenMarkedWatched_MovieIsUpdated()
		{
			movieDetailsPanel.SelectedMovie.IsWatched = false;
			moviesSection.SelectedMovie = movieDetailsPanel.SelectedMovie;
			var response = new UpdateFavoriteResponse();
			mockMoviesService.Setup(x => x.UpdateWatched(movieDetailsPanel.SelectedMovie.Id, false)).Callback(() => movieDetailsPanel.SelectedMovie.IsWatched = true).ReturnsAsync(new UpdateWatchedResponse());

			movieDetailsPanel.IsWatchedCommand.Execute("");

			movieDetailsPanel.SelectedMovie.IsWatched.Should().BeTrue();
			moviesSection.SelectedMovie.IsWatched.Should().BeTrue();
		}

		[TestMethod]
		public void ShowImages_WorksOk()
		{
			var randomImages = fixture.CreateMany<MovieImageDto>().ToList();
			var response = new DataResponseBase<IEnumerable<MovieImageDto>>(randomImages);
			mockMoviesService.Setup(x => x.GetMovieImagesById(movieDetailsPanel.SelectedMovie.Id)).ReturnsAsync(response);

			movieDetailsPanel.AreImagesShowing = false;
			movieDetailsPanel.ShowImagesCommand.Execute("");

			var expectedImages = TestData.Mapper.Map<List<ImageViewModel>>(randomImages);
			expectedImages.Add(movieDetailsPanel.SelectedMovie.CoverImage);

			movieDetailsPanel.ImagePresenter.Images.Should().BeEquivalentTo(expectedImages);
			movieDetailsPanel.AreImagesShowing.Should().BeTrue();
		}

		[TestMethod]
		public void WhenImagePresenterRequestsExit_ImagesAreHidden()
		{
			movieDetailsPanel.AreImagesShowing = true;

			movieDetailsPanel.ImagePresenter.Exit();

			movieDetailsPanel.AreImagesShowing.Should().BeFalse();
		}
	}
}