using AutoMapper;

using Common.Setup;

using System.Collections.Generic;

namespace MovOrg.Organizer.Setup
{
	public class ProfilePluginData : IProfilePluginData
	{
		public IEnumerable<Profile> Profiles => new List<Profile>
		{
			new ViewModelsProfile()
		};
	}
}