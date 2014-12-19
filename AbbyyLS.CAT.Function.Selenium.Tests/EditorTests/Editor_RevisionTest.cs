using System;
using System.ComponentModel;
using System.Threading;
using NUnit.Framework;
using System.Collections.Generic;

namespace AbbyyLS.CAT.Function.Selenium.Tests.Editor.Revisions
{
	/// <summary>
	/// Группа тестов для проверки истории версий в редакторе
	/// </summary>
	public class Editor_RevisionTest : BaseTest
	{
		/// <summary>
		/// Конструктор теста
		/// </summary>
		/// <param name="browserName">Название браузера</param>
		public Editor_RevisionTest(string browserName)
			: base(browserName)
		{

		}

		// название проекта для проведения тестов
		protected string _projectNameRevisionsTest = "RevisionsTest" + "_" + DateTime.UtcNow.Ticks;

		// флаг создан ли проект (создается один раз перед всеми тестами)
		private bool _projectCreated;

		/// <summary>
		/// Старт тестов, переменные
		/// </summary>
		[SetUp]
		public void SetupTest()
		{
			// Не выходить из браузера после теста
			QuitDriverAfterTest = false;

			if (!_projectCreated)
			{
				GoToWorkspace();

				// создаем документ с нужным файлом, нужной ТМ, подкючаем МТ и глоссарий
				CreateProject(_projectNameRevisionsTest, TestFile.LongTxtFile,
				true, TestFile.LongTmxFile,
					Workspace_CreateProjectDialogHelper.SetGlossary.New, "",
					true, Workspace_CreateProjectDialogHelper.MT_TYPE.DefaultMT,
					false,
					2);

				Thread.Sleep(2000);

				// Открываем диалог выбора исполнителя
				OpenAssignDialog(_projectNameRevisionsTest);

				// Выбор в качестве исполнителя для первой задачи первого юзера
				SetResponsible(1, UserName, false);

				// Выбор в качестве исполнителя для второй задачи второго юзера
				SetResponsible(2, UserName2, false);
				ResponsiblesDialog.ClickCloseBtn();

				// Открытие страницы проекта
				OpenProjectPage(_projectNameRevisionsTest);

				// Подтверждение назначения
				ProjectPage.ClickAllAcceptBtns();
				Thread.Sleep(1000);

				// Открываем документ
				OpenDocument();
				Thread.Sleep(1000);

				_projectCreated = true;
			}
			//если мы не в редакторе, то зайти в редактор
			else
			{
				if (WorkspacePage.CheckIfWorkspace())
				{
					// Открытие страницы проекта
					OpenProjectPage(_projectNameRevisionsTest);

					// Открываем документ
					OpenDocument();
					Thread.Sleep(1000);
				}
			}
		}

		/// <summary>
		/// ТЕСТ: проверка неактивности кнопки Rollback , если ни одна из ревизий не выбрана
		/// </summary>
		[Test]
		public void RollbackBtnDisabledTest()
		{
			var segmentNumber = 1;
			EditorPage.ScrollToRequiredSegment(segmentNumber);

			// Добавить текст в Target
			var text = "Text" + DateTime.Now.Ticks;
			AddTextTarget(segmentNumber, text);
			// Дождаться автосохранения
			AutoSave();

			// Вернуться в сегмент
			ClickSegmentTarget(segmentNumber);

			// Открыть вкладку Ревизии
			OpenRevisionTab();

			// Проверить, что количество ревизий больше 0 (перевод появился в ревизиях)
			Assert.IsTrue(
				RevisionPage.GetRevisionListCount() > 0,
				"Ошибка: перевод не появился в ревизиях");

			// Проверить, что кнопка заблокирована
			Assert.IsFalse(
				RevisionPage.GetIsRollbackBtnEnabled(),
				"Ошибка: кнопка Rollback должна быть заблокирована");
		}

		/// <summary>
		/// ТЕСТ: проверка активности кнопки Rollback , если ревизия выбрана
		/// </summary>
		[Test]
		public void RollbackBtnEnabledTest()
		{
			var segmentNumber = 2;
			EditorPage.ScrollToRequiredSegment(segmentNumber);

			// Добавить текст в Target
			var text = "Text" + DateTime.Now.Ticks;
			AddTextTarget(segmentNumber, text);

			// Дождаться автосохранения
			AutoSave();

			// Вернуться в сегмент
			ClickSegmentTarget(segmentNumber);

			// Открыть вкладку Ревизии
			OpenRevisionTab();

			// Проверить, что количество ревизий больше 0 (перевод появился в ревизиях)
			Assert.IsTrue(
				RevisionPage.GetRevisionListCount() > 0,
				"Ошибка: перевод не появился в ревизиях");

			// Выбрать ревизию
			Assert.IsTrue(
				RevisionPage.ClickRevision(1),
				"Ошибка: ревизии нет");

			// Проверить, что кнопка не заблокирована
			Assert.IsTrue(
				RevisionPage.GetIsRollbackBtnEnabled(),
				"Ошибка: кнопка Rollback не должна быть заблокирована");
		}

