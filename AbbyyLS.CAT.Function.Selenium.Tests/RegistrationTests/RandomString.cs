using System;
using System.Text.RegularExpressions;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	public static class RandomString
	{
		/// <summary>
		/// Сгенерировать рандомную строку
		/// </summary>
		public static string GenerateRandomString()
		{
			return Guid.NewGuid().ToString().Replace("-", string.Empty);
		}

		/// <summary>
		/// Сгенерировать рандомную строку
		/// </summary>
		public static string Generate(int numberOfDigits)
		{
			return Regex.Replace(GenerateRandomString(), "[A-Za-z ]", string.Empty).Substring(0, numberOfDigits);
		}
	}
}
