using AutoMapper;

using Organizers.MovOrg.Adapters.Items;
using Organizers.MovOrg.Domain;

namespace EntryPoint.Mapper.Profiles
{
	public class ViewModelsProfile : Profile
	{
		public ViewModelsProfile()
		{
			CreateMap<Movie, MovieViewModel>()
				.IgnoreDestinationMember(x => x.ShowAllActors);

			CreateMap<MovieImageData, ImageDataViewModel>();
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
		}
	}
}