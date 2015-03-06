using NUnit.Framework;

namespace AbbyyLS.CAT.Function.Selenium.Tests.Editor.Another
{
	internal class EditorAnotherTest : EditorBaseTest
	{
		/// <summary>
		/// Конструктор теста
		/// </summary>
		/// <param name="browserName">Название браузера</param>
		public EditorAnotherTest(string browserName)
			: base(browserName)
		{

		}

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
