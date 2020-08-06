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

		private int maxItemsShown = 8;
		public int MaxItemsShown { get => maxItemsShown; set => SetMaxItemsShown(value); }

		public ObservableCollection<ImageViewModel> Images { get; set; } = new ObservableCollection<ImageViewModel>();
		public PagedCollectionView<ImageViewModel> ImagesCollectionView { get; private set; }

		public ICommand PreviousImageCommand { get => new SyncCommand(() => ChangeImage(Direction.Previous)); }

		public ICommand NextImageCommand { get => new SyncCommand(() => ChangeImage(Direction.Next)); }

		public ICommand ShowNextPageCommand { get => new SyncCommand(() => ChangePage(Direction.Next)); }

		public ICommand ShowPreviousPageCommand { get => new SyncCommand(() => ChangePage(Direction.Previous)); }

		public ICommand ExitCommand { get => new SyncCommand(() => Exit()); }

		public ImagePresenterViewModel()
		{
			ImagesCollectionView = new PagedCollectionView<ImageViewModel>(Images, MaxItemsShown);
		}

		private void SetMaxItemsShown(int value)
		{
			ImagesCollectionView.MaxPagedItems = value;
			maxItemsShown = value;
		}

		private void Exit()
		{
			RequestedExit?.Invoke(null, new EventArgs());
		}

		private void ChangeImage(Direction dir)
		{
			var index = Images.IndexOf(CurrentImage);
			if (dir == Direction.Previous)
				index--;
			else
				index++;

			if (index < ImagesCollectionView.MinIndexValid || index < 0) index = ImagesCollectionView.MaxIndexValid;
			if (index > ImagesCollectionView.MaxIndexValid || index >= Images.Count) index = ImagesCollectionView.MinIndexValid;

			CurrentImage = Images[index];
		}

		private void ChangePage(Direction dir)
		{
			if (dir == Direction.Next)
				ImagesCollectionView.NextPage();
			else
				ImagesCollectionView.PreviousPage();

			ImagesCollectionView.CollectionView.MoveCurrentToFirst();
			CurrentImage = (ImageViewModel)ImagesCollectionView.CollectionView.CurrentItem;
		}
	}
}