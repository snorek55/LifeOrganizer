using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Common.Extensions
{
	public static class CollectionViewExtensions
	{
		public static int Count(this ICollectionView collectionView)
		{
			return collectionView.Cast<object>().Count();
		}

		public static List<T> GetFilteredData<T>(this ICollectionView collectionView)
		{
			return collectionView.Cast<T>().ToList();
		}
	}
}