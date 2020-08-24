using AutoMapper;

namespace Common.Setup
{
	public class StringToNullableFloatValueConverter : IValueConverter<string, float?>
	{
		public float? Convert(string sourceMember, ResolutionContext context)
		{
			bool isOk = float.TryParse(sourceMember, out float f);
			if (isOk)
				return f;
			else
				return null;
		}
	}
}