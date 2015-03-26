using NUnit.Framework;
using System.Windows.Forms;
using System.Threading;

namespace AbbyyLS.CAT.Function.Selenium.Tests.Editor.Character
{
	class EditorCharacterTest : EditorBaseTest
	{
		/// <summary>
		/// Конструктор теста
		/// </summary>
		/// <param name="browserName">Название браузера</param>
		public EditorCharacterTest(string browserName)
			: base(browserName)
		{

		}

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