		/// <summary>
		/// ТЕСТ: проверка сортировки по дате/времени по кнопке Time
		/// </summary>
		[Test]
		public void TimeBtnTest()
		{
			const int segmentNumber = 3;
			EditorPage.ScrollToRequiredSegment(segmentNumber);

			// Количество 
			const int revisionNumberCount = 3;

			// Добавить ревизии
			AddTranslationsToSegment(segmentNumber, revisionNumberCount);

			// Проверить количество ревизий
			Assert.IsTrue(
				RevisionPage.GetRevisionListCount() >= revisionNumberCount,
				"Ошибка: количество ревизий не совпадает");

			// Текст и время первой ревизии До
			var revisionTextBefore = RevisionPage.GetRevisionText(1);
			var revisionTimeBefore = RevisionPage.GetRevisionTime(1);

			Console.WriteLine("до: " + revisionTextBefore + " " + revisionTimeBefore);

			// Кликнуть Time
			RevisionPage.ClickTimeToSort();

			// Текст и время первой ревизии После
			var revisionTextAfter = RevisionPage.GetRevisionText(1);
			var revisionTimeAfter = RevisionPage.GetRevisionTime(1);

			Console.WriteLine("после: " + revisionTextAfter + " " + revisionTimeAfter);

			// Изменился ли текст
			var isTextChanged = revisionTextBefore != revisionTextAfter;
			// Изменилось ли время
			var isTimeChanged = revisionTimeBefore != revisionTimeAfter;

			var isOk = isTextChanged && isTimeChanged;
			var errorMessage = "";

			// Проверить, что текст изменился
			if (!isTextChanged)
			{
				errorMessage += "Ошибка: текст не изменился\n";
			}

			// Проверить, что время изменилось
			if (!isTimeChanged)
			{
				errorMessage += "Ошибка: время не изменилось\n";
			}

			// Вывести ошибки
			Assert.IsTrue(isOk, errorMessage);
		}

		/// <summary>
		/// ТЕСТ: проверка хоткея Автосохранение
		/// </summary>
		[Test]
		public void AutoSaveTest()
		{
			var segmentNumber = 4;
			EditorPage.ScrollToRequiredSegment(segmentNumber);

			// Добавить текст в Target
			var text = "Text" + DateTime.Now.Ticks;
			AddTextTarget(segmentNumber, text);

			// Дождаться автосохранения
			AutoSave();

			// Вернуться в сегмент
			ClickSegmentTarget(segmentNumber);

			// Открыть вкладку Ревизии
			OpenRevisionTab();

			// Проверить, что количество ревизий больше 0 (перевод появился в ревизиях)
			Assert.IsTrue(
				RevisionPage.GetRevisionListCount() > 0,
				"Ошибка: перевод не появился в ревизиях");

			// Проверить тип
			Assert.AreEqual(
				Editor_RevisionPageHelper.RevisionType.AutoSave,
				RevisionPage.GetRevisionType(1),
				"Ошибка: тип ревизии не совпадает");
		}

		/// <summary>
		/// ТЕСТ: проверка хоткея Confirm
		/// </summary>
		/// <param name="segmentNumber">номер сегмента</param>
		/// <param name="byHotkeyOrButton">хоткей или кнопка</param>
		[Test, Sequential]
		public void ConfirmTest(
			[Values(5, 6)] 
			int segmentNumber,
			[Values(ByHotkey, ByButton)]
			string byHotkeyOrButton)
		{
			// Добавить текст в Target
			var text = "Text" + DateTime.Now.Ticks;
			EditorPage.ScrollToRequiredSegment(segmentNumber);
			AddTextTarget(segmentNumber, text);

			// Вернуться в сегмент
			ClickSegmentTarget(segmentNumber);

			// Подтвердить
			if (byHotkeyOrButton == ByHotkey)
			{
				// Нажать хоткей подтверждения
				ClickConfirmHotkey(segmentNumber);
			}

			else if (byHotkeyOrButton == ByButton)
			{
				// Нажать кнопку подтверждения
				ClickConfirmBtn(segmentNumber);
			}

			// Вернуться в сегмент
			ClickSegmentTarget(segmentNumber);

			// Открыть вкладку Ревизии
			OpenRevisionTab();

			// Проверить, что количество ревизий больше 0 (перевод появился в ревизиях)
			Assert.IsTrue(
				RevisionPage.GetRevisionListCount() > 0,
				"Ошибка: перевод не появился в ревизиях");

			// Проверить тип
			Assert.AreEqual(
				Editor_RevisionPageHelper.RevisionType.Confirmed,
				RevisionPage.GetRevisionType(1),
				"Ошибка: тип ревизии не совпадает");
		}

