using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;

namespace AbbyyLs.CAT.Function.Selenium.Tests
{
	class CatPanelResultsTest : BaseTest
	{
		public CatPanelResultsTest(string url, string workspaceUrl, string browserName)
			: base (url, workspaceUrl, browserName)
		{
		}

		/// <summary>
		/// Старт тестов. Авторизация
		/// </summary>
		[SetUp]
		public void Setup()
		{
			Authorization();
		}

		/// <summary>
		/// ТЕСТ: Проверка выдач и переводов из МТ
		/// </summary>
		[Test]
		[TestCase(Workspace_CreateProjectDialogHelper.MT_TYPE.DefaultMT)]
		[TestCase(Workspace_CreateProjectDialogHelper.MT_TYPE.Bing)]
		[TestCase(Workspace_CreateProjectDialogHelper.MT_TYPE.Google)]
		[TestCase(Workspace_CreateProjectDialogHelper.MT_TYPE.Moses)]
		[TestCase(Workspace_CreateProjectDialogHelper.MT_TYPE.Yandex)]
		public void CatPanelResultsMT(Workspace_CreateProjectDialogHelper.MT_TYPE mt_type)
		{
			// Создание проекта с файлом с ТМ без файла и с заданным МТ
			CreateProject(ProjectName, DocumentFile, true, "", false, true, mt_type, true);
			WorkspacePage.WaitDocumentProjectDownload(ProjectName);
			
			// Открываем проект
			OpenProjectPage(ProjectName);
			
			// Открываем документ
			OpenDocument();
			
			// Проверка, что на CATPanel что-то есть
			Assert.IsTrue(EditorPage.GetCATPanelNotEmpty(), 
				"Ошибка: нет переводов в панели САТ");

			// Проверка наличия MT
			Assert.AreEqual(EditorPage.GetCATTranslationRowNumber(EditorPageHelper.CAT_TYPE.MT), 1,
				"Ошибка: МТ нет в списке или МТ не первая");

			// Проверка процента
			Assert.AreNotEqual(CatPanel.GetCATTranslationProcentMatch(0), 0,
				"Ошибка: Неверный процент совпадения");
		}

		/// <summary>
		/// Тест: Проверка выдач и переводов из TM
		/// </summary>
		[Test]
		public void CatPanelResultsTM()
		{
			// Создание проекта с файлом с МТ с файлом
			CreateProject(ProjectName, EditorTXTFile, true, EditorTMXFile, false, false, 
				Workspace_CreateProjectDialogHelper.MT_TYPE.None, true);

			WorkspacePage.WaitDocumentProjectDownload(ProjectName);

			// Открываем проект
			OpenProjectPage(ProjectName);

			// Открываем документ
			OpenDocument();

			// Проверка, что на CATPanel что-то есть
			Assert.IsTrue(EditorPage.GetCATPanelNotEmpty(), 
				"Ошибка: нет переводов в панели САТ");

			// Проверка наличия TМ
			Assert.AreEqual(EditorPage.GetCATTranslationRowNumber(EditorPageHelper.CAT_TYPE.TM), 1,
				"Ошибка: ТМ нет в списке или ТМ не первая");

			// Проверка процента
			Assert.AreNotEqual(CatPanel.GetCATTranslationProcentMatch(0), 0, 
				"Ошибка: Неверный процент совпадения. Неверный процент");
		}

