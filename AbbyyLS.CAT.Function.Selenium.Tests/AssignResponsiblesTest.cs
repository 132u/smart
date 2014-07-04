using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Threading;

namespace AbbyyLs.CAT.Function.Selenium.Tests
{
	class AssignResponsiblesTest : BaseTest
	{
		public AssignResponsiblesTest(string url, string workspaceUrl, string browserName)
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
		/// Проверка отображения окна с правами пользователя при нажатии прогресса в Workflow
		/// </summary>
		[Test]
		public void ResponsiblesWorkspaceOnProgressLink()
		{
			// Создание проекта
			CreateProject(ProjectName, EditorTXTFile);

			// Ожидаем пока загрузится документ
			WorkspacePage.WaitProjectLoad(ProjectName);

			// Открываем инфо первого проекта
			WorkspacePage.OpenProjectInfo(ProjectName);

			// Открываем инфо документа 
			WorkspacePage.OpenDocumentInfo(1);

			// Открываем окно с правами пользователя через прогресс документа
			WorkspacePage.ClickDocumentProgress();
			Thread.Sleep(1000);

			Assert.IsTrue(AssignResponsibles.GetIsResponsiblesSetupDisplayed(),
				"Ошибка: Окно прав пользователя не отображается.");
		}

		/// <summary>
		/// Проверка отображения окна с правами пользователя при нажатии кнопки открытия прав в Workflow
		/// </summary>
		[Test]
		public void ResponsiblesWorkspaceOnAssignBtn()
		{
			// Создание проекта
			CreateProject(ProjectName, EditorTXTFile);

			// Ожидаем пока загрузится документ
			WorkspacePage.WaitProjectLoad(ProjectName);

			// Открываем инфо первого проекта
			WorkspacePage.OpenProjectInfo(ProjectName);

			// Открываем инфо документа 
			WorkspacePage.OpenDocumentInfo(1);

			// Открываем окно с правами пользователя через кнопку прав
			WorkspacePage.ClickDocumentAssignBtn();
			Thread.Sleep(1000);

			Assert.IsTrue(AssignResponsibles.GetIsResponsiblesSetupDisplayed(),
				"Ошибка: Окно прав пользователя не отображается.");
		}

		/// <summary>
		/// Проверка отображения окна с правами пользователя при нажатии прогресса в окне проекта
		/// </summary>
		[Test]
		public void ResponsiblesProjectOnProgressLink()
		{
			// Создание проекта
			CreateProject(ProjectName, EditorTXTFile);

			// Ожидаем пока загрузится документ
			WorkspacePage.WaitProjectLoad(ProjectName);

			// Открываем проект
			OpenProjectPage(ProjectName);

			// Открываем окно прав исполнителей
			ProjectPage.ClickDocumentProgress();
			Thread.Sleep(1000);

			Assert.IsTrue(AssignResponsibles.GetIsResponsiblesSetupDisplayed(),
				"Ошибка: Окно прав пользователя не отображается.");
		}

		/// <summary>
		/// Проверка отображения окна с правами пользователя при нажатии кнопки открытия прав в окне проекта
		/// </summary>
		[Test]
		public void ResponsiblesProjectOnAssignBtn()
		{
			// Создание проекта
			CreateProject(ProjectName, EditorTXTFile);

			// Ожидаем пока загрузится документ
			WorkspacePage.WaitProjectLoad(ProjectName);

			// Открываем проект
			OpenProjectPage(ProjectName);

			// Открываем инфо документа 
			ProjectPage.OpenDocumentInfo(1);

			// Открываем окно прав исполнителей
			ProjectPage.ClickAssignBtn();
			Thread.Sleep(1000);

			Assert.IsTrue(AssignResponsibles.GetIsResponsiblesSetupDisplayed(),
				"Ошибка: Окно прав пользователя не отображается.");
		}

		/// <summary>
		/// Проверка отображения окна с правами пользователя при нажатии кнопки Progress при выборе документа
		/// </summary>
		[Test]
		public void ResponsiblesProjectOnProgressBtn()
		{
			// Создание проекта
			CreateProject(ProjectName, EditorTXTFile);

			// Ожидаем пока загрузится документ
			WorkspacePage.WaitProjectLoad(ProjectName);

			// Открываем проект
			OpenProjectPage(ProjectName);

			// Открываем инфо документа 
			ProjectPage.SelectDocument(1);

			// Открываем окно прав исполнителей
			ProjectPage.ClickProgressBtn();
			Thread.Sleep(1000);

			Assert.IsTrue(AssignResponsibles.GetIsResponsiblesSetupDisplayed(),
				"Ошибка: Окно прав пользователя не отображается.");
		}

		/// <summary>
		/// Проверка отображения окна с правами пользователя при загрузке документа в проект
		/// </summary>
		[Test]
		public void ResponsiblesWorkspaceUploadDocument()
		{
			// Создание проекта
			CreateProject(ProjectName);

			// Ожидаем пока загрузится проект
			WorkspacePage.WaitProjectLoad(ProjectName);

			// Открываем инфо проекта
			WorkspacePage.OpenProjectInfo(ProjectName);

			// Открываем диалог загрузки документа
			WorkspacePage.ClickDocumentUploadBtn();

			// Ожидаем пока загрузится диалог
			ProjectPage.WaitImportDialogDisplay();

			// Открываем диалог добавления документа
			ProjectPage.ClickAddDocumentInImport();

			// Добавляем документ
			FillAddDocumentForm(EditorTXTFile);

			ProjectPage.ClickNextImportDialog();
			ProjectPage.ClickNextImportDialog();

			Assert.IsTrue(AssignResponsibles.GetIsResponsiblesSetupDisplayed(),
				"Ошибка: Окно прав пользователя не отображается.");
		}

		/// <summary>
		/// Проверка отображения окна с правами пользователя при загрузке документа в окне проекта
		/// </summary>
		[Test]
		public void ResponsiblesProjectUploadDocument()
		{
			// Создание проекта
			CreateProject(ProjectName);

			// Ожидаем пока загрузится проект
			WorkspacePage.WaitProjectLoad(ProjectName);

			// Открываем проект
			OpenProjectPage(ProjectName);

			// Открываем диалог загрузки документа
			ProjectPage.ClickImportBtn();

			// Ожидаем пока загрузится диалог
			ProjectPage.WaitImportDialogDisplay();

			// Открываем диалог добавления документа
			ProjectPage.ClickAddDocumentInImport();

			// Добавляем документ
			FillAddDocumentForm(EditorTXTFile);

			ProjectPage.ClickNextImportDialog();
			ProjectPage.ClickNextImportDialog();

			Assert.IsTrue(AssignResponsibles.GetIsResponsiblesSetupDisplayed(),
				"Ошибка: Окно прав пользователя не отображается.");
		}
	}
}
