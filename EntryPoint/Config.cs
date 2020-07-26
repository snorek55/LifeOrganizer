using Microsoft.Extensions.Configuration;

using Organizers.Common.Config;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace EntryPoint
{
	public class Config : IConfig
	{
		private string path = AppDomain.CurrentDomain.BaseDirectory + ConfigurationManager.AppSettings["RecentMoviesSearchesPath"];

		public Config()
		{
			if (!File.Exists(path))
				File.Create(path);
		}

		public async Task AddSearchedTitleAsync(string suggestedTitle)
		{
			var lines = await File.ReadAllLinesAsync(path);
			if (!lines.Contains(suggestedTitle))
			{
				await File.AppendAllLinesAsync(path, new List<string> { suggestedTitle });
				File.ReadAllLines(path);
			}
		}

		public string GetConnectionString()
		{
			//Workaround for Migrations because ConfigurationManager returns null
			var configuration = new ConfigurationBuilder()
			 .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
			 .AddXmlFile(@"Properties/App.config")
			 .Build();
			var connString = configuration.GetValue<string>("connectionStrings:add:SqlServerConnectionString:connectionString");
			return connString;
		}

		public string GetIMDbApiKey()
		{
			return ConfigurationManager.AppSettings["IMDbApiKey"];
		}

		public async Task<bool> WasAlreadySearched(string term)
		{
			var lines = await File.ReadAllLinesAsync(path);
			return lines.ToList().Any(x => x.Equals(term, StringComparison.OrdinalIgnoreCase));
		}
	}
}