using AutoMapper;

using Domain;

using Organizers.MovOrg.Adapters.Items;

namespace EntryPoint.Mapper.Profiles
{
	public class ViewModelsProfile : Profile
	{
		public ViewModelsProfile()
		{
			CreateMap<Movie, MovieViewModel>();

			CreateMap<MovieDirector, DirectorViewModel>()
				.ForMember(d => d.Id, opt => opt.MapFrom(s => s.DirectorId))
				.ForMember(d => d.Name, opt => opt.MapFrom(s => s.Director.Name));

			CreateMap<MovieWriter, WriterViewModel>()
				.ForMember(d => d.Id, opt => opt.MapFrom(s => s.WriterId))
				.ForMember(d => d.Name, opt => opt.MapFrom(s => s.Writer.Name));

			CreateMap<MovieActor, ActorViewModel>()
			.ForMember(d => d.Id, opt => opt.MapFrom(s => s.Actor.Id))
			.ForMember(d => d.Name, opt => opt.MapFrom(s => s.Actor.Name))
			.ForMember(d => d.AsCharacter, opt => opt.MapFrom(s => s.Actor.AsCharacter))
			.ForMember(d => d.ImageUrl, opt => opt.MapFrom(s => s.Actor.ImageUrl));

			CreateMap<MovieCompany, CompanyViewModel>()
			.ForMember(d => d.Id, opt => opt.MapFrom(s => s.Company.Id))
			.ForMember(d => d.Name, opt => opt.MapFrom(s => s.Company.Name));

			CreateMap<Rating, RatingViewModel>()
			.ForMember(d => d.SourceName, opt => opt.MapFrom(s => s.RatingSource.Name))
			.ForMember(d => d.Score, opt => opt.MapFrom(y => (y.Score != null) ? y.Score.ToString() : "N/A"));
		}
	}
}