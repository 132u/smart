using System;
using System.Linq;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	public static class RandomString
	{
		private static Random _random = new Random(Environment.TickCount);

		public static string GetDigits(int length)
		{
			string digits = "0123456789";

			return buildString(digits, length);
		}

		public static string GetLetters(int length)
		{
			string letters = "abcdefghijklmnopqrstuvwxyz";

			return buildString(letters, length);
		}

		private static string buildString(string chars, int length) 
		{
			var random = new Random();
			var result = new string(
				Enumerable.Repeat(chars, length)
						  .Select(s => s[random.Next(s.Length)])
						  .ToArray());

			return result;
		}
	}
}
