using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Organizers.Common.Adapters
{
	public class ImagePresenterViewModel : BaseViewModel
	{
		private enum Direction
		{
			Next,
			Previous
		};

		public event EventHandler RequestedExit = (s, e) => { };

		public ImageViewModel CurrentImage { get; set; }

		public ObservableCollection<ImageViewModel> Images { get; set; } = new ObservableCollection<ImageViewModel>();

		public ICommand PreviousImageCommand { get => new SyncCommand(() => Go(Direction.Previous)); }

		public ICommand NextImageCommand { get => new SyncCommand(() => Go(Direction.Next)); }

		public ICommand ExitCommand { get => new SyncCommand(() => Exit()); }

		private void Exit()
		{
			RequestedExit?.Invoke(null, new EventArgs());
		}

		private void Go(Direction dir)
		{
			var index = Images.IndexOf(CurrentImage);
			if (dir == Direction.Previous)
				index--;
			else
				index++;

			if (index < 0) index = Images.Count - 1;
			if (index >= Images.Count) index = 0;

			CurrentImage = Images[index];
		}
	}
}