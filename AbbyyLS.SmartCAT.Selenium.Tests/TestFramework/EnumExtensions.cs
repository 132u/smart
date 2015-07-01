using System;
using System.ComponentModel;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestFramework
{
	public static class EnumExtensions
	{
		public static string Description(this Enum value)
		{
			var descriptionAttribute = (DescriptionAttribute[])(value.GetType().GetField(value.ToString())).GetCustomAttributes(typeof(DescriptionAttribute), false);
			return descriptionAttribute.Length > 0 ? descriptionAttribute[0].Description : value.ToString();
		}
	}
}
