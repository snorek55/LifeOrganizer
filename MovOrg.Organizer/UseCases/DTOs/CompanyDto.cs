using MovOrg.Organizer.Domain;

namespace MovOrg.Organizer.UseCases.DTOs
{
	public class CompanyDto
	{
		public string CompanyName { get; set; }
		public string MovieId { get; set; }
		public string CompanyId { get; set; }
		public Company Company { get; set; }
	}
}