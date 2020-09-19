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
				.IgnoreDestinationMember(x => x.SelectedRating)
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
			CreateMap<MovieActor, ActorViewModel>(MemberList.None)
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

			CreateMap<MovieViewModel, MovieListItemDto>(MemberList.Destination)
				.MapFrom(d => d.CoverImageUrl, s => s.CoverImage.Image);
		}
	}
}