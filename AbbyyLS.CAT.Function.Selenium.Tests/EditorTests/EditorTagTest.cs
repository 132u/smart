using NUnit.Framework;

namespace AbbyyLS.CAT.Function.Selenium.Tests.Editor.Tag
{
	class EditorTagTest : EditorBaseTest
	{
				/// <summary>
		/// Конструктор теста
		/// </summary>
		/// <param name="browserName">Название браузера</param>
		public EditorTagTest(string browserName)
			: base(browserName)
		{

		}
		/// <summary>
		/// Проверка работы в редакторе добавления символа переноса строки по кнопке
		/// </summary>
		[Test]
		public void TagButtonTest()
		{
			// Кликнуть по кнопке добавления символа переноса строки
			EditorPage.ClickInsertTagBtn();

			// Проверка, что в ячейке появился символ переноса строки
			Assert.IsTrue(
				EditorPage.GetIsTagPresent(1),
				"Ошибка: в ячейке Target не появился символ переноса строки");
		}

		/// <summary>
		/// Проверка работы в редакторе добавления символа переноса строки по хоткею
		/// </summary>
		[Test]
		public void TagHotkeyTest()
		{
			// Хоткей добавления символа переноса строки
			EditorPage.AddTextTarget(1, OpenQA.Selenium.Keys.F8);

			// Проверка, что в ячейке появился символ переноса строки
			Assert.IsTrue(
				EditorPage.GetIsTagPresent(1),
				"Ошибка: в ячейке Target не появился символ переноса строки");
		}

	}
}
