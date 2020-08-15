using Main.GUI.Setup;

using System;
using System.IO;
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
			AppDomain.CurrentDomain.AssemblyResolve += ResolveAssembly;

			var injector = new DependencyInjector(this);
			MainWindow = injector.Get<MainWindow>();
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
	}
}