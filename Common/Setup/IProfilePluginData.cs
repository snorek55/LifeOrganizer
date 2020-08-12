using AutoMapper;

using System.Collections.Generic;

namespace Common.Setup
{
	public interface IProfilePluginData
	{
		public IEnumerable<Profile> Profiles { get; }
	}
}