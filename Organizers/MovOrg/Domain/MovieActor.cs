﻿namespace Domain
{
	public class MovieActor
	{
		public string MovieId { get; set; }

		public Movie Movie { get; set; }

		public string ActorId { get; set; }

		public Actor Actor { get; set; }
	}
}