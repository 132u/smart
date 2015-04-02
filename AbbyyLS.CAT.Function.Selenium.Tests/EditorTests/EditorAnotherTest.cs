using NUnit.Framework;
using AbbyyLS.CAT.Function.Selenium.Tests.Driver;

namespace AbbyyLS.CAT.Function.Selenium.Tests.Editor.Another
{
	internal class EditorAnotherTest<TWebDriverSettings> : EditorBaseTest<TWebDriverSettings> where TWebDriverSettings : IWebDriverSettings, new()
	{
		/// <summary>
		/// Тестирования кнопки "Back" в редакторе
		/// </summary>
		[Test]
		public void HomeButtonTest()
		{
			// Кнопка Back, проверка перехода
			EditorClickHomeBtn();
		}

		/// <summary>
		/// Проверка работы в редакторе кнопки открытия словаря
		/// </summary>
		[Test]
		public void DictionaryButtonTest()
		{
			OpenEditorDictionary();
		}
	}
}
