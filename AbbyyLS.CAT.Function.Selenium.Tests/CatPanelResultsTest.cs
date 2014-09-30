using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;

namespace AbbyyLs.CAT.Function.Selenium.Tests
{
	/// <summary>
	/// Группа тестов для проверки панели выдачи переводов в редакторе
	/// </summary>
	class CatPanelResultsTest : BaseTest
	{
		/// <summary>
		/// Конструктор теста
		/// </summary>
		 
		 
		/// <param name="browserName">Название браузера</param>
		public CatPanelResultsTest(string browserName)
			: base(browserName)
		{
			System.Console.WriteLine("CatPanelResultsTest");
		}



		/// <summary>
		/// Подготовка для каждого теста
		/// </summary>
		[SetUp]
		public void Setup()
		{
			// Не выходить из браузера после теста
			quitDriverAfterTest = false;

			// Переход на страницу workspace
			GoToWorkspace();
		}



		/// <summary>
		/// ТЕСТ: Проверка выдач и переводов из МТ
		/// </summary>
		[Test]
		[Ignore("Temporarily not working")]
		[TestCase(Workspace_CreateProjectDialogHelper.MT_TYPE.DefaultMT)]
		[TestCase(Workspace_CreateProjectDialogHelper.MT_TYPE.Bing)]
		[TestCase(Workspace_CreateProjectDialogHelper.MT_TYPE.Google)]
		[TestCase(Workspace_CreateProjectDialogHelper.MT_TYPE.Moses)]
		[TestCase(Workspace_CreateProjectDialogHelper.MT_TYPE.Yandex)]
		public void CheckMT(Workspace_CreateProjectDialogHelper.MT_TYPE mt_type)
		{
			// Создание проекта с файлом с ТМ без файла и с заданным МТ
			CreateProject(ProjectName, DocumentFile, true, "", false, true, mt_type, true);

			// Ожидаем пока загрузится документ.
			if (!WorkspacePage.WaitProjectLoad(ProjectName))
				Assert.Fail("Ошибка: Проект не загрузился.");

			// Открываем проект
			OpenProjectPage(ProjectName);

			// Открываем документ
			OpenDocument();
			Thread.Sleep(1000);

			EditorPage.ClickSourceCell(1);

			// Проверка, что на CATPanel что-то есть
			Assert.IsTrue(EditorPage.GetCATPanelNotEmpty(),
				"Ошибка: нет переводов в панели САТ.");

			// Проверка наличия MT
			Assert.AreEqual(1, EditorPage.GetCATTranslationRowNumber(EditorPageHelper.CAT_TYPE.MT),
				"Ошибка: МТ нет в списке или МТ не первая.");

			// Проверка процента
			Assert.AreNotEqual(0, CatPanel.GetCATTranslationProcentMatch(0),
				"Ошибка: Неверный процент совпадения.");

			// Проверка соответствия терминов САТ и подсвеченных слов сегмента
			// Предварительно удаляем из строки сегмента тэги и переводим в нижний регистр
			Assert.AreEqual(EditorPage.GetSourceText(1).Replace("1", String.Empty).ToLower(),
				CatPanel.GetCATTerms()[0],
				"Ошибка: Подсвеченные слова в сегменте не соответствуют терминам САТ.");
		}

