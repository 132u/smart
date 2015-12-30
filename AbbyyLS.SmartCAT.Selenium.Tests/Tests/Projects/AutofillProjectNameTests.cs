﻿using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects
{
	[Parallelizable(ParallelScope.Fixtures)]
	[PriorityMajor]
	[Standalone]
	class AutofillProjectNameTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUpBaseProjectTest()
		{
			_workspacePage = new WorkspacePage(Driver);
			_projectsPage = new ProjectsPage(Driver);
			_newProjectDocumentUploadPage = new NewProjectDocumentUploadPage(Driver);

			_filePath = PathProvider.DocumentFile;
			_fileName = Path.GetFileName(PathProvider.DocumentFile);
			_projectName = Path.GetFileNameWithoutExtension(PathProvider.DocumentFile);

			_workspacePage.GoToProjectsPage();
		}

		[Test(Description = "Проверяет автозаполнение имени проекта")]
		public void AutofillProjectName()
		{
			_projectsPage.ClickCreateProjectButton();

			_newProjectDocumentUploadPage
				.UploadDocumentFile(_filePath)
				.ClickSettingsButton();

			Assert.IsTrue(_newProjectDocumentUploadPage.IsProjectNameMatchExpected(_projectName),
				"Произошла ошибка:\n имя проекта не совпадает с ожидаемым");
		}

		[Test(Description = "Проверяет автозаполнение имени проекта по первому файлу")]
		public void AutofillProjectNameAddTwoFiles()
		{
			var secondFilePath = PathProvider.DocumentFile2;

			_projectsPage.ClickCreateProjectButton();

			_newProjectDocumentUploadPage
				.UploadDocumentFile(_filePath)
				.UploadDocumentFile(secondFilePath)
				.ClickSettingsButton();

			Assert.IsTrue(_newProjectDocumentUploadPage.IsProjectNameMatchExpected(_projectName),
				"Произошла ошибка:\n имя проекта не совпадает с ожидаемым");
		}

		[Test(Description = "Проверяет отсутствие автозаполнения при удалении единственного файла")]
		public void AutofillProjectNameDeleteFile()
		{
			_projectsPage.ClickCreateProjectButton();

			_newProjectDocumentUploadPage
				.UploadDocumentFile(_filePath)
				.DeleteDocument(_fileName)
				.ClickSkipDocumentUploadButton();

			Assert.IsTrue(_newProjectDocumentUploadPage.IsProjectNameMatchExpected(string.Empty),
				"Произошла ошибка:\n имя проекта не совпадает с ожидаемым");
		}

		[Test(Description = "Проверяет автозаполнение имени проекта по второму файлу")]
		public void AutofillProjectNameDeleteOneFile()
		{
			var secondFilePath = PathProvider.DocumentFile2;
			var secondFileName = Path.GetFileNameWithoutExtension(PathProvider.DocumentFile2);

			_projectsPage.ClickCreateProjectButton();

			_newProjectDocumentUploadPage
				.UploadDocumentFile(_filePath)
				.UploadDocumentFile(secondFilePath)
				.DeleteDocument(_fileName)
				.ClickSettingsButton();

			Assert.IsTrue(_newProjectDocumentUploadPage.IsProjectNameMatchExpected(secondFileName),
				"Произошла ошибка:\n имя проекта не совпадает с ожидаемым");
		}

		private string _fileName;
		private string _filePath;
		private string _projectName;

		private WorkspacePage _workspacePage;
		private NewProjectDocumentUploadPage _newProjectDocumentUploadPage;
		private ProjectsPage _projectsPage;
	}
}
