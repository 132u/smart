using NUnit.Framework;
using System.Windows.Forms;

namespace AbbyyLS.CAT.Function.Selenium.Tests.Editor.Concordance
{
	/// <summary>
	/// Группа тестов кнопок редактора
	/// </summary>
	[Category("Standalone")]
	public class EditorConcordanceTest : EditorBaseTest
	{
		/// <summary>
		/// Конструктор теста
		/// </summary>
		/// <param name="browserName">Название браузера</param>
		public EditorConcordanceTest(string browserName)
			: base(browserName)
		{

		}

		/// <summary>
		/// Проверка работы в редакторе кнопки конкордного поиска
		/// </summary>
		[Test]
		public void ConcordanceSearchButtonTest()
		{
			// Кликнуть по кнопке
			EditorPage.ClickConcordanceBtn();

			// Проверка, что открылся поиск
			Assert.IsTrue(
				EditorPage.WaitConcordanceSearchDisplay(),
				"Ошибка: Поиск не открылся.");
		}

		/// <summary>
		/// Проверка работы в редакторе хоткея конкордного поиска
		/// </summary>
		[Test]
		public void ConcordanceSearchHotkeyTest()
		{
			// Клик в сегменте
			EditorPage.ClickInSegment(1);
			// Хоткей Ctrl k
			SendKeys.SendWait(@"^{k}");

			// Проверка, что открылся поиск
			Assert.IsTrue(
				EditorPage.WaitConcordanceSearchDisplay(),
				"Ошибка: Поиск не открылся.");
		}
	}
}