using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

using NUnit.Framework;

using AbbyyLS.CAT.Function.Selenium.Tests.Driver;

namespace AbbyyLS.CAT.Function.Selenium.Tests.Editor.Revisions
{
	/// <summary>
	/// Группа тестов для проверки истории версий в редакторе
	/// </summary>
	public class Editor_RevisionTest<TWebDriverSettings> : BaseTest<TWebDriverSettings> where TWebDriverSettings : IWebDriverSettings, new()
	{
		// название проекта для проведения тестов
		protected string _projectNameRevisionsTest = "RevisionsTest" + "_" + DateTime.UtcNow.Ticks;

		// флаг создан ли проект (создается один раз перед всеми тестами)
		protected bool _projectCreated;

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
				Logger.Trace("Переход на стр WS");
				GoToUrl(RelativeUrlProvider.Workspace);

				CreateProject(
					projectName: _projectNameRevisionsTest,
					downloadFile: PathProvider.LongTxtFile,
					createNewTM: true,
					tmFile: PathProvider.LongTmxFile,
					setGlossary: Workspace_CreateProjectDialogHelper.SetGlossary.New,
					glossaryName: "",
					chooseMT: true,
					mtType: Workspace_CreateProjectDialogHelper.MT_TYPE.ABBYY,
					isNeedCheckProjectAppearInList: false,
					translationTasksNumber: 2);

				// Открываем диалог выбора исполнителя
				OpenAssignDialog(_projectNameRevisionsTest);

				// Выбор в качестве исполнителя для первой задачи первого юзера
				SetResponsible(1, NickName, false);

				// Выбор в качестве исполнителя для второй задачи второго юзера
				SetResponsible(2, NickName2, false);
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
		[Ignore]
		[NUnit.Framework.Category("Standalone")]
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
		[Ignore]
		[NUnit.Framework.Category("Standalone")]
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
		/// ТЕСТ: проверка хоткея Автосохранение
		/// </summary>
		[Test]
		[Ignore]
		[NUnit.Framework.Category("Standalone")]
		public void AutoSaveTest()
		{
			const int segmentNumber = 4;
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
				Editor_RevisionPageHelper.RevisionType.ManualInput,
				RevisionPage.GetRevisionType(1),
				"Ошибка: тип ревизии не совпадает");
		}

		/// <summary>
		/// ТЕСТ: подтвердить после автосохранения
		/// </summary>
		[Test]
		[Ignore]
		[NUnit.Framework.Category("Standalone")]
		public void ConfirmAfterSaveTest()
		{
			// Добавить текст в Target
			var text = "Text" + DateTime.Now.Ticks;
			const int segmentNumber = 7;

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
			Logger.Trace("количество строк в ревизиях: " + revisionListCount);

			// Проверить, что количество больше 0 (перевод появился в ревизиях)
			Assert.IsTrue(
				revisionListCount > 0,
				"Ошибка: перевод не появился в ревизиях");

			// Проверить тип
			Assert.AreEqual(
				Editor_RevisionPageHelper.RevisionType.ManualInput,
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
		/// ТЕСТ: подтверждение нескольких переводов одного сегмента (несколько ревизий)
		/// </summary>
		[Test]
		[Ignore]
		[NUnit.Framework.Category("Standalone")]
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
			Logger.Trace("количество строк в ревизиях: " + revisionListCount);

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
		[Ignore]
		[NUnit.Framework.Category("Standalone")]
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

			const int expectedRevisionsCount = 1;

			// Проверить количество ревизий - должна быть 1
			Assert.AreEqual(
				expectedRevisionsCount, 
				RevisionPage.GetRevisionListCount(), 
				string.Format(
					"Ошибка: неверное количество ревизий в окне редактора. Ожидаемое число ревизий - {0}, реальное - {1}",
					expectedRevisionsCount,
					RevisionPage.GetRevisionListCount()));

			// Проверить тип ревизии
			Assert.AreEqual(
				Editor_RevisionPageHelper.RevisionType.ManualInput,
				RevisionPage.GetRevisionType(1),
				"Ошибка: неправильный тип ревизии");
		}

		/// <summary>
		/// ТЕСТ: отмена Rollback
		/// </summary>
		[Test]
		[Ignore("Процесс восстановления ревизии был упрощен (восстановление без подтверждения.) Тикет PRX-6309")]
		[NUnit.Framework.Category("Standalone")]
		public void CancelRollback()
		{
			// Добавить текст в Target
			const int segmentNumber = 18;
			EditorPage.ScrollToRequiredSegment(segmentNumber);

			AddTranslationAndConfirm(segmentNumber, "Text" + DateTime.Now.Ticks);

			// Вернуться в сегмент
			ClickSegmentTarget(segmentNumber);

			var revisionCountBefore = RevisionPage.GetRevisionListCount();

			// Выделить ревизию
			Assert.IsTrue(RevisionPage.ClickRevision(1), "Ошибка: нет ревизии №1.");
			Assert.IsTrue(RevisionPage.GetIsRollbackBtnEnabled(), "Ошибка: кнопка Rollback заблокирована.");

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
				Editor_RevisionPageHelper.RevisionType.Restored,
				RevisionPage.GetRevisionType(1),
				"Ошибка: появилась ревизия Rollback");
		}

