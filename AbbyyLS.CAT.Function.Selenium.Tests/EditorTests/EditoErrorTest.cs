using NUnit.Framework;

namespace AbbyyLS.CAT.Function.Selenium.Tests.Editor.Error
{
	class EditoErrorTest : EditorBaseTest
	{
		/// <summary>
		/// Конструктор теста
		/// </summary>
		/// <param name="browserName">Название браузера</param>
		public EditoErrorTest(string browserName)
			: base(browserName)
		{

		}

		/// <summary>
		/// Проверка работы в редакторе кнопки поиска ошибки терминологии
		/// </summary>
		[Test]
		public void FindErrorButtonTest()
		{
			// Кликнуть по кнопке
			EditorPage.ClickFindErrorBtn();

			// Проверка, что открылась форма
			Assert.IsTrue(
				EditorPage.WaitMessageFormDisplay(),
				"Ошибка: Форма с сообщением не открылась.");
		}

		/// <summary>
		/// Проверка работы в редакторе хоткея поиска ошибки терминологии
		/// </summary>
		[Test]
		public void FindErrorHotkeyTest()
		{
			// Нажать хоткей
			EditorPage.AddTextTarget(1, OpenQA.Selenium.Keys.F7);

			// Проверка, что открылась форма
			Assert.IsTrue(
				EditorPage.WaitMessageFormDisplay(),
				"Ошибка: Форма с сообщением не открылась.");
		}

	}
}
