using System;
using NUnit.Framework;

namespace AbbyyLS.CAT.Function.Selenium.Tests.Editor
{
	[Category("Standalone")]
	public class EditorBaseTest : BaseTest
	{
		/// <summary>
		/// Конструктор теста
		/// </summary>
		/// <param name="browserName">Название браузера</param>
		public EditorBaseTest(string browserName)
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

	}
}
