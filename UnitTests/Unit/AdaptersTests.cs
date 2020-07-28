using EntryPoint.Mapper;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using Organizers.Common;
using Organizers.Common.Config;
using Organizers.MovOrg.Adapters.Sections;
using Organizers.MovOrg.Domain;
using Organizers.MovOrg.UseCases;
using Organizers.MovOrg.UseCases.Responses;

using System;
using System.Collections.Generic;

namespace Tests.Unit
{
	[TestClass]
	public class AdaptersTests
	{
		private MoviesSectionViewModel moviesSectionViewModel;
		private Mock<IMoviesService> mockMoviesService;
		private Mock<IDispatcher> mockDispatcher;
		private IAutoMapper autoMapper;

		public AdaptersTests()
		{
			mockMoviesService = new Mock<IMoviesService>();
			var response = new GetAllMoviesFromLocalResponse(new List<Movie>() { new Movie { Id = "TestMovie", Title = "Test" }, new Movie { Id = "TestMovie2", Title = "TestMovie2" } });
			//Must setup for constructor in MoviesSection
			mockMoviesService.Setup(x => x.GetAllMoviesFromLocal()).ReturnsAsync(response);
			mockDispatcher = new Mock<IDispatcher>();
			mockDispatcher.Setup(x => x.BeginInvoke(It.IsAny<Action>())).Callback<Action>(a => a.Invoke());
			autoMapper = new MapperImpl();
			moviesSectionViewModel = new MoviesSectionViewModel(mockMoviesService.Object, autoMapper, mockDispatcher.Object);
		}

		[TestMethod]
		public void SearchCommand_ShouldAddMoviesWithSameTitle_WhenExecuted()
		{
			var response = new GetSuggestedTitleMoviesResponse(new List<Movie>() { new Movie { Id = "TestMovie", Title = "Test" }, new Movie { Id = "NotTestMovie", Title = "NotValid" } });
			var searchWord = "Test";
			mockMoviesService.Setup(x => x.GetMoviesFromSuggestedTitleAsync(searchWord, false)).ReturnsAsync(response);
			moviesSectionViewModel.SuggestedTitle = searchWord;
			moviesSectionViewModel.SearchCommand.Execute("");

			Assert.AreEqual(2, moviesSectionViewModel.Movies.Count);
			Assert.AreEqual("TestMovie", moviesSectionViewModel.Movies[0].Id);
		}

		[TestMethod]
		public void ClearSearchCommand_ShouldShowMoviesFromLocal_WhenExecuted()
		{
			//TODO: make sure async is responsive
			var response = new GetAllMoviesFromLocalResponse(new List<Movie>() { new Movie { Id = "TestMovie", Title = "Test" }, new Movie { Id = "TestMovie2", Title = "TestMovie2" } });
			mockMoviesService.Setup(x => x.GetAllMoviesFromLocal()).ReturnsAsync(response);

			moviesSectionViewModel.ClearSearchCommand.Execute("");

			Assert.AreEqual(2, moviesSectionViewModel.Movies.Count);
			Assert.AreEqual("TestMovie", moviesSectionViewModel.Movies[0].Id);
		}

		[TestMethod]
		public void Top250Command_ShouldUpdateMoviesAndShowAllFromLocal_WhenExecuted()
		{
			var movies = new List<Movie>() { new Movie { Id = "TestMovie", Title = "Test" }, new Movie { Id = "TestMovie2", Title = "TestMovie2" } };
			var updateResponse = new UpdateTopMoviesResponse();
			var localResponse = new GetAllMoviesFromLocalResponse(movies);
			mockMoviesService.Setup(x => x.UpdateTopMovies()).Callback(() => movies[0].Rank = 2).ReturnsAsync(updateResponse);
			mockMoviesService.Setup(x => x.GetAllMoviesFromLocal()).ReturnsAsync(localResponse);

			moviesSectionViewModel.Top250Command.Execute("");

			Assert.AreEqual(2, moviesSectionViewModel.Movies.Count);
			Assert.AreEqual("TestMovie", moviesSectionViewModel.Movies[0].Id);
			Assert.AreEqual(2, moviesSectionViewModel.Movies[0].Rank);
		}
	}
}