namespace MovOrg.Organizers.Domain
{
	public abstract class MoviePerson
	{
		public Movie Movie { get; set; }
		public string MovieId { get; set; }

		public Person Person { get; set; }
		public string PersonId { get; set; }
	}
}