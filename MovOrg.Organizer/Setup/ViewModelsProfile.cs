using AutoMapper;

using Common.Adapters;
using Common.Extensions;

using MovOrg.Organizer.Adapters.Items;
using MovOrg.Organizer.Domain;
using MovOrg.Organizer.UseCases.DTOs;

namespace MovOrg.Organizer.Setup
{
	public class ViewModelsProfile : Profile
	{
		public ViewModelsProfile()
		{
			CreateMap<Movie, MovieViewModel>()
				.IgnoreDestinationMember(x => x.ShowAllActors)
				.IgnoreDestinationMember(x => x.SelectedSimilar)
				.MapFrom(d => d.CoverImage, s =>
					new ImageViewModel
					{
						Image = s.CoverImageUrl,
						Title = "Cover"
					}
				);

			CreateMap<MovieImageData, ImageViewModel>();
			CreateMap<BoxOffice, BoxOfficeViewModel>();
			CreateMap<Trailer, TrailerViewModel>();
			CreateMap<MovieActor, ActorViewModel>()
				.MapFrom(d => d.Id, s => s.Person.Id)
				.MapFrom(d => d.Name, s => s.Person.Name)
				.MapFrom(d => d.ImageUrl, s => s.Person.ImageUrl);

			CreateMap<MovieDirector, DirectorViewModel>()
				.MapFrom(d => d.Id, s => s.Person.Id)
				.MapFrom(d => d.Name, s => s.Person.Name)
				.MapFrom(d => d.ImageUrl, s => s.Person.ImageUrl);

			CreateMap<MovieWriter, WriterViewModel>()
				.MapFrom(d => d.Id, s => s.Person.Id)
				.MapFrom(d => d.Name, s => s.Person.Name)
				.MapFrom(d => d.ImageUrl, s => s.Person.ImageUrl);

			CreateMap<MovieCompany, CompanyViewModel>()
				.MapFrom(d => d.Id, s => s.Company.Id)
				.MapFrom(d => d.Name, s => s.Company.Name);

			CreateMap<Rating, RatingViewModel>()
				.MapFrom(d => d.Score, y => (y.Score != null) ? y.Score.ToString() : "N/A");

			CreateMap<MovieSimilar, SimilarMovieViewModel>(MemberList.None)
				.MapFrom(d => d.CoverImageUrl, s => s.Similar.CoverImageUrl);

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
		}
	}
}