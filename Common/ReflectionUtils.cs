using System;
using System.Reflection;

namespace Common
{
	public static class ReflectionUtils
	{
		//https://stackoverflow.com/questions/214086/how-can-you-get-the-names-of-method-parameters
		public static string GetParamName(Action a, int index)
		{
			var method = a.Method;

			return GetParamNameFromMethodInfo(method, index);
		}

		public static string GetParamName<T1>(Action<T1> a, int index)
		{
			var method = a.Method;

			return GetParamNameFromMethodInfo(method, index);
		}

		private static string GetParamNameFromMethodInfo(MethodInfo method, int index)
		{
			string retVal = string.Empty;

			if (method != null && method.GetParameters().Length > index)
				retVal = method.GetParameters()[index].Name;

			return retVal;
		}
	}
}