		/// <summary>
		/// ТЕСТ: подтвердить после автосохранения
		/// </summary>
		[Test]
		public void ConfirmAfterSaveTest()
		{
			// Добавить текст в Target
			var text = "Text" + DateTime.Now.Ticks;
			var segmentNumber = 7;
			EditorPage.ScrollToRequiredSegment(segmentNumber);

			AddTextTarget(segmentNumber, text);

			// Дождаться автосохранения
			AutoSave();

			// Вернуться в сегмент
			ClickSegmentTarget(segmentNumber);

			// Открыть вкладку Ревизии
			OpenRevisionTab();

			// Получить количество ревизий
			var revisionListCount = RevisionPage.GetRevisionListCount();
			Console.WriteLine("количество строк в ревизиях: " + revisionListCount);

			// Проверить, что количество больше 0 (перевод появился в ревизиях)
			Assert.IsTrue(
				revisionListCount > 0,
				"Ошибка: перевод не появился в ревизиях");

			// Проверить тип
			Assert.AreEqual(
				Editor_RevisionPageHelper.RevisionType.AutoSave,
				RevisionPage.GetRevisionType(1),
				"Ошибка: тип ревизии не совпадает");

			// Подтвердить
			ClickConfirmBtn(segmentNumber);
			// Вернуться
			ClickSegmentTarget(segmentNumber);

			// Количество ревизий
			var revisionListCountAfter = RevisionPage.GetRevisionListCount();

			bool isOk = true;
			string errorMessage = "";

			// Проверить, что количество ревизий не изменилось
			if (revisionListCountAfter != revisionListCount)
			{
				isOk = false;
				errorMessage += "Ошибка: количество ревизий изменилось\n";
			}

			// Проверить тип ревизии
			if (RevisionPage.GetRevisionType(1) != Editor_RevisionPageHelper.RevisionType.Confirmed)
			{
				isOk = false;
				errorMessage += "Ошибка: неправильный тип ревизии (д.б. Confirmed)";
			}

			// Вывести ошибки
			Assert.IsTrue(isOk, errorMessage);
		}

		/// <summary>
		/// ТЕСТ: вставка перевода из MT
		/// </summary>
		/// <param name="segmentNumber">номер сегмента</param>
		/// <param name="byHotkeyOrDoubleClick">хоткей или даблклик</param>
		[Test, Sequential]
		public void PasteFromMTTest(
			[Values(8, 9)] 
			int segmentNumber,
			[Values(ByDoubleClick, ByHotkey)] 
			string byHotkeyOrDoubleClick)
		{
			EditorPage.ScrollToRequiredSegment(segmentNumber);

			EditorPage.ClickSourceCell(segmentNumber);

			// Проверить, что есть переводы в панели CAT
			Assert.IsTrue(EditorPage.GetCATPanelNotEmpty(), "Ошибка: нет переводов в панели САТ");

			int catTranslationNum = EditorPage.GetCATTranslationRowNumber(EditorPageHelper.CAT_TYPE.MT);

			Assert.IsTrue(catTranslationNum > 0, "Ошибка: нет MT в CAT");
			Assert.IsTrue(catTranslationNum < 10, "Ошибка: строка с MT должна быть ближе, чем 10 (для хоткея)");

			// Target
			if (byHotkeyOrDoubleClick == ByHotkey)
			{
				// Ctrl+N - для вставки перевода из CAT-MT (N - номер в панели)
				EditorPage.PutCatMatchByHotkey(segmentNumber, catTranslationNum);
			}
			else if (byHotkeyOrDoubleClick == ByDoubleClick)
			{
				// Двойной клик
				EditorPage.DoubleClickCATPanel(catTranslationNum);
			}

			// Проверить текст
			Assert.IsTrue(EditorPage.GetTargetText(segmentNumber).Length > 0, "Ошибка: текст не добавился");

			// Проверить, что ревизия сохранилась
			Assert.IsTrue(RevisionPage.GetRevisionListCount() > 0, "Ошибка: ревизия не сохранилась");

			// Проверить тип
			Assert.AreEqual(
				Editor_RevisionPageHelper.RevisionType.InsertMT,
				RevisionPage.GetRevisionType(1),
				"Ошибка: неправильный тип ревизии");
		}

		/// <summary>
		/// ТЕСТ: вставка перевода из MT после подтверждения
		/// </summary>
		[Test]
		public void PasteFromMtAfterConfirmTest()
		{
			int segmentNumber = 10;
			EditorPage.ScrollToRequiredSegment(segmentNumber);

			EditorPage.ClickSourceCell(segmentNumber);

			// Проверить, что есть переводы в панели CAT
			Assert.IsTrue(EditorPage.GetCATPanelNotEmpty(), "Ошибка: нет переводов в панели САТ");

			int catTranslationNum = EditorPage.GetCATTranslationRowNumber(EditorPageHelper.CAT_TYPE.MT);

			Assert.IsTrue(catTranslationNum > 0, "Ошибка: перевод не MT");
			Assert.IsTrue(catTranslationNum < 10, "Ошибка: строка с MT должна быть ближе, чем 10 (для хоткея)");

			// Добавить текст в Target
			AddTranslationAndConfirm(segmentNumber, "Text" + DateTime.Now.Ticks);

			// Вернуться в сегмент
			ClickSegmentTarget(segmentNumber);

			// Двойной клик по MT
			EditorPage.DoubleClickCATPanel(catTranslationNum);

			// Дождаться, пока появится вторая ревизия
			Assert.IsTrue(RevisionPage.WaitRevisionAppear(2), "Ошибка: не появилась вторая ревизия");

			// Проверить тип
			Assert.AreEqual(
				Editor_RevisionPageHelper.RevisionType.InsertMT,
				RevisionPage.GetRevisionType(1),
				"Ошибка: неправильный тип 1 (свежей) ревизии");

			Assert.AreEqual(
				Editor_RevisionPageHelper.RevisionType.Confirmed,
				RevisionPage.GetRevisionType(2),
				"Ошибка: неправильный тип 2 (старой) ревизии");
		}

