using Organizers.Main.Adapters.MainMenu;

using System;

namespace Common.Adapters
{
	public interface IContainerPluginData
	{
		string Label { get; }
		MainMenuIconType Icon { get; }
		BaseOrganizerContainerViewModel OrganizerContainer { get; }
		Uri ResourceDictionaryUri { get; }
	}
}