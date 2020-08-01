using Microsoft.Extensions.Configuration;

using Organizers.Common.Config;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EntryPoint
{
	public class Config : IConfig
	{
		private string recentMoviesSearchesPath;
		private string recentMoviesSearchesFile = ConfigurationManager.AppSettings["RecentMoviesSearchesPath"];

		public Config()
		{
			CreateRecentMoviesSearchesIfNotExists();
		}

		private void CreateRecentMoviesSearchesIfNotExists()
		{
			if (recentMoviesSearchesFile == null)
			{
				Debug.WriteLine($"Returned invalid path for movies searches file. {recentMoviesSearchesPath}. Will use default.");
				recentMoviesSearchesPath = AppDomain.CurrentDomain.BaseDirectory + "RecentMoviesSeaches.txt";
			}
			else
			{
				recentMoviesSearchesPath = AppDomain.CurrentDomain.BaseDirectory + recentMoviesSearchesFile;
			}

			if (!File.Exists(recentMoviesSearchesPath))
				File.Create(recentMoviesSearchesPath);
		}

		public async Task AddSearchedTitleAsync(string suggestedTitle)
		{
			var lines = await File.ReadAllLinesAsync(recentMoviesSearchesPath);
			if (!lines.Contains(suggestedTitle))
			{
				await File.AppendAllLinesAsync(recentMoviesSearchesPath, new List<string> { suggestedTitle });
				File.ReadAllLines(recentMoviesSearchesPath);
			}
		}

		public virtual string GetConnectionString()
		{
			//Workaround for Migrations because ConfigurationManager returns null
			var configuration = new ConfigurationBuilder()
			 .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
			 .AddXmlFile(@"Properties/App.config")
			 .Build();
			var connString = configuration.GetValue<string>("connectionStrings:add:SqlServerConnectionString:connectionString");
			return connString;
		}

		public virtual string GetIMDbApiKey()
		{
			return ConfigurationManager.AppSettings["IMDbApiKey"];
		}

		public async Task<bool> WasAlreadySearched(string term)
		{
			var lines = await File.ReadAllLinesAsync(recentMoviesSearchesPath);
			return lines.ToList().Any(x => x.Equals(term, StringComparison.OrdinalIgnoreCase));
		}

		public virtual string GetRatingSourceLogoUrl(string ratingSourceName)
		{
			var logoKeyName = ratingSourceName + "Logo";
			return ConfigurationManager.AppSettings[logoKeyName];
		}
	}
}