		/// <summary>
		/// ТЕСТ: вставка перевода из TM
		/// </summary>
		/// <param name="segmentNumber">номер сегмента</param>
		/// <param name="byHotkeyOrDoubleClick">хоткей или даблклик</param>
		[Test, Sequential]
		public void PasteFromTMTest(
			[Values(11, 12)] 
			int segmentNumber,
			[Values(ByDoubleClick, ByHotkey)] 
			string byHotkeyOrDoubleClick)
		{
			EditorPage.ScrollToRequiredSegment(segmentNumber);

			//Выбираем первый сегмент
			EditorPage.ClickTargetCell(segmentNumber);

			// Проверить, что есть переводы в панели CAT
			Assert.IsTrue(EditorPage.GetCATPanelNotEmpty(), "Ошибка: нет переводов в панели САТ");

			int catTranslationNum = EditorPage.GetCATTranslationRowNumber(EditorPageHelper.CAT_TYPE.TM);

			Assert.IsTrue(catTranslationNum > 0, "Ошибка: перевод не TM");
			Assert.IsTrue(catTranslationNum < 10, "Ошибка: строка с TM должна быть ближе, чем 10 (для хоткея)");

			if (byHotkeyOrDoubleClick == ByHotkey)
			{
				// Ctrl+1 - для вставки перевода из CAT-MT
				EditorPage.PutCatMatchByHotkey(segmentNumber, catTranslationNum);
			}
			else if (byHotkeyOrDoubleClick == ByDoubleClick)
			{
				// Двойной клик
				EditorPage.DoubleClickCATPanel(catTranslationNum);
			}

			// Проверить текст
			Assert.IsTrue(EditorPage.GetTargetText(segmentNumber).Length > 0, "Ошибка: текст не добавился");

			// Проверить, что ревизия сохранилась
			Assert.IsTrue(RevisionPage.GetRevisionListCount() > 0, "Ошибка: ревизия не сохранилась");

			// Проверить тип
			Assert.AreEqual(
				Editor_RevisionPageHelper.RevisionType.InsertTM,
				RevisionPage.GetRevisionType(1),
				"Ошибка: неправильный тип ревизии");
		}

		/// <summary>
		/// ТЕСТ: подтверждение нескольких переводов одного сегмента (несколько ревизий)
		/// </summary>
		[Test]
		public void ConfirmSomeTranslations()
		{
			var segmentNumber = 13;
			EditorPage.ScrollToRequiredSegment(segmentNumber);
			EditorPage.ClickSourceCell(segmentNumber);

			// Подтвердить несколько переводов в сегменте
			var translationNumber = 2;
			AddTranslationsToSegment(segmentNumber, translationNumber);

			// Открыть вкладку Ревизии
			OpenRevisionTab();

			// Получить количество ревизий
			var revisionListCount = RevisionPage.GetRevisionListCount();
			Console.WriteLine("количество строк в ревизиях: " + revisionListCount);

			// Проверить количество ревизий (== translationNumber)
			Assert.AreEqual(
				translationNumber,
				revisionListCount,
				"Ошибка: неверное количество ревизий: " + revisionListCount);

			// Проверить типы ревизий
			for (int i = 0; i < revisionListCount; ++i)
			{
				Assert.AreEqual(
					Editor_RevisionPageHelper.RevisionType.Confirmed,
					RevisionPage.GetRevisionType(i + 1),
					"Ошибка: неправильный тип ревизии N " + (i + 1) + " (сверху)");
			}
		}

		/// <summary>
		/// ТЕСТ: автосохранение несколько переводов в одну ревизию
		/// </summary>
		[Test]
		public void AutosaveSomeTranslations()
		{
			const int segmentNumber = 14;
			const int translationNumber = 2;

			EditorPage.ScrollToRequiredSegment(segmentNumber);

			for (var i = 0; i < translationNumber; ++i)
			{
				var text = "Text" + DateTime.Now.Ticks;

				// Добавить текст
				AddTextTarget(segmentNumber, text);

				// Дождаться автосохранения
				AutoSave();
			}
			// Вернуться в сегмент
			ClickSegmentTarget(segmentNumber);

			// Открыть вкладку Ревизии
			OpenRevisionTab();

			// Проверить количество ревизий - должна быть 1
			Assert.AreEqual(1, RevisionPage.GetRevisionListCount(), "Ошибка: неверное количество ревизий");

			// Проверить тип ревизии
			Assert.AreEqual(
				Editor_RevisionPageHelper.RevisionType.AutoSave,
				RevisionPage.GetRevisionType(1),
				"Ошибка: неправильный тип ревизии");
		}

