using NUnit.Framework;

using AbbyyLS.CAT.Function.Selenium.Tests.Driver;

namespace AbbyyLS.CAT.Function.Selenium.Tests.Editor.Tag
{
	class EditorTagTests<TWebDriverSettings> : EditorBaseTest<TWebDriverSettings> where TWebDriverSettings : IWebDriverSettings, new()
	{
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
