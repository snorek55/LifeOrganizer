using Organizers.Common.Adapters;

using System.Linq;

namespace DesktopGui.Common.DesignTemplates
{
	public class ImagePresenterDesignTemplate : ImagePresenterViewModel
	{
		public ImagePresenterDesignTemplate()
		{
			for (int i = 0; i < 10; i++)
			{
				Images.Add(new ImageDesignTemplate());
			}

			CurrentImage = Images.First();
		}
	}
}