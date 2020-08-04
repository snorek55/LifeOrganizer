using AutoMapper;

using IMDbApiLib.Models;

using Organizers.MovOrg.Domain;

namespace EntryPoint.Mapper.Profiles
{
	public class IMDbProfile : Profile
	{
		public IMDbProfile()
		{
			CreateMap<TitleData, Movie>()
				.IgnoreAllPropertiesWithAnInaccessibleSetter()
				.Ignore(dest => dest.Description)
				.Ignore(dest => dest.LastUpdatedDetails)
				.Ignore(dest => dest.LastUpdatedTop250)
				.Ignore(dest => dest.Rank)
				.Ignore(d => d.IsFavorite)
				.Ignore(d => d.IsWatched)
				.Ignore(d => d.IsMustWatch)
				.Ignore(d => d.Ratings)
				.AfterMap((src, dest) => dest.BoxOffice.Movie = dest)
				.AfterMap((src, dest) => dest.BoxOffice.Id = dest.Id)
				.AfterMap((src, dest) => dest.Trailer.Id = dest.Id)
				.AfterMap((src, dest) => dest.Trailer.Movie = dest)
				.AfterMap((s, d) =>
				{
					foreach (var actor in d.ActorList)
					{
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
				});

			CreateMap<SearchResult, Movie>(MemberList.Source)
				.DoNotValidate(s => s.ResultType);

			CreateMap<Top250DataDetail, Movie>(MemberList.Source)
				.DoNotValidate(s => s.FullTitle)
				.DoNotValidate(s => s.Crew)
				.DoNotValidate(s => s.IMDbRatingCount);

			CreateMap<ActorShort, MovieActor>(MemberList.None)
				.ForMember(d => d.Person, o => o.MapFrom(s => new Person { Id = s.Id, Name = s.Name, ImageUrl = s.Image }))
				.ForMember(d => d.PersonId, o => o.MapFrom(s => s.Id))
				.Ignore(d => d.Movie)
				.Ignore(d => d.MovieId);

			CreateMap<StarShort, MovieDirector>(MemberList.None)
				.ForMember(d => d.Person, o => o.MapFrom(s => new Person { Id = s.Id, Name = s.Name }))
				.ForMember(d => d.PersonId, o => o.MapFrom(s => s.Id))
				.Ignore(d => d.Movie);

			CreateMap<StarShort, MovieWriter>(MemberList.None)
				.ForMember(d => d.Person, o => o.MapFrom(s => new Person { Id = s.Id, Name = s.Name }))
				.ForMember(d => d.PersonId, o => o.MapFrom(s => s.Id))
				.Ignore(d => d.Movie)
				.Ignore(d => d.MovieId);

			CreateMap<CompanyShort, MovieCompany>(MemberList.None)
				.ForMember(d => d.Company, o => o.MapFrom(s => new Company { Id = s.Id, Name = s.Name }))
				.ForMember(d => d.CompanyId, o => o.MapFrom(s => s.Id))
				.Ignore(d => d.Movie)
				.Ignore(d => d.MovieId);

			CreateMap<TrailerData, Trailer>()
				.Ignore(d => d.Movie)
				.Ignore(d => d.Id);

			CreateMap<BoxOfficeShort, BoxOffice>()
				.Ignore(d => d.Movie)
				.Ignore(d => d.Id);
		}
	}
}