using AutoFixture;

using Common.Adapters;
using Common.Extensions;

using FluentAssertions;
using FluentAssertions.Common;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using MovOrg.Organizer.Adapters.Items;
using MovOrg.Organizer.Adapters.Sections;
using MovOrg.Organizer.Domain;
using MovOrg.Organizer.UseCases;
using MovOrg.Organizer.UseCases.DTOs;
using MovOrg.Organizer.UseCases.Responses;
using MovOrg.Tests.Setup;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;

namespace MovOrg.Tests.Unit.Adapters
{
	[TestClass]
	public class MoviesSectionTests
	{
		private MoviesSectionViewModel moviesSectionViewModel;
		private Mock<IMoviesService> mockMoviesService = new Mock<IMoviesService>();
		private Mock<IDispatcher> mockDispatcher = new Mock<IDispatcher>();
		private Fixture fixture = new Fixture();

		private List<Movie> moviesInLocal;
		private List<MovieListItemDto> MoviesListItemDtos => TestData.Mapper.Map<List<MovieListItemDto>>(moviesInLocal);

		private GetAllMoviesFromLocalResponse response;

		public MoviesSectionTests()
		{
			mockDispatcher.Setup(x => x.BeginInvoke(It.IsAny<Action>())).Callback<Action>(a => a.Invoke());
		}

		[TestInitialize]
		public void Initialize()
		{
			fixture = new Fixture();
			fixture.Behaviors.Add(new OmitOnRecursionBehavior());
			var movies = fixture.CreateMany<Movie>().ToList();
			movies[0].Rank = 0;
			foreach (var movie in movies)
			{
				movie.IsMustWatch = false;
				movie.IsFavorite = false;
				movie.IsWatched = false;
			}

			response = new GetAllMoviesFromLocalResponse(TestData.Mapper.Map<List<MovieListItemDto>>(movies));
			moviesInLocal = movies;
			mockMoviesService.Setup(x => x.GetAllMoviesFromLocal()).ReturnsAsync(response);
			moviesSectionViewModel = new MoviesSectionViewModel(mockMoviesService.Object, TestData.Mapper, mockDispatcher.Object);
		}

		#region SearchCommandTests

		//TODO: bad testing. must create a list without movie info (movilistitemdto) and compare
		[TestMethod]
		public void SearchCommand_ShouldAddMoviesWithSameTitle_WhenTheyHaveNotBeenAlreadySearched()
		{
			var response = fixture.Build<GetSuggestedTitleMoviesResponse>()
				.With(x => x.AlreadySearched, false)
				.With(x => x.Error, string.Empty)
				.Create();

			var searchWord = fixture.Create<string>();
			mockMoviesService.Setup(x => x.GetMoviesFromSuggestedTitleAsync(searchWord, false)).ReturnsAsync(response);
			moviesSectionViewModel.SuggestedTitle = searchWord;
			var currentList = TestData.Mapper.Map<List<MovieListItemDto>>(moviesSectionViewModel.Movies);
			currentList.Should().BeEquivalentTo(MoviesListItemDtos);
			moviesSectionViewModel.SearchCommand.Execute("");

			var expectedVms = TestData.Mapper.Map<List<MovieViewModel>>(response.Movies);
			moviesSectionViewModel.Movies.Should().BeEquivalentTo(expectedVms);
		}

		[TestMethod]
		public void SearchCommand_ShouldFilterMoviesWithSameTitle_WhenAlreadySearched()
		{
			var response = fixture.Build<GetSuggestedTitleMoviesResponse>()
				.With(x => x.AlreadySearched, true)
				.With(x => x.Movies, moviesInLocal)
				.With(x => x.Error, string.Empty)
				.Create();

			fixture.Register(() => "a5");
			var searchWord = fixture.Create<string>();
			mockMoviesService.Setup(x => x.GetMoviesFromSuggestedTitleAsync(searchWord, false)).ReturnsAsync(response);
			moviesSectionViewModel.SuggestedTitle = searchWord;
			var currentList = TestData.Mapper.Map<List<MovieListItemDto>>(moviesSectionViewModel.Movies);
			currentList.Should().BeEquivalentTo(MoviesListItemDtos);
			moviesSectionViewModel.SearchCommand.Execute("");
			//TODO: improve search engine
			var moviesWithCorrectTitle = response.Movies.ToList().FindAll(x => x.Title.Contains(searchWord));
			var expectedDtos = TestData.Mapper.Map<List<MovieListItemDto>>(moviesWithCorrectTitle);
			var expectedVms = TestData.Mapper.Map<List<MovieViewModel>>(expectedDtos);
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
			moviesSectionViewModel.SuggestedTitle = searchWord; var currentList = TestData.Mapper.Map<List<MovieListItemDto>>(moviesSectionViewModel.Movies);
			currentList.Should().BeEquivalentTo(MoviesListItemDtos); moviesSectionViewModel.SearchCommand.Execute("");

			moviesSectionViewModel.Movies.Should().BeEmpty();
		}

