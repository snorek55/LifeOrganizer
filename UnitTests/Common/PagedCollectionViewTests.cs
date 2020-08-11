using Common.Adapters;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.ObjectModel;

namespace Tests.Unit
{
	[TestClass]
	public class PagedCollectionViewTests
	{
		private readonly ObservableCollection<string> TestList = new ObservableCollection<string>
		{
			"1",
			"2",
			"3",
			"4"
		};

		private PagedCollectionView<string> pagedView;

		[TestInitialize]
		public void Initialize()
		{
			pagedView = new PagedCollectionView<string>(TestList, 2);
		}

		[TestMethod]
		public void InitialValuesAreCorrect()
		{
			Assert.AreEqual(4, TestList.Count);
		}

		[TestMethod]
		public void TotalPages_AreCorrectForExactNumbers()
		{
			Assert.AreEqual(2, pagedView.TotalPages);
		}

		[TestMethod]
		public void TotalPages_RoundsUpIfNecessary()
		{
			pagedView.MaxPagedItems = 3;
			Assert.AreEqual(2, pagedView.TotalPages);
		}

		[TestMethod]
		public void ShouldShowFirstPageOnCreation()
		{
			Assert.AreEqual(2, pagedView.CurrentNumItemsShowing);
		}

		[TestMethod]
		public void WhenChangingMaxPagedItems_ShouldChangedItemsShown()
		{
			pagedView.MaxPagedItems = 3;
			Assert.AreEqual(3, pagedView.CurrentNumItemsShowing);
		}

		[TestMethod]
		public void WhenNextPage_ShouldShowNextPage()
		{
			pagedView.NextPage();

			Assert.AreEqual(2, pagedView.CurrentNumItemsShowing);
			int i = 2;
			foreach (var item in pagedView.CollectionView)
			{
				Assert.AreEqual(pagedView.SourceCollection[i], item, $"Item with index {i} not equal. Expected {pagedView.SourceCollection[i]} => Actual:{item}");
				i++;
			}
		}
	}
}