using Organizers.Common.Adapters;

using System;
using System.Collections.Generic;

namespace Organizers.MovOrg.Adapters.Items
{
	public class MovieViewModel : BaseViewModel
	{
		#region General Info

		public string Id { get; set; }

		public string Title { get; set; }

		public string Image { get; set; }

		public int? Rank { get; set; }

		#endregion General Info

		#region Details

		public string Plot { get; set; }

		public string Description { get; set; } = "";

		public string Year { get; set; }

		public string Tagline { get; set; }

		public string Keywords { get; set; }

		public string RuntimeStr { get; set; }

		public string Awards { get; set; }

		public string Genres { get; set; }

		public string Countries { get; set; }

		public string Languages { get; set; }

		public string WikipediaUrl { get; set; }

		public DateTime? ReleaseDate { get; set; }

		public string LastUpdatedDetails { get; set; }

		public BoxOfficeViewModel BoxOffice { get; set; }

		public TrailerViewModel Trailer { get; set; }

		public List<DirectorViewModel> DirectorList { get; set; } = new List<DirectorViewModel>();

		public List<ActorViewModel> ActorList { get; set; } = new List<ActorViewModel>();

		public List<CompanyViewModel> CompanyList { get; set; } = new List<CompanyViewModel>();

		public List<WriterViewModel> WriterList { get; set; } = new List<WriterViewModel>();

		public List<RatingViewModel> Ratings { get; set; } = new List<RatingViewModel>();

		#endregion Details

		#region User Preferences

		public bool IsFavorite { get; set; }

		public bool IsMustWatch { get; set; }

		public bool IsWatched { get; set; }

		#endregion User Preferences
	}
}