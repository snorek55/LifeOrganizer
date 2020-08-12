using AutoMapper;

using Common.Setup;

using Infrastructure.Setup;

using System.Collections.Generic;

namespace MovOrg.Infrastructure.Setup
{
	public class ProfilePluginData : IProfilePluginData
	{
		public IEnumerable<Profile> Profiles => new List<Profile>
		{
			new IMDbProfile()
		};
	}
}