using System;
using System.Collections.Generic;

namespace MovOrg.Organizer.Adapters.Items
{
	//TODO: must split between movieactor info and actor pure info
	public class ActorViewModel : PersonViewModel
	{
		#region ActorMovieInfo

		public string AsCharacter { get; set; }
		public bool IsStar { get; set; }

		#endregion ActorMovieInfo

		public DateTime? BirthDate { get; set; }

		public DateTime? DeathDate { get; set; }

		public string Roles { get; set; }

		public string Summary { get; set; }

		public string Awards { get; set; }
		public string Height { get; set; }

		//TODO: create new dto for this
		public List<MovieViewModel> CastMovies { get; set; }

		public bool IsFavorite { get; internal set; }
		public string WikipediaUrl { get; internal set; }
	}
}