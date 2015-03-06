using NUnit.Framework;

namespace AbbyyLS.CAT.Function.Selenium.Tests.Editor.UndoRedo
{
	/// <summary>
	/// Группа тестов кнопок редактора
	/// </summary>
	public class EditorUndoRedoButtonsTest : EditorBaseTest
	{
		/// <summary>
		/// Конструктор теста
		/// </summary>
		/// <param name="browserName">Название браузера</param>
		public EditorUndoRedoButtonsTest(string browserName)
			: base(browserName)
		{

		}
		/// <summary>
		/// Метод тестирования кнопки Undo и Redo при вводе текста
		/// </summary>
		[Test]
		public void UndoRedoButtonTextTest()
		{
			const int segmentNumber = 1;
			const string text = "some text";
			const string textundo = "";

			// Вводим текст в первый сегмент
			EditorPage.AddTextTarget(segmentNumber, text);

			// Нажать кнопку отмены
			EditorPage.ClickUndoBtn();

			// Проверить, что в target убралась одна буква
			var targetxt = EditorPage.GetTargetText(segmentNumber);
			Assert.AreEqual(textundo, targetxt, "Ошибка: Target не очистился после клика по кнопке Undo.");

			// Нажать кнопку возврата отмененного действия
			EditorPage.ClickRedoBtn();

			// Убедиться, что в текст соответствует введенному
			targetxt = EditorPage.GetTargetText(segmentNumber);
			Assert.AreEqual(text, targetxt, "Ошибка: Текст в Target не восстановился после клика по кнопке Redo.");
		}

		/// <summary>
		/// Метод тестирования хоткея Undo и Redo при вводе текста
		/// </summary>
		[Test]
		public void UndoRedoHotkeyTextTest()
		{
			const int segmentNumber = 1;
			const string text = "some text";
			const string textundo = "";

			// Вводим текст в первый сегмент
			EditorPage.AddTextTarget(segmentNumber, text);

			// Нажать хоткей отмены
			EditorPage.UndoByHotkey(segmentNumber);

			// Убедиться, что в target нет текста
			var targetxt = EditorPage.GetTargetText(segmentNumber);

			Assert.AreEqual(textundo, targetxt, "Ошибка: Target не очистился после клика Hotkey Undo(Ctrl + z).");

			// Нажать хоткей возврата отмененного действия
			EditorPage.RedoByHotkey(segmentNumber);

			// Убедиться, что в текст соответствует введенному
			targetxt = EditorPage.GetTargetText(segmentNumber);
			Assert.AreEqual(text, targetxt, "Ошибка: Текст в Target не восстановился после клика Hotkey Redo(Ctrl + y).");
		}

		/// <summary>
		/// Метод тестирования кнопки Undo и Redo при подтверждении сегмента
		/// </summary>
		[Test]
		public void UndoRedoButtonSegmentTest()
		{
			const int segmentNumber = 1;
			var sourcetxt = EditorPage.GetSourceText(segmentNumber);

			// Копируем текст в первый сегмент
			ToTargetButton();

			// Подтверждаем
			EditorPage.ClickConfirmBtn();
			WaitSegmentConfirm(segmentNumber);

			// Нажать кнопку отмены
			EditorPage.ClickUndoBtn();
			
			// Убедиться, что текст в target такой же как в source
			var targetxt = EditorPage.GetTargetText(segmentNumber);
			Assert.AreEqual(
				sourcetxt, 
				targetxt, 
				"Ошибка: Текст не соответствует введенному.");

			// Убедиться, что сегмент стал неподтвержденным
			Assert.IsFalse(EditorPage.GetIsSegmentConfirm(segmentNumber),
				"Ошибка: Сегмент не должен быть подтвержденным.");

			// Нажать кнопку возврата отмененного действия
			EditorPage.ClickRedoBtn();

			// Убедиться, что текст в target такой же как в source
			targetxt = EditorPage.GetTargetText(segmentNumber);
			Assert.AreEqual(
				sourcetxt, 
				targetxt, 
				"Ошибка: Текст не соответствует введенному.");

			// Убедиться, что сегмент стал подтвержденным
			Assert.IsTrue(
				EditorPage.GetIsSegmentConfirm(segmentNumber),
				"Ошибка: Сегмент должен быть подтвержденным.");
		}