		/// <summary>
		/// ТЕСТ: проверка кнопки Rollback
		/// </summary>
		/// <param name="segmentNumber">номер строки таргет</param>
		/// <param name="rollbackNumber">номер ревизии для отката (нумерация с 1 - первый добавленный перевод)</param>
		[Test, Sequential]
		public void RollbackTest([Values(15, 16, 17)] int segmentNumber, [Values(3, 2, 1)] int rollbackNumber)
		{
			const int translationNumber = 3;

			EditorPage.ScrollToRequiredSegment(segmentNumber);

			// Проверка параметров
			Assert.IsTrue(
				rollbackNumber > 0,
				"Неверный параметр: rollbackNumber - номер добавленного перевода, начиная с 1");

			Assert.IsTrue(
				translationNumber >= rollbackNumber,
				"Неверный параметр: rollbackNumber должен быть меньше translationNumber");

			// Подтвердить несколько переводов в одном сегменте
			var translationList = AddTranslationsToSegment(segmentNumber, translationNumber);

			// Текст ревизии для отката
			var revisionText = translationList[rollbackNumber - 1];

			EditorPage.ClickTargetCell(segmentNumber);

			// Проверить, что все ревизии сохранились
			OpenRevisionTab();

			Assert.AreEqual(
				translationNumber,
				RevisionPage.GetRevisionListCount(),
				"Ошибка: неправильное количество ревизий");

			// Выделить ревизию
			var revisionRollBackNumber = (translationNumber - rollbackNumber + 1);

			Assert.IsTrue(RevisionPage.ClickRevision(revisionRollBackNumber), "Ошибка: такой ревизии нет");
			Assert.IsTrue(RevisionPage.GetIsRollbackBtnEnabled(), "Ошибка: кнопка Rollback заблокирована");

			// Кликнуть Rollback
			RevisionPage.ClickRollbackBtn();

			// Дождаться открытия диалога подтверждения отката
			Assert.IsTrue(RevisionPage.WaitRollbackDialogAppear(), "Ошибка: не появился диалог подтверждения отката");

			// Нажать да
			RevisionPage.ClickYesRollbackDlg();
			RevisionPage.WaitUntilRollbackDialogDisappear();

			bool isOk = true;
			string errorMessage = "\n";

			EditorPage.WaitUntilDisplayTargetText(segmentNumber, revisionText);

			EditorPage.ClickTargetCell(segmentNumber);

			// Проверить, что текст в сегменте совпадает с текстом в ревизии
			if (EditorPage.GetTargetText(segmentNumber) != revisionText)
			{
				isOk = false;
				errorMessage += "Ошибка: в сегменте неправильный текст (д.б. " + revisionText + ")\n";
			}

			// Проверить, что добавилась новая ревизия
			if (RevisionPage.GetRevisionListCount() != (translationNumber + 1))
			{
				isOk = false;
				errorMessage += "Ошибка: количество ревизий не увеличилось\n";
			}

			// Проверить, что тип новой ревизии - Rollback
			if (Editor_RevisionPageHelper.RevisionType.Rollback != RevisionPage.GetRevisionType(1))
			{
				isOk = false;
				errorMessage += "Ошибка: тип ревизии неправильный";
			}

			// Вывести ошибки
			Assert.IsTrue(isOk, errorMessage);
		}

		/// <summary>
		/// ТЕСТ: отмена Rollback
		/// </summary>
		[Test]
		public void CancelRollback()
		{
			// Добавить текст в Target
			int segmentNumber = 18;
			EditorPage.ScrollToRequiredSegment(segmentNumber);

			AddTranslationAndConfirm(segmentNumber, "Text" + DateTime.Now.Ticks);

			// Вернуться в сегмент
			ClickSegmentTarget(segmentNumber);

			int revisionCountBefore = RevisionPage.GetRevisionListCount();

			// Выделить ревизию
			Assert.IsTrue(RevisionPage.ClickRevision(1), "Ошибка: такой ревизии нет");
			Assert.IsTrue(RevisionPage.GetIsRollbackBtnEnabled(), "Ошибка: кнопка Rollback заблокирована");

			// Кликнуть Rollback
			RevisionPage.ClickRollbackBtn();

			// Дождаться открытия диалога подтверждения отката
			Assert.IsTrue(RevisionPage.WaitRollbackDialogAppear(), "Ошибка: не появился диалог подтверждения отката");

			// Нажать нет
			RevisionPage.ClickNoRollbackDlg();
			RevisionPage.WaitUntilRollbackDialogDisappear();

			// Вернуться в сегмент
			ClickSegmentTarget(segmentNumber);

			// Проверить, что количество ревизий не увеличилось
			Assert.AreEqual(
				revisionCountBefore,
				RevisionPage.GetRevisionListCount(),
				"Ошибка: количество ревизий изменилось");

			// Проверить, что тип последней ревизии - не Rollback
			Assert.AreNotEqual(
				Editor_RevisionPageHelper.RevisionType.Rollback,
				RevisionPage.GetRevisionType(1),
				"Ошибка: появилась ревизия Rollback");
		}

		/// <summary>
		/// ТЕСТ: подтвердить вставленный из МТ перевод (должно быть две ревизии)
		/// </summary>
		[Test]
		public void ConfirmInsertedMtTest()
		{
			var segmentNumber = 19;

			EditorPage.ScrollToRequiredSegment(segmentNumber);

			EditorPage.ClickSourceCell(segmentNumber);

			// Проверить, что есть переводы MT
			Assert.IsTrue(EditorPage.GetCATPanelNotEmpty(), "Ошибка: нет переводов в МТ");

			var catTranslationNum = EditorPage.GetCATTranslationRowNumber(EditorPageHelper.CAT_TYPE.MT);

			Assert.IsTrue(catTranslationNum > 0, "Ошибка: перевод не МТ");
			Assert.IsTrue(catTranslationNum < 10, "Ошибка: строка с TM должна быть ближе, чем 10 (для хоткея)");

			// Вставить из МТ
			EditorPage.PutCatMatchByHotkey(segmentNumber, catTranslationNum);
			// Подтвердить
			ClickConfirmBtn(segmentNumber);

			// Вернуться
			ClickSegmentTarget(segmentNumber);

			// Проверить, что обе ревизии сохранились
			Assert.IsTrue(RevisionPage.GetRevisionListCount() == 2, "Ошибка: должно сохраниться две ревизии");

			// Проверить тип
			Assert.AreEqual(
				Editor_RevisionPageHelper.RevisionType.Confirmed,
				RevisionPage.GetRevisionType(1),
				"Ошибка: неправильный тип 1 (свежей) ревизии (д.б. Confirmed)");

			// Проверить тип
			Assert.AreEqual(
				Editor_RevisionPageHelper.RevisionType.InsertMT,
				RevisionPage.GetRevisionType(2),
				"Ошибка: неправильный тип 2 (старой) ревизии (д.б. Insert MT)");
		}

