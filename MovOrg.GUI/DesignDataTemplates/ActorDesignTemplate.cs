using MovOrg.Organizers.Adapters.Items;

namespace DesktopGui.MovOrg.DesignDataTemplates
{
	internal class ActorDesignTemplate : ActorViewModel
	{
		public ActorDesignTemplate()
		{
			Name = "Lorem";
			AsCharacter = "Ipsum";
			ImageUrl = "https://imdb-api.com/images/original/MV5BMjI0MTg3MzI0M15BMl5BanBnXkFtZTcwMzQyODU2Mw@@._V1_Ratio0.7273_AL_.jpg";
			IsStar = true;
		}
	}
}