		/// <summary>
		/// Тест: Проверка выдач и переводов из TM
		/// </summary>
		[Test]
		public void CheckTM()
		{
			System.Console.WriteLine("CheckTM");

			// Создание проекта с файлом с МТ с файлом
			CreateProject(ProjectName, EditorTXTFile, true, EditorTMXFile, false, false,
				Workspace_CreateProjectDialogHelper.MT_TYPE.None, true);

			// Ожидаем пока загрузится документ.
			if (!WorkspacePage.WaitProjectLoad(ProjectName))
				Assert.Fail("Ошибка: Проект не загрузился.");

			// Открываем проект
			OpenProjectPage(ProjectName);

			// Открываем документ
			OpenDocument();
			Thread.Sleep(1000);

			EditorPage.ClickSourceCell(1);

			// Проверка, что на CATPanel что-то есть
			Assert.IsTrue(EditorPage.GetCATPanelNotEmpty(),
				"Ошибка: нет переводов в панели САТ.");

			// Проверка наличия TМ
			Assert.AreEqual(1, EditorPage.GetCATTranslationRowNumber(EditorPageHelper.CAT_TYPE.TM),
				"Ошибка: ТМ нет в списке или ТМ не первая.");

			// Проверка процента
			Assert.AreNotEqual(0, CatPanel.GetCATTranslationProcentMatch(0),
				"Ошибка: Неверный процент совпадения. Неверный процент.");
		}

		/// <summary>
		/// Тест: Проверка выдач и переводов из глоссария для первого сегмента
		/// </summary>
		[Test]
		public void GlossaryFirstSegment()
		{
			string uniqueGlossaryName = GlossaryName + DateTime.Now.ToString();

			// Добавление словаря для глоссария
			Dictionary<string, string> dictionary = new Dictionary<string, string>
			{ 
				{"first", "первый"},
				{"sentence", "предложение"}
			};

			// Создание глоссария с уникальным именем
			SwitchGlossaryTab();
			CreateGlossaryByName(uniqueGlossaryName);

			// Создание глоссария на основании словаря
			SetGlossaryByDictinary(dictionary);

			// Создание проекта с файлом
			SwitchWorkspaceTab();
			// Создание проекта с файлом с ТМ без файла и с заданным МТ
			CreateProject(ProjectName, EditorTXTFile, true);
			//CreateProject(ProjectName, EditorTXTFile);

			// Ожидаем пока загрузится документ.
			if (!WorkspacePage.WaitProjectLoad(ProjectName))
				Assert.Fail("Ошибка: Проект не загрузился.");

			// Открываем проект
			OpenProjectPage(ProjectName);

			// Добавляем созданный глоссарий
			ProjectPage.SetGlossaryByName(uniqueGlossaryName);

			// Открываем документ
			OpenDocument();
			Thread.Sleep(1000);

			EditorPage.ClickSourceCell(1);

			// Проверка, что на CATpanel что-то есть
			Assert.IsTrue(EditorPage.GetCATPanelNotEmpty(),
				"Ошибка: нет переводов в панели САТ.");

			// Проверка наличия TB
			Assert.AreEqual(1, EditorPage.GetCATTranslationRowNumber(EditorPageHelper.CAT_TYPE.TB),
				"Ошибка: ТB нет в списке или ТB не первая.");

			// Проверка соответствия терминов САТ и подсвеченных слов сегмента
			List<string> segmentTermins = EditorPage.GetSegmentSelectedTexts(1);
			segmentTermins.Sort();

			List<string> catTermins = CatPanel.GetCATTerms();
			catTermins.Sort();

			Assert.AreEqual(segmentTermins, catTermins,
				"Ошибка: Подсвеченные слова в сегменте не соответствуют терминам САТ.");

			// Проверка соответствия заданных терминов и подсвеченных слов сегмента
			Assert.AreEqual(segmentTermins, dictionary.Keys,
				"Ошибка: Подсвеченные слова в сегменте не соответствуют терминам САТ.");
		}

