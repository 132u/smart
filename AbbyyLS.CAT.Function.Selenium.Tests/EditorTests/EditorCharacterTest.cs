using System.Threading;
using System.Windows.Forms;

using NUnit.Framework;

using AbbyyLS.CAT.Function.Selenium.Tests.Driver;

namespace AbbyyLS.CAT.Function.Selenium.Tests.Editor.Character
{
	class EditorCharacterTest<TWebDriverSettings> : EditorBaseTest<TWebDriverSettings> where TWebDriverSettings : IWebDriverSettings, new()
	{
		/// <summary>
		/// Проверка работы в редакторе кнопки вставки спецсимвола
		/// </summary>
		[Test]
		public void CharacterButtonTest()
		{
			// Кликнуть по кнопке
			EditorPage.ClickCharacterBtn();

			// Проверка, что открылась форма
			Assert.IsTrue(
				EditorPage.WaitCharFormDisplay(),
				"Ошибка: Форма выбора спецсимвола не открылась.");
		}

		/// <summary>
		/// Проверка работы в редакторе хоткея вставки спецсимвола
		/// </summary>
		[Test]
		[Category("SCAT_102")]
		public void CharacterHotkeyTest()
		{
			// Клик в сегменте
			EditorPage.ClickInSegment(1);
			// Sleep не убирать, иначе хоткей не работает
			Thread.Sleep(1000);
			// Хоткей Ctrl Shift i
			SendKeys.SendWait(@"^+{i}");
			// Проверка, что открылась форма
			Assert.IsTrue(
				EditorPage.WaitCharFormDisplay(),
				"Ошибка: Форма выбора спецсимвола не открылась.");
		}
	}
}
