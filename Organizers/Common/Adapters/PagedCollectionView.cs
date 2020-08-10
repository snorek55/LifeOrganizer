using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

namespace Organizers.Common.Adapters
{
	public class PagedCollectionView<T> : NotifyPropertyChangedImpl where T : class
	{
		private int maxPagedItems;

		public int MaxPagedItems { get => maxPagedItems; set => SetMaxPagedItems(value); }
		public int CurrentPage { get; private set; } = 1;

		private int totalPages;

		//https://stackoverflow.com/questions/17944/how-to-round-up-the-result-of-integer-division
		public int TotalPages { get => totalPages; private set => SetTotalPages(value); }

		public int TotalCount { get => SourceCollection.Count; }
		public int CurrentNumItemsShowing { get => CollectionView.Cast<object>().Count(); }

		public ObservableCollection<T> SourceCollection { get; set; }

		public ICollectionView CollectionView { get; }

		public int MaxIndexValid { get => CurrentPage * MaxPagedItems - 1; }
		public int MinIndexValid { get => MaxIndexValid - MaxPagedItems + 1; }

		public PagedCollectionView(ObservableCollection<T> sourceCollection, int maxPagedItems)
		{
			SourceCollection = sourceCollection ?? throw new ArgumentNullException(nameof(sourceCollection));

			CollectionView = CollectionViewSource.GetDefaultView(SourceCollection);

			MaxPagedItems = maxPagedItems;
			if (maxPagedItems <= 0) throw new ArgumentException("MaxPagedItems cannot be 0 or negative");
			CollectionView.Filter = new Predicate<object>(x => PagingFiltering(x as T));

			Refresh();
			sourceCollection.CollectionChanged += SourceCollection_CollectionChanged;
		}

		private void SourceCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			Refresh();
		}

		private void SetMaxPagedItems(int value)
		{
			if (value <= 0)
				throw new ArgumentException("MaxPagedItems cannot be 0 or negative");

			maxPagedItems = value;
			Refresh();
		}

		private bool PagingFiltering(T t)
		{
			var index = SourceCollection.IndexOf(t);

			return index <= MaxIndexValid && index >= MinIndexValid;
		}

		public void NextPage()
		{
			if (CurrentPage == TotalPages)
				CurrentPage = 1;
			else
				CurrentPage++;

			Refresh();
		}

		public void PreviousPage()
		{
			if (CurrentPage == 1)//1 base index
				CurrentPage = TotalPages;
			else
				CurrentPage--;

			Refresh();
		}

		public void Refresh()
		{
			TotalPages = (TotalCount + MaxPagedItems - 1) / MaxPagedItems;

			CollectionView.Refresh();
		}

		private void SetTotalPages(int value)
		{
			totalPages = value;
		}
	}
}