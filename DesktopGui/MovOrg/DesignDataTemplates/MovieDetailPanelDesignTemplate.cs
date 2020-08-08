using Organizers.MovOrg.Adapters.Items;
using Organizers.MovOrg.Adapters.Sections;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DesktopGui.MovOrg.DesignDataTemplates
{
	internal class MovieDetailPanelDesignTemplate : MovieDetailsPanelViewModel
	{
		public MovieDetailPanelDesignTemplate() : base(null, null)
		{
			SelectedMovie = new MovieViewModelDesignTemplate();
		}

		private class MovieViewModelDesignTemplate : MovieViewModel
		{
			public MovieViewModelDesignTemplate()
			{
				Title = "Title template";
				Year = "9999";
				CoverImage = "https://imdb-api.com/images/original/MV5BMjAxMzY3NjcxNF5BMl5BanBnXkFtZTcwNTI5OTM0Mw@@._V1_Ratio0.6791_AL_.jpg";
				Plot = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum";

				Tagline = "Lorem ipsum dolor sit amet";

				ReleaseDate = DateTime.Now.ToString();
				RuntimeStr = "228 min.";

				Keywords = "Lorem,ipsum,dolor,sit,amet";

				Awards = "Lorem,ipsum,dolor,sit,amet";
				Countries = "Lorem,ipsum,dolor,sit,amet";
				Genres = "Lorem,ipsum,dolor,sit,amet";
				Languages = "Lorem,ipsum,dolor,sit,amet";

				DirectorList = new List<DirectorViewModel>
				{
					new DirectorViewModel
					{
						Name="Pepe Mujica",
						Id = "1"
					}
				};

				CompanyList = new List<CompanyViewModel>
				{
					new CompanyViewModel
					{
						Name="Patata"
					}
				};

				WriterList = new List<WriterViewModel>
				{
					new WriterViewModel
					{
						Name="Tomate"
					}
				};

				BoxOffice = new BoxOfficeViewModel
				{
					OpeningWeekendUSA = "124332423$",
					Budget = "325436554654€",
					CumulativeWorldwideGross = "3534543€",
					GrossUsa = "324€"
				};

				ActorList = new ObservableCollection<ActorViewModel>();
				for (int i = 0; i < 6; i++)
				{
					var actor = new ActorDesignTemplate();
					ActorList.Add(actor);
				}

				Ratings = new List<RatingViewModel>();
				for (int i = 0; i < 5; i++)
				{
					var rating = new RatingDesignTemplate();
					Ratings.Add(rating);
				}
			}
		}
	}
}