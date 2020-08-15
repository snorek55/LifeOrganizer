using AutoMapper.Configuration;

using Common.Adapters;
using Common.Setup;

using Main.GUI.Setup;
using Main.GUI.ViewModels;

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace Main.GUI
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();
		}

		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			var injector = new DependencyInjector();

			InitializeMapper(injector);

			AppDomain.CurrentDomain.AssemblyResolve += ResolveAssembly;
			InitializeContainers(injector);

			this.MainWindow = injector.Get<MainWindow>();
			MainWindow.Show();
		}

		//WPF does not handle well resolving referenced resources when they are externally loaded.
		//https://stackoverflow.com/questions/20495778/pack-uri-to-resources-in-referenced-assembly
		private Assembly ResolveAssembly(object sender, ResolveEventArgs e)
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

		private void InitializeContainers(DependencyInjector injector)
		{
			var mainVM = injector.Get<MainWindowViewModel>();

			foreach (var containerData in injector.GetMany<IContainerPluginData>())
			{
				this.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = containerData.ResourceDictionaryUri });
				mainVM.MainMenuContainers.Add(containerData);
			}
			mainVM.SelectedItem = mainVM.MainMenuContainers.FirstOrDefault();
		}

		private void InitializeMapper(DependencyInjector injector)
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