		/// <summary>
		/// Тест: Проверка выдач и переводов из глоссария для одного сегмента
		/// </summary>
		[Test]
		public void CatPanelResultsGlossaryOneSegment()
		{
			string uniqueGlossaryName = GlossaryName + DateTime.Now.ToString();
			string engTerm = "Earth";
			string rusTerm = "Земля";

			// Создание глоссария с уникальным именем
			SwitchGlossaryTab();
			CreateGlossaryByName(uniqueGlossaryName);

			// Нажать New item
			GlossaryPage.ClickNewItemBtn();
			// Дождаться появления строки для ввода
			GlossaryPage.WaitConceptTableAppear();
			// Заполнить термин
			GlossaryPage.FillTerm(1, engTerm);
			GlossaryPage.FillTerm(2, rusTerm);

			// Расширить окно, чтобы кнопка была видна, иначе Selenium ее "не видит" и выдает ошибку
			Driver.Manage().Window.Maximize();
			// Нажать Сохранить
			GlossaryPage.ClickSaveTermin();
			GlossaryPage.WaitConceptGeneralSave();

			// Создание проекта с файлом
			SwitchWorkspaceTab();
			CreateProject(ProjectName, DocumentFile);
			WorkspacePage.WaitDocumentProjectDownload(ProjectName);
			
			// Открываем проект
			OpenProjectPage(ProjectName);

			// Добавляем созданный глоссарий
			ProjectPage.SetGlossaryByName(uniqueGlossaryName);

			// Открываем документ
			OpenDocument();
			
			// Проверка, что на CATpanel что-то есть
			Assert.IsTrue(EditorPage.GetCATPanelNotEmpty(), 
				"Ошибка: нет переводов в панели САТ");

			// Проверка наличия MT
			Assert.AreEqual(EditorPage.GetCATTranslationRowNumber(EditorPageHelper.CAT_TYPE.TB), 1, 
				"Ошибка: ТB нет в списке или ТB не первая");
			
			// Проверка термина в выдаче
			Assert.AreEqual(EditorPage.GetCATPanelText(0), rusTerm, 
				"Ошибка: Термин в выдаче не соответствует заданному");

			// Проверяем подсвеченный текст на соотвествие заданному термину
			List<string> catSelectedTexts = EditorPage.GetSegmentSelectedTexts(1);
			int countMathes = 0;
			foreach (string catSelectedText in catSelectedTexts)
			{
				if (catSelectedText == engTerm)
					countMathes++;
			}
			Assert.IsTrue(countMathes > 0, 
				"Ошибка: в подсвеченном тексте не найдено заданного термина");
		}

		/// <summary>
		/// Тест: Проверка выдач и переводов из глоссария для несколькких сегментов
		/// </summary>
		[Test]
		public void CatPanelResultsGlossaryFewSegments()
		{
			string uniqueGlossaryName = GlossaryName + DateTime.Now.ToString();
			List<string> catSelectedTexts = new List<string>();

			// Добавление словаря для глоссария
			Dictionary<string, string> dictionary = new Dictionary<string, string>
			{ 
				{"First", "Первый"},
				{"Second", "Второй"},
				{"One", "Один"},
				{"More", "Еще"},
				{"Test", "Тест"}
			};

			// Создание глоссария с уникальным именем
			SwitchGlossaryTab();
			CreateGlossaryByName(uniqueGlossaryName);

			foreach (KeyValuePair<string, string> pair in dictionary)
			{
				// Нажать New item
				GlossaryPage.ClickNewItemBtn();
				// Дождаться появления строки для ввода
				GlossaryPage.WaitConceptTableAppear();
				// Заполнить термин
				GlossaryPage.FillTerm(1, pair.Key);
				GlossaryPage.FillTerm(2, pair.Value);

				// Расширить окно, чтобы кнопка была видна, иначе Selenium ее "не видит" и выдает ошибку
				Driver.Manage().Window.Maximize();
				// Нажать Сохранить
				GlossaryPage.ClickSaveTermin();
				GlossaryPage.WaitConceptGeneralSave();
				Thread.Sleep(1000);
			}

			// Создание проекта с файлом
			SwitchWorkspaceTab();
			CreateProject(ProjectName, EditorTXTFile);
			WorkspacePage.WaitDocumentProjectDownload(ProjectName);

			// Открываем проект
			OpenProjectPage(ProjectName);

			// Добавляем созданный глоссарий
			ProjectPage.SetGlossaryByName(uniqueGlossaryName);

			// Открываем документ
			OpenDocument();

			// Проверяем сегменты
			// Второй сегмент
			catSelectedTexts = EditorPage.GetSegmentSelectedTexts(2);

			// Проверяем, что есть одно совпадение
			Assert.AreEqual(catSelectedTexts.Count, 1, 
				"Ошибка: количество совпадений второго сегмента не корректно");

			// Третий сегмент
			catSelectedTexts = EditorPage.GetSegmentSelectedTexts(3);

			// Проверяем, что есть три совпадения
			Assert.AreEqual(catSelectedTexts.Count, 3, 
				"Ошибка: количество совпадений третьего сегмента не корректно");

			// Четвертый сегмент
			catSelectedTexts = EditorPage.GetSegmentSelectedTexts(4);

			// Проверяем, что совпадения отсутствуют
			Assert.AreEqual(catSelectedTexts.Count, 0, 
				"Ошибка: количество совпадений четвертого сегмента не корректно");
		}
	}
}
