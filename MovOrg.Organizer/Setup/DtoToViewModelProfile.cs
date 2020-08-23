using AutoMapper;

using Common.Adapters;
using Common.Extensions;

using MovOrg.Organizer.Adapters.Items;
using MovOrg.Organizer.UseCases.DTOs;

namespace MovOrg.Organizer.Setup
{
	public class DtoToViewModelProfile : Profile
	{
		public DtoToViewModelProfile()
		{
			CreateMap<MovieWithDetailsDto, MovieViewModel>(MemberList.Source)
			.IgnoreSourceMember(s => s.CoverImageUrl)
			.MapFrom(d => d.CoverImage, s => new ImageViewModel
			{
				Image = s.CoverImageUrl,
				Title = "Cover"
			});

			CreateMap<ActorDto, ActorViewModel>(MemberList.None)
				.MapFrom(x => x.Name, s => s.PersonName)
				.MapFrom(x => x.ImageUrl, y => y.PersonImageUrl);

			CreateMap<DirectorDto, DirectorViewModel>(MemberList.None)
				.MapFrom(x => x.Name, s => s.PersonName);

			CreateMap<WriterDto, WriterViewModel>(MemberList.None)
				.MapFrom(x => x.Name, s => s.PersonName);

			CreateMap<CompanyDto, CompanyViewModel>(MemberList.None)
				.MapFrom(x => x.Name, y => y.CompanyName);

			CreateMap<SimilarDto, SimilarMovieViewModel>(MemberList.None);

			CreateMap<RatingDto, RatingViewModel>(MemberList.None);

			CreateMap<MovieImageDto, ImageViewModel>(MemberList.None);

			CreateMap<MovieListItemDto, MovieViewModel>(MemberList.Source)
				.IgnoreSourceMember(s => s.CoverImageUrl)
				.MapFrom(d => d.CoverImage, s => new ImageViewModel
				{
					Image = s.CoverImageUrl,
					Title = "Cover"
				});
		}
	}
}