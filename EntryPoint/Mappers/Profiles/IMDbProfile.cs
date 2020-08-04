﻿using AutoMapper;

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
				.IgnoreSourceMember(s => s.IMDbRating)
				.IgnoreDestinationMember(d => d.Description)
				.IgnoreDestinationMember(d => d.LastUpdatedDetails)
				.IgnoreDestinationMember(d => d.LastUpdatedTop250)
				.IgnoreDestinationMember(d => d.Rank)
				.IgnoreDestinationMember(d => d.IsFavorite)
				.IgnoreDestinationMember(d => d.IsWatched)
				.IgnoreDestinationMember(d => d.IsMustWatch)
				.IgnoreDestinationMember(d => d.Ratings)
				.AfterMap((s, d) => d.BoxOffice.Movie = d)
				.AfterMap((s, d) => d.BoxOffice.Id = d.Id)
				.AfterMap((s, d) => d.Trailer.Id = d.Id)
				.AfterMap((s, d) => d.Trailer.Movie = d)
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
				.IgnoreSourceMember(s => s.ResultType);

			CreateMap<Top250DataDetail, Movie>(MemberList.Source)
				.IgnoreSourceMember(s => s.IMDbRating)
				.IgnoreSourceMember(s => s.FullTitle)
				.IgnoreSourceMember(s => s.Crew)
				.IgnoreSourceMember(s => s.IMDbRatingCount);

			CreateMap<ActorShort, MovieActor>(MemberList.None)
				.ForMember(d => d.Person, o => o.MapFrom(s => new Person { Id = s.Id, Name = s.Name, ImageUrl = s.Image }))
				.ForMember(d => d.PersonId, o => o.MapFrom(s => s.Id))
				.IgnoreDestinationMember(d => d.Movie)
				.IgnoreDestinationMember(d => d.MovieId);

			CreateMap<StarShort, MovieDirector>(MemberList.None)
				.ForMember(d => d.Person, o => o.MapFrom(s => new Person { Id = s.Id, Name = s.Name }))
				.ForMember(d => d.PersonId, o => o.MapFrom(s => s.Id))
				.IgnoreDestinationMember(d => d.Movie);

			CreateMap<StarShort, MovieWriter>(MemberList.None)
				.ForMember(d => d.Person, o => o.MapFrom(s => new Person { Id = s.Id, Name = s.Name }))
				.ForMember(d => d.PersonId, o => o.MapFrom(s => s.Id))
				.IgnoreDestinationMember(d => d.Movie)
				.IgnoreDestinationMember(d => d.MovieId);

			CreateMap<CompanyShort, MovieCompany>(MemberList.None)
				.ForMember(d => d.Company, o => o.MapFrom(s => new Company { Id = s.Id, Name = s.Name }))
				.ForMember(d => d.CompanyId, o => o.MapFrom(s => s.Id))
				.IgnoreDestinationMember(d => d.Movie)
				.IgnoreDestinationMember(d => d.MovieId);

			CreateMap<TrailerData, Trailer>()
				.IgnoreDestinationMember(d => d.Movie)
				.IgnoreDestinationMember(d => d.Id);

			CreateMap<BoxOfficeShort, BoxOffice>()
				.IgnoreDestinationMember(d => d.Movie)
				.IgnoreDestinationMember(d => d.Id);
		}
	}
}