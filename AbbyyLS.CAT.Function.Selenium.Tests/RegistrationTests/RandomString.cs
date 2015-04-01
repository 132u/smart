using System;
using System.Text.RegularExpressions;
using System.Text;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	public static class RandomString
	{
		private static Random _random = new Random(Environment.TickCount);

		public static string GenerateRandomString()
		{
			return Guid.NewGuid().ToString().Replace("-", string.Empty);
		}

		public static string Generate(int numberOfDigits)
		{
			return Regex.Replace(GenerateRandomString(), "[A-Za-z ]", string.Empty).Substring(0, numberOfDigits);
		}
		
		public static string GenerateString(int length)
		{
			string chars = "abcdefghijklmnopqrstuvwxyz";
			StringBuilder builder = new StringBuilder(length);

			for (int i = 0; i < length; ++i)
				builder.Append(chars[_random.Next(chars.Length)]);

			return builder.ToString();
		}
	}
}
