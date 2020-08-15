using MahApps.Metro.Controls;

using Organizers;
using Organizers.Main.Adapters;

namespace DesktopGui.Main
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : MetroWindow
	{
		public MainWindow(MainWindowViewModel dataContext)
		{
			InitializeComponent();

			DataContext = dataContext;
		}
	}
}