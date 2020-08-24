using Common.WPF;

using System;
using System.Globalization;
using System.Windows;

namespace MovOrg.GUI
{
	public class RankToVisibilityConverter : BaseConverter<RankToVisibilityConverter>
	{
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			int? rank = (int?)value;
			if (rank.HasValue)
				return Visibility.Visible;
			else
				return Visibility.Collapsed;
		}

		public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}