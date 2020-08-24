using AutoMapper;

using Common.Extensions;

using IMDbApiLib.Models;

using MovOrg.Organizer.Domain;
using MovOrg.Organizer.UseCases.DTOs;

namespace MovOrg.Infrastructure.Setup
{
	public class DomainToDtoProfile : Profile
	{
		public DomainToDtoProfile()
		{
			CreateMap<Movie, MovieWithDetailsDto>(MemberList.Destination);
			CreateMap<Movie, UpdateMovieDetailsDto>(MemberList.Destination);

			CreateMap<MovieActor, ActorDto>(MemberList.None);
			CreateMap<MovieCompany, CompanyDto>(MemberList.None);
			CreateMap<MovieWriter, WriterDto>(MemberList.None);
			CreateMap<MovieDirector, DirectorDto>(MemberList.None);
			CreateMap<MovieSimilar, SimilarDto>(MemberList.None)
				.MapFrom(x => x.CoverImageUrl, y => y.Similar.CoverImageUrl);
			CreateMap<Rating, RatingDto>(MemberList.None);
			CreateMap<MovieImageData, MovieImageDto>(MemberList.None)
				.ReverseMap();

			CreateMap<Movie, MovieListItemDto>(MemberList.Destination)
				.ReverseMap();

			CreateMap<Top250DataDetail, MovieListItemDto>(MemberList.Destination)
				.MapFrom(d => d.CoverImageUrl, s => s.Image);

			//TODO: new dto
			CreateMap<SearchResult, MovieListItemDto>(MemberList.Destination)
				.IgnoreDestinationMember(d => d.IMDbRating)
				.IgnoreDestinationMember(d => d.Rank)
				.IgnoreDestinationMember(d => d.Year)
				.MapFrom(d => d.CoverImageUrl, s => s.Image);
		}
	}
}