		/// <summary>
		/// ТЕСТ: подтвердить вставленный из МТ перевод (должно быть две ревизии)
		/// </summary>
		[Test]
		[Ignore]
		[NUnit.Framework.Category("SCAT_102")]
		public void ConfirmInsertedMtTest()
		{
			const int segmentNumber = 19;

			EditorPage.ScrollToRequiredSegment(segmentNumber);

			EditorPage.ClickSourceCell(segmentNumber);

			// Проверить, что панель CAT не пуста
			Assert.IsTrue(
				EditorPage.GetCATPanelNotEmpty(),
				"Ошибка: нет переводов в CAT панеле.");

			var catTranslationNum = EditorPage.GetCatTranslationRowNumber(EditorPageHelper.CAT_TYPE.MT);

			Assert.IsTrue(catTranslationNum > 0, "Ошибка: MT перевода нет в CAT панеле.");
			Assert.IsTrue(catTranslationNum < 10, "Ошибка: строка с MT должна быть ближе, чем 10 (для хоткея).");

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
		[Ignore]
		[NUnit.Framework.Category("Standalone")]
		public void RemovePartTextTest()
		{
			// Добавить текст в Target
			const int segmentNumber = 20;
			var text = "Text" + DateTime.Now.Ticks;
			EditorPage.ClickLastVisibleSegment();
			EditorPage.ScrollToRequiredSegment(segmentNumber);

			AddTranslationAndConfirm(segmentNumber, text);

			// Вернуться в сегмент
			ClickSegmentTarget(segmentNumber);

			// Ввести новый текст (старый, удалив часть текста)
			var textToRemove = text.Substring(2, 5);
			Logger.Trace("textToRemove: " + textToRemove);
			text = text.Replace(textToRemove, "");
			Logger.Trace("text: " + text);

			AddTranslationAndConfirm(segmentNumber, text);

			// Вернуться в сегмент
			ClickSegmentTarget(segmentNumber);

			// Проверить, что ревизии две
			Assert.AreEqual(2, RevisionPage.GetRevisionListCount(), "Ошибка: должно быть две ревизии");

			// Проверить, что в последней ревизии выделен удаленный текст
			Assert.IsTrue(RevisionPage.GetHasRevisionDeletedTextPart(1),
				"Ошибка: в ревизии нет пометки об удаленном тексте");
		}

		/// <summary>
		/// ТЕСТ: добавление текста в подтвержденный, проверка в ревизии пометки о добавлении
		/// </summary>
		[Test]
		[Ignore]
		[NUnit.Framework.Category("Standalone")]
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
			Logger.Trace("textToAdd: " + textToAdd);
			text = text.Insert(2, textToAdd);
			Logger.Trace("text: " + text);
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
		/// ТЕСТ: проверка типа ревизии в xliff
		/// </summary>
		[Test]
		[Ignore]
		[NUnit.Framework.Category("Standalone")]
		public void RevisionsInXlf()
		{
			// название проекта для проведения тестов
			var projectNameRevisionsTest3 = "RevisionsTestXlf" + "_" + DateTime.UtcNow.Ticks;

			GoToUrl(RelativeUrlProvider.Workspace);

			// создаем документ с нужным файлом
			CreateProject(projectNameRevisionsTest3, PathProvider.EditorXlfFile);

			// Открываем диалог выбора исполнителя
			OpenAssignDialog(projectNameRevisionsTest3);

			// Выбор в качестве исполнителя для первой задачи первого юзера
			SetResponsible(1, NickName, false);
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
		/// ТЕСТ: сортировка ревизий по юзеру
		/// </summary>
		[NUnit.Framework.Category("PRX_7069")]
		[NUnit.Framework.Category("Standalone")]
		[Ignore()]
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
			Authorization(Login2, Password2);

			// Открытие страницы проекта
			OpenProjectPage(_projectNameRevisionsTest);

			// Подтверждение назначения
			ProjectPage.ClickAllAcceptBtns();
			Thread.Sleep(1000);

			// Открываем документ
			OpenDocument();
			Thread.Sleep(1000);

			// Прокрутить до нужного сегмента
			EditorPage.ScrollToRequiredSegment(segmentNumber);

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
				NickName2,
				RevisionPage.GetRevisionUser(1),
				"Ошибка: ревизия последнего пользователя не первая после первой сортировки");

			//сортировка по юзеру
			RevisionPage.ClickUserToSort();

			// Проверить автора первой ревизии в списке
			Assert.AreEqual(
				NickName,
				RevisionPage.GetRevisionUser(1),
				"Ошибка: ревизия первого пользователя не первая после второй сортировки");

			// Выходим из редактора
			EditorClickHomeBtn();

			// Разлогиниться
			WorkspacePage.ClickAccount();
			WorkspacePage.ClickLogoff();
		}

