﻿using Organizers.Common.Config;

using System.Threading.Tasks;

namespace Tests.Common
{
	public class TestConfig : IConfig
	{
		public void AddSearchedTitle(string suggestedTitle)
		{
			throw new System.NotImplementedException();
		}

		public string GetConnectionString()
		{
			return "Filename=Test.db";
		}

		public string GetIMDbApiKey()
		{
			throw new System.NotImplementedException();
		}

		public Task<bool> WasAlreadySearched(string term)
		{
			throw new System.NotImplementedException();
		}
	}
}