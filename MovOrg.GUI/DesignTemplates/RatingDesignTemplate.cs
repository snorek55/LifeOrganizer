﻿using MovOrg.Organizer.Adapters.Items;

namespace MovOrg.GUI.DesignTemplates
{
	internal class RatingDesignTemplate : RatingViewModel
	{
		public RatingDesignTemplate()
		{
			SourceName = "Lorem";
			SourceLogoUrl = "https://www.themoviedb.org/assets/1/v4/logos/312x276-primary-green-74212f6247252a023be0f02a5a45794925c3689117da9d20ffe47742a665c518.png";

			Score = "9.9";
		}
	}
}