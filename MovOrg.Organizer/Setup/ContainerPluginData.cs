using Common.Adapters;

using MovOrg.Organizer.Adapters.Container;

using System;

namespace MovOrg.Organizer.Setup
{
	public class ContainerPluginData : IContainerPluginData
	{
		public string Label => "Movies";
		public MainMenuIconType Icon => MainMenuIconType.Movies;
		public BaseOrganizerContainerViewModel OrganizerContainer { get; private set; }
		public Uri ResourceDictionaryUri => new Uri(@"pack://application:,,,/MovOrg.GUI;component/MovOrgDataTemplates.xaml");

		public ContainerPluginData(MovOrgContainerViewModel movOrgContainerViewModel)
		{
			OrganizerContainer = movOrgContainerViewModel;
		}
	}
}