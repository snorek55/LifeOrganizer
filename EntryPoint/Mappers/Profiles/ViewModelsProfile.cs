using AutoMapper;

using Organizers.Common.Adapters;
using Organizers.MovOrg.Adapters.Items;
using Organizers.MovOrg.Domain;

namespace EntryPoint.Mapper.Profiles
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
						Image = s.CoverImage,
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
				.MapFrom(d => d.Movie, s => s.Similar);
		}
	}
}