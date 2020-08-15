using Common.Adapters;
using Common.WPF;

using MahApps.Metro.IconPacks;

using System;
using System.Globalization;

namespace Main.GUI.Converters
{
	public class MainMenuIconConverter : BaseConverter<MainMenuIconConverter>
	{
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			switch ((MainMenuIconType)value)
			{
				case MainMenuIconType.Movies:
					return new PackIconFeatherIcons() { Kind = PackIconFeatherIconsKind.Film };

				case MainMenuIconType.Games:
					return new PackIconMaterialDesign() { Kind = PackIconMaterialDesignKind.VideogameAsset };

				case MainMenuIconType.Diet:
					return new PackIconZondicons() { Kind = PackIconZondiconsKind.LocationFood };

				case MainMenuIconType.Gym:
					return new PackIconFontAwesome() { Kind = PackIconFontAwesomeKind.DumbbellSolid };

				case MainMenuIconType.Options:
					return new PackIconFontAwesome() { Kind = PackIconFontAwesomeKind.CogSolid };

				case MainMenuIconType.About:
					return new PackIconMaterial() { Kind = PackIconMaterialKind.Information };

				default:
					return null;
			}
		}

		public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}