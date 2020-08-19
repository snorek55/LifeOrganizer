﻿using MovOrg.Organizer.Domain;

namespace MovOrg.Organizer.UseCases.DTOs
{
	public class DirectorDto
	{
		public string PersonName { get; set; }
		public object PersonId { get; set; }
		public Person Person { get; set; }
		public object MovieId { get; set; }
	}
}