		/// <summary>
		/// Тест: Проверка выдач и переводов из глоссария для одного сегмента
		/// </summary>
		[Test]
		public void GlossaryThirdSegment()
		{
			string uniqueGlossaryName = GlossaryName + DateTime.Now.ToString();

			// Добавление словаря для глоссария
			Dictionary<string, string> dictionary = new Dictionary<string, string>
			{ 
				{"more", "еще"},
				{"one", "один"},
				{"test", "тест"}
			};

			// Создание глоссария с уникальным именем
			SwitchGlossaryTab();
			CreateGlossaryByName(uniqueGlossaryName);

			// Создание глоссария на основании словаря
			SetGlossaryByDictinary(dictionary);

			// Создание проекта с файлом
			SwitchWorkspaceTab();
			CreateProject(ProjectName, EditorTXTFile, true);

			// Ожидаем пока загрузится документ.
			if (!WorkspacePage.WaitProjectLoad(ProjectName))
				Assert.Fail("Ошибка: Проект не загрузился.");

			// Открываем проект
			OpenProjectPage(ProjectName);

			// Добавляем созданный глоссарий
			ProjectPage.SetGlossaryByName(uniqueGlossaryName);

			// Открываем документ
			OpenDocument();
			Thread.Sleep(1000);

			// Переходим на третий сегмент
			EditorPage.ClickSourceCell(3);

			// Проверка, что на CATpanel что-то есть
			Assert.IsTrue(EditorPage.GetCATPanelNotEmpty(),
				"Ошибка: нет переводов в панели САТ.");

			// Проверка наличия TB
			Assert.AreEqual(1, EditorPage.GetCATTranslationRowNumber(EditorPageHelper.CAT_TYPE.TB),
				"Ошибка: ТB нет в списке или ТB не первая.");

			// Проверка соответствия терминов САТ и подсвеченных слов сегмента
			List<string> segmentTermins = EditorPage.GetSegmentSelectedTexts(3);
			segmentTermins.Sort();

			List<string> catTermins = CatPanel.GetCATTerms();
			catTermins.Sort();

			Assert.AreEqual(segmentTermins, catTermins,
				"Ошибка: Подсвеченные слова в сегменте не соответствуют терминам САТ.");

			// Проверка соответствия заданных терминов и подсвеченных слов сегмента
			Assert.AreEqual(segmentTermins, dictionary.Keys,
				"Ошибка: Подсвеченные слова в сегменте не соответствуют терминам САТ.");
		}

		/// <summary>
		/// Тест: Проверка количества терминов в нескольких сегментах
		/// </summary>
		[Test]
		public void GlossaryFewSegments()
		{
			System.Console.WriteLine("GlossaryFewSegments");
			string uniqueGlossaryName = GlossaryName + DateTime.Now.ToString();
			List<string> catSelectedTexts = new List<string>();

			// Добавление словаря для глоссария
			Dictionary<string, string> dictionary = new Dictionary<string, string>
			{ 
				{"second", "второй"},
				{"one", "один"},
				{"more", "еще"},
				{"test", "тест"}
			};

			// Создание глоссария с уникальным именем
			SwitchGlossaryTab();
			CreateGlossaryByName(uniqueGlossaryName);

			// Создание глоссария на основании словаря
			SetGlossaryByDictinary(dictionary);

			// Создание проекта с файлом
			SwitchWorkspaceTab();
			CreateProject(ProjectName, EditorTXTFile, true);

			// Ожидаем пока загрузится документ.
			if (!WorkspacePage.WaitProjectLoad(ProjectName))
				Assert.Fail("Ошибка: Проект не загрузился.");

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
			Assert.AreEqual(1, catSelectedTexts.Count,
				"Ошибка: количество совпадений второго сегмента не корректно.");

			// Третий сегмент
			catSelectedTexts = EditorPage.GetSegmentSelectedTexts(3);

			// Проверяем, что есть три совпадения
			Assert.AreEqual(3, catSelectedTexts.Count,
				"Ошибка: количество совпадений третьего сегмента не корректно.");

			// Четвертый сегмент
			catSelectedTexts = EditorPage.GetSegmentSelectedTexts(4);

			// Проверяем, что совпадения отсутствуют
			Assert.AreEqual(0, catSelectedTexts.Count,
				"Ошибка: количество совпадений четвертого сегмента не корректно.");
		}
	}
}
