using NUnit.Framework;

namespace AbbyyLS.CAT.Function.Selenium.Tests.Editor.Tab
{
	/// <summary>
	/// Группа тестов кнопок редактора
	/// </summary>
	[Category("Standalone")]
	public class EditorTabTest : EditorBaseTest
	{
		/// <summary>
		/// Конструктор теста
		/// </summary>
		/// <param name="browserName">Название браузера</param>
		public EditorTabTest(string browserName)
			: base(browserName)
		{

		}

		/// <summary>
		/// Метод тестирования кнопки перемещения курсора между полями source и target без хоткея
		/// </summary>
		[Test]
		public void TabButtonTest()
		{
			var segmentNumber = 1;

			// Перешли из Target в Source по кнопке
			SourceTargetSwitchButton(segmentNumber);

			// Проверить где находится курсор, и если в поле source, то все ок
			Assert.True(
				EditorPage.GetIsCursorInSourceCell(segmentNumber),
				"Ошибка: после кнопки Toggle не перешли в Target");
		}

		/// <summary>
		/// Метод тестирования хоткея перемещения курсора между полями source и target
		/// </summary>
		[Test]
		[Category("SCAT_102")]
		public void TabHotkeyTest()
		{
			var segmentNumber = 1;

			// Перешли из Target в Source по хоткею
			SourceTargetSwitchHotkey(segmentNumber);

			// Проверить где находится курсор, и если в поле source, то все ок
			Assert.True(EditorPage.GetIsCursorInSourceCell(segmentNumber),
				"Ошибка: после хоткея Toggle не перешли в Target");
		}

	}
}