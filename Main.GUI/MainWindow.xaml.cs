using MahApps.Metro.Controls;

using Main.Organizer;

namespace Main.GUI
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