		/// <summary>
		/// ТЕСТ: проверка хоткея Confirm
		/// </summary>
		/// <param name="segmentNumber">номер сегмента</param>
		/// <param name="byHotkeyOrButton">хоткей или кнопка</param>
		[Test, Sequential]
		[Ignore]
		[NUnit.Framework.Category("SCAT_102")]
		[NUnit.Framework.Category("Standalone")]
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
		/// ТЕСТ: вставка перевода из MT
		/// </summary>
		/// <param name="segmentNumber">номер сегмента</param>
		/// <param name="byHotkeyOrDoubleClick">хоткей или даблклик</param>
		[NUnit.Framework.Category("SCAT_102")]
		[Ignore]
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

			int catTranslationNum = EditorPage.GetCatTranslationRowNumber(EditorPageHelper.CAT_TYPE.MT);

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
		/// ТЕСТ: вставка перевода из TM
		/// </summary>
		/// <param name="segmentNumber">номер сегмента</param>
		/// <param name="byHotkeyOrDoubleClick">хоткей или даблклик</param>
		[Test, Sequential]
		[Ignore]
		[NUnit.Framework.Category("Standalone")]
		[NUnit.Framework.Category("SCAT_102")]
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

			int catTranslationNum = EditorPage.GetCatTranslationRowNumber(EditorPageHelper.CAT_TYPE.TM);

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
		/// ТЕСТ: проверка кнопки Rollback
		/// </summary>
		/// <param name="segmentNumber">номер строки таргет</param>
		/// <param name="rollbackNumber">номер ревизии для отката (нумерация с 1 - первый добавленный перевод)</param>
		[Test, Sequential]
		[Ignore]
		[NUnit.Framework.Category("Standalone")]
		[NUnit.Framework.Category("PRX-1835")]
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


			var isOk = true;
			var errorMessage = "\n";

			EditorPage.WaitUntilDisplayTargetText(segmentNumber, revisionText);

			EditorPage.ClickTargetCell(segmentNumber);

			// Проверить, что текст в сегменте совпадает с текстом в ревизии
			if (EditorPage.GetTargetText(segmentNumber) != revisionText)
			{
				isOk = false;
				errorMessage += "Ошибка: в сегменте неправильный текст (д.б. " + revisionText + ")\n";
			}
			var revisionCount = RevisionPage.GetRevisionListCount();

			// Проверить, что добавилась новая ревизия
			if (revisionCount != (translationNumber + 1))
			{
				isOk = false;
				errorMessage += string.Format("Ошибка: количество ревизий ожидаемое: {0}, реальное {1}\n", translationNumber + 1, revisionCount);
			}

