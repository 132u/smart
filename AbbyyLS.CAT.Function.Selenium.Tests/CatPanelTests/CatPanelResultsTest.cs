using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;

namespace AbbyyLS.CAT.Function.Selenium.Tests.Editor.Panel
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
		}



		/// <summary>
		/// Подготовка для каждого теста
		/// </summary>
		[SetUp]
		public void Setup()
		{
			// Не выходить из браузера после теста
			QuitDriverAfterTest = false;

			// Переход на страницу workspace
			GoToUrl(RelativeUrlProvider.Workspace);
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
			CreateProject(ProjectName, PathProvider.DocumentFile, true, "", Workspace_CreateProjectDialogHelper.SetGlossary.None, "", true, mt_type, true);
			
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
			Logger.Trace("CheckTM");

			// Создание проекта с файлом с МТ с файлом
			CreateProject(ProjectName, PathProvider.EditorTxtFile, true, PathProvider.EditorTmxFile);
			
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
	}
}
