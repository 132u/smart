using NUnit.Framework;

namespace AbbyyLS.CAT.Function.Selenium.Tests.Editor.ToTarget
{
	class EditorToTargetTest : EditorBaseTest
	{
		/// <summary>
		/// Конструктор теста
		/// </summary>
		/// <param name="browserName">Название браузера</param>
		public EditorToTargetTest(string browserName)
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
		[Category("SCAT_102")]
		public void ToTargetHotkeyTest()
		{
			// Хоткей Copy, проверить содержимое Target
			ToTargetHotkey();
		}

	}
}
