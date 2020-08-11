using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Common.Extensions
{
	public static class ObservableCollectionExtensions
	{
		public static void AddRange<T>(this ObservableCollection<T> observableCollection, IEnumerable<T> listToAdd)
		{
			foreach (var item in listToAdd)
			{
				observableCollection.Add(item);
			}
		}
	}
}