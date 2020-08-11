using Common.Adapters;

using System.Linq;

namespace Common.WPF.DesignTemplates
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