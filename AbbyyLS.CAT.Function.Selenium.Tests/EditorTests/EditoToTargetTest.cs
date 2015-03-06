using NUnit.Framework;

namespace AbbyyLS.CAT.Function.Selenium.Tests.Editor
{
	class EditoToTargetTest : EditorBaseTest
	{
		/// <summary>
		/// Конструктор теста
		/// </summary>
		/// <param name="browserName">Название браузера</param>
		public EditoToTargetTest(string browserName)
			: base(browserName)
		{

		}

		/// <summary>
		/// Метод тестирования кнопки копирования оригинала в перевод
		/// </summary>
		[Test]
		public void ToTargetButtonTest()
		{
			// Кнопка Copy, проверить содержимое Target
			ToTargetButton();
		}

		/// <summary>
		/// Метод тестирования хоткея копирования оригинала в перевод
		/// </summary>
		[Test]
		public void ToTargetHotkeyTest()
		{
			// Хоткей Copy, проверить содержимое Target
			ToTargetHotkey();
		}

	}
}
