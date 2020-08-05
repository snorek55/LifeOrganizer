using System.Windows.Controls;

namespace DesktopGui.Common
{
	/// <summary>
	/// Lógica de interacción para ImagePresenter.xaml
	/// </summary>
	public partial class ImagePresenter : UserControl
	{
		public ImagePresenter()
		{
			InitializeComponent();
		}

		private void ListView_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			PART_ScrollViewer.ScrollToHome();
		}
	}
}