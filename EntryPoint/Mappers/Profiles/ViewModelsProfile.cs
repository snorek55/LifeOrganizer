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
			//TODO: extension mapfrom
			CreateMap<Movie, MovieViewModel>()
				.IgnoreDestinationMember(x => x.ShowAllActors)
				.IgnoreDestinationMember(x => x.SelectedSimilar)
				.ForMember(d => d.CoverImage, opt => opt.MapFrom(s =>
					new ImageViewModel
					{
						Image = s.CoverImage,
						Title = "Cover"
					}
				));

			CreateMap<MovieImageData, ImageViewModel>();
			CreateMap<BoxOffice, BoxOfficeViewModel>();
			CreateMap<Trailer, TrailerViewModel>();
			CreateMap<MovieActor, ActorViewModel>()
				.ForMember(d => d.Id, opt => opt.MapFrom(s => s.Person.Id))
				.ForMember(d => d.Name, opt => opt.MapFrom(s => s.Person.Name))
				.ForMember(d => d.ImageUrl, opt => opt.MapFrom(s => s.Person.ImageUrl));

			CreateMap<MovieDirector, DirectorViewModel>()
				.ForMember(d => d.Id, opt => opt.MapFrom(s => s.Person.Id))
				.ForMember(d => d.Name, opt => opt.MapFrom(s => s.Person.Name))
				.ForMember(d => d.ImageUrl, opt => opt.MapFrom(s => s.Person.ImageUrl));

			CreateMap<MovieWriter, WriterViewModel>()
			.ForMember(d => d.Id, opt => opt.MapFrom(s => s.Person.Id))
			.ForMember(d => d.Name, opt => opt.MapFrom(s => s.Person.Name))
			.ForMember(d => d.ImageUrl, opt => opt.MapFrom(s => s.Person.ImageUrl));

			CreateMap<MovieCompany, CompanyViewModel>()
			.ForMember(d => d.Id, opt => opt.MapFrom(s => s.Company.Id))
			.ForMember(d => d.Name, opt => opt.MapFrom(s => s.Company.Name));

			CreateMap<Rating, RatingViewModel>()
			.ForMember(d => d.Score, opt => opt.MapFrom(y => (y.Score != null) ? y.Score.ToString() : "N/A"));

			CreateMap<MovieSimilar, SimilarMovieViewModel>(MemberList.None)
				.ForMember(d => d.Movie, o => o.MapFrom(s => s.Similar));
		}
	}
}