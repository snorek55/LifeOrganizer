﻿namespace MovOrg.Organizers.Domain
{
	public class MovieActor : MoviePerson
	{
		public string AsCharacter { get; set; }

		public bool IsStar { get; set; }
	}
}