		/// <summary>
		/// ТЕСТ: удаление части подвержденного текста, проверка в ревизии пометки об удалении
		/// </summary>
		[Test]
		public void RemovePartTextTest()
		{
			// Добавить текст в Target
			const int segmentNumber = 20;
			EditorPage.ScrollToRequiredSegment(segmentNumber);
			var text = "Text" + DateTime.Now.Ticks;
			AddTranslationAndConfirm(segmentNumber, text);

			// Вернуться в сегмент
			ClickSegmentTarget(segmentNumber);

			// Ввести новый текст (старый, удалив часть текста)
			var textToRemove = text.Substring(2, 5);
			Console.WriteLine("textToRemove: " + textToRemove);
			text = text.Replace(textToRemove, "");
			Console.WriteLine("text: " + text);

			AddTranslationAndConfirm(segmentNumber, text);

			// Вернуться в сегмент
			ClickSegmentTarget(segmentNumber);

			// Проверить, что ревизии две
			Assert.AreEqual(2, RevisionPage.GetRevisionListCount(), "Ошибка: должно быть две ревизии");

			// Проверить, что в последней ревизии выделен удаленный текст
			Assert.IsTrue(
				RevisionPage.GetHasRevisionDeletedTextPart(1),
				"Ошибка: в ревизии нет пометки об удаленном тексте");
		}

		/// <summary>
		/// ТЕСТ: добавление текста в подтвержденный, проверка в ревизии пометки о добавлении
		/// </summary>
		[Test]
		public void AddPartTextTest()
		{
			// Добавить текст в Target
			var text = "Text" + DateTime.Now.Ticks;
			const int segmentNumber = 21;
			EditorPage.ScrollToRequiredSegment(segmentNumber);
			AddTranslationAndConfirm(segmentNumber, text);

			// Вернуться в сегмент
			ClickSegmentTarget(segmentNumber);

			// Ввести новый текст (старый, добавив часть нового текста)
			var textToAdd = "newText";
			Console.WriteLine("textToAdd: " + textToAdd);
			text = text.Insert(2, textToAdd);
			Console.WriteLine("text: " + text);
			AddTranslationAndConfirm(segmentNumber, text);

			// Вернуться в сегмент
			ClickSegmentTarget(segmentNumber);

			// Проверить, что ревизии две
			Assert.AreEqual(2, RevisionPage.GetRevisionListCount(), "Ошибка: должно быть две ревизии");

			// Проверить, что в последней ревизии выделен добавленный текст
			Assert.IsTrue(RevisionPage.GetHasRevisionInsertedTextPart(1),
				"Ошибка: в ревизии нет пометки о добавленном тексте");
		}
		/// <summary>
		/// ТЕСТ: вставка перевода из TB
		/// </summary>
		/// <param name="segmentNumber">номер сегмента</param>
		/// <param name="byHotkey">хоткей или даблклик</param>
		[Test, Sequential]
		public void PasteFromTBTest(
			[Values(22, 23)] int segmentNumber,
			[Values(true, false)] bool byHotkey)
		{
			EditorPage.ScrollToRequiredSegment(segmentNumber);

			//Выбираем сегмент
			EditorPage.ClickTargetCell(segmentNumber);

			//текст из сорс для создания термина словаря
			var sourceTerm = EditorPage.GetSourceText(segmentNumber);

			// Нажать кнопку вызова формы для добавления термина
			EditorPage.ClickAddTermBtn();

			// добавляем термин
			AddTermGlossary(sourceTerm, "термин глоссария");

			EditorPage.ClickTargetCell(segmentNumber);

			// Проверить, что есть переводы в панели CAT
			Assert.IsTrue(EditorPage.GetCATPanelNotEmpty(), "Ошибка: нет переводов в панели САТ");

			var catTranslationNum = EditorPage.GetCATTranslationRowNumber(EditorPageHelper.CAT_TYPE.TB);

			Assert.IsTrue(catTranslationNum > 0, "Ошибка: перевод не TB");
			Assert.IsTrue(catTranslationNum < 10, "Ошибка: строка с TB должна быть ближе, чем 10 (для хоткея)");

			if (byHotkey)
			{
				// хоткей для TB
				EditorPage.SendKeysTarget(segmentNumber, OpenQA.Selenium.Keys.Control + catTranslationNum);
			}
			else
			{
				// Двойной клик
				EditorPage.DoubleClickCATPanel(catTranslationNum);
			}

			// Проверить текст
			Assert.IsTrue(EditorPage.GetTargetText(segmentNumber).Length > 0, "Ошибка: текст не добавился");

			// Проверить, что ревизия сохранилась
			Assert.IsTrue(RevisionPage.GetRevisionListCount() > 0, "Ошибка: ревизия не сохранилась");

			// Проверить тип
			Assert.AreEqual(Editor_RevisionPageHelper.RevisionType.InsertTb, RevisionPage.GetRevisionType(1),
				"Ошибка: неправильный тип ревизии");
		}

