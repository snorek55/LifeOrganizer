﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
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

		public string GetConnectionString(bool isMigrations = false)
		{
			if (!isMigrations)
				return Configuration.ConnectionStrings.ConnectionStrings["SqlServerConnectionString"].ConnectionString;

			var conf = ConfigurationManager.OpenExeConfiguration(AppDomain.CurrentDomain.BaseDirectory + Assembly.GetCallingAssembly().GetName().Name + ".dll");
			string connString;
			connString = conf.ConnectionStrings.ConnectionStrings["SqlServerConnectionString"].ConnectionString;

			return connString;
		}

		public string GetIMDbApiKey()
		{
			return Configuration.AppSettings.Settings["IMDbApiKey"].Value;
		}

		public async Task<bool> WasAlreadySearched(string term)
		{
			var lines = await File.ReadAllLinesAsync(RecentMoviesSearchesPath);
			return lines.ToList().Any(x => x.Equals(term, StringComparison.OrdinalIgnoreCase));
		}

		public string GetRatingSourceLogoUrl(string ratingSourceName)
		{
			var logoKeyName = ratingSourceName + "Logo";
			return Configuration.AppSettings.Settings[logoKeyName].Value;
		}
	}
}