using Common.Domain;

using System;

namespace MovOrg.Organizer.Domain
{
	public class Person : Entity
	{
		public string Name { get; set; }
		public string Role { get; set; }
		public string Summary { get; set; }

		public DateTime? BirthDate { get; set; }

		public DateTime? DeathDate { get; set; }

		public string Awards { get; set; }

		public string Height { get; set; }

		public string ImageUrl { get; set; }

		//public List<MovieActor> ActedIn { get; set; }

		//public List<MovieDirector> DirectorOf { get; set; }

		//	public List<MovieWriter> WriterOf { get; set; }
	}
}