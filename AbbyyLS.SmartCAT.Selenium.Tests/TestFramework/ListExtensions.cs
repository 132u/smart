using System.Collections.Generic;
using System.Linq;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestFramework
{
	/// <summary>
	/// Дополнительные методы для работы со списком
	/// </summary>
	public static class ListExtensions
	{
		/// <summary>
		/// Сравнить два строковых списка
		/// </summary>
		/// <param name="firstList">первый список</param>
		/// <param name="secondList">второй список</param>
		public static bool Match(this List<string> firstList, List<string> secondList)
		{
			if (firstList == null && secondList == null) 
			{
				return true;
			}

			return firstList != null && secondList != null
				&& firstList.Count() == secondList.Count()
				&& !firstList.Except(secondList).Any();
		}
	}
}
