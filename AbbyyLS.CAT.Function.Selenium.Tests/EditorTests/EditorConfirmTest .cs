using NUnit.Framework;

namespace AbbyyLS.CAT.Function.Selenium.Tests.Editor.Confirm
{
	/// <summary>
	/// Группа тестов кнопок редактора
	/// </summary>
	[Category("Standalone")]
	public class EditorConfirmTest : EditorBaseTest
	{
		/// <summary>
		/// Конструктор теста
		/// </summary>
		/// <param name="browserName">Название браузера</param>
		public EditorConfirmTest(string browserName)
			: base(browserName)
		{

		}

		/// <summary>
		/// Метод тестирования кнопки подтвеждения сегмента
		/// </summary>
		[Test]
		public void ConfirmButtonTest()
		{
			// Добавить текст, подтвердить, проверка подтверждвения
			AddTranslationAndConfirm();
		}

		/// <summary>
		/// Проверка работы в редакторе Confirm по хоткею Ctrl+Enter
		/// </summary>
		[Test]
		public void ConfirmHotkeyTest()
		{
			// Написать текст в первом сегменте в target
			EditorPage.AddTextTarget(1, "some words for example");
			EditorPage.PressHotKey(1, OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.Return);
			// Убедиться что сегмент подтвержден
			Assert.IsTrue(WaitSegmentConfirm(1), "Ошибка: Подтверждение (Confirm) не прошло");
		}

	}
}