using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects
{
	[Parallelizable(ParallelScope.Fixtures)]
	[PriorityMajor]
	[Standalone]
	[Projects]
	class DeleteProjectTests<TWebDriverProvider> : BaseProjectTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test]
		public void DeleteProjectNoFileTest()
		{
			_createProjectHelper.CreateNewProject(_projectUniqueName);

			_projectsPage
				.ClickProjectCheckboxInList(_projectUniqueName)
				.ClickDeleteButton();

			_deleteDialog.ClickConfirmDeleteButton();

			Assert.IsFalse(_projectsPage.IsProjectExist(_projectUniqueName),
				"Произошла ошибка:\n проект {0} найден в списке проектов", _projectUniqueName);
		}

		[Test, Description("S-13753")]
		public void DeleteProjectWithFileTest()
		{
			_createProjectHelper.CreateNewProject(
				_projectUniqueName, filesPaths: new[] { PathProvider.DocumentFile, PathProvider.DocumentFile2 });

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickProjectCheckboxInList(_projectUniqueName)
				.ClickDeleteButton();

			_deleteDialog.ClickConfirmDeleteButton();

			Assert.IsFalse(_projectsPage.IsProjectExist(_projectUniqueName),
				"Произошла ошибка:\n проект {0} найден в списке проектов", _projectUniqueName);
		}

		[Test, Description("S-13750")]
		public void DeleteDocumentFromProject()
		{
			var document = PathProvider.DocumentFile;

			_createProjectHelper.CreateNewProject(_projectUniqueName);

			_projectsPage.ClickProject(_projectUniqueName);

			_projectSettingsHelper
				.UploadDocument(new[] { document })
				.DeleteDocument(Path.GetFileNameWithoutExtension(document));

			Assert.IsTrue(
				_projectsPage.IsDocumentRemovedFromProject(_projectUniqueName, document),
				"Произошла ошибка: документ {0} присутствует в проекте {1}", document, _projectUniqueName);
		}

		[Test]
		public void DeleteFileFromWizard()
		{
			var document = PathProvider.DocumentFile;

			_projectsPage.ClickCreateProjectButton();

			_newProjectDocumentUploadPage
				.UploadDocumentFiles(new[] { document })
				.DeleteDocument(Path.GetFileName(document));

			Assert.IsTrue(_newProjectDocumentUploadPage.IsFileDeleted(document),
				"Произошла ошибка:\n файл {0} не удалился.", document);
		}

		[Test, Description("S-7166")]
		public void DeleteFewProjectsTest()
		{
			var projectUniqueName2 = _createProjectHelper.GetProjectUniqueName();
			var projectUniqueName3 = _createProjectHelper.GetProjectUniqueName();

			_createProjectHelper.CreateNewProject(
				projectName: projectUniqueName2,
				filesPaths: new[] { PathProvider.TxtFileForMatchTest },
				createNewTm: true,
				tmxFilesPaths: new[] { PathProvider.TmxFileForMatchTest },
				useMachineTranslation: true);
			_createProjectHelper.CreateNewProject(projectUniqueName3);
			_createProjectHelper.CreateNewProject(_projectUniqueName, filesPaths: new[] { PathProvider.DocumentFile});

			_projectsPage
				.ClickProjectCheckboxInList(_projectUniqueName)
				.ClickProjectCheckboxInList(projectUniqueName2)
				.ClickProjectCheckboxInList(projectUniqueName3)
				.ClickDeleteButton();

			_deleteDialog.ClickConfirmDeleteButton();

			Assert.IsFalse(_projectsPage.IsProjectExist(_projectUniqueName),
				"Произошла ошибка:\n проект {0} найден в списке проектов", _projectUniqueName);

			Assert.IsFalse(_projectsPage.IsProjectExist(projectUniqueName2),
				"Произошла ошибка:\n проект {0} найден в списке проектов", projectUniqueName2);

			Assert.IsFalse(_projectsPage.IsProjectExist(projectUniqueName3),
				"Произошла ошибка:\n проект {0} найден в списке проектов", projectUniqueName3);
		}

		[Test, Description("S-13755")]
		public void DeleteMultiLingualDocumentTest()
		{
			var fileName = Path.GetFileNameWithoutExtension(PathProvider.EditorTxtFile);

			_createProjectHelper.CreateNewProject(
				_projectUniqueName,
				filesPaths: new[] { PathProvider.DocumentFile, PathProvider.EditorTxtFile  });

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickProjectSettingsButton(_projectUniqueName);

			_projectSettingsDialog
				.SelectTargetLanguages(Language.German)
				.SaveSettingsExpectingProjectsPage();

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.OpenDocumentInfoForProject(_projectUniqueName)
				.SelectDocument(_projectUniqueName, PathProvider.EditorTxtFile, jobs: true)
				.ClickDeleteInProjectMenuButton(_projectUniqueName);

			Assert.AreEqual("Delete document with all translations?", _deleteDialog.GetTextFromDeleteDialog(),
				"Произошла ошибка: неверный текст в диалоге удаления документа с джобами.");

			_deleteDialog.ClickConfirmDeleteButton();

			_projectsPage.OpenProjectInfo(_projectUniqueName);

			Assert.IsTrue(_projectsPage.IsDocumentRemovedFromProject(_projectUniqueName, fileName),
				"Произошла ошибка: документ '{0}' не удалился из проекта '{1}'.", fileName, _projectUniqueName);
		}

		[Test, Description("S-13754")]
		public void DeleteDocumentJobTest()
		{
			var fileName = Path.GetFileNameWithoutExtension(PathProvider.EditorTxtFile);

			_createProjectHelper.CreateNewProject(
				_projectUniqueName,
				filesPaths: new[] { PathProvider.DocumentFile, PathProvider.EditorTxtFile });

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickProjectSettingsButton(_projectUniqueName);

			_projectSettingsDialog
				.SelectTargetLanguages(Language.German)
				.SaveSettingsExpectingProjectsPage();

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.OpenDocumentInfoForProject(_projectUniqueName)
				.SelectDocumentJob(_projectUniqueName, fileName, Language.German);

			Assert.IsTrue(_projectsPage.IsDeleteButtonInProjectPanelDisabled(_projectUniqueName),
				"Произошла ошибка: кнопка проекта '{0}' активна.", _projectUniqueName);
		}

		[Test, Description("S-13751")]
		public void DeleteFewDocumentsTest()
		{
			var fileName = Path.GetFileNameWithoutExtension(PathProvider.DocumentFile);
			var fileName2 = Path.GetFileNameWithoutExtension(PathProvider.DocumentFile2);

			_createProjectHelper.CreateNewProject(
				_projectUniqueName,
				filesPaths: new[]
				{
					PathProvider.DocumentFile, 
					PathProvider.EditorTxtFile, 
					PathProvider.DocumentFile2
				});

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.SelectDocument(_projectUniqueName, PathProvider.DocumentFile)
				.SelectDocument(_projectUniqueName, PathProvider.DocumentFile2)
				.ClickDeleteInProjectMenuButton(_projectUniqueName);

			_deleteDialog.ClickConfirmDeleteButton();

			_projectsPage.OpenProjectInfo(_projectUniqueName);

			Assert.IsTrue(_projectsPage.IsDocumentRemovedFromProject(_projectUniqueName, fileName),
				"Произошла ошибка: документ '{0}' не удалился из проекта '{1}'.", fileName, _projectUniqueName);

			Assert.IsTrue(_projectsPage.IsDocumentRemovedFromProject(_projectUniqueName, fileName2),
				"Произошла ошибка: документ '{0}' не удалился из проекта '{1}'.", fileName2, _projectUniqueName);
		}

		[Test, Description("S-13752")]
		public void DeleteAllDocumentsFromProjectTest()
		{
			var fileName = Path.GetFileNameWithoutExtension(PathProvider.DocumentFile);
			var fileName2 = Path.GetFileNameWithoutExtension(PathProvider.DocumentFile2);

			_createProjectHelper.CreateNewProject(
				_projectUniqueName,
				filesPaths: new[]
				{
					PathProvider.DocumentFile,
					PathProvider.DocumentFile2
				});

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.SelectDocument(_projectUniqueName, PathProvider.DocumentFile)
				.SelectDocument(_projectUniqueName, PathProvider.DocumentFile2)
				.ClickDeleteInProjectMenuButton(_projectUniqueName);

			Assert.AreEqual("Delete project(s) or document(s)?", _deleteDialog.GetTextFromDeleteDialog(),
				"Произошла ошибка: неверный текст в диалоге удаления проекта/документа.");

			_deleteDialog.ClickDeleteDocumentsButton();

			Assert.IsTrue(_projectsPage.IsProjectAppearInList(_projectUniqueName),
				"Произошла ошикба: проект удалился.");

			_projectsPage.OpenProjectInfo(_projectUniqueName);

			Assert.IsTrue(_projectsPage.IsDocumentRemovedFromProject(_projectUniqueName, fileName),
				"Произошла ошикба: проект содержит документ {0}.", fileName);

			Assert.IsTrue(_projectsPage.IsDocumentRemovedFromProject(_projectUniqueName, fileName2),
				"Произошла ошикба: проект содержит документ {0}.", fileName2);
		}
	}
}
