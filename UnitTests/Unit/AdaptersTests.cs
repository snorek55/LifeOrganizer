using AutoFixture;

using EntryPoint.Mapper;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using Organizers.Common;
using Organizers.Common.Config;
using Organizers.MovOrg.Adapters.Items;
using Organizers.MovOrg.Adapters.Sections;
using Organizers.MovOrg.Domain;
using Organizers.MovOrg.UseCases;
using Organizers.MovOrg.UseCases.Responses;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;

namespace Tests.Unit
{
	[TestClass]
	public class AdaptersTests
	{
		private MoviesSectionViewModel moviesSectionViewModel;
		private Mock<IMoviesService> mockMoviesService = new Mock<IMoviesService>();
		private Mock<IDispatcher> mockDispatcher = new Mock<IDispatcher>();
		private IAutoMapper autoMapper = new MapperImpl();
		private Fixture fixture = new Fixture();

		private List<Movie> moviesInLocal;
		private List<MovieViewModel> MoviesVMInLocal => autoMapper.Map<List<MovieViewModel>>(moviesInLocal);

		private GetAllMoviesFromLocalResponse response;

		//Must setup for constructor in MoviesSection
		public AdaptersTests()
		{
			mockDispatcher.Setup(x => x.BeginInvoke(It.IsAny<Action>())).Callback<Action>(a => a.Invoke());
		}

		[TestInitialize]
		public void Initialize()
		{
			fixture = new Fixture();
			fixture.Behaviors.Add(new OmitOnRecursionBehavior());
			response = fixture.Create<GetAllMoviesFromLocalResponse>();
			moviesInLocal = response.Movies.ToList();
			mockMoviesService.Setup(x => x.GetAllMoviesFromLocal()).ReturnsAsync(response);
			moviesSectionViewModel = new MoviesSectionViewModel(mockMoviesService.Object, autoMapper, mockDispatcher.Object);
		}

		#region SearchCommandTests

		[TestMethod]
		public void SearchCommand_ShouldAddMoviesWithSameTitle_WhenTheyHaveNotBeenAlreadySearched()
		{
			var response = fixture.Build<GetSuggestedTitleMoviesResponse>().With(x => x.AlreadySearched, false).Create();

			var searchWord = fixture.Create<string>();
			mockMoviesService.Setup(x => x.GetMoviesFromSuggestedTitleAsync(searchWord, false)).ReturnsAsync(response);
			moviesSectionViewModel.SuggestedTitle = searchWord;
			moviesSectionViewModel.Movies.Should().BeEquivalentTo(MoviesVMInLocal);
			moviesSectionViewModel.SearchCommand.Execute("");

			var expectedVms = autoMapper.Map<List<MovieViewModel>>(response.Movies);
			moviesSectionViewModel.Movies.Should().BeEquivalentTo(expectedVms);
		}

		[TestMethod]
		public void SearchCommand_ShouldFilterMoviesWithSameTitle_WhenAlreadySearched()
		{
			var response = fixture.Build<GetSuggestedTitleMoviesResponse>()
				.With(x => x.AlreadySearched, true)
				.With(x => x.Movies, moviesInLocal)
				.Create();

			fixture.Register(() => "a5");
			var searchWord = fixture.Create<string>();
			mockMoviesService.Setup(x => x.GetMoviesFromSuggestedTitleAsync(searchWord, false)).ReturnsAsync(response);
			moviesSectionViewModel.SuggestedTitle = searchWord;
			moviesSectionViewModel.Movies.Should().BeEquivalentTo(MoviesVMInLocal);
			moviesSectionViewModel.SearchCommand.Execute("");

			var moviesWithCorrectTitle = response.Movies.ToList().FindAll(x => x.Title.Contains(searchWord) || x.Description.Contains(searchWord));
			var expectedVms = autoMapper.Map<List<MovieViewModel>>(moviesWithCorrectTitle);

			var collectionView = CollectionViewSource.GetDefaultView(moviesSectionViewModel.Movies);

			collectionView.Should().HaveSameCount(expectedVms);
			collectionView.Should().BeEquivalentTo(expectedVms);
		}

		[TestMethod]
		public void SearchCommand_ShouldReturnNoMovies_WhenTitleDoesNotExist_AndIsNotAlreadySearched()
		{
			var response = fixture.Build<GetSuggestedTitleMoviesResponse>()
				.With(x => x.Movies, new List<Movie>())
				.With(x => x.AlreadySearched, false)
				.Create();
			var searchWord = fixture.Create<string>();
			mockMoviesService.Setup(x => x.GetMoviesFromSuggestedTitleAsync(searchWord, false)).ReturnsAsync(response);
			moviesSectionViewModel.SuggestedTitle = searchWord;
			moviesSectionViewModel.Movies.Should().BeEquivalentTo(MoviesVMInLocal);
			moviesSectionViewModel.SearchCommand.Execute("");

			moviesSectionViewModel.Movies.Should().BeEmpty();
		}

		#endregion SearchCommandTests

		#region ClearSearchCommandTests

		[TestMethod]
		public void ClearSearchCommand_ShouldShowMoviesFromLocal_WhenExecuted()
		{
			var response = new GetAllMoviesFromLocalResponse(moviesInLocal);
			mockMoviesService.Setup(x => x.GetAllMoviesFromLocal()).ReturnsAsync(response);

			moviesSectionViewModel.Movies.Clear();

			moviesSectionViewModel.Movies.Should().BeEmpty();

			moviesSectionViewModel.ClearSearchCommand.Execute("");

			moviesSectionViewModel.Movies.Should().BeEquivalentTo(MoviesVMInLocal);
		}

		#endregion ClearSearchCommandTests

		#region Top250CommandTests

		[TestMethod]
		public void Top250Command_ShouldUpdateMoviesAndShowAllFromLocal_WhenExecuted()
		{
			var updateResponse = new UpdateTopMoviesResponse();
			mockMoviesService.Setup(x => x.UpdateTopMovies()).Callback(() => response.Movies.First().Rank = 2).ReturnsAsync(updateResponse);

			moviesSectionViewModel.Movies.Should().BeEquivalentTo(MoviesVMInLocal);
			moviesSectionViewModel.Movies[0].Rank.Should().NotBe(2);
			moviesSectionViewModel.Top250Command.Execute("");

			moviesSectionViewModel.Movies.Should().BeEquivalentTo(MoviesVMInLocal);
			moviesSectionViewModel.Movies[0].Rank.Should().Be(2);
		}

		#endregion Top250CommandTests
	}
}