		/// <summary>
		/// Метод тестирования хоткея Undo и Redo при подтверждении сегмента
		/// </summary>
		[Test]
		public void UndoRedoHotkeySegmentTest()
		{
			const int segmentNumber = 1;
			var sourcetxt = EditorPage.GetSourceText(segmentNumber);

			// Копируем текст в первый сегмент
			ToTargetButton(segmentNumber);

			// Подтверждаем
			EditorPage.ClickConfirmBtn();

			// Нажать хоткей отмены
			EditorPage.UndoByHotkey(segmentNumber);

			// Убедиться, что текст в target такой же как в source
			var targetxt = EditorPage.GetTargetText(segmentNumber);
			Assert.AreEqual(
				sourcetxt, 
				targetxt, 
				"Ошибка: Текст не соответствует введенному.");

			// Убедиться, что сегмент стал неподтвержденным
			Assert.IsFalse(
				EditorPage.GetIsSegmentConfirm(segmentNumber),
				"Ошибка: Сегмент не должен быть подтвержденным.");

			// Нажать хоткей возврата отмененного действия
			EditorPage.RedoByHotkey(segmentNumber);

			// Убедиться, что текст в target такой же как в source
			targetxt = EditorPage.GetTargetText(segmentNumber);
			Assert.AreEqual(sourcetxt, targetxt, "Ошибка: Текст не соответствует введенному.");

			// Убедиться, что сегмент стал подтвержденным
			Assert.IsTrue(
				EditorPage.GetIsSegmentConfirm(segmentNumber),
				"Ошибка: Сегмент должен быть подтвержденным.");
		}

		/// <summary>
		/// Метод тестирования кнопки Undo и Redo при подстановке из CAT-панели
		/// </summary>
		[Test]
		public void UndoRedoButtonCatTest()
		{
			var segmentNumber = 1;
			
			// Почистить таргет
			EditorPage.AddTextTarget(segmentNumber, "");

			//Выбираем первый сегмент
			EditorPage.ClickTargetCell(segmentNumber);

			// Подставляем перевод из CAT
			PasteFromCatReturnCatLineNumber(1, EditorPageHelper.CAT_TYPE.TM);

			// Нажать кнопку отмены
			EditorPage.ClickUndoBtn();

			// Проверить, что в target пусто
			var targetxt = EditorPage.GetTargetText(segmentNumber);
			Assert.AreEqual("", targetxt, "Ошибка: после Undo в Target есть текст");

			// Нажать кнопку возврата отмененного действия
			EditorPage.ClickRedoBtn();

			// Проверить, что в target не пусто
			targetxt = EditorPage.GetTargetText(segmentNumber);
			Assert.AreNotEqual("", targetxt, "Ошибка: после Redo в Target нет текста");
		}

		/// <summary>
		/// Метод тестирования хоткея Undo и Redo при подстановке из CAT-панели
		/// </summary>
		[Test]
		public void UndoRedoHotkeyCatTest()
		{
			var segmentNumber = 1;

			// Почистить таргет
			EditorPage.AddTextTarget(segmentNumber, "");

			//Выбираем первый сегмент
			EditorPage.ClickTargetCell(segmentNumber);

			// Подставляем перевод из CAT
			PasteFromCatReturnCatLineNumber(1, EditorPageHelper.CAT_TYPE.TM);

			// Нажать хоткей отмены
			EditorPage.UndoByHotkey(segmentNumber);

			// Проверить, что в target пусто
			var targetxt = EditorPage.GetTargetText(segmentNumber);
			Assert.AreEqual("", targetxt, "Ошибка: после Undo в Target есть текст");

			// Нажать хоткей возврата отмененного действия
			EditorPage.RedoByHotkey(segmentNumber);

			// Проверить, что в target не пусто
			targetxt = EditorPage.GetTargetText(segmentNumber);
			Assert.AreNotEqual("", targetxt, "Ошибка: после Redo в Target нет текста");

			// Почистить таргет
			EditorPage.AddTextTarget(segmentNumber, "");
		}
	}
}
