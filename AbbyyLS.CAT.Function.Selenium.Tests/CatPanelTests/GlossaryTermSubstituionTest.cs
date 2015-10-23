using System;
using System.Collections.Generic;
using System.Threading;

using NUnit.Framework;

using AbbyyLS.CAT.Function.Selenium.Tests.Driver;

namespace AbbyyLS.CAT.Function.Selenium.Tests.Editor.Panel
{
	[Category("Standalone")]
	class GlossaryTermSubstituionTest<TWebDriverSettings> : BaseTest<TWebDriverSettings> where TWebDriverSettings : IWebDriverSettings, new()
	{
		/// <summary>
		/// Подготовка для каждого теста
		/// </summary>
		[SetUp]
		public void Setup()
		{
			// Не выходить из браузера после теста
			QuitDriverAfterTest = true;

			// Переход на страницу workspace
			GoToUrl(RelativeUrlProvider.Workspace);

			WorkspacePage.CloseTour();
		}

		/// <summary>
		/// Тест: Проверка выдач и переводов из глоссария для первого сегмента
		/// </summary>
		[Test]
		public void GlossaryFirstSegment()
		{
			var uniqueGlossaryName = GlossaryName + DateTime.Now;

			// Добавление словаря для глоссария
			var dictionary = new Dictionary<string, string>
			{ 
				{"first", "первый"},
				{"sentence", "предложение"}
			};

			// Создание глоссария с уникальным именем
			Logger.Info("Создаём глоссарий.");
			SwitchGlossaryTab();
			CreateGlossaryByName(uniqueGlossaryName);

			// Создание глоссария на основании словаря
			Logger.Info("Заполняем глоссарий из словаря.");
			SetGlossaryByDictinary(dictionary);

			// Создание проекта с файлом
			Logger.Info("Создаём проект с файлом с ТМ без файла и с заданным МТ.");
			SwitchWorkspaceTab();
			CreateProject(ProjectUniqueName, PathProvider.EditorTxtFile, true);

			// Открываем проект
			OpenProjectPage(ProjectUniqueName);

			// Добавляем созданный глоссарий
			Logger.Info("Добавляем созданный глоссарий(перешли внутрь проекта).");
			ProjectPage.SetGlossaryByName(uniqueGlossaryName);
			ProjectPage.WaitDocumentDownloadFinish();

			// Выставляем права на редактирование
			AssignTask(ProjectUniqueName);

			// Открываем документ
			OpenDocument();
			Thread.Sleep(1000);

			EditorPage.ClickSourceCell(1);

			// Проверка, что на CATpanel что-то есть
			Assert.IsTrue(EditorPage.GetCATPanelNotEmpty(),
				"Ошибка: нет переводов в панели САТ.");

			// Проверка наличия TB
			Assert.AreEqual(1, EditorPage.GetCatTranslationRowNumber(EditorPageHelper.CAT_TYPE.TB),
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
			var uniqueGlossaryName = GlossaryName + DateTime.Now;

			// Добавление словаря для глоссария
			var dictionary = new Dictionary<string, string>
			{ 
				{"more", "еще"},
				{"one", "один"},
				{"test", "тест"}
			};

			// Создание глоссария с уникальным именем
			Logger.Info("Создаём глоссарий.");
			SwitchGlossaryTab();
			CreateGlossaryByName(uniqueGlossaryName);

			// Создание глоссария на основании словаря
			Logger.Info("Заполняем глоссарий из словаря.");
			SetGlossaryByDictinary(dictionary);

			// Создание проекта с файлом
			Logger.Info("Создаём проект с файлом с ТМ без файла и с заданным МТ.");
			SwitchWorkspaceTab();
			CreateProject(ProjectUniqueName, PathProvider.EditorTxtFile, true);

			// Открываем проект
			OpenProjectPage(ProjectUniqueName);

			// Добавляем созданный глоссарий
			Logger.Info("Добавляем созданный глоссарий(перешли внутрь проекта).");
			ProjectPage.SetGlossaryByName(uniqueGlossaryName);
			ProjectPage.WaitDocumentDownloadFinish();
			
			// Выставляем права на редактирование
			AssignTask(ProjectUniqueName);

			// Открываем документ
			OpenDocument();
			Thread.Sleep(1000);

			// Переходим на третий сегмент
			EditorPage.ClickSourceCell(3);

			// Проверка, что на CATpanel что-то есть
			Assert.IsTrue(EditorPage.GetCATPanelNotEmpty(),
				"Ошибка: нет переводов в панели САТ.");

			// Проверка наличия TB
			Assert.AreEqual(1, EditorPage.GetCatTranslationRowNumber(EditorPageHelper.CAT_TYPE.TB),
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
			Logger.Trace("GlossaryFewSegments");

			var uniqueGlossaryName = GlossaryName + DateTime.Now;
			var catSelectedTexts = new List<string>();

			// Добавление словаря для глоссария
			var dictionary = new Dictionary<string, string>
			{ 
				{"second", "второй"},
				{"one", "один"},
				{"more", "еще"},
				{"test", "тест"}
			};

			// Создание глоссария с уникальным именем
			Logger.Info("Создаём глоссарий.");
			SwitchGlossaryTab();
			CreateGlossaryByName(uniqueGlossaryName);

			// Создание глоссария на основании словаря
			Logger.Info("Заполняем глоссарий из словаря.");
			SetGlossaryByDictinary(dictionary);

			// Создание проекта с файлом
			Logger.Info("Создаём проект с файлом с ТМ без файла и с заданным МТ.");
			SwitchWorkspaceTab();
			CreateProject(ProjectUniqueName, PathProvider.EditorTxtFile, true);

			// Открываем проект
			OpenProjectPage(ProjectUniqueName);

			// Добавляем созданный глоссарий
			Logger.Info("Добавляем созданный глоссарий(перешли внутрь проекта).");
			ProjectPage.SetGlossaryByName(uniqueGlossaryName);
			ProjectPage.WaitDocumentDownloadFinish();

			// Выставляем права на редактирование
			AssignTask(ProjectUniqueName);

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
