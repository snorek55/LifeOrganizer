using Common.Adapters;

using Organizers.Main.Adapters.MainMenu;

using System;

namespace MovOrg.Organizers.Adapters.Container
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