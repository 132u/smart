using System.Threading;

using NUnit.Framework;

using AbbyyLS.CAT.Function.Selenium.Tests.Driver;

namespace AbbyyLS.CAT.Function.Selenium.Tests.Editor.Concordance
{
	/// <summary>
	/// Группа тестов кнопок редактора
	/// </summary>
	[Category("Standalone")]
	public class EditorUnfinishedTest<TWebDriverSettings> : EditorBaseTest<TWebDriverSettings> where TWebDriverSettings : IWebDriverSettings, new()
	{
		/// <summary>
		/// Метод тестирования кнопки поиска следующего незаконченного сегмента
		/// </summary>
		[Test]
		public void UnfinishedButtonNextSegmentTest()
		{
			// Добавить текст в первый сегмент, подтвердить, проверка подтверждвения
			AddTranslationAndConfirm(1, "some words for example");

			// Переключаемся в первый сегмент
			EditorPage.ClickTargetCell(1);

			// Нажать кнопку поиска следующего незаконченного сегмента
			EditorPage.ClickUnfinishedBtn();

			Thread.Sleep(1000);

			EditorPage.ClickToggleBtn();

			// Проверить, активен второй сегмент
			Assert.IsTrue(
				EditorPage.GetIsCursorInSourceCell(2),
				"Ошибка: Произошел переход не на нужный (второй) сегмент.");
		}

		/// <summary>
		/// Метод тестирования хоткея поиска следующего незаконченного сегмента
		/// </summary>
		[Test]
		[Category("SCAT_102")]
		public void UnfinishedHotkeyNextSegmentTest()
		{
			EditorPage.ClearTarget(2);
			// Добавить текст в первый сегмент, подтвердить, проверка подтверждвения
			AddTranslationAndConfirm(1, "some words for example");

			// Переключаемся в первый сегмент
			EditorPage.ClickTargetCell(1);

			// Нажать хоткей поиска следующего незаконченного сегмента
			EditorPage.NextUnfinishedSegmentByHotkey(1);

			Thread.Sleep(1000);

			EditorPage.ClickToggleBtn();

			// Проверить, активен второй сегмент
			Assert.IsTrue(
				EditorPage.GetIsCursorInSourceCell(2),
				"Ошибка: Произошел переход не на нужный (второй) сегмент.");

		}

		/// <summary>
		/// Метод тестирования кнопки поиска следующего незаконченного сегмента
		/// </summary>
		[Test]
		public void UnfinishedButtonSkipSegmentTest()
		{
			// Добавить текст во второй сегмент, подтвердить, проверка подтверждвения
			AddTranslationAndConfirm(2, "some words for example");

			// Переключаемся в первый сегмент
			EditorPage.ClickTargetCell(1);

			// Нажать кнопку поиска следующего незаконченного сегмента
			EditorPage.ClickUnfinishedBtn();

			Thread.Sleep(1000);

			EditorPage.ClickToggleBtn();

			// Проверить, активен третий сегмент
			Assert.IsTrue(
				EditorPage.GetIsCursorInSourceCell(3),
				"Ошибка: Произошел переход не на нужный (третий) сегмент.");
		}

		/// <summary>
		/// Метод тестирования хоткея поиска следующего незаконченного сегмента
		/// </summary>
		[Test]
		[Category("SCAT_102")]
		public void UnfinishedHotkeySkipSegmentTest()
		{
			// Добавить текст во второй сегмент, подтвердить, проверка подтверждвения
			AddTranslationAndConfirm(2, "some words for example");

			// Переключаемся в первый сегмент
			EditorPage.ClickTargetCell(1);

			// Нажать хоткей поиска следующего незаконченного сегмента
			EditorPage.NextUnfinishedSegmentByHotkey(1);

			Thread.Sleep(1000);

			EditorPage.ClickToggleBtn();

			// Проверить, активен третий сегмент
			Assert.IsTrue(
				EditorPage.GetIsCursorInSourceCell(3),
				"Ошибка: Произошел переход не на нужный (третий) сегмент.");

		}
	}
}