using MovOrg.Organizer.Adapters.Items;
using MovOrg.Organizer.Adapters.Sections;

using System;
using System.Collections.Generic;

namespace MovOrg.GUI.DesignTemplates
{
	internal class ActorDetailPanelDesignTemplate : ActorDetailsPanelViewModel
	{
		public ActorDetailPanelDesignTemplate() : base(null, null)
		{
			SelectedActor = new ActorViewModelDesignTemplate();
		}

		private class ActorViewModelDesignTemplate : ActorViewModel
		{
			public ActorViewModelDesignTemplate()
			{
				Name = "Name template";
				Roles = "9999";
				ImageUrl = "https://imdb-api.com/images/original/MV5BNTUzOTMwNTM0OV5BMl5BanBnXkFtZTcwNDQwMTUxMw@@._V1_Ratio0.7256_AL_.jpg";

				Summary = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum";

				BirthDate = DateTime.Now;
				DeathDate = DateTime.Now;

				Height = "Lorem cm";
				Awards = "Lorem,ipsum,dolor,sit,amet";

				CastMovies = new List<MovieViewModel>
				{
					new MovieDetailPanelDesignTemplate.MovieViewModelDesignTemplate()
				};
			}
		}
	}
}