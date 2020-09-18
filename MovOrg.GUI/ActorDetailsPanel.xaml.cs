using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Media;

namespace MovOrg.GUI
{
	/// <summary>
	/// Lógica de interacción para MovieDetailsPanel.xaml
	/// </summary>
	public partial class ActorDetailsPanel : UserControl
	{
		public ActorDetailsPanel()
		{
			InitializeComponent();
			if (!DesignerProperties.GetIsInDesignMode(this))
			{
				mainGrid.Background = Brushes.Transparent;
			}
		}
	}
}