using System.Threading;
using NUnit.Framework;
using System.Collections.Generic;
using System;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	/// <summary>
	/// Группа тестов кнопок редактора
	/// </summary>
	public class EditorButtonsTest : BaseTest
	{
		/// <summary>
		/// Конструктор теста
		/// </summary>
		/// <param name="browserName">Название браузера</param>
		public EditorButtonsTest(string browserName)
			: base(browserName)
		{

		}

		// Имя проекта, использующегося в нескольких тестах
		// Проект не изменяется при проведении тестов
		private string _projectNoChangesName = "";

		/// <summary>
		/// Начальная подготовка для группы тестов
		/// </summary>
		[TestFixtureSetUp]
		public void SetupAll()
		{
			try
			{
				// Создание уникального имени проекта
				CreateUniqueNamesByDatetime();

				// Запись имени для дальнейшего использования в группе тестов
				_projectNoChangesName = ProjectName;
			}
			catch(Exception ex)
			{
				ExitDriver();
				Logger.ErrorException("Ошибка в конструкторе : " + ex.Message, ex);
				throw;
			}
		}

		/// <summary>
		/// Начальная подготовка для каждого теста
		/// </summary>
		[SetUp]
		public void Setup()
		{
			// Не выходить из браузера после теста
			QuitDriverAfterTest = false;

			// 1. Переход на страницу workspace
			GoToUrl(RelativeUrlProvider.Workspace);

			// 2. Создание проекта с 1 документом внутри
			// При проверке PreviousStage нужно создать новый проект с уникальным именем, т.к. необходимо внести изменения в задачи
			// При проверке Tag нужно чтобы в документе проекта присутствовал tag
			if (TestContext.CurrentContext.Test.Name.Contains("PreviousStage"))
			{
				// Создание проекта с уникальным именем
				CreateProjectIfNotCreated(
					ProjectName,
					PathProvider.EditorTxtFile, 
					false, 
					"", 
					Workspace_CreateProjectDialogHelper.SetGlossary.None, 
					"", 
					true, 
					Workspace_CreateProjectDialogHelper.MT_TYPE.DefaultMT);
				// Открытие настроек проекта
				WorkspacePage.OpenProjectPage(ProjectName);
			}
			else if (TestContext.CurrentContext.Test.Name.Contains("Tag"))
			{
				// Создание проекта с уникальным именем
				CreateProjectIfNotCreated(ProjectName, PathProvider.DocumentFile);
				// Открытие настроек проекта
				WorkspacePage.OpenProjectPage(ProjectName);
			}
			else
			{
				// Создание проекта с неизменяемым именем, для проведения нескольких тестов
				CreateProjectIfNotCreated(
					_projectNoChangesName,
					PathProvider.EditorTxtFile);
				// Открытие настроек проекта
				WorkspacePage.OpenProjectPage(_projectNoChangesName);
			}
			
			// 3. Назначение задачи на пользователя
			if (ProjectPage.GetDocumentTask(1) == "")
				AssignTask();

			// 4. Открытие документа
			OpenDocument();
		}

		/// <summary>
		/// Конечные действия для каждого теста
		/// </summary>
		[TearDown]
		public void TearDown()
		{
			try
			{
				// Дождаться сохранения сегментов
				EditorPage.WaitUntilAllSegmentsSave();
			}
			catch
			{
			}
		}

		/// <summary>
		/// Метод тестирования кнопки "Back" в редакторе
		/// </summary>
		[Test]
		public void HomeButtonTest()
		{
			// Кнопка Back, проверка перехода
			EditorClickHomeBtn();
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
			EditorPage.AddTextTarget(1, "some words for example" + OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.Return);

			// Убедиться что сегмент подтвержден
			Assert.IsTrue(WaitSegmentConfirm(1), "Ошибка: Подтверждение (Confirm) не прошло");
		}

		/// <summary>
		/// Метод тестирования кнопки Undo и Redo при вводе текста
		/// </summary>
		[Test]
		public void UndoRedoButtonTextTest()
		{
			const int segmentNumber = 1;
			const string text = "some text";
			const string textundo = "some tex";

			// Вводим текст в первый сегмент
			EditorPage.AddTextTarget(segmentNumber, text);

			// Нажать кнопку отмены
			EditorPage.ClickUndoBtn();

			// Проверить, что в target убралась одна буква
			var targetxt = EditorPage.GetTargetText(segmentNumber);
			Assert.AreEqual(textundo, targetxt, "Ошибка: после Undo в Target не убрана одна буква");

			// Нажать кнопку возврата отмененного действия
			EditorPage.ClickRedoBtn();

			// Убедиться, что в текст соответствует введенному
			targetxt = EditorPage.GetTargetText(segmentNumber);
			Assert.AreEqual(text, targetxt, "Ошибка: Текст не соответствует введенному.");
		}

		/// <summary>
		/// Метод тестирования хоткея Undo и Redo при вводе текста
		/// </summary>
		[Test]
		public void UndoRedoHotkeyTextTest()
		{
			const int segmentNumber = 1;
			const string text = "some text";
			const string textundo = "some tex";

			// Вводим текст в первый сегмент
			EditorPage.AddTextTarget(segmentNumber, text);

			// Нажать хоткей отмены
			EditorPage.UndoByHotkey(segmentNumber);

			// Убедиться, что в target нет текста
			var targetxt = EditorPage.GetTargetText(segmentNumber);

			Assert.AreEqual(textundo, targetxt, "Ошибка: после Undo в Target не убрана одна буква");

			// Нажать хоткей возврата отмененного действия
			EditorPage.RedoByHotkey(segmentNumber);

			// Убедиться, что в текст соответствует введенному
			targetxt = EditorPage.GetTargetText(segmentNumber);
			Assert.AreEqual(text, targetxt, "Ошибка: Текст не соответствует введенному.");
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
		public void UnfinishedHotkeyNextSegmentTest()
		{
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

		/// <summary>
		/// Метод тестирования кнопки копирования оригинала в перевод
		/// </summary>
		[Test]
		public void ToTargetButtonTest()
		{
			// Кнопка Copy, проверить содержимое Target
			ToTargetButton();
		}

		/// <summary>
		/// Метод тестирования хоткея копирования оригинала в перевод
		/// </summary>
		[Test]
		public void ToTargetHotkeyTest()
		{
			// Хоткей Copy, проверить содержимое Target
			ToTargetHotkey();
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
		public void TabHotkeyTest()
		{
			var segmentNumber = 1;

			// Перешли из Target в Source по хоткею
			SourceTargetSwitchHotkey(segmentNumber);

			// Проверить где находится курсор, и если в поле source, то все ок
			Assert.True(EditorPage.GetIsCursorInSourceCell(segmentNumber),
				"Ошибка: после хоткея Toggle не перешли в Target");
		}

		/// <summary>
		/// Метод тестирования кнопки отката на предыдущее состояние сегмента
		/// </summary>
		[Test]
		public void PreviousStageButtonTest()
		{
			// Добавить текст в сегмент, подтвердить, проверка подтверждвения
			AddTranslationAndConfirm(1, "some words for example");

			// Выйти из редактора
			EditorPage.ClickHomeBtn();

			// Дождаться открытия страницы проекта
			ProjectPage.WaitPageLoad();

			//Открываем Workflow в настройках проекта
			OpenWorkflowSettings();

			// Добавление новой задачи
			ProjectPage.ClickProjectSettingsWorkflowNewTask();

			// Изменение типа новой задачи
			ProjectPage.SetWFTaskListProjectSettings(2, "Editing");

			// Сохранение проекта
			ProjectPage.ClickProjectSettingsSave();
			
			// Переходим на страницу проектов
			SwitchWorkspaceTab();

			// Открываем диалог выбора исполнителя
			OpenAssignDialog(ProjectName);

			// Выбор в качестве исполнителя для второй задачи группы Administrator
			SetResponsible(2, UserName, false);

			// Закрываем форму
			ResponsiblesDialog.ClickCloseBtn();

			// Открытие страницы проекта
			OpenProjectPage(ProjectName);

			// Открытие документа
			ProjectPage.OpenDocument(1);
			ResponsiblesDialog.WaitUntilChooseTaskDialogDisplay();

			// Выбор задачи Editing
			EditorPage.ClickEditingTaskBtn();

			//После выбора задачи жмём на Continue
			EditorPage.ClickContBtn();

			// Проверяем что нет замочка в сегменете
			Assert.False(
				EditorPage.GetIsSegmentLock(1),
				"Ошибка: Сегмент заблокирован.");

			// Переходим к первому сегменту
			EditorPage.ClickTargetCell(1);

			// Проверяем что кнопка отката разблокирована
			Assert.False(
				EditorPage.GetIsRollbackBtnLock(),
				"Ошибка: Кнопка отката изменений сегмента заблокирована.");

			// Жмем кнопку отката изменений
			EditorPage.ClickRollbackBtn();

			// Проверяем что появился замочек в сегменете
			Assert.True(
				EditorPage.GetIsSegmentLock(1),
				"Ошибка: Не появился замочек у перевода сегмента.");
		}

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

		/// <summary>
		/// Проверка работы в редакторе кнопки открытия словаря
		/// </summary>
		[Test]
		public void DictionaryButtonTest()
		{
			OpenEditorDictionary();
		}

		/// <summary>
		/// Проверка работы в редакторе кнопки поиска ошибки терминологии
		/// </summary>
		[Test]
		public void FindErrorButtonTest()
		{
			// Кликнуть по кнопке
			EditorPage.ClickFindErrorBtn();

			// Проверка, что открылась форма
			Assert.IsTrue(
				EditorPage.WaitMessageFormDisplay(),
				"Ошибка: Форма с сообщением не открылась.");
		}

		/// <summary>
		/// Проверка работы в редакторе хоткея поиска ошибки терминологии
		/// </summary>
		[Test]
		public void FindErrorHotkeyTest()
		{
			// Нажать хоткей
			EditorPage.AddTextTarget(1, OpenQA.Selenium.Keys.F7);

			// Проверка, что открылась форма
			Assert.IsTrue(
				EditorPage.WaitMessageFormDisplay(),
				"Ошибка: Форма с сообщением не открылась.");
		}

		/// <summary>
		/// Метод тестирования кнопки изменения регистра для всего текста
		/// </summary>
		[Test]
		public void ChangeCaseTextButtonTest()
		{
			int segmentNumber = 1;

			// Написать текст в первом сегменте в target
			EditorPage.AddTextTarget(segmentNumber, "the example sentence");
			// Нажать хоткей выделения всего содержимого ячейки
			EditorPage.SelectAlltextByHotkey(segmentNumber);
			// Запустить проверку
			CheckChangeCase("the example sentence", "The Example Sentence", "THE EXAMPLE SENTENCE", true, segmentNumber);
		}

		/// <summary>
		/// Метод тестирования хоткея изменения регистра для всего текста
		/// </summary>
		[Test]
		public void ChangeCaseTextHotkeyTest()
		{
			int segmentNumber = 1;

			// Написать текст в первом сегменте в target
			EditorPage.AddTextTarget(segmentNumber, "the example sentence");
			// Нажать хоткей выделения всего содержимого ячейки
			EditorPage.SelectAlltextByHotkey(segmentNumber);
			// Запустить проверку
			CheckChangeCase("the example sentence", "The Example Sentence", "THE EXAMPLE SENTENCE", false, segmentNumber);
		}

		/// <summary>
		/// Метод тестирования кнопки изменения регистра для слова (не первого)
		/// </summary>
		[Test]
		public void ChangeCaseSomeWordButtonTest()
		{
			int segmentNumber = 1;

			// Написать текст в первом сегменте в target
			EditorPage.AddTextTarget(segmentNumber, "some words for example");
			// Нажать хоткей выделения последнего слова
			EditorPage.SelectLastWordByHotkey(segmentNumber);
			// Запустить проверку
			CheckChangeCase(
				"some words for example", 
				"some words for Example", 
				"some words for EXAMPLE", 
				true, 
				segmentNumber);
		}

		/// <summary>
		/// Метод тестирования хоткея изменения регистра для слова (не первого)
		/// </summary>
		[Test]
		public void ChangeCaseSomeWordHotkeyTest()
		{
			int segmentNumber = 1;

			// Написать текст в первом сегменте в target
			EditorPage.AddTextTarget(segmentNumber, "some words for example");
			// Нажать хоткей выделения последнего слова
			EditorPage.SelectLastWordByHotkey(segmentNumber);
			// Запустить проверку
			CheckChangeCase(
				"some words for example", 
				"some words for Example", 
				"some words for EXAMPLE", 
				false, 
				segmentNumber);
		}

		/// <summary>
		/// Метод тестирования кнопки и хоткея изменения регистра для первого слова
		/// </summary>
		[Test]
		public void ChangeCaseFirstWordTestByHotKey()
		{			
			int segmentNumber = 1;

			// Написать текст в первом сегменте в target
			EditorPage.AddTextTarget(segmentNumber, "some words for example");
			// Нажать хоткей перехода в начало строки
			EditorPage.CursorToTargetLineBeginningByHotkey(segmentNumber);
			// Нажать хоткей выделения первого слова
			EditorPage.SelectFirstWordTargetByHotkey(segmentNumber);
				// Запустить проверку по хоткею
			CheckChangeCase(
				"some words for example", 
				"Some words for example", 
				"SOME words for example", 
				false, 
				segmentNumber);

		}
		[Test]
		public void ChangeCaseFirstWordTestByBtn()
		{
			int segmentNumber = 1;

			// Написать текст в первом сегменте в target
			EditorPage.AddTextTarget(segmentNumber, "some words for example");
			// Нажать хоткей перехода в начало строки
			EditorPage.CursorToTargetLineBeginningByHotkey(segmentNumber);
			// Нажать хоткей выделения первого слова
			EditorPage.SelectFirstWordTargetByHotkey(segmentNumber);
			// Запустить проверку по кнопке
		   	CheckChangeCase(
				"some words for example", 
				"Some words for example", 
				"SOME words for example", 
				true, 
				segmentNumber);
		}

		/// <summary>
		/// Метод тестирования хоткея изменения регистра для слов через дефис 
		/// </summary>
		[Test]
		public void ChangeCaseHyphenWordTestByHotKey()
		{
			int segmentNumber = 1;

			// Написать текст в первом сегменте в target
			EditorPage.AddTextTarget(segmentNumber, "some-words for example");
			// Нажать хоткей перехода в начало строки
			EditorPage.CursorToTargetLineBeginningByHotkey(segmentNumber);
			// Нажать хоткей выделения первого слова
			EditorPage.SelectFirstWordTargetByHotkey(segmentNumber);
			// Запустить проверку по хоткею
			CheckChangeCase(
				"some-words for example", 
				"Some-Words for example", 
				"SOME-WORDS for example", 
				false, 
				segmentNumber);
		}

		/// <summary>
		/// Метод тестирования кнопки изменения регистра для слов через дефис 
		/// </summary>
		[Test]
		public void ChangeCaseHyphenWordTestByBtn()
		{
			int segmentNumber = 1;

			// Написать текст в первом сегменте в target
			EditorPage.AddTextTarget(segmentNumber, "some-words for example");
			// Нажать хоткей перехода в начало строки
			EditorPage.CursorToTargetLineBeginningByHotkey(segmentNumber);
			// Нажать хоткей выделения первого слова
			EditorPage.SelectFirstWordTargetByHotkey(segmentNumber);
			// Запустить проверку по хоткею
			CheckChangeCase(
				"some-words for example", 
				"Some-Words for example", 
				"SOME-WORDS for example", 
				true, 
				segmentNumber);
		}

		/// <summary>
		/// Метод тестирования кнопки изменения регистра для части слова
		/// </summary>
		[Test]
		public void ChangeCasePartWordTestByBtn()
		{
			int segmentNumber = 1;

			// Написать текст в первом сегменте в target
			EditorPage.AddTextTarget(segmentNumber, "some-words for example");
			// Нажать хоткей перемещения курсора к четвертому слову
			EditorPage.PutCursorAfterThirdWordByHotkey(segmentNumber);
			// Нажать хоткей выделения трех символов в слове
			EditorPage.SelectNextThreeSymbolsByHotkey(segmentNumber);
			// Запустить проверку по кнопке
			CheckChangeCase(
				"some words for example", 
				"some words for eXample", 
				"some words for eXAMple", 
				true, 
				segmentNumber);
		}

		/// <summary>
		/// Метод тестирования хоткея изменения регистра для части слова
		/// </summary>
		[Test]
		public void ChangeCasePartWordTestByHotKey()
		{
			int segmentNumber = 1;

			// Написать текст в первом сегменте в target
			EditorPage.AddTextTarget(segmentNumber, "some words for example");
			// Нажать хоткей перемещения курсора к четвертому слову
			EditorPage.PutCursorAfterThirdWordByHotkey(segmentNumber);
			// Нажать хоткей выделения трех символов в слове
			EditorPage.SelectNextThreeSymbolsByHotkey(segmentNumber);
			// Запустить проверку по хоткею
			CheckChangeCase(
				"some words for example", 
				"some words for eXample", 
				"some words for eXAMple", 
				false, 
				segmentNumber);
		}

		/// <summary>
		/// Метод тестирования хоткея изменения регистра для части слова ЭТО ВРЕМЕННЫЙ ТЕСТ ПОКА НЕ БУДЕТ ПОФИКШЕН БАГ PRX-4037
		/// </summary>
		[Test]
		public void ChangeCasePartWordTestByHotKeyCurentRealization()
		{
			int segmentNumber = 1;

			// Написать текст в первом сегменте в target
			EditorPage.AddTextTarget(segmentNumber, "some words for example");
			// Нажать хоткей перемещения курсора к четвертому слову
			EditorPage.PutCursorAfterThirdWordByHotkey(segmentNumber);
			// Нажать хоткей выделения трех символов в слове
			EditorPage.SelectNextThreeSymbolsByHotkey(segmentNumber);
			// Запустить проверку по хоткею
			CheckChangeCase(
				"some words for eXAMple", 
				"some words for eXAMple", 
				"some words for example", 
				false, 
				segmentNumber);
		}

		/// <summary>
		/// Метод тестирования кнопки изменения регистра для части слова  ЭТО ВРЕМЕННЫЙ ТЕСТ ПОКА НЕ БУДЕТ ПОФИКШЕН БАГ PRX-4037
		/// </summary>
		[Test]
		public void ChangeCasePartWordTestByBtnCurentRealization()
		{
			int segmentNumber = 1;

			// Написать текст в первом сегменте в target
			EditorPage.AddTextTarget(segmentNumber, "some words for example");
			// Нажать хоткей перемещения курсора к четвертому слову
			EditorPage.PutCursorAfterThirdWordByHotkey(segmentNumber);
			// Нажать хоткей выделения трех символов в слове
			EditorPage.SelectNextThreeSymbolsByHotkey(segmentNumber);
			// Запустить проверку по хоткею
			CheckChangeCase(
				"some words for eXAMple", 
				"some words for eXAMple", 
				"some words for example", 
				true, 
				segmentNumber);
		}

		/// <summary>
		/// Метод тестирования кнопки изменения регистра для слова (не первого) 
		/// </summary>
		[Test]
		public void ChangeCaseSomeWordButtonNonStandardTest()
		{
			int segmentNumber = 1;
			// Написать текст в первом сегменте в target
			EditorPage.AddTextTarget(segmentNumber, "some wOrDs fOr example");
			// Нажать хоткей выделения второго и третьего слов
			EditorPage.SelectSecondThirdWordsByHotkey(segmentNumber);
			// Запустить проверку кнопка
			CheckChangeCase(
				"some WORDS FOR example", 
				"some words for example", 
				"some Words For example", 
				true, 
				segmentNumber);
		}

		/// <summary>
		/// Метод тестирования хоткея изменения регистра для слова (не первого)
		/// </summary>
		[Test]
		public void ChangeCaseSomeWordHotkeyNonStandardTest()
		{
			int segmentNumber = 1;

			// Написать текст в первом сегменте в target
			EditorPage.AddTextTarget(segmentNumber, "some wOrDs fOr example");
			// Нажать хоткей выделения второго и третьего слов
			EditorPage.SelectSecondThirdWordsByHotkey(segmentNumber);
			// Запустить проверку хоткей
			CheckChangeCase(
				"some WORDS FOR example", 
				"some words for example", 
				"some Words For example", 
				false, 
				segmentNumber);
		}

		/// <summary>
		/// Метод тестирования кнопки изменения регистра для слова первого
		/// </summary>
		[Test]
		public void ChangeCaseFirstWordButtonNonStandardTest()
		{
			int segmentNumber = 1;

			// Написать текст в первом сегменте в target
			EditorPage.AddTextTarget(segmentNumber, "sOMe words for example");
			EditorPage.CursorToTargetLineBeginningByHotkey(segmentNumber);
			EditorPage.SelectFirstWordTargetByHotkey(segmentNumber);
			// Запустить проверку
			CheckChangeCase(
				"some words for example", 
				"Some words for example", 
				"SOME Words For example", 
				true, 
				segmentNumber);
		}

		/// <summary>
		/// Метод тестирования хоткея изменения регистра для слова первого
		/// </summary>
		[Test]
		public void ChangeCaseFirstWordHotkeyNonStandardTest()
		{
			int segmentNumber = 1;

			// Написать текст в первом сегменте в target
			EditorPage.AddTextTarget(segmentNumber, "sOMe words for example");
			EditorPage.SelectFirstWordTargetByHotkey(segmentNumber);
			// Запустить проверку
			CheckChangeCase(
				"some words for example", 
				"Some words for example", 
				"SOME Words For example", 
				true, 
				segmentNumber);
		}

		/// <summary>
		/// Метод тестирования кнопки изменения регистра для слова (не первого) текущая реализация
		/// </summary>
		[Test]
		public void ChangeCaseSomeWordButtonNonStandardTestCurrentRealization()
		{
			int segmentNumber = 1;

			// Написать текст в первом сегменте в target
			EditorPage.AddTextTarget(segmentNumber, "some wOrDs fOr example");
			// Нажать хоткей выделения последнего слова
			EditorPage.SelectSecondThirdWordsByHotkey(segmentNumber);
			// Запустить проверку
			CheckChangeCase(
				"some Words For example", 
				"some WORDS FOR example", 
				"some words for example", 
				true, 
				segmentNumber);
		}

		/// <summary>
		/// Метод тестирования хоткея изменения регистра для слова (не первого) текущая реализация
		/// </summary>
		[Test]
		public void ChangeCaseSomeWordHotkeyNonStandardTestCurrentRealization()
		{
			int segmentNumber = 1;

			// Написать текст в первом сегменте в target
			EditorPage.AddTextTarget(segmentNumber, "some wOrDs fOr example");
			// Нажать хоткей выделения последнего слова
			EditorPage.SelectSecondThirdWordsByHotkey(segmentNumber);
			// Запустить проверку
			CheckChangeCase(
				"some Words For example", 
				"some WORDS FOR example", 
				"some words for example", 
				true, 
				segmentNumber);
		}

		/// <summary>
		/// Метод тестирования кнопки изменения регистра для слова первого текущая реализация
		/// </summary>
		[Test]
		public void ChangeCaseFirstWordButtonNonStandardTestCurrentRealization()
		{
			int segmentNumber = 1;

			// Написать текст в первом сегменте в target
			EditorPage.AddTextTarget(segmentNumber, "sOMe words for example");
			EditorPage.CursorToTargetLineBeginningByHotkey(segmentNumber);
			EditorPage.SelectFirstWordTargetByHotkey(segmentNumber);
			// Запустить проверку
			CheckChangeCase(
				"Some words for example", 
				"SOME words for example", 
				"some words for example", 
				true, 
				segmentNumber);
		}

		/// <summary>
		/// Метод тестирования хоткея изменения регистра для слова первого текущая реализация
		/// </summary>
		[Test]
		public void ChangeCaseFirstWordHotkeyNonStandardTestCurrentRealization()
		{
			int segmentNumber = 1;

			// Написать текст в первом сегменте в target
			EditorPage.AddTextTarget(segmentNumber, "sOMe words for example");
			EditorPage.CursorToTargetLineBeginningByHotkey(segmentNumber);
			EditorPage.SelectFirstWordTargetByHotkey(segmentNumber);
			// Запустить проверку
			CheckChangeCase(
				"Some words for example", 
				"SOME words for example", 
				"some words for example", 
				true, 
				segmentNumber);
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
			// Нажать хоткей
			EditorPage.SearchByHotkey(1);

			// Проверка, что открылся поиск
			Assert.IsTrue(
				EditorPage.WaitConcordanceSearchDisplay(),
				"Ошибка: Поиск не открылся.");
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
		public void CharacterHotkeyTest()
		{
			// Нажать хоткей
			EditorPage.AddTextTarget(1, OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.Shift + "i");

			// Проверка, что открылась форма
			Assert.IsTrue(
				EditorPage.WaitCharFormDisplay(),
				"Ошибка: Форма выбора спецсимвола не открылась.");
		}



		/// <summary>
		/// Проверить изменение регистра
		/// </summary>
		/// <param name="sourceText">начальный текст</param>
		/// <param name="textAfterFirstChange">текст после первого изменения</param>
		/// <param name="textAfterSecondChange">текст после второго изменения</param>
		/// <param name="byButtonTrueByHotkeyFalse">по кнопке или по хоткею</param>
		/// <param name="segmentNumber">порядковый номер сегмента</param>
		protected void CheckChangeCase(
			string sourceText, 
			string textAfterFirstChange, 
			string textAfterSecondChange, 
			bool byButtonTrueByHotkeyFalse, 
			int segmentNumber)
		{
			// Список текстов для сравнения после изменения регистра
			var listToCompare = new List<string>
			{
				textAfterFirstChange,
				textAfterSecondChange,
				sourceText
			};

			for (int i = 0; i < listToCompare.Count; ++i)
			{
				// Нажать изменениe регистра
				ClickChangeCase(byButtonTrueByHotkeyFalse, segmentNumber);

				// Убедиться, что регистр слова изменился правильно - сравнить со значением в listToCompare
				Assert.AreEqual(listToCompare[i], EditorPage.GetTargetText(segmentNumber), "Ошибка: не совпал текст при изменении регистра");
			}
		}

		/// <summary>
		/// Нажать Изменить регистр
		/// </summary>
		/// <param name="byButtonTrueByHotkeyFalse">true - по кнопке, false - по хоткею</param>
		/// <param name="segmentNumber">номер сегмента</param>
		protected void ClickChangeCase(bool byButtonTrueByHotkeyFalse, int segmentNumber)
		{
			if (byButtonTrueByHotkeyFalse)
			{
				// Нажать кнопку
				EditorPage.ClickChangeCaseBtn();
			}
			else
			{
				// Нажать хоткей
				EditorPage.ChangeCaseByHotkey(segmentNumber);
			}
		}
	}
}