		/// <summary>
		/// ТЕСТ: сортировка ревизий по юзеру
		/// </summary>
		[Test]
		public void SortByUser()
		{
			const int segmentNumber = 24;
			const int translationNumber = 3;

			EditorPage.ScrollToRequiredSegment(segmentNumber);

			// Подтвердить несколько переводов в одном сегменте
			AddTranslationsToSegment(segmentNumber, translationNumber);

			// Проверить, что все ревизии сохранились
			OpenRevisionTab();
			Assert.AreEqual(
				translationNumber,
				RevisionPage.GetRevisionListCount(),
				"Ошибка: неправильное количество ревизий");

			// Выходим из редактора
			EditorClickHomeBtn();

			// Разлогиниться
			WorkspacePage.ClickAccount();
			WorkspacePage.ClickLogoff();

			Thread.Sleep(2000);

			// войти другим пользователем
			Authorization("TestAccount", true);

			// Открытие страницы проекта
			OpenProjectPage(_projectNameRevisionsTest);

			// Подтверждение назначения
			ProjectPage.ClickAllAcceptBtns();
			Thread.Sleep(1000);

			// Открываем документ
			OpenDocument();
			Thread.Sleep(1000);

			// Подтвердить несколько переводов в одном сегменте
			AddTranslationsToSegment(segmentNumber, translationNumber);

			// Проверить, что все ревизии сохранились
			OpenRevisionTab();
			Assert.AreEqual(
				2 * translationNumber,
				RevisionPage.GetRevisionListCount(),
				"Ошибка: неправильное количество ревизий");


			// Проверить автора первой ревизии в списке
			Assert.AreEqual(
				UserName2,
				RevisionPage.GetRevisionUser(1),
				"Ошибка: ревизия последнего пользователя не первая после первой сортировки");

			//сортировка по юзеру
			RevisionPage.ClickUserToSort();

			// Проверить автора первой ревизии в списке
			Assert.AreEqual(
				UserName,
				RevisionPage.GetRevisionUser(1),
				"Ошибка: ревизия первого пользователя не первая после второй сортировки");

			// Выходим из редактора
			EditorClickHomeBtn();

			// Разлогиниться
			WorkspacePage.ClickAccount();
			WorkspacePage.ClickLogoff();
		}

		/// <summary>
		/// ТЕСТ: проверка сохранения типа ревизии MT или ТМ после претранслейта
		/// </summary>
		/// <param name="catType">тип подстановки МТ или ТМ</param>
		/// <param name="expectedRevisionType">тип ревизии в соответствии с подстановкой из кат панели</param>
		[Test, Sequential]
		public void RevisionsAfterPretranslate(
			[Values(EditorPageHelper.CAT_TYPE.TM, EditorPageHelper.CAT_TYPE.MT)] 
			EditorPageHelper.CAT_TYPE catType,
			[Values(Editor_RevisionPageHelper.RevisionType.InsertTM, Editor_RevisionPageHelper.RevisionType.InsertMT)]
			Editor_RevisionPageHelper.RevisionType expectedRevisionType)
		{
			const int segmentNumber = 26;
			
			EditorPage.ScrollToRequiredSegment(segmentNumber);

			EditorPage.ClickSourceCell(segmentNumber);

			// Проверить, что есть переводы в панели CAT
			Assert.IsTrue(
				EditorPage.GetCATPanelNotEmpty(),
				"Ошибка: нет переводов в панели САТ");

			var catTranslationNum = EditorPage.GetCATTranslationRowNumber(catType);

			Assert.IsTrue(
				catTranslationNum > 0,
				"Ошибка: нет типа в CAT");
			Assert.IsTrue(
				catTranslationNum < 10,
				"Ошибка: строка с типом должна быть ближе, чем 10 (для хоткея)");

			// Ctrl+N - для вставки перевода из CAT (N - номер в панели)
			EditorPage.SendKeysTarget(
				segmentNumber,
				OpenQA.Selenium.Keys.Control + catTranslationNum);

			// Проверить текст
			Assert.IsTrue(
				EditorPage.GetTargetText(segmentNumber).Length > 0,
				"Ошибка: текст не добавился");

			// Проверить, что ревизия сохранилась
			Assert.IsTrue(
				RevisionPage.GetRevisionListCount() > 0,
				"Ошибка: ревизия не сохранилась");

			// Проверить тип
			Assert.AreEqual(
				expectedRevisionType,
				RevisionPage.GetRevisionType(1),
				"Ошибка: неправильный тип ревизии");

			// Выходим из редактора
			EditorClickHomeBtn();

			// Выполняем претранслейт
			implementPretranslate(catType);

			// Открываем документ
			OpenDocument();
			Thread.Sleep(1000);

			EditorPage.ClickSourceCell(segmentNumber);

			// Проверить тип
			Assert.AreEqual(
				expectedRevisionType,
				RevisionPage.GetRevisionType(1),
				"Ошибка: неправильный тип ревизии");
		}

