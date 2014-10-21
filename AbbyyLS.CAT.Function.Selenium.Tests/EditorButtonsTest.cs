﻿using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
using System.IO;
using System.Text;
using System.Configuration;
using System.Diagnostics;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using OpenQA.Selenium.Interactions;

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
		private string projectNoChangesName = "";



		/// <summary>
		/// Начальная подготовка для группы тестов
		/// </summary>
		[TestFixtureSetUp]
		public void SetupAll()
		{
			// Создание уникального имени проекта
			CreateUniqueNamesByDatetime();

			// Запись имени для дальнейшего использования в группе тестов
			projectNoChangesName = ProjectName;
		}

		/// <summary>
		/// Начальная подготовка для каждого теста
		/// </summary>
		[SetUp]
		public void Setup()
		{
			// Не выходить из браузера после теста
			quitDriverAfterTest = false;

			// 1. Переход на страницу workspace
			GoToWorkspace();

			// 2. Создание проекта с 1 документом внутри
			// При проверке PreviousStage нужно создать новый проект с уникальным именем, т.к. необходимо внести изменения в задачи
			// При проверке Tag нужно чтобы в документе проекта присутствовал tag
			if (TestContext.CurrentContext.Test.Name.Contains("PreviousStage"))
			{
				// Создание проекта с уникальным именем
				CreateProjectIfNotCreated(ProjectName, EditorTXTFile, false, "", false, true, Workspace_CreateProjectDialogHelper.MT_TYPE.DefaultMT);
				// Открытие настроек проекта
				WorkspacePage.OpenProjectPage(ProjectName);
			}
			else if (TestContext.CurrentContext.Test.Name.Contains("Tag"))
			{
				// Создание проекта с уникальным именем
				CreateProjectIfNotCreated(ProjectName, DocumentFile);
				// Открытие настроек проекта
				WorkspacePage.OpenProjectPage(ProjectName);
			}
			else
			{
				// Создание проекта с неизменяемым именем, для проведения нескольких тестов
				CreateProjectIfNotCreated(projectNoChangesName, EditorTXTFile);
				// Открытие настроек проекта
				WorkspacePage.OpenProjectPage(projectNoChangesName);
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
			catch { }
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
			Assert.IsTrue(WaitSegmentConfirm(1),
				"Ошибка: Подтверждение (Confirm) не прошло");
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
			string targetxt = EditorPage.GetTargetText(segmentNumber);
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
			EditorPage.SendKeysTarget(segmentNumber, OpenQA.Selenium.Keys.Control + "z");

			// Убедиться, что в target нет текста
			string targetxt = EditorPage.GetTargetText(segmentNumber);
			Assert.AreEqual(textundo, targetxt, "Ошибка: после Undo в Target не убрана одна буква");

			// Нажать хоткей возврата отмененного действия
			EditorPage.SendKeysTarget(segmentNumber, OpenQA.Selenium.Keys.Control + "y");

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
			string sourcetxt = EditorPage.GetSourceText(segmentNumber);

			// Копируем текст в первый сегмент
			ToTargetButton();

			// Подтверждаем
			EditorPage.ClickConfirmBtn();

			// Нажать кнопку отмены
			EditorPage.ClickUndoBtn();
			
			// Убедиться, что текст в target такой же как в source
			string targetxt = EditorPage.GetTargetText(segmentNumber);
			Assert.AreEqual(sourcetxt, targetxt, "Ошибка: Текст не соответствует введенному.");

			// Убедиться, что сегмент стал неподтвержденным
			Assert.IsFalse(EditorPage.GetIsSegmentConfirm(segmentNumber),
				"Ошибка: Сегмент не должен быть подтвержденным.");

			// Нажать кнопку возврата отмененного действия
			EditorPage.ClickRedoBtn();

			// Убедиться, что текст в target такой же как в source
			targetxt = EditorPage.GetTargetText(segmentNumber);
			Assert.AreEqual(sourcetxt, targetxt, "Ошибка: Текст не соответствует введенному.");

			// Убедиться, что сегмент стал подтвержденным
			Assert.IsTrue(EditorPage.GetIsSegmentConfirm(segmentNumber),
				"Ошибка: Сегмент должен быть подтвержденным.");
		}

		/// <summary>
		/// Метод тестирования хоткея Undo и Redo при подтверждении сегмента
		/// </summary>
		[Test]
		public void UndoRedoHotkeySegmentTest()
		{
			int segmentNumber = 1;
			string sourcetxt = EditorPage.GetSourceText(segmentNumber);

			// Копируем текст в первый сегмент
			ToTargetButton(segmentNumber);

			// Подтверждаем
			EditorPage.ClickConfirmBtn();

			// Нажать хоткей отмены
			EditorPage.SendKeysTarget(segmentNumber, OpenQA.Selenium.Keys.Control + "z");

			// Убедиться, что текст в target такой же как в source
			string targetxt = EditorPage.GetTargetText(segmentNumber);
			Assert.AreEqual(sourcetxt, targetxt, "Ошибка: Текст не соответствует введенному.");

			// Убедиться, что сегмент стал неподтвержденным
			Assert.IsFalse(EditorPage.GetIsSegmentConfirm(segmentNumber),
				"Ошибка: Сегмент не должен быть подтвержденным.");

			// Нажать хоткей возврата отмененного действия
			EditorPage.SendKeysTarget(segmentNumber, OpenQA.Selenium.Keys.Control + "y");

			// Убедиться, что текст в target такой же как в source
			targetxt = EditorPage.GetTargetText(segmentNumber);
			Assert.AreEqual(sourcetxt, targetxt, "Ошибка: Текст не соответствует введенному.");

			// Убедиться, что сегмент стал подтвержденным
			Assert.IsTrue(EditorPage.GetIsSegmentConfirm(segmentNumber),
				"Ошибка: Сегмент должен быть подтвержденным.");
		}

		/// <summary>
		/// Метод тестирования кнопки Undo и Redo при подстановке из CAT-панели
		/// </summary>
		[Test]
		public void UndoRedoButtonCatTest()
		{
			int segmentNumber = 1;
			
			// Почистить таргет
			EditorPage.AddTextTarget(segmentNumber, "");

			//Выбираем первый сегмент
			EditorPage.ClickTargetCell(segmentNumber);

			// Подставляем перевод из CAT
			EditorPage.PasteFromCAT(1, EditorPageHelper.CAT_TYPE.TM, true);

			// Нажать кнопку отмены
			EditorPage.ClickUndoBtn();

			// Проверить, что в target пусто
			string targetxt = EditorPage.GetTargetText(segmentNumber);
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
			int segmentNumber = 1;

			// Почистить таргет
			EditorPage.AddTextTarget(segmentNumber, "");

			//Выбираем первый сегмент
			EditorPage.ClickTargetCell(segmentNumber);

			// Подставляем перевод из CAT
			EditorPage.PasteFromCAT(1, EditorPageHelper.CAT_TYPE.TM, true);

			// Нажать хоткей отмены
			EditorPage.SendKeysTarget(segmentNumber, OpenQA.Selenium.Keys.Control + "z");

			// Проверить, что в target пусто
			string targetxt = EditorPage.GetTargetText(segmentNumber);
			Assert.AreEqual("", targetxt, "Ошибка: после Undo в Target есть текст");

			// Нажать хоткей возврата отмененного действия
			EditorPage.SendKeysTarget(segmentNumber, OpenQA.Selenium.Keys.Control + "y");

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
			Assert.IsTrue(EditorPage.GetIsCursorInSourceCell(2),
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
			EditorPage.SendKeysTarget(1, OpenQA.Selenium.Keys.F9);

			Thread.Sleep(1000);

			EditorPage.ClickToggleBtn();

			// Проверить, активен второй сегмент
			Assert.IsTrue(EditorPage.GetIsCursorInSourceCell(2),
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
			Assert.IsTrue(EditorPage.GetIsCursorInSourceCell(3),
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
			EditorPage.SendKeysTarget(1, OpenQA.Selenium.Keys.F9);

			Thread.Sleep(1000);

			EditorPage.ClickToggleBtn();

			// Проверить, активен третий сегмент
			Assert.IsTrue(EditorPage.GetIsCursorInSourceCell(3),
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
			int segmentNumber = 1;
			// Перешли из Target в Source по кнопке
			SourceTargetSwitchButton(segmentNumber);

			// Проверить где находится курсор, и если в поле source, то все ок
			Assert.True(EditorPage.GetIsCursorInSourceCell(segmentNumber),
				"Ошибка: после кнопки Toggle не перешли в Target");
		}

		/// <summary>
		/// Метод тестирования хоткея перемещения курсора между полями source и target
		/// </summary>
		[Test]
		public void TabHotkeyTest()
		{
			int segmentNumber = 1;
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

			// Дождаться загрузки воркспейса
			WorkspacePage.WaitPageLoad();

			// Перейти к проекту
			OpenProjectPage(ProjectName);

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
			Thread.Sleep(1000);

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
			ResponsiblesDialog.ClickChoosenTask(2);
			EditorPage.WaitPageLoad();

			// Проверяем что нет замочка в сегменете
			Assert.False(EditorPage.GetIsSegmentLock(1),
				"Ошибка: Сегмент заблокирован.");

			// Переходим к первому сегменту
			EditorPage.ClickTargetCell(1);

			// Проверяем что кнопка отката разблокирована
			Assert.False(EditorPage.GetIsRollbackBtnLock(),
				"Ошибка: Кнопка отката изменений сегмента заблокирована.");

			// Жмем кнопку отката изменений
			EditorPage.ClickRollbackBtn();

			// Проверяем что появился замочек в сегменете
			Assert.True(EditorPage.GetIsSegmentLock(1),
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
			Assert.IsTrue(EditorPage.GetIsTagPresent(1),
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
			Assert.IsTrue(EditorPage.GetIsTagPresent(1),
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
			Assert.IsTrue(EditorPage.WaitMessageFormDisplay(),
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
			Assert.IsTrue(EditorPage.WaitMessageFormDisplay(),
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
			EditorPage.SendKeysTarget(segmentNumber, OpenQA.Selenium.Keys.Shift + OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.Home);
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
			EditorPage.SendKeysTarget(segmentNumber, OpenQA.Selenium.Keys.Shift + OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.Home);
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
			EditorPage.SendKeysTarget(segmentNumber, OpenQA.Selenium.Keys.Shift + OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.ArrowLeft);
			// Запустить проверку
			CheckChangeCase("some words for example", "some words for Example", "some words for EXAMPLE", true, segmentNumber);
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
			EditorPage.SendKeysTarget(segmentNumber, OpenQA.Selenium.Keys.Shift + OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.ArrowLeft);
			// Запустить проверку
			CheckChangeCase("some words for example", "some words for Example", "some words for EXAMPLE", false, segmentNumber);
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
			EditorPage.SendKeysTarget(segmentNumber, OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.Home);
			// Нажать хоткей выделения первого слова
			EditorPage.SendKeysTarget(segmentNumber, OpenQA.Selenium.Keys.Shift + OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.ArrowRight);
			// Запустить проверку по хоткею
			CheckChangeCase("some words for example", "Some words for example", "SOME words for example", false, segmentNumber);
		}
		[Test]
		public void ChangeCaseFirstWordTestByBtn()
		{
			int segmentNumber = 1;
			// Написать текст в первом сегменте в target
			EditorPage.AddTextTarget(segmentNumber, "some words for example");
			// Нажать хоткей перехода в начало строки
			EditorPage.SendKeysTarget(segmentNumber, OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.Home);
			// Нажать хоткей выделения первого слова
			EditorPage.SendKeysTarget(segmentNumber, OpenQA.Selenium.Keys.Shift + OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.ArrowRight);
		   // Запустить проверку по кнопке
		   	CheckChangeCase("some words for example", "Some words for example", "SOME words for example", true, segmentNumber);
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
			Assert.IsTrue(EditorPage.WaitConcordanceSearchDisplay(),
				"Ошибка: Поиск не открылся.");
		}

		/// <summary>
		/// Проверка работы в редакторе хоткея конкордного поиска
		/// </summary>
		[Test]
		public void ConcordanceSearchHotkeyTest()
		{
			// Нажать хоткей
			EditorPage.AddTextTarget(1, OpenQA.Selenium.Keys.Control + "k");

			// Проверка, что открылся поиск
			Assert.IsTrue(EditorPage.WaitConcordanceSearchDisplay(),
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
			Assert.IsTrue(EditorPage.WaitCharFormDisplay(),
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
			Assert.IsTrue(EditorPage.WaitCharFormDisplay(),
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
		protected void CheckChangeCase(string sourceText, string textAfterFirstChange, string textAfterSecondChange, bool byButtonTrueByHotkeyFalse, int segmentNumber)
		{
			// Список текстов для сравнения после изменения регистра
			List<string> listToCompare = new List<string>();
			listToCompare.Add(textAfterFirstChange);
			listToCompare.Add(textAfterSecondChange);
			listToCompare.Add(sourceText);

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
		/// <param name="rowNumber">номер сегмента</param>
		protected void ClickChangeCase(bool byButtonTrueByHotkeyFalse, int rowNumber)
		{
			if (byButtonTrueByHotkeyFalse)
			{
				// Нажать кнопку
				EditorPage.ClickChangeCaseBtn();
			}
			else
			{
				// Нажать хоткей
				EditorPage.SendKeysTarget(rowNumber, OpenQA.Selenium.Keys.Shift + OpenQA.Selenium.Keys.F3);
			}
		}
	}
}
