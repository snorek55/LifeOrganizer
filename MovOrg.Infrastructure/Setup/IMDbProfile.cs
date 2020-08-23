using AutoMapper;

using Common.Extensions;

using IMDbApiLib.Models;

using MovOrg.Organizer.Domain;

using System.Linq;

namespace MovOrg.Infrastructure.Setup
{
	public class IMDbProfile : Profile
	{
		public IMDbProfile()
		{
			CreateMap<TitleData, Movie>()
				.IgnoreAllPropertiesWithAnInaccessibleSetter()
				.IgnoreDestinationMember(d => d.Images)
				.IgnoreDestinationMember(d => d.Description)
				.IgnoreDestinationMember(d => d.LastUpdatedDetails)
				.IgnoreDestinationMember(d => d.LastUpdatedTop250)
				.IgnoreDestinationMember(d => d.Rank)
				.IgnoreDestinationMember(d => d.IsFavorite)
				.IgnoreDestinationMember(d => d.IsWatched)
				.IgnoreDestinationMember(d => d.IsMustWatch)
				.IgnoreDestinationMember(d => d.Ratings)
				.MapFrom(d => d.CoverImageUrl, s => s.Image)
				.AfterMap((s, d) =>
				{
					d.BoxOffice.Movie = d;
					d.BoxOffice.Id = d.Id;
				})
				.AfterMap((s, d) =>
				{
					d.Trailer.Id = d.Id;
					d.Trailer.Movie = d;
				})
				.AfterMap((s, d) =>
				{
					//TODO: refactor this

					int i = 0;
					foreach (var image in s.Images.Items)
					{
						d.Images.Add(new MovieImageData
						{
							Id = (i++).ToString(),
							MovieId = d.Id,
							Movie = d,
							Image = image.Image,
							Title = image.Title
						});
					}

					foreach (var actor in d.ActorList)
					{
						if (s.StarList.Any(x => x.Id == actor.PersonId))
							actor.IsStar = true;

						actor.Movie = d;
						actor.MovieId = d.Id;
					}

					foreach (var director in d.DirectorList)
					{
						director.Movie = d;
						director.MovieId = d.Id;
					}
					foreach (var writer in d.WriterList)
					{
						writer.Movie = d;
						writer.MovieId = d.Id;
					}

					foreach (var company in d.CompanyList)
					{
						company.Movie = d;
						company.MovieId = d.Id;
					}

					foreach (var similar in d.Similars)
					{
						similar.Movie = d;
						similar.MovieId = d.Id;
					}
				});

			CreateMap<SearchResult, Movie>(MemberList.Source)
				.IgnoreSourceMember(s => s.ResultType)
				.MapFrom(d => d.CoverImageUrl, s => s.Image);

			CreateMap<Top250DataDetail, Movie>(MemberList.Source)
				.IgnoreSourceMember(s => s.IMDbRating)
				.IgnoreSourceMember(s => s.FullTitle)
				.IgnoreSourceMember(s => s.Crew)
				.IgnoreSourceMember(s => s.IMDbRatingCount)
				.MapFrom(d => d.CoverImageUrl, s => s.Image);

			CreateMap<ActorShort, MovieActor>(MemberList.None)
				.MapFrom(d => d.Person, s => new Person { Id = s.Id, Name = s.Name, ImageUrl = s.Image })
				.MapFrom(d => d.PersonId, s => s.Id)
				.IgnoreDestinationMember(d => d.Movie)
				.IgnoreDestinationMember(d => d.MovieId)
				.IgnoreDestinationMember(d => d.IsStar);

			CreateMap<StarShort, MovieDirector>(MemberList.None)
				.MapFrom(d => d.Person, s => new Person { Id = s.Id, Name = s.Name })
				.MapFrom(d => d.PersonId, s => s.Id)
				.IgnoreDestinationMember(d => d.Movie);

			CreateMap<StarShort, MovieWriter>(MemberList.None)
				.MapFrom(d => d.Person, s => new Person { Id = s.Id, Name = s.Name })
				.MapFrom(d => d.PersonId, s => s.Id)
				.IgnoreDestinationMember(d => d.Movie)
				.IgnoreDestinationMember(d => d.MovieId);

			CreateMap<CompanyShort, MovieCompany>(MemberList.None)
				.MapFrom(d => d.Company, s => new Company { Id = s.Id, Name = s.Name })
				.MapFrom(d => d.CompanyId, s => s.Id)
				.IgnoreDestinationMember(d => d.Movie)
				.IgnoreDestinationMember(d => d.MovieId);

			CreateMap<TrailerData, Trailer>()
				.IgnoreDestinationMember(d => d.Movie)
				.IgnoreDestinationMember(d => d.Id);

			CreateMap<BoxOfficeShort, BoxOffice>()
				.IgnoreDestinationMember(d => d.Movie)
				.IgnoreDestinationMember(d => d.Id);

			CreateMap<SimilarShort, MovieSimilar>()
				.MapFrom(d => d.Similar, s => new Movie { Id = s.Id, CoverImageUrl = s.Image })
				.MapFrom(d => d.SimilarId, s => s.Id)
				.IgnoreDestinationMember(d => d.Movie)
				.IgnoreDestinationMember(d => d.MovieId);
		}
	}
}