		/// <summary>
		/// ТЕСТ: проверка типа ревизии в xliff
		/// </summary>
		[Test]
		public void RevisionsInXlf()
		{
			// название проекта для проведения тестов
			var projectNameRevisionsTest3 = "RevisionsTestXlf" + "_" + DateTime.UtcNow.Ticks;

			GoToWorkspace();

			// создаем документ с нужным файлом
			CreateProject(projectNameRevisionsTest3, TestFile.EditorXlfFile);

			// Открываем диалог выбора исполнителя
			OpenAssignDialog(projectNameRevisionsTest3);

			// Выбор в качестве исполнителя для первой задачи первого юзера
			SetResponsible(1, UserName, false);
			ResponsiblesDialog.ClickCloseBtn();

			// Открытие страницы проекта
			OpenProjectPage(projectNameRevisionsTest3);

			// Подтверждение назначения
			ProjectPage.ClickAllAcceptBtns();
			Thread.Sleep(1000);

			// Открываем документ
			OpenDocument();
			Thread.Sleep(1000);

			const int segmentNumber = 1;

			EditorPage.ScrollToRequiredSegment(segmentNumber);

			EditorPage.ClickSourceCell(segmentNumber);

			// Проверить тип
			Assert.AreEqual(
				Editor_RevisionPageHelper.RevisionType.Pretranslation,
				RevisionPage.GetRevisionType(1),
				"Ошибка: неправильный тип ревизии");
		}

		/// <summary>
		/// Добавить текст в Target
		/// </summary>
		/// <param name="rowNum">номер строки для добавления</param>
		/// <param name="text">текст</param>
		protected void AddTextTarget(int rowNum, string text)
		{
			EditorPage.AddTextTarget(rowNum, text);
		}

		/// <summary>
		/// Кликнуть по Target сегмента
		/// </summary>
		/// <param name="segmentRowNumber">номер сегмента</param>
		protected void ClickSegmentTarget(int segmentRowNumber)
		{
			// Кликнуть по Target
			EditorPage.ClickTargetCell(segmentRowNumber);
		}

		/// <summary>
		/// Кликнуть Confirm, дождаться подтверждвения
		/// </summary>
		/// <param name="segmentRowWaitConfirm">номер сегмента для ожидания подтверждения</param>
		protected void ClickConfirmBtn(int segmentRowWaitConfirm)
		{
			EditorPage.ClickConfirmBtn();
			
			// Дождаться подтверждения
			Assert.IsTrue(WaitSegmentConfirm(segmentRowWaitConfirm), "Ошибка: Confirm не прошел");
		}

		/// <summary>
		/// Нажать хоткей Confirm
		/// </summary>
		/// <param name="segmentRowNum">номер сегмента</param>
		protected void ClickConfirmHotkey(int segmentRowNum)
		{
			// хоткей Confirm
			EditorPage.ConfirmByHotkey(segmentRowNum);
			// Дождаться подтверждения
			Assert.IsTrue(WaitSegmentConfirm(segmentRowNum), "Ошибка: Confirm не прошел");
		}

		/// <summary>
		/// Открыть вкладку ревизии
		/// </summary>
		protected void OpenRevisionTab()
		{
			// Проверить, что вкладка Ревизии открыта
			if (!RevisionPage.GetIsRevisionTabDisplay())
			{
				// Открыть вкладку Ревизии
				Assert.IsTrue(
					RevisionPage.OpenRevisionTab(),
					"Ошибка: вкладка с ревизиями не открылась");
			}
		}

		/// <summary>
		/// Добавить несколько переводов в сегмент с подтверждением каждого
		/// </summary>
		/// <param name="segmentRowNumber">номер сегмента</param>
		/// <param name="translationNumber">количество переводов</param>
		/// <returns>список внесенных переводов</returns>
		protected List<string> AddTranslationsToSegment(int segmentRowNumber, int translationNumber)
		{
			var textList = new List<string>();

			// Добавить текст в Target
			for (var i = 0; i < translationNumber; ++i)
			{
				var text = "Text" + DateTime.Now.Ticks;

				// Добавить текст
				AddTranslationAndConfirm(segmentRowNumber, text);
				textList.Add(text);
			}

			// Вернуться
			EditorPage.ClickTargetCell(segmentRowNumber);

			return textList;
		}

		private void implementPretranslate(EditorPageHelper.CAT_TYPE catType)
		{
			switch (catType)
			{
				case EditorPageHelper.CAT_TYPE.TM:
					var tmNameRevTest = "TmForPretranslateRevisionsTest" + "_" + DateTime.UtcNow.Ticks;
					ProjectPageAddTmImportTmx(tmNameRevTest, TestFile.OneLineTmxFile);
					SetTmPretranslate(tmNameRevTest);
					break;

				case EditorPageHelper.CAT_TYPE.MT:
					const string mtName = "Default MT (MT)";
					SetTmPretranslate(mtName);
					break;

				default:
					throw new InvalidEnumArgumentException();

			}

		}

		const string ByHotkey = "byHotkey";
		const string ByButton = "byButton";
		const string ByDoubleClick = "byDoubleClick";
	}
}