using Organizers.MovOrg.Domain;

using System;

namespace Tests.Common
{
	public class TestData
	{
		protected readonly Movie testMovieSeeded = new Movie
		{
			Id = "TestMovie",
			Title = "TestTitle"
		};

		protected readonly Movie testMovieSeededWithDetails = new Movie
		{
			Id = "TestMovieWithDetails",
			Title = "TestTitle"
		};

		protected readonly Movie testMovieNotSeeded = new Movie
		{
			Id = "TestMovie2",
			Title = "TestTitle2"
		};

		protected readonly Director testDirectorSeeded = new Director
		{
			Id = "TestDirector",
			Name = "TestName"
		};

		protected readonly Director testDirectorNotSeeded = new Director
		{
			Id = "TestDirector2",
			Name = "TestName2"
		};

		protected readonly Actor testActorSeeded = new Actor
		{
			Id = "TestActor",
			Name = "TestName"
		};

		protected readonly Actor testActorNotSeeded = new Actor
		{
			Id = "TestActor2",
			Name = "TestName2"
		};

		protected readonly MovieDirector testMovieDirectorSeeded;

		protected readonly MovieActor testMovieActorSeeded;

		public TestData()
		{
			testMovieDirectorSeeded = new MovieDirector
			{
				Director = testDirectorSeeded,
				DirectorId = testDirectorSeeded.Id,
				Movie = testMovieSeededWithDetails,
				MovieId = testMovieSeededWithDetails.Id
			};

			testMovieActorSeeded = new MovieActor
			{
				Actor = testActorSeeded,
				ActorId = testActorSeeded.Id,
				Movie = testMovieSeededWithDetails,
				MovieId = testMovieSeededWithDetails.Id
			};
			testMovieSeededWithDetails.DirectorList.Add(testMovieDirectorSeeded);
			testMovieSeededWithDetails.ActorList.Add(testMovieActorSeeded);
			testMovieSeededWithDetails.LastUpdatedDetails = DateTime.Now;
		}
	}
}