		#endregion SearchCommandTests

		#region ClearSearchCommandTests

		[TestMethod]
		public void ClearSearchCommand_ShouldShowMoviesFromLocal_WhenExecuted()
		{
			var expectedList = TestData.Mapper.Map<List<MovieListItemDto>>(moviesInLocal);
			var response = new GetAllMoviesFromLocalResponse(expectedList);
			mockMoviesService.Setup(x => x.GetAllMoviesFromLocal()).ReturnsAsync(response);

			moviesSectionViewModel.Movies.Clear();

			moviesSectionViewModel.Movies.Should().BeEmpty();

			moviesSectionViewModel.ClearSearchCommand.Execute("");

			var actualList = TestData.Mapper.Map<List<MovieListItemDto>>(moviesSectionViewModel.Movies);
			actualList.Should().BeEquivalentTo(expectedList);
		}

		#endregion ClearSearchCommandTests

		#region Top250CommandTests

		[TestMethod]
		public void Top250Command_ShouldUpdateMoviesAndShowAllFromLocal_WhenExecuted()
		{
			//TODO: response movie must be the same object as moviesinlocal for this to workout. must change
			var updateResponse = new UpdateTopMoviesResponse();
			mockMoviesService.Setup(x => x.UpdateTopMovies()).Callback(() => response.Movies.First().Rank = 2).ReturnsAsync(updateResponse);

			var currentList = TestData.Mapper.Map<List<MovieListItemDto>>(moviesSectionViewModel.Movies);
			currentList.Should().BeEquivalentTo(MoviesListItemDtos);
			moviesSectionViewModel.Movies[0].Rank.Should().NotBe(2);
			moviesSectionViewModel.Top250Command.Execute("");

			//var actualList = autoMapper.Map<List<MovieListItemDto>>(moviesSectionViewModel.Movies);
			//actualList.Should().BeEquivalentTo(MoviesListItemDtos);
			moviesSectionViewModel.Movies[0].Rank.Should().Be(2);
		}

		#endregion Top250CommandTests

		[TestMethod]
		public void WhenSelectingMovie_InfoIsShownInMoveDetails()
		{
			var movies = fixture.CreateMany<MovieViewModel>();
			moviesSectionViewModel.Movies.Clear();
			moviesSectionViewModel.Movies.AddRange(movies);

			moviesSectionViewModel.SelectedMovie = moviesSectionViewModel.Movies[0];

			moviesSectionViewModel.MovieDetailsPanel.SelectedMovie.IsSameOrEqualTo(moviesSectionViewModel.Movies[0]);
		}

		[TestMethod]
		public void WhenSortingAlphabetically_SortsOK()
		{
			moviesSectionViewModel.Movies[0].Title = "zzzz";
			var expectedCollection = moviesSectionViewModel.Movies.OrderBy(x => x.Title);

			moviesSectionViewModel.SortAlphabeticallyCommand.Execute("");

			var collectionView = CollectionViewSource.GetDefaultView(moviesSectionViewModel.Movies);
			var actualCollection = collectionView.GetFilteredData<MovieViewModel>();

			actualCollection.Should().HaveSameCount(expectedCollection);
			actualCollection.Should().BeInAscendingOrder(x => x.Title);
		}

		[TestMethod]
		public void WhenMarkedMustWatchOnly_OnlyMustWatchMoviesAreShown()
		{
			moviesSectionViewModel.Movies[0].IsMustWatch = true;

			moviesSectionViewModel.OnlyMustWatch = true;

			var collectionView = CollectionViewSource.GetDefaultView(moviesSectionViewModel.Movies);
			var actualCollection = collectionView.GetFilteredData<MovieViewModel>();

			actualCollection.Should().HaveCount(1);
			actualCollection.Should().Contain(moviesSectionViewModel.Movies[0]);
		}

		[TestMethod]
		public void WhenMarkedFavoritesOnly_OnlyFavoritesMoviesAreShown()
		{
			moviesSectionViewModel.Movies[0].IsFavorite = true;

			moviesSectionViewModel.OnlyFavorites = true;

			var collectionView = CollectionViewSource.GetDefaultView(moviesSectionViewModel.Movies);
			var actualCollection = collectionView.GetFilteredData<MovieViewModel>();

			actualCollection.Should().HaveCount(1);
			actualCollection.Should().Contain(moviesSectionViewModel.Movies[0]);
		}

		[TestMethod]
		public void WhenMarkedWatchedOnly_OnlyWatchedMoviesAreShown()
		{
			moviesSectionViewModel.Movies[0].IsWatched = true;

			moviesSectionViewModel.OnlyWatched = true;

			var collectionView = CollectionViewSource.GetDefaultView(moviesSectionViewModel.Movies);
			var actualCollection = collectionView.GetFilteredData<MovieViewModel>();

			actualCollection.Should().HaveCount(1);
			actualCollection.Should().Contain(moviesSectionViewModel.Movies[0]);
		}
	}
}