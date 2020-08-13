using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Setup
{
	public class Config : IConfig
	{
		private string RecentMoviesSearchesPath
		{
			get
			{
				var path = AppDomain.CurrentDomain.BaseDirectory + RecentMoviesSearchesPathFromAppSettings;
				if (!File.Exists(path))
					File.Create(path);
				return path;
			}
		}

		private string RecentMoviesSearchesPathFromAppSettings => Configuration.AppSettings.Settings["RecentMoviesSearchesPath"].Value;
		protected Configuration Configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

		public async Task AddSearchedTitleAsync(string suggestedTitle)
		{
			var lines = await File.ReadAllLinesAsync(RecentMoviesSearchesPath);
			if (!lines.Contains(suggestedTitle))
			{
				await File.AppendAllLinesAsync(RecentMoviesSearchesPath, new List<string> { suggestedTitle });
				File.ReadAllLines(RecentMoviesSearchesPath);
			}
		}

		public virtual string GetConnectionString()
		{
			//Workaround for Migrations because ConfigurationManager returns null
			//var configuration = new ConfigurationBuilder()
			// .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
			// .Add(@"Properties/App.config")
			// .Build();

			var connString = Configuration.ConnectionStrings.ConnectionStrings["SqlServerConnectionString"].ConnectionString;//.GetValue<string>("connectionStrings:add:SqlServerConnectionString:connectionString");
			return connString;
		}

		public virtual string GetIMDbApiKey()
		{
			return Configuration.AppSettings.Settings["IMDbApiKey"].Value;
		}

		public async Task<bool> WasAlreadySearched(string term)
		{
			var lines = await File.ReadAllLinesAsync(RecentMoviesSearchesPath);
			return lines.ToList().Any(x => x.Equals(term, StringComparison.OrdinalIgnoreCase));
		}

		public virtual string GetRatingSourceLogoUrl(string ratingSourceName)
		{
			var logoKeyName = ratingSourceName + "Logo";
			return Configuration.AppSettings.Settings[logoKeyName].Value;
		}
	}
}