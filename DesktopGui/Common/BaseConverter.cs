using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace DesktopGui.Common
{
	public abstract class BaseConverter<T> : MarkupExtension, IValueConverter where T : class, new()
	{
		#region Private Members

		private static T converter = null;

		#endregion Private Members

		#region Markup Extension

		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return converter ?? (converter = new T());
		}

		#endregion Markup Extension

		#region Value converter

		public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

		public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);

		#endregion Value converter
	}
}