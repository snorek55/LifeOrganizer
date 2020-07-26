﻿using Organizers.Common.Domain;

namespace Organizers.MovOrg.Domain
{
	public class Trailer : Entity
	{
		public Movie Movie { get; set; }
		public string ThumbnailUrl { get; set; }
		public string LinkEmbed { get; set; }
	}
}