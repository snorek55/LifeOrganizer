using Organizers.Common.Domain;

namespace Domain
{
	public class BoxOffice : Entity
	{
		public Movie Movie { get; set; }

		public string Budget { get; set; }
		public string OpeningWeekendUSA { get; set; }
		public string GrossUSA { get; set; }
		public string CumulativeWorldwideGross { get; set; }
	}
}