using MovOrg.Organizer.Domain;

namespace MovOrg.Organizer.UseCases.DTOs
{
	public class ActorDto
	{
		public string PersonName { get; set; }

		public string PersonImageUrl { get; set; }

		public string AsCharacter { get; set; }

		public bool IsStar { get; set; }
		public object MovieId { get; set; }
		public object PersonId { get; set; }
		public Person Person { get; set; }
	}
}