			// Проверить, что тип новой ревизии - Rollback
			if (Editor_RevisionPageHelper.RevisionType.Restored != RevisionPage.GetRevisionType(1))
			{
				isOk = false;
				errorMessage += "Ошибка: тип ревизии неправильный";
			}

			// Вывести ошибки
			Assert.IsTrue(isOk, errorMessage);
		}

		/// <summary>
		/// ТЕСТ: вставка перевода из TB
		/// </summary>
		/// <param name="segmentNumber">номер сегмента</param>
		/// <param name="byHotkey">хоткей или даблклик</param>
		[Test, Sequential]
		[Ignore]
		[NUnit.Framework.Category("Standalone")]
		public void PasteFromTBTest(
			[Values(22, 23)] int segmentNumber,
			[Values(true, false)] bool byHotkey)
		{
			EditorPage.ClickLastVisibleSegment();
			EditorPage.ScrollToRequiredSegment(segmentNumber);

			//Выбираем сегмент
			EditorPage.ClickTargetCell(segmentNumber);

			//текст из сорс для создания термина словаря
			var sourceTerm = EditorPage.GetSourceText(segmentNumber);

			// Нажать кнопку вызова формы для добавления термина
			EditorPage.ClickAddTermBtn();

			if (byHotkey)
			{
				// добавляем термин
				AddTermGlossary(sourceTerm, "термин глоссария byHotkey");
			}
			else
			{
				// добавляем термин
				AddTermGlossary(sourceTerm, "термин глоссария");
			}
			EditorPage.ClickTargetCell(segmentNumber);

			// Проверить, что есть переводы в панели CAT
			Assert.IsTrue(EditorPage.GetCATPanelNotEmpty(), "Ошибка: нет переводов в панели САТ");

			var catTranslationNum = EditorPage.GetCatTranslationRowNumber(EditorPageHelper.CAT_TYPE.TB);

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
		/// ТЕСТ: проверка сохранения типа ревизии MT или ТМ после претранслейта
		/// </summary>
		/// <param name="catType">тип подстановки МТ или ТМ</param>
		/// <param name="expectedRevisionType">тип ревизии в соответствии с подстановкой из кат панели</param>
		[Test, Sequential]
		[Ignore]
		[NUnit.Framework.Category("SCAT_102")]
		[NUnit.Framework.Category("PRX-1835")]
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

			var catTranslationNum = EditorPage.GetCatTranslationRowNumber(catType);

			Assert.IsTrue(
				catTranslationNum > 0,
				"Ошибка: нет типа в CAT");
			Assert.IsTrue(
				catTranslationNum < 10,
				"Ошибка: строка с типом должна быть ближе, чем 10 (для хоткея)");

			// Ctrl+N - для вставки перевода из CAT (N - номер в панели)
			EditorPage.PasteTranslationToTargetByHotkey(segmentNumber, catTranslationNum.ToString());

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
		/// Добавить текст в Target
		/// </summary>
		/// <param name="rowNum">номер строки для добавления</param>
		/// <param name="text">текст</param>
		protected void AddTextTarget(int rowNum, string text)
		{
			EditorPage.AddTextTarget(rowNum, text);
		}

		protected void ClickSegmentTarget(int segmentRowNumber)
		{
			Logger.Trace(string.Format("Кликнуть по Target сегмента №{0}", segmentRowNumber));
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

		protected void implementPretranslate(EditorPageHelper.CAT_TYPE catType)
		{
			switch (catType)
			{
				case EditorPageHelper.CAT_TYPE.TM:
					var tmNameRevTest = "TmForPretranslateRevisionsTest" + "_" + DateTime.UtcNow.Ticks;
					ProjectPageAddTmImportTmx(tmNameRevTest, PathProvider.OneLineTmxFile);
					SetPretranslate(tmNameRevTest);
					break;

				case EditorPageHelper.CAT_TYPE.MT:
					SetPretranslate();
					break;

				default:
					throw new InvalidEnumArgumentException();

			}
		}

		protected const string ByHotkey = "byHotkey";
		protected const string ByButton = "byButton";
		protected const string ByDoubleClick = "byDoubleClick";
	}
}