using MovOrg.Organizer.Domain;

namespace MovOrg.Organizer.UseCases.DTOs
{
	public class WriterDto
	{
		public string PersonName { get; set; }
		public object PersonId { get; set; }
		public Person Person { get; set; }
		public object MovieId { get; set; }
	}
}