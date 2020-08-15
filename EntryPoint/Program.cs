using AutoMapper.Configuration;

using Common.Adapters;
using Common.Setup;

using Main.GUI;
using Main.GUI.ViewModels;

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace EntryPoint
{
	internal static class Program
	{
		/// <summary>
		///  The main entry point for the application.
		/// </summary>
		[STAThread]
		private static void Main()
		{
			var injector = new DependencyInjector();

			InitializeMapper(injector);

			AppDomain.CurrentDomain.AssemblyResolve += ResolveAssembly;
			InitializeContainers(injector);

			injector.App.Run(injector.Get<MainWindow>());
		}

		//WPF does not handle well resolving referenced resources when they are externally loaded.
		//https://stackoverflow.com/questions/20495778/pack-uri-to-resources-in-referenced-assembly
		private static Assembly ResolveAssembly(object sender, ResolveEventArgs e)
		{
			var addinAssembly = Assembly.GetExecutingAssembly();
			var missingAssemblyName = new AssemblyName(e.Name);

			// Sometimes the WPF assembly resolver cannot even find the executing assembly...
			if (missingAssemblyName.FullName == addinAssembly.FullName)
				return addinAssembly;

			var addinFolder = Path.GetDirectoryName(addinAssembly.Location);
			var dllPath = Path.Combine(addinFolder, missingAssemblyName.Name + ".dll");

			// If we find the DLL in the add-in folder, load and return it.
			if (File.Exists(dllPath))
				return Assembly.LoadFrom(dllPath);

			// nothing found
			return null;
		}

		private static void InitializeContainers(DependencyInjector injector)
		{
			var mainVM = injector.Get<MainWindowViewModel>();

			foreach (var containerData in injector.GetMany<IContainerPluginData>())
			{
				injector.App.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = containerData.ResourceDictionaryUri });
				mainVM.MainMenuContainers.Add(containerData);
			}
			mainVM.SelectedItem = mainVM.MainMenuContainers.FirstOrDefault();
		}

		private static void InitializeMapper(DependencyInjector injector)
		{
			var autoMapper = injector.Get<IAutoMapper>();
			var configExpression = new MapperConfigurationExpression();

			foreach (var profileData in injector.GetMany<IProfilePluginData>())
			{
				configExpression.AddProfiles(profileData.Profiles);
			}

			autoMapper.CreateMapper(configExpression);
		}
	}
}