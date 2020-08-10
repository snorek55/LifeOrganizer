using System.ComponentModel;

namespace Organizers.Common.Adapters
{
	public class NotifyPropertyChangedImpl : INotifyPropertyChanged
	{
#pragma warning disable 67

		public event PropertyChangedEventHandler PropertyChanged;

#pragma warning restore 67
	}
}