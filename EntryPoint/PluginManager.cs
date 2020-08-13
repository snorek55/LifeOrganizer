using Common.Setup;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using System.Text;

namespace EntryPoint
{
	public class PluginManager
	{
		private CompositionContainer compositionContainer;

		[ImportMany(typeof(IDependencyResolver))]
		public IEnumerable<IDependencyResolver> DependencyResolvers { get; set; }

		public void LoadPlugins(IServiceCollection serviceCollection, string path, string pattern)
		{
			var dirCat = new DirectoryCatalog(path, pattern);

			try
			{
				var aggregateCatalog = new AggregateCatalog();
				aggregateCatalog.Catalogs.Add(dirCat);

				compositionContainer = new CompositionContainer(aggregateCatalog);
				compositionContainer.ComposeParts(this);

				LoadPluginDependencies(serviceCollection);
			}
			catch (ReflectionTypeLoadException typeLoadException)
			{
				var builder = new StringBuilder();
				foreach (Exception loaderException in typeLoadException.LoaderExceptions)
				{
					builder.AppendFormat("{0}\n", loaderException.Message);
				}

				throw new TypeLoadException(builder.ToString(), typeLoadException);
			}
		}

		private void LoadPluginDependencies(IServiceCollection serviceCollection)
		{
			foreach (IDependencyResolver module in DependencyResolvers)
			{
				module.Setup(serviceCollection